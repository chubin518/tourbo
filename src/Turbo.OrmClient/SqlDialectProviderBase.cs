using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.OrmClient.Expressions;
using Turbo.OrmClient.Extensions;

namespace Turbo.OrmClient
{
    public abstract class SqlDialectProviderBase : ISqlDialectProvider
    {
        public virtual char Prefix => '@';

        public virtual string SelectIdentitySql => "";

        public abstract ISqlExpression<T> SqlExpression<T>();

        public string ToCountStatement(ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            return new StringBuilder("SELECT COUNT(1)")
                .AppendLine(sqlClause.From)
                .AppendLine($"WHERE {sqlClause.Where}")
                .AppendLine(sqlClause.Group)
                .AppendLine(sqlClause.Having).ToString();
        }

        public virtual string ToDeleteStatement(ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            return $"DELETE FROM {modelDef.Name} WHERE {sqlClause.Where}";
        }

        public virtual string ToInsertStatement(object fields, ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            StringBuilder sbSql = new StringBuilder($"INSERT INTO {modelDef.Name}");
            bool allFields = sqlClause.InsertFields == null || sqlClause.InsertFields.Count() <= 0;
            if (allFields)
            {
                modelDef = fields.GetType().GetModelDefinition();
                List<string> insertFields = new List<string>();
                foreach (FieldDefinition field in modelDef.FieldDefinitions)
                {
                    if (field.IsPrimaryKey)
                    {
                        continue;
                    }
                    else
                    {
                        sqlClause.Params.Add(Prefix + field.Name, field.GetValue(fields));
                        insertFields.Add(field.Name);
                    }
                }
                sqlClause.InsertFields = insertFields;
            }

            return sbSql.AppendLine($"({string.Join(",", sqlClause.InsertFields)})")
                    .AppendLine($"VALUES ({string.Join(",", sqlClause.InsertFields.Select(field => Prefix + field))})")
                    .AppendLine(SelectIdentitySql).ToString();
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

        public virtual string ToUpdateStatement(object fields, ModelDefinition modelDef, SqlClauseContext sqlClause)
        {
            StringBuilder sbSql = new StringBuilder($"UPDATE {modelDef.Name}");

            bool allUpdates = sqlClause.UpdateFields == null || sqlClause.UpdateFields.Count() <= 0;
            if (allUpdates)
            {
                modelDef = fields.GetType().GetModelDefinition();
                List<string> updateFields = new List<string>();
                foreach (FieldDefinition field in modelDef.FieldDefinitions)
                {
                    sqlClause.Params.Add(Prefix + field.Name, field.GetValue(fields));
                    if (field.IsPrimaryKey)
                    {
                        sqlClause.Where = string.Concat(field, "=", Prefix, field);
                    }
                    else
                    {
                        updateFields.Add(field.Name);
                    }
                }
                sqlClause.UpdateFields = updateFields;
            }
            return sbSql.AppendLine($"SET {string.Join(",", sqlClause.UpdateFields.Select(field => string.Concat(field, "=", Prefix, field)))}")
                .AppendLine($"WHERE {sqlClause.Where}").ToString();
        }
    }
}
