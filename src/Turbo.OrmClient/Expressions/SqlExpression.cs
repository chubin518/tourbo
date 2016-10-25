using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Turbo.OrmClient.Extensions;

namespace Turbo.OrmClient.Expressions
{
    public partial class SqlExpression<T> : ExpressionVisitor
    {
        public readonly SqlClauseContext SqlClause = new SqlClauseContext();

        public ModelDefinition modelDef { get; private set; }

        private ISqlDialectProvider SqlDialectProvider;

        public SqlExpression(ISqlDialectProvider dialectProvider)
        {
            SqlDialectProvider = dialectProvider;
            modelDef = typeof(T).GetModelDefinition();
            SqlClause.From = " FROM " + modelDef.Name;
        }

        public string SelectClause()
        {
            return SqlDialectProvider.ToSelectStatement(modelDef, SqlClause);
        }

        public string InsertClause()
        {
            return SqlDialectProvider.ToInsertStatement(modelDef, SqlClause);
        }

        public string UpdateClause()
        {
            return SqlDialectProvider.ToUpdateStatement(modelDef, SqlClause);
        }

        public string DeleteClause()
        {
            return SqlDialectProvider.ToDeleteStatement(modelDef, SqlClause);
        }
    }
}
