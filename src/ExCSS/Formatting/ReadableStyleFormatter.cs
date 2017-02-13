using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExCSS
{
    public class ReadableStyleFormatter : IStyleFormatter
    {
        public ReadableStyleFormatter()
        {
            Indentation = "\t";
            NewLine = "\n";
        }

        private string Indent(string content)
        {
            return Indentation + content.Replace(NewLine, NewLine + Indentation);
        }

        public string Indentation { get; set; }
        public string NewLine { get; set; }

        string IStyleFormatter.Sheet(IEnumerable<IStyleFormattable> rules)
        {
            var sb = Pool.NewStringBuilder();
            var first = true;
            using (var writer = new StringWriter(sb))
            {
                foreach (var rule in rules)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        writer.Write(NewLine);
                        writer.Write(NewLine);
                    }
                    rule.ToCss(writer, this);
                }
            }
            return sb.ToPool();
        }
        string IStyleFormatter.Block(IEnumerable<IStyleFormattable> rules)
        {
            var sb = Pool.NewStringBuilder().Append('{').Append(' ');
            using (var writer = new StringWriter(sb))
            {
                foreach (var rule in rules)
                {
                    writer.Write(NewLine);
                    writer.Write(Indent(rule.ToCss(this)));
                    writer.Write(NewLine);
                }
            }
            return sb.Append('}').ToPool();
        }

        string IStyleFormatter.Declaration(string name, string value, bool important)
        {
            return CompressedStyleFormatter.Instance.Declaration(name, value, important);
        }

        string IStyleFormatter.Declarations(IEnumerable<string> declarations)
        {
            return string.Join(NewLine, declarations.Select(m => m + ";"));
        }

        string IStyleFormatter.Medium(bool exclusive, bool inverse, string type,
            IEnumerable<IStyleFormattable> constraints)
        {
            return CompressedStyleFormatter.Instance.Medium(exclusive, inverse, type, constraints);
        }

        string IStyleFormatter.Constraint(string name, string value)
        {
            return CompressedStyleFormatter.Instance.Constraint(name, value);
        }

        string IStyleFormatter.Rule(string name, string value)
        {
            return CompressedStyleFormatter.Instance.Rule(name, value);
        }

        string IStyleFormatter.Rule(string name, string prelude, string rules)
        {
            return CompressedStyleFormatter.Instance.Rule(name, prelude, rules);
        }

        string IStyleFormatter.Style(string selector, IStyleFormattable rules)
        {
            var sb = Pool.NewStringBuilder().Append(selector).Append(" {");
            var content = rules.ToCss(this);
            if (!string.IsNullOrEmpty(content))
            {
                sb.Append(NewLine);
                sb.Append(Indent(content));
                sb.Append(NewLine);
            }
            else
            {
                sb.Append(' ');
            }
            return sb.Append('}').ToPool();
        }

        string IStyleFormatter.Comment(string data)
        {
            return CompressedStyleFormatter.Instance.Comment(data);
        }
    }
}