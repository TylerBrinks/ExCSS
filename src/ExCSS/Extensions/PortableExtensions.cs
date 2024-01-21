using System;
using System.Linq;
using System.Reflection;

#if !NET40 && !SL50

namespace ExCSS
{
    internal static class PortableExtensions
    {
        public static string ConvertFromUtf32(this int utf32)
        {
            return char.ConvertFromUtf32(utf32);
        }

        public static PropertyInfo[] GetProperties(this Type type)
        {
            return type.GetRuntimeProperties().ToArray();
        }
    }
}

#endif