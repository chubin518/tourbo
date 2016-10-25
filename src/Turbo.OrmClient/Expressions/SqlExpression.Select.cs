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
        public SqlExpression<T> Select(params string[] fields)
        {
            if (null == fields || fields.Count() <= 0)
            {
                fields = modelDef.FieldDefinitions?.Select(item => item.Name).ToArray();
            }
            SqlClause.SelectFields = fields;
            return this;
        }

        public SqlExpression<T> Select(Expression<Func<T, object>> fields)
        {
            return Select(fields.GetFieldNames());
        }

        public SqlExpression<T> Distinct()
        {
            SqlClause.Distinct = " DISTINCT ";
            return this;
        }

        public SqlExpression<T> GroupBy(params string[] fields)
        {
            SqlClause.Group = Environment.NewLine + "GROUP BY " + string.Join(",", fields);
            return this;
        }

        public SqlExpression<T> GroupBy(Expression<Func<T, object>> fields)
        {
            return GroupBy(fields.GetFieldNames());
        }

        public SqlExpression<T> OrderBy(params string[] fields)
        {
            SqlClause.OrderBy = Environment.NewLine + "ORDER BY " + string.Join(",", fields);
            return this;
        }

        public SqlExpression<T> OrderBy(Expression<Func<T, object>> fields)
        {
            return OrderBy(fields.GetFieldNames());
        }

        public SqlExpression<T> OrderByDescending(params string[] fields)
        {
            SqlClause.OrderBy = Environment.NewLine + "ORDER BY " + string.Join(",", fields) + " DESC";
            return this;
        }

        public SqlExpression<T> OrderByDescending(Expression<Func<T, object>> fields)
        {
            return OrderByDescending(fields.GetFieldNames());
        }

        public SqlExpression<T> Having(string sqlFilter)
        {
            if (!string.IsNullOrWhiteSpace(sqlFilter))
            {
                SqlClause.Having = Environment.NewLine + "Having " + sqlFilter;
            }
            return this;
        }

        public SqlExpression<T> Take(uint take = 0)
        {
            SqlClause.Take = take;
            return this;
        }

        public SqlExpression<T> Skip(uint skip = 0)
        {
            SqlClause.Skip = skip;
            return this;
        }
    }
}
