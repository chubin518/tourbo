using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Turbo.OrmClient.Extensions
{
    internal static class TypeExtensions
    {

        public static string Name(this Type type)
        {
            return type.Name;
        }

        public static bool IsValueType(this Type type)
        {
            return type.IsValueType;
        }
        public static bool IsEnum(this Type type)
        {
            return type.IsEnum;
        }
        public static bool IsGenericType(this Type type)
        {
            return type.IsGenericType;
        }
        public static bool IsInterface(this Type type)
        {
            return type.IsInterface;
        }
        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        public static TypeCode GetTypeCode(Type type)
        {
            return Type.GetTypeCode(type);
        }
        public static bool AssignableFrom(this Type type, Type fromType)
        {
            return type.IsAssignableFrom(fromType);
        }
        public static MethodInfo GetPublicInstanceMethod(this Type type, string name, Type[] types)
        {
            var method = type.GetRuntimeMethod(name, types);
            return (method != null && method.IsPublic && !method.IsStatic) ? method : null;
        }

        public static Boolean IsAnonymousType(this Type type)
        {
            if (type == null)
                return false;

            bool hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }

        public static IEnumerable<PropertyInfo> GetPublicInstanceProperties(this Type type)
        {
            return type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }


        private static Dictionary<Type, ModelDefinition> modelDefinitionCache = new Dictionary<Type, ModelDefinition>();

        public static ModelDefinition GetModelDefinition(this Type modelType)
        {
            ModelDefinition modelDef;

            if (modelDefinitionCache.TryGetValue(modelType, out modelDef))
                return modelDef;

            if (modelType.IsValueType() || modelType == typeof(string))
                return null;

            modelDef = new ModelDefinition()
            {
                ModelType = modelType,
                Name = modelType.Name(),
                Schema = ""
            };

            IEnumerable<PropertyInfo> objProperties = modelType.GetPublicInstanceProperties();
            FieldDefinition field = null;
            foreach (PropertyInfo property in objProperties)
            {
                field = new FieldDefinition
                {
                    FieldType = property.GetUnderlyingType(),
                    Name = property.Name,
                    GetValueFn = property.GetPropertyGetterFn(),
                    SetValueFn = property.GetPropertySetterFn(),
                    IsPrimaryKey = string.Equals(property.Name, Constant.DEFAULTKEY, StringComparison.CurrentCultureIgnoreCase),
                    PropertyInfo = property
                };
                modelDef.FieldDefinitions.Add(field);
            }

            Dictionary<Type, ModelDefinition> snapshot, newCache;
            do
            {
                snapshot = modelDefinitionCache;
                newCache = new Dictionary<Type, ModelDefinition>(modelDefinitionCache) { [modelType] = modelDef };

            }
            while (!ReferenceEquals(Interlocked.CompareExchange(ref modelDefinitionCache, newCache, snapshot), snapshot));
            return modelDef;
        }
    }
}
