using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Turbo.Data
{
    public class ModelDefinition
    {
        public string Schema { get; set; }

        public string Name { get; set; }

        public Type ModelType { get; set; }

        public IList<FieldDefinition> FieldDefinitions { get; private set; }

        public ModelDefinition()
        {
            FieldDefinitions = new List<FieldDefinition>();
        }
    }
}
