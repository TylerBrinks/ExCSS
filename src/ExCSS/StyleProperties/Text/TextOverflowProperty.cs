namespace ExCSS
{
    internal sealed class TextOverflowProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.TextOverflowConverter.OrDefault();

        internal TextOverflowProperty()
            : base(PropertyNames.TextOverflow)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
