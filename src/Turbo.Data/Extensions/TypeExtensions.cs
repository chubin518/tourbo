using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Turbo.Data.Extensions
{
    internal static class TypeExtensions
    {

        public static string Name(this Type type)
        {
#if NET45
            return type.Name;
#else
            return type.GetTypeInfo().Name;
#endif
        }

        public static bool IsValueType(this Type type)
        {
#if NET45
            return type.IsValueType;
#else
            return type.GetTypeInfo().IsValueType;
#endif
        }
        public static bool IsEnum(this Type type)
        {
#if NET45
            return type.IsEnum;
#else
            return type.GetTypeInfo().IsEnum;
#endif
        }
        public static bool IsGenericType(this Type type)
        {
#if NET45
            return type.IsGenericType;
#else
            return type.GetTypeInfo().IsGenericType;
#endif
        }
        public static bool IsInterface(this Type type)
        {
#if NET45
            return type.IsInterface;
#else
            return type.GetTypeInfo().IsInterface;
#endif
        }
        public static bool IsNullableType(this Type type)
        {
            return (type.IsGenericType() && type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

#if NET45
        public static TypeCode GetTypeCode(Type type)
        {
            return Type.GetTypeCode(type);
        }
#else
        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, bool inherit)
        {
            return type.GetTypeInfo().GetCustomAttributes(inherit);
        }

        public static TypeCode GetTypeCode(Type type)
        {
            if (type == null) return TypeCode.Empty;
            TypeCode result;
            if (typeCodeLookup.TryGetValue(type, out result)) return result;

            if (type.IsEnum())
            {
                type = Enum.GetUnderlyingType(type);
                if (typeCodeLookup.TryGetValue(type, out result)) return result;
            }
            return TypeCode.Object;
        }
        static readonly Dictionary<Type, TypeCode> typeCodeLookup = new Dictionary<Type, TypeCode>
        {
            {typeof(bool), TypeCode.Boolean },
            {typeof(byte), TypeCode.Byte },
            {typeof(char), TypeCode.Char},
            {typeof(DateTime), TypeCode.DateTime},
            {typeof(decimal), TypeCode.Decimal},
            {typeof(double), TypeCode.Double },
            {typeof(short), TypeCode.Int16 },
            {typeof(int), TypeCode.Int32 },
            {typeof(long), TypeCode.Int64 },
            {typeof(object), TypeCode.Object},
            {typeof(sbyte), TypeCode.SByte },
            {typeof(float), TypeCode.Single },
            {typeof(string), TypeCode.String },
            {typeof(ushort), TypeCode.UInt16 },
            {typeof(uint), TypeCode.UInt32 },
            {typeof(ulong), TypeCode.UInt64 },
        };
#endif
        public static MethodInfo GetPublicInstanceMethod(this Type type, string name, Type[] types)
        {
#if NET45
            return type.GetMethod(name, BindingFlags.Instance | BindingFlags.Public, null, types, null);
#else
            var method = type.GetRuntimeMethod(name, types);
            return (method != null && method.IsPublic && !method.IsStatic) ? method : null;
#endif
        }

        public static IEnumerable<PropertyInfo> GetPublicInstanceProperties(this Type type)
        {
#if NET45
            return modelType.GetProperties( BindingFlags.Public | BindingFlags.Instance);
#else
            return type.GetTypeInfo().GetProperties(BindingFlags.Public | BindingFlags.Instance);
#endif
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
            return null;
        }
    }
}
