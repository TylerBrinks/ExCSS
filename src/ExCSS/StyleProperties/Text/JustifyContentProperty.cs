namespace ExCSS
{
    internal sealed class JustifyContentProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.JustifyContentConverter;

        public JustifyContentProperty()
            : base(PropertyNames.JustifyContent)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}