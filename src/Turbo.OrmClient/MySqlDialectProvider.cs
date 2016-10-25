using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient
{
    public class MySqlDialectProvider : SqlDialectProviderBase
    {
        public override string SelectIdentitySql => "SELECT LAST_INSERT_ID() AS ID";
    }
}
