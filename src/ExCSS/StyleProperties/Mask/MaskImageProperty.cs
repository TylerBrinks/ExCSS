namespace ExCSS
{
    internal sealed class MaskImageProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.MultipleImageSourceConverter.OrDefault();

        internal MaskImageProperty()
            : base(PropertyNames.MaskImage)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
