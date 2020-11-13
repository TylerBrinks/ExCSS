using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#if !NET40 && !SL50

namespace ExCSS
{
    internal static class PortableExtensions
    {
        public static string ConvertFromUtf32(this int utf32)
        {
            return char.ConvertFromUtf32(utf32);
        }

        //public static int ConvertToUtf32(this string s, int index)
        //{
        //    return char.ConvertToUtf32(s, index);
        //}

        public static Task Delay(this CancellationToken token, int timeout)
        {
            return Task.Delay(Math.Max(timeout, 4), token);
        }

        public static bool Implements<T>(this Type type)
        {
            return type.GetTypeInfo().ImplementedInterfaces.Contains(typeof(T));
        }

        public static PropertyInfo[] GetProperties(this Type type)
        {
            return type.GetRuntimeProperties().ToArray();
        }

        public static ConstructorInfo[] GetConstructors(this Type type)
        {
            return type.GetTypeInfo().DeclaredConstructors.Where(c => c.IsPublic && !c.IsStatic).ToArray();
        }

        public static FieldInfo GetField(this Type type, string name)
        {
            return type.GetTypeInfo().GetDeclaredField(name);
        }

        public static PropertyInfo GetProperty(this Type type, string name)
        {
            return type.GetTypeInfo().GetDeclaredProperty(name);
        }

        public static bool IsAbstractClass(this Type type)
        {
            return type.GetTypeInfo().IsAbstract;
        }

        public static Type[] GetTypes(this Assembly assembly)
        {
            return assembly.DefinedTypes.Select(t => t.AsType()).ToArray();
        }

        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }
    }
}

#endif