using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Turbo.OrmClient
{
    public class SqlServerDialectProvider : SqlDialectProviderBase
    {
        public override string SelectIdentitySql => "SELECT SCOPE_IDENTITY() AS ID";

        public override string ToSelectStatement(ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            StringBuilder sb = new StringBuilder(sqlClause.Select)
                .AppendLine(sqlClause.From)
                .AppendLine($"WHERE {sqlClause.Where}")
                .AppendLine(sqlClause.Group)
                .AppendLine(sqlClause.Having);

            if (sqlClause.Take == 0 && sqlClause.Skip == 0)
                return sb.AppendLine(sqlClause.OrderBy).ToString();

            if (sqlClause.Skip == 0)
            {
                if (sqlClause.Take == int.MaxValue)
                    return sb.AppendLine(sqlClause.OrderBy).ToString();

                return new StringBuilder("SELECT")
                    .AppendLine(sqlClause.Distinct)
                    .AppendLine(" TOP " + sqlClause.Take)
                    .AppendLine(string.Join(",", sqlClause.SelectFields))
                    .AppendLine(sqlClause.From)
                    .AppendLine($"WHERE {sqlClause.Where}")
                    .AppendLine(sqlClause.Group)
                    .AppendLine(sqlClause.Having)
                    .AppendLine(sqlClause.OrderBy).ToString();
            }

            if (string.IsNullOrWhiteSpace(sqlClause.OrderBy))
            {
                sqlClause.OrderBy = $"ORDER BY {modelDef.PrimaryKey}";
            }

            return new StringBuilder("SELECT * FROM (")
                  .AppendLine($"SELECT {sqlClause.Distinct} {string.Join(",", sqlClause.SelectFields)} , ROW_NUMBER() OVER ({sqlClause.OrderBy}) As RowNum")
                  .AppendLine(sqlClause.From)
                  .AppendLine($"WHERE {sqlClause.Where}")
                  .AppendLine(sqlClause.Group)
                  .AppendLine(sqlClause.Having)
                  .AppendLine(") AS RowConstrainedResult")
                  .AppendLine($"WHERE RowNum > {sqlClause.Skip} AND RowNum <= {(sqlClause.Take == int.MaxValue ? sqlClause.Take : sqlClause.Skip + sqlClause.Take)}")
                  .ToString();
        }
    }
}
