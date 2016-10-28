using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Turbo.OrmClient.Dapper;

namespace Turbo.OrmClient
{
    public class SqlClauseContext
    {
        public SqlClauseContext()
        {
            Params = new DynamicParameters();
        }

        public DynamicParameters Params { get; private set; }

        public string Select
        {
            get
            {
                StringBuilder sbClause = new StringBuilder("SELECT")
                    .AppendLine(Distinct)
                    .AppendLine(string.Join(",", SelectFields));
                return sbClause.ToString();
            }
        }

        public string From { get; set; }

        public string Where { get; set; } = " 1=1 ";

        public string Group { get; set; }

        public string Having { get; set; }

        public string OrderBy { get; set; }

        public uint Skip { get; set; }

        public uint Take { get; set; }

        public string Distinct { get; set; }

        public IEnumerable<string> SelectFields { get; set; }

        public IEnumerable<string> InsertFields { get; set; }

        public IEnumerable<string> UpdateFields { get; set; }
    }
}
