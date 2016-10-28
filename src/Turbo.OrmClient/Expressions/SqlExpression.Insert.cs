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
        public virtual SqlExpression<T> Insert(Expression<Func<T, object>> fields)
        {
            _sqlContext.InsertFields = fields.GetFieldNames();
            return this;
        }

    }
}
