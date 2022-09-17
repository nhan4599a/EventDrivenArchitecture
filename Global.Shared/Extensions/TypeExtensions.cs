using System;

namespace Global.Shared.Extensions
{
    public static class TypeExtensions
    {
        public static object CreateInstance(this Type type)
        {
            return type.GetConstructor(Type.EmptyTypes).Invoke(null);
        }

        public static object? InvokeGeneric(
            this Type type,
            string methodName,
            object? callingInstance,
            Type genericParameter,
            params object?[]? parameters)
        {
            var genericParameters = new Type[] { genericParameter };
            return Invoke(type, methodName, callingInstance, genericParameters, parameters);
        }

        public static object? Invoke(
            this Type type,
            string methodName,
            object? callingInstance,
            Type?[]? genericParameters,
            params object?[]? parameters)
        {
            var targetingMethod = type.GetMethod(methodName)!;
            if (targetingMethod.IsGenericMethod)
            {
                targetingMethod = targetingMethod.MakeGenericMethod(genericParameters);
            }
            return targetingMethod.Invoke(callingInstance, parameters);
        }
    }
}
