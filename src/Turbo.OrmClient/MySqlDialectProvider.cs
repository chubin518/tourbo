using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.OrmClient.Expressions;

namespace Turbo.OrmClient
{
    public class MySqlDialectProvider : SqlDialectProviderBase
    {
        public override ISqlExpression<T> SqlExpression<T>()
        {
            return new MySqlExpression<T>(this);
        }

        public override string SelectIdentitySql => "SELECT LAST_INSERT_ID() AS ID";
    }
}
