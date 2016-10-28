using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Turbo.OrmClient.Dapper;
using Turbo.OrmClient.Extensions;

namespace Turbo.OrmClient.Expressions
{
    public partial class SqlExpression<T> : ExpressionVisitor, ISqlExpression<T>
    {
        private readonly SqlClauseContext _sqlContext;

        protected readonly ISqlDialectProvider _dialectProvider;

        public DynamicParameters Params
        {
            get
            {
                return _sqlContext.Params;
            }
        }

        public ModelDefinition modelDef { get; private set; }

        public SqlExpression(ISqlDialectProvider dialectProvider)
        {
            _dialectProvider = dialectProvider;
            _sqlContext = new SqlClauseContext();
            modelDef = typeof(T).GetModelDefinition();
            _sqlContext.From = " FROM " + modelDef.Name;
        }

        public void CreateParam(object value, ref string name)
        {
            name = _dialectProvider.Prefix + (Params.ParameterNames.Count()).ToString();
            Params.Add(name, value);
        }

        public string SelectClause()
        {
            if (_sqlContext.SelectFields == null
                || _sqlContext.SelectFields.Count() <= 0)
            {
                _sqlContext.SelectFields = modelDef.FieldDefinitions?.Select(item => item.Name);
            }
            return _dialectProvider.ToSelectStatement(modelDef, _sqlContext);
        }

        public string InsertClause(object fields)
        {
            return _dialectProvider.ToInsertStatement(fields, modelDef, _sqlContext);
        }

        public string UpdateClause(object fields)
        {
            return _dialectProvider.ToUpdateStatement(fields, modelDef, _sqlContext);
        }

        public string DeleteClause()
        {
            return _dialectProvider.ToDeleteStatement(modelDef, _sqlContext);
        }

        public string CountClause()
        {
            return _dialectProvider.ToCountStatement(modelDef, _sqlContext);
        }
    }
}
