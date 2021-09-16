using System.IO;

namespace ExCSS
{
    internal sealed class SimpleSelector : StylesheetNode, ISelector
    {
        public SimpleSelector() : this(Priority.Zero, Keywords.Asterisk)
        {
        }

        public SimpleSelector(string match) : this(Priority.OneTag, match)
        {
        }

        public SimpleSelector(Priority specificity, string code)
        {
            Specificity = specificity;
            Text = code;
        }

        public static readonly SimpleSelector All = new();
        public Priority Specificity { get; }
        public string Text { get; }

        public static SimpleSelector PseudoElement(string pseudoElement)
        {
            return new(Priority.OneTag, PseudoElementNames.Separator + pseudoElement);
        }

        public static SimpleSelector PseudoClass(string pseudoClass)
        {
            return new(Priority.OneClass, PseudoClassNames.Separator + pseudoClass);
        }

        public static SimpleSelector Class(string match)
        {
            return new(Priority.OneClass, "." + match);
        }

        public static SimpleSelector Id(string match)
        {
            return new(Priority.OneId, "#" + match);
        }

        public static SimpleSelector AttrAvailable(string match, string prefix = null)
        {
            var front = match;

            if (!string.IsNullOrEmpty(prefix))
            {
                front = FormFront(prefix, match);
                _ = FormMatch(prefix, match);
            }

            var code = FormCode(front);
            return new SimpleSelector(Priority.OneClass, code);
        }

        public static SimpleSelector AttrMatch(string match, string value, string prefix = null)
        {
            var front = match;

            if (!string.IsNullOrEmpty(prefix))
            {
                front = FormFront(prefix, match);
                _ = FormMatch(prefix, match);
            }

            var code = FormCode(front, "=", value.StylesheetString());
            return new SimpleSelector(Priority.OneClass, code);
        }

        public static SimpleSelector AttrNotMatch(string match, string value, string prefix = null)
        {
            var front = match;

            if (!string.IsNullOrEmpty(prefix))
            {
                front = FormFront(prefix, match);
                _ = FormMatch(prefix, match);
            }

            var code = FormCode(front, "!=", value.StylesheetString());
            return new SimpleSelector(Priority.OneClass, code);
        }

        public static SimpleSelector AttrList(string match, string value, string prefix = null)
        {
            var front = match;

            if (!string.IsNullOrEmpty(prefix))
            {
                front = FormFront(prefix, match);
                _ = FormMatch(prefix, match);
            }

            var code = FormCode(front, "~=", value.StylesheetString());
            return new SimpleSelector(Priority.OneClass, code);
        }

        public static SimpleSelector AttrBegins(string match, string value, string prefix = null)
        {
            var front = match;

            if (!string.IsNullOrEmpty(prefix))
            {
                front = FormFront(prefix, match);
                _ = FormMatch(prefix, match);
            }

            var code = FormCode(front, "^=", value.StylesheetString());
            return new SimpleSelector(Priority.OneClass, code);
        }

        public static SimpleSelector AttrEnds(string match, string value, string prefix = null)
        {
            var front = match;

            if (!string.IsNullOrEmpty(prefix))
            {
                front = FormFront(prefix, match);
                _ = FormMatch(prefix, match);
            }

            var code = FormCode(front, "$=", value.StylesheetString());
            return new SimpleSelector(Priority.OneClass, code);
        }

        public static SimpleSelector AttrContains(string match, string value, string prefix = null)
        {
            var front = match;

            if (!string.IsNullOrEmpty(prefix))
            {
                front = FormFront(prefix, match);
                _ = FormMatch(prefix, match);
            }

            var code = FormCode(front, "*=", value.StylesheetString());
            return new SimpleSelector(Priority.OneClass, code);
        }

        public static SimpleSelector AttrHyphen(string match, string value, string prefix = null)
        {
            var front = match;

            if (!string.IsNullOrEmpty(prefix))
            {
                front = FormFront(prefix, match);
                _ = FormMatch(prefix, match);
            }

            var code = FormCode(front, "|=", value.StylesheetString());
            return new SimpleSelector(Priority.OneClass, code);
        }

        public static SimpleSelector Type(string match)
        {
            return new(match);
        }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            writer.Write(Text);
        }

        private static string FormCode(string content)
        {
            return string.Concat("[", content, "]");
        }

        private static string FormCode(string name, string op, string value)
        {
            var content = string.Concat(name, op, value);
            return FormCode(content);
        }

        private static string FormFront(string prefix, string match)
        {
            return string.Concat(prefix, Combinators.Pipe, match);
        }

        private static string FormMatch(string prefix, string match)
        {
            return prefix.Is(Keywords.Asterisk) ? match : string.Concat(prefix, PseudoClassNames.Separator, match);
        }
    }
}