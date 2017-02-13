using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    internal sealed class PeriodicValueConverter : IValueConverter
    {
        private readonly IValueConverter _converter;
        private readonly string[] _labels;

        public PeriodicValueConverter(IValueConverter converter, string[] labels)
        {
            _converter = converter;
            _labels = labels.Length == 4 ? labels : Enumerable.Repeat(string.Empty, 4).ToArray();
        }

        public IPropertyValue Convert(IEnumerable<Token> value)
        {
            var list = new List<Token>(value);
            var options = new IPropertyValue[4];

            if (list.Count == 0)
            {
                return null;
            }

            for (var i = 0; (i < options.Length) && (list.Count != 0); i++)
            {
                options[i] = _converter.VaryStart(list);

                if (options[i] == null)
                {
                    return null;
                }
            }

            return list.Count == 0 ? new PeriodicValue(options, value, _labels) : null;
        }

        public IPropertyValue Construct(Property[] properties)
        {
            if (properties.Length == 4)
            {
                var options = new IPropertyValue[4];
                options[0] = _converter.Construct(properties.Where(m => m.Name == _labels[0]).ToArray());
                options[1] = _converter.Construct(properties.Where(m => m.Name == _labels[1]).ToArray());
                options[2] = _converter.Construct(properties.Where(m => m.Name == _labels[2]).ToArray());
                options[3] = _converter.Construct(properties.Where(m => m.Name == _labels[3]).ToArray());
                return (options[0] != null) && (options[1] != null) && (options[2] != null) && (options[3] != null)
                    ? new PeriodicValue(options, Enumerable.Empty<Token>(), _labels)
                    : null;
            }

            return null;
        }

        private sealed class PeriodicValue : IPropertyValue
        {
            private readonly IPropertyValue _bottom;
            private readonly string[] _labels;
            private readonly IPropertyValue _left;
            private readonly IPropertyValue _right;
            private readonly IPropertyValue _top;

            public PeriodicValue(IPropertyValue[] options, IEnumerable<Token> tokens, string[] labels)
            {
                _top = options[0];
                _right = options[1] ?? _top;
                _bottom = options[2] ?? _top;
                _left = options[3] ?? _right;
                Original = new TokenValue(tokens);
                _labels = labels;
            }

            public string[] Values
            {
                get
                {
                    var top = _top.CssText;
                    var right = _right.CssText;
                    var bottom = _bottom.CssText;
                    var left = _left.CssText;

                    if (!right.Is(left))
                    {
                        return new[] {top, right, bottom, left};
                    }
                    if (!top.Is(bottom))
                    {
                        return new[] {top, right, bottom};
                    }

                    return right.Is(top) 
                        ? new[] {top} 
                        : new[] {top, right};
                }
            }

            public string CssText => string.Join(" ", Values);

            public TokenValue Original { get; }

            public TokenValue ExtractFor(string name)
            {
                if (name.Is(_labels[0]))
                {
                    return _top.Original;
                }

                if (name.Is(_labels[1]))
                {
                    return _right.Original;
                }

                if (name.Is(_labels[2]))
                {
                    return _bottom.Original;
                }

                if (name.Is(_labels[3]))
                {
                    return _left.Original;
                }

                return null;
            }
        }
    }
}