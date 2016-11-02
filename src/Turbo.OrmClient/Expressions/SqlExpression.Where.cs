using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient.Expressions
{
    public partial class SqlExpression<T>
    {
        private readonly StringBuilder _sbWhere = new StringBuilder();

        public virtual SqlExpression<T> Where(Expression<Func<T, bool>> predicate)
        {
            Visit(predicate);
            _sqlContext.Where = _sbWhere.ToString();
            return this;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            _sbWhere.Append("(");
            this.Visit(node.Left);
            string strOperator = "";
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    strOperator = "=";
                    break;
                case ExpressionType.NotEqual:
                    strOperator = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    strOperator = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    strOperator = ">=";
                    break;
                case ExpressionType.LessThan:
                    strOperator = "<=";
                    break;
                case ExpressionType.LessThanOrEqual:
                    strOperator = "<=";
                    break;
                case ExpressionType.AndAlso:
                    strOperator = "AND";
                    break;
                case ExpressionType.OrElse:
                    strOperator = "OR";
                    break;
                case ExpressionType.Add:
                    strOperator = "+";
                    break;
                case ExpressionType.Subtract:
                    strOperator = "-";
                    break;
                case ExpressionType.Multiply:
                    strOperator = "*";
                    break;
                case ExpressionType.Divide:
                    strOperator = "/";
                    break;
                case ExpressionType.Call:
                    strOperator = "LIKE";
                    break;
                default:
                    strOperator = node.ToString();
                    break;

            }
            _sbWhere.AppendFormat(" {0} ", strOperator);
            Visit(node.Right);
            _sbWhere.Append(")");
            return node;
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            object value;
            if (node.Value == null)
            {
                value = DBNull.Value;
            }
            else
            {
                switch (Type.GetTypeCode(node.Value.GetType()))
                {
                    case TypeCode.Boolean:
                        value = (bool)node.Value ? 1 : 0;
                        break;
                    default:
                        value = node.Value;
                        break;
                }
            }
            string name = "";
            CreateParam(value, ref name);
            _sbWhere.Append(name);
            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (null != node.Expression)
            {
                if (node.Expression.NodeType == ExpressionType.Parameter)
                {
                    _sbWhere.Append(node.Member.Name);
                }
            }
            return node;
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            return MethodCallProcess(node, false);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            if (node.NodeType == ExpressionType.Not)
            {
                if (node.Operand is MemberExpression)
                {
                    BinaryExpression binary = Expression.MakeBinary(ExpressionType.Equal, node.Operand, Expression.Constant(false));
                    this.Visit(binary);
                }
                else if (node.Operand is MethodCallExpression)
                {
                    MethodCallProcess(node.Operand as MethodCallExpression, true);
                }
                else if (node.Operand is BinaryExpression)
                {
                    BinaryExpression operand = node.Operand as BinaryExpression;
                    this.Visit(Expression.MakeBinary(ExpressionType.NotEqual, operand.Left, operand.Right));
                }
                return node;
            }
            else if (node.NodeType == ExpressionType.Convert)
            {
                return base.VisitUnary(node);
            }
            throw new NotSupportedException("未实现 {0} 一元运算符的解析。");
        }

        private Expression MethodCallProcess(MethodCallExpression node, bool isOpposite)
        {
            string @operator = "";
            switch (node.Method.Name)
            {
                case "ToString":
                    return base.VisitMethodCall(node);
                case "Equals":
                    @operator = isOpposite ? "<>" : "=";
                    switch (node.Arguments[0].NodeType)
                    {
                        case ExpressionType.Constant:
                            MethodCallProcess(node, @operator, (node.Arguments[1] as ConstantExpression).Value);
                            break;
                        case ExpressionType.MemberAccess:
                            VisitMember(node.Arguments[0] as MemberExpression);
                            _sbWhere.Append(@operator);
                            VisitConstant(node.Arguments[1] as ConstantExpression);
                            break;
                        case ExpressionType.Convert:
                            BinaryExpression binary = Expression.MakeBinary(ExpressionType.Equal, node.Arguments[0], node.Arguments[1]);
                            return Visit(binary);
                        default:
                            throw new NotSupportedException(string.Format("暂未实现 {0} 方法的解析。", node.Method.Name));
                    }
                    break;
                case "Contains":
                    @operator = isOpposite ? "NOT LIKE" : "LIKE";
                    MethodCallProcess(node, @operator, string.Format("%{0}%", (node.Arguments[0] as ConstantExpression).Value));
                    break;
                case "StartsWith":
                    @operator = isOpposite ? "NOT LIKE" : "LIKE";
                    MethodCallProcess(node, @operator, string.Format("{0}%", (node.Arguments[1] as ConstantExpression).Value));
                    break;
                case "EndsWith":
                    @operator = isOpposite ? "NOT LIKE" : "LIKE";
                    MethodCallProcess(node, @operator, string.Format("%{0}", (node.Arguments[1] as ConstantExpression).Value));
                    break;
                case "IsNullOrWhiteSpace":
                case "IsNullOrEmpty":
                    @operator = isOpposite ? "<>" : "=";
                    MemberExpression me = (MemberExpression)node.Arguments[0];
                    _sbWhere.AppendFormat("ISNULL({0},'') {1} ''", me.Member.Name, @operator);
                    break;
                default:
                    throw new NotSupportedException(string.Format("暂未实现 {0} 方法的解析。", node.Method.Name));
            }
            return node;
        }

        private void MethodCallProcess(MethodCallExpression expression, string @operator, object argument)
        {
            this.Visit(expression.Object);
            _sbWhere.AppendFormat(" {0} ", @operator);
            ConstantExpression ce = Expression.Constant(argument);
            this.Visit(ce);
        }
    }
}
