using System;
using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    /// <summary>
    /// Validates a <c>font-palette</c> value (CSS Fonts Module Level 4): the keywords <c>normal</c>/
    /// <c>light</c>/<c>dark</c>, or a <c>&lt;dashed-ident&gt;</c> naming an <c>@font-palette-values</c> rule.
    /// Anything else is dropped at parse time. The authored value text is preserved verbatim, mirroring
    /// <see cref="ClipPathValueConverter"/>. (The <c>palette-mix()</c> form of the grammar is not supported.)
    /// </summary>
    internal sealed class FontPaletteValueConverter : IValueConverter
    {
        public IPropertyValue Convert(IEnumerable<Token> value)
        {
            var tokens = value.Where(t => t.Type != TokenType.Whitespace).ToArray();

            if (tokens.Length == 1 && tokens[0].Type == TokenType.Ident &&
                IsKeywordOrDashedIdent(tokens[0].Data))
            {
                return new FontPaletteValue(value);
            }

            return null;
        }

        public IPropertyValue Construct(Property[] properties)
        {
            return properties.Guard<FontPaletteValue>();
        }

        private static bool IsKeywordOrDashedIdent(string ident) =>
            ident.Isi(Keywords.Normal) || ident.Isi("light") || ident.Isi("dark") ||
            ident.StartsWith("--", StringComparison.Ordinal);

        private sealed class FontPaletteValue : IPropertyValue
        {
            public FontPaletteValue(IEnumerable<Token> tokens)
            {
                Original = new TokenValue(tokens);
            }

            public string CssText => Original.Text;

            public TokenValue Original { get; }

            public TokenValue ExtractFor(string name) => Original;
        }
    }
}
