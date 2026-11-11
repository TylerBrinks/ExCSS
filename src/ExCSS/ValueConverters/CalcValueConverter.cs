using System.Collections.Generic;

namespace ExCSS
{
    /// <summary>
    /// Accepts a calc()/min()/max()/clamp() expression whose type-checked category is within
    /// <c>allowed</c>, and preserves the authored text. Composed onto the existing length/percent/number/
    /// angle converters via <c>.Or(...)</c> so support cascades to every property built on them.
    /// </summary>
    internal sealed class CalcValueConverter : IValueConverter
    {
        private readonly CalcCategory _allowed;

        public CalcValueConverter(CalcCategory allowed)
        {
            _allowed = allowed;
        }

        public IPropertyValue Convert(IEnumerable<Token> value)
        {
            if (value.OnlyOrDefault() is not FunctionToken function) return null;
            if (!CalcParser.IsCalcFamily(function.Data)) return null;

            var node = CalcParser.Parse(function);
            if (node == null) return null;

            var category = CalcTypeChecker.Check(node);
            if (category == null || (category.Value & ~_allowed) != 0) return null;

            return new CalcValue(value);
        }

        public IPropertyValue Construct(Property[] properties)
        {
            return properties.Guard<CalcValue>();
        }

        private sealed class CalcValue : IPropertyValue
        {
            public CalcValue(IEnumerable<Token> tokens)
            {
                Original = new TokenValue(tokens);
            }

            public string CssText => Original.Text;

            public TokenValue Original { get; }

            public TokenValue ExtractFor(string name) => Original;
        }
    }
}
