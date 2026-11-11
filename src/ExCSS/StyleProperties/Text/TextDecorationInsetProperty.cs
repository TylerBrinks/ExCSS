namespace ExCSS
{
    internal sealed class TextDecorationInsetProperty : Property
    {
        private static readonly IValueConverter StyleConverter =
            new TokenGrammarValueConverter(TextEmphasisGrammar.TryParseInset).OrDefault();

        internal TextDecorationInsetProperty()
            : base(PropertyNames.TextDecorationInset)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
