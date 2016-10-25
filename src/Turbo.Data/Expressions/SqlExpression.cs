using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Turbo.Data.Extensions;

namespace Turbo.Data.Expressions
{
    public partial class SqlExpression<T>
    {
        private const string TrueLiteral = " 1=1 ";

        private ModelDefinition modelDef;

        public SqlExpression()
        {
            modelDef = typeof(T).GetModelDefinition();

        }
    }
}
