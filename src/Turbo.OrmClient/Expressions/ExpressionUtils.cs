using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Turbo.OrmClient.Extensions;

namespace Turbo.OrmClient.Expressions
{
    public static class ExpressionUtils
    {
        public static string[] GetFieldNames<T>(this Expression<Func<T, object>> expr)
        {
            var member = expr.Body as MemberExpression;
            if (member != null)
            {
                if (member.Member.DeclaringType.AssignableFrom(typeof(T)))
                    return new[] { member.Member.Name };
            }
            NewExpression newExpr = expr.Body as NewExpression;
            if (newExpr != null)
                return newExpr.Arguments.OfType<MemberExpression>().Select(x => x.Member.Name).ToArray();

            MemberInitExpression init = expr.Body as MemberInitExpression;
            if (init != null)
                return init.Bindings.Select(x => x.Member.Name).ToArray();

            throw new ArgumentException("Invalid Fields List Expression: " + expr);
        }
    }
}
