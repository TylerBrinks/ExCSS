using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    internal sealed class PairedValueConverter : IValueConverter
    {
        private readonly IValueConverter _converter;
        private readonly string[] _labels;

        public PairedValueConverter(IValueConverter converter, string[] labels)
        {
            _converter = converter;
            _labels = labels.Length == 2 ? labels : Enumerable.Repeat(string.Empty, 2).ToArray();
        }

        public IPropertyValue Convert(IEnumerable<Token> value)
        {
            var list = new List<Token>(value);
            var options = new IPropertyValue[2];

            if (list.Count == 0) return null;

            for (var i = 0; i < options.Length && list.Count != 0; i++)
            {
                options[i] = _converter.VaryStart(list);

                if (options[i] == null) return null;
            }

            return list.Count == 0 ? new PairedValue(options, value, _labels) : null;
        }

        public IPropertyValue Construct(Property[] properties)
        {
            if (properties.Length != 2) return null;

            var options = new IPropertyValue[2];
            options[0] = _converter.Construct(properties.Where(m => m.Name == _labels[0]).ToArray());
            options[1] = _converter.Construct(properties.Where(m => m.Name == _labels[1]).ToArray());

            return options[0] != null && options[1] != null
                ? new PairedValue(options, Enumerable.Empty<Token>(), _labels)
                : null;
        }

        private sealed class PairedValue : IPropertyValue
        {
            private readonly string[] _labels;

            private readonly IPropertyValue _first;
            private readonly IPropertyValue _second;

            public PairedValue(IPropertyValue[] options, IEnumerable<Token> tokens, string[] labels)
            {
                _labels = labels;
                _first = options[0];
                _second = options[1] ?? _first;
                
                Original = new TokenValue(tokens);
            }

            private string[] Values
            {
                get
                {
                    var first = _first.CssText;
                    var second = _second.CssText;

                    return first.Is(second)
                        ? new[] {first}
                        : new[] {first, second};
                }
            }

            public string CssText => string.Join(" ", Values);

            public TokenValue Original { get; }

            public TokenValue ExtractFor(string name)
            {
                if (name.Is(_labels[0])) return _first.Original;

                if (name.Is(_labels[1])) return _second.Original;

                return null;
            }
        }
    }
}