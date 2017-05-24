using System;
using System.Reflection;

namespace ThreeK.Game.Helper
{
    public class DynamicInvoke
    {
        public static object Invoke(object callee, Type genericType, string methodName)
        {
            return Invoke(callee, genericType, methodName, null);
        }

        /// <summary>
        /// Dynamically invoke generic method, use dynamic Type instance instead of a certain Type.<para/>
        /// For example, you want to invoke JsonUtility.FromJson<MyData>(json), but I want to use a dynamic type instead of MyData.
        /// </summary>
        /// <param name="callee">Target holds the method that you want to invoke</param>
        /// <param name="genericType">Type of T</param>
        /// <param name="methodName">Method name</param>
        /// <param name="parameters">Parameters</param>
        /// <returns></returns>
        public static object Invoke(object callee, Type genericType, string methodName, object[] parameters)
        {
            return Invoke(callee.GetType(), genericType, methodName, parameters);
        }

        public static object Invoke(Type callee, Type genericType, string methodName)
        {
            return Invoke(callee, genericType, methodName, null);
        }

        public static object Invoke(Type callee, Type genericType, string methodName, object[] parameters)
        {
            MethodInfo method = callee.GetMethod(methodName, new[] { typeof(string) });
            method = method.MakeGenericMethod(genericType);
            object value = method.Invoke(null, parameters);
            return value;
        }
    }
}
