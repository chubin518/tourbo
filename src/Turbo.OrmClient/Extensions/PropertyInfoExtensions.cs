using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq.Expressions;

namespace Turbo.OrmClient.Extensions
{
    public static class PropertyInfoExtensions
    {
        public static Action<object, object> GetPropertySetterFn(this PropertyInfo propertyInfo)
        {
            var propertySetMethod = propertyInfo.GetSetMethod();
            if (propertySetMethod == null)
                return null;

#if NO_EXPRESSIONS
            return (o, convertedValue) =>
            {
                propertySetMethod.Invoke(o, new[] { convertedValue });
                return;
            };
#else
            var instance = Expression.Parameter(typeof(object), "i");
            var argument = Expression.Parameter(typeof(object), "a");

            var instanceParam = Expression.Convert(instance, propertyInfo.DeclaringType);
            var valueParam = Expression.Convert(argument, propertyInfo.PropertyType);

            var setterCall = Expression.Call(instanceParam, propertyInfo.GetSetMethod(), valueParam);

            return Expression.Lambda<Action<object, object>>(setterCall, instance, argument).Compile();
#endif
        }

        public static Func<object, object> GetPropertyGetterFn(this PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            if (getMethod == null)
                return null;
#if NO_EXPRESSIONS
			return o => getMethod.Invoke(o, new object[] { });
#else
            ParameterExpression oInstanceParam = Expression.Parameter(typeof(object), "oInstanceParam");
            UnaryExpression instanceParam = Expression.Convert(oInstanceParam, propertyInfo.DeclaringType);
            MethodCallExpression getMethodExpression = Expression.Call(instanceParam, getMethod);
            UnaryExpression convertExpression = Expression.Convert(getMethodExpression, typeof(object));
            return Expression.Lambda<Func<object, object>>(convertExpression, oInstanceParam).Compile();
#endif
        }
        public static Type GetUnderlyingType(this PropertyInfo propertyInfo)
        {
            return propertyInfo.PropertyType.IsNullableType()
                 ? Nullable.GetUnderlyingType(propertyInfo.PropertyType)
                 : propertyInfo.PropertyType;
        }
    }
}