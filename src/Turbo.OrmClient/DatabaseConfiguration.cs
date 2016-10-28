using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.Common;

namespace Turbo.OrmClient
{
    internal static class DatabaseConfiguration
    {
        private static readonly Dictionary<string, ISqlDialectProvider> ProviderDictionary =
            new Dictionary<string, ISqlDialectProvider>
            {
                { "System.Data.SqlClient", new SqlServerDialectProvider() },
                { "MySql.Data.MySqlClient", new MySqlDialectProvider() }
            };

        public static IDbConnection GetConnection(string name)
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings[name];
            DbProviderFactory factory = DbProviderFactories.GetFactory(connectionStrings.ProviderName);
            IDbConnection connnection = factory.CreateConnection();
            connnection.ConnectionString = connectionStrings.ConnectionString;
            return connnection;
        }

        public static ISqlDialectProvider GetSqlProvider(string name)
        {
            var connection = ConfigurationManager.ConnectionStrings[name];
            ISqlDialectProvider provider = null;
            ProviderDictionary.TryGetValue(connection.ProviderName, out provider);
            return provider;
        }
    }
}
