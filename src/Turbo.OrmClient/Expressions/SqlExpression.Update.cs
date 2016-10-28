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

        public SqlExpression<T> Update(Expression<Func<T, object>> fields)
        {
            _sqlContext.UpdateFields = fields.GetFieldNames();
            return this;
        }
    }
}
