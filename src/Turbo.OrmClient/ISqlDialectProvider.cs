using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient
{
    public interface ISqlDialectProvider
    {
        char Prefix { get; }

        string SelectIdentitySql { get; }

        string ToSelectStatement(ModelDefinition modelDef, SqlClauseContext sqlClause);

        string ToUpdateStatement(ModelDefinition modelDef, SqlClauseContext sqlClause);

        string ToInsertStatement(ModelDefinition modelDef, SqlClauseContext sqlClause);

        string ToDeleteStatement(ModelDefinition modelDef, SqlClauseContext sqlClause);
    }
}
