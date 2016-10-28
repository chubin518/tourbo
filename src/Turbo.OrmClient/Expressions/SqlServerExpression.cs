using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient.Expressions
{
    public class SqlServerExpression<T> : SqlExpression<T>
    {
        public SqlServerExpression(ISqlDialectProvider dialectProvider) :
            base(dialectProvider)
        {

        }
    }
}
