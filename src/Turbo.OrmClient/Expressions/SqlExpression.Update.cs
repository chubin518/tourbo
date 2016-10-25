using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Turbo.OrmClient.Expressions
{
    public partial class SqlExpression<T>
    {
        public SqlExpression<T> Update(params string[] fields)
        {
            if (null == fields || fields.Count() == 0)
            {
                fields = modelDef.FieldDefinitions?.Where(item => !item.IsPrimaryKey).Select(item => item.Name).ToArray();
            }
            SqlClause.UpdateFields = fields;
            return this;
        }

        public SqlExpression<T> Update(Expression<Func<T, object>> fields)
        {
            return Update(fields.GetFieldNames());
        }
    }
}
