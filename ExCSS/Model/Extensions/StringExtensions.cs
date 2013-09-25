using System;
using System.Text;

namespace ExCSS.Model.Extensions
{
    public static class StringExtensions
    {
        public static string Indent(this string value, bool friendlyForamt, int indentation)
        {
            if (!friendlyForamt)
            {
                return value;
            }

            var tabs = new StringBuilder();
            for (var i = 0; i < indentation; i++)
            {
                tabs.Append("\t");
            }

            return string.Format("{0}{1}", tabs, value);
        }

        public static string NewLineIndent(this string value, bool friendlyFormat, int indentation)
        {
            if (!friendlyFormat)
            {
                return value;
            }

            return Environment.NewLine + value.Indent(true, indentation);
        }
    }
}
