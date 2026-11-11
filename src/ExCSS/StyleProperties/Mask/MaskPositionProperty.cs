namespace ExCSS
{
    internal sealed class MaskPositionProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.PointConverter.FromList().OrDefault();

        internal MaskPositionProperty()
            : base(PropertyNames.MaskPosition, PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
