using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Turbo.Data
{
    public class FieldDefinition
    {
        public string Name { get; set; }

        public Type FieldType { get; set; }

        public bool IsPrimaryKey { get; set; }

        public PropertyInfo PropertyInfo { get; set; }

        public Func<object, object> GetValueFn { get; set; }

        public Action<object, object> SetValueFn { get; set; }

        public object GetValue(object onInstance)
        {
            return this.GetValueFn?.Invoke(onInstance);
        }
    }
}
