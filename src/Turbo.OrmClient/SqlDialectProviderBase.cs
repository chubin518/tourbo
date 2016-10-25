using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient
{
    public abstract class SqlDialectProviderBase : ISqlDialectProvider
    {
        public virtual char Prefix => '@';

        public virtual string SelectIdentitySql => "";

        public virtual string ToDeleteStatement(ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            return $"DELETE FROM {modelDef.Name} WHERE {sqlClause.Where}";
        }

        public virtual string ToInsertStatement(ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            return $"INSERT INTO {modelDef.Name} ({string.Join(",", sqlClause.InsertFields)})"
                + Environment.NewLine
                + $"VALUES ({string.Join(",", sqlClause.InsertFields.Select(field => Prefix + field))}) "
                + SelectIdentitySql;
        }

        public virtual string ToSelectStatement(ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            StringBuilder sb = new StringBuilder(sqlClause.Select)
                .AppendLine(sqlClause.From)
                .AppendLine($"WHERE {sqlClause.Where}")
                .AppendLine(sqlClause.Group)
                .AppendLine(sqlClause.Having);
            if (!string.IsNullOrWhiteSpace(sqlClause.OrderBy))
            {
                sb.AppendLine(sqlClause.OrderBy);
            }
            if (sqlClause.Take > 0 || sqlClause.Skip > 0)
            {
                sb.AppendLine("LIMIT ");
                if (sqlClause.Skip == 0)
                {
                    sb.Append(sqlClause.Take);
                }
                else
                {
                    sb.Append($"{sqlClause.Take} OFFSET {sqlClause.Skip}");
                }
            }
            return sb.ToString();
        }

        public virtual string ToUpdateStatement(ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            return $"UPDATE {modelDef.Name}"
                + Environment.NewLine
                + $"SET {string.Join(",", sqlClause.UpdateFields.Select(field => field + "=" + Prefix + field))}"
                + Environment.NewLine
                + $"WHERE {sqlClause.Where}";
        }
    }
}
