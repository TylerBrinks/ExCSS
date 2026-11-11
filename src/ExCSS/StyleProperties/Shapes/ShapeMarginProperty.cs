namespace ExCSS
{
    internal sealed class ShapeMarginProperty : Property
    {
        private static readonly IValueConverter StyleConverter = new NonNegativeValueConverter(Converters.LengthOrPercentConverter).OrDefault();

        internal ShapeMarginProperty()
            : base(PropertyNames.ShapeMargin, PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
