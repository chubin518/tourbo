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

        public virtual SqlExpression<T> Insert(params string[] fields)
        {
            if (null == fields || fields.Count() == 0)
            {
                fields = modelDef.FieldDefinitions?.Where(item => !item.IsPrimaryKey).Select(item => item.Name).ToArray();
            }
            SqlClause.InsertFields = fields;
            return this;
        }
        public virtual SqlExpression<T> Insert(Expression<Func<T, object>> fields)
        {
            return Insert(fields.GetFieldNames());
        }

    }
}
