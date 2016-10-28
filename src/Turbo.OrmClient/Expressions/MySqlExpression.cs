using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient.Expressions
{
    public class MySqlExpression<T> : SqlExpression<T>
    {
        public MySqlExpression(ISqlDialectProvider dialectProvider) :
            base(dialectProvider)
        {

        }
    }
}
