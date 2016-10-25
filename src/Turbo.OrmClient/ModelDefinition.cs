using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.OrmClient
{
    public class ModelDefinition
    {
        public string Schema { get; set; }

        public string Name { get; set; }

        public Type ModelType { get; set; }

        public FieldDefinition PrimaryKey
        {
            get { return this.FieldDefinitions.First(x => x.IsPrimaryKey); }
        }

        public IList<FieldDefinition> FieldDefinitions { get; private set; }

        public ModelDefinition()
        {
            FieldDefinitions = new List<FieldDefinition>();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
