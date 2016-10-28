using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.OrmClient.Expressions;

namespace Turbo.OrmClient
{
    public interface ISqlDialectProvider
    {
        char Prefix { get; }

        string SelectIdentitySql { get; }

        ISqlExpression<T> SqlExpression<T>();

        string ToSelectStatement(ModelDefinition modelDef, SqlClauseContext sqlClause);

        string ToUpdateStatement(object fields, ModelDefinition modelDef, SqlClauseContext sqlClause);

        string ToInsertStatement(object fields, ModelDefinition modelDef, SqlClauseContext sqlClause);

        string ToDeleteStatement(ModelDefinition modelDef, SqlClauseContext sqlClause);

        string ToCountStatement(ModelDefinition modelDef, SqlClauseContext sqlClause);
    }
}
