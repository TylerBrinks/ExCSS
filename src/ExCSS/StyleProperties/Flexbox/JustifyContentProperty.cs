namespace ExCSS
{
    internal sealed class JustifyContentProperty : ShorthandProperty
    {
        private static readonly IValueConverter StyleConverter = Converters.JustifyContentConverter;

        internal JustifyContentProperty()
            : base(PropertyNames.JustifyContent)
        { }

        internal override IValueConverter Converter => StyleConverter;
    }
}
