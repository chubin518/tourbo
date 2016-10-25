using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Turbo.OrmClient.Test
{
    public class Author
    {
        public Author() { }
        public Int32 Id { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime? LastActivity { get; set; }
        public Decimal? Earnings { get; set; }
        public bool Active { get; set; }
        public string City { get; set; }
        public string Comments { get; set; }
        public Int16 Rate { get; set; }
    }
}
