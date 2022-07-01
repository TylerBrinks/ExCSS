using System.Collections.Generic;

namespace ExCSS
{
    public sealed class UrlValueConverter : IValueConverter
    {
        public IPropertyValue Convert(IEnumerable<Token> value)
        {
            var str = value.ToUri();
            return str != null ? new UrlValue(str, value) : null;
        }

        public IPropertyValue Construct(Property[] properties)
        {
            return properties.Guard<UrlValue>();
        }

        public sealed class UrlValue : IPropertyValue
        {
            public readonly string _value;

            public UrlValue(string value, IEnumerable<Token> tokens)
            {
                _value = value;
                Original = new TokenValue(tokens);
            }

            public string CssText => _value.StylesheetUrl();

            public TokenValue Original { get; }

            public TokenValue ExtractFor(string name)
            {
                return Original;
            }
        }
    }
}
