namespace ExCSS
{
    internal sealed class BorderEndStartRadiusProperty : Property
    {
        private static readonly IValueConverter StyleConverter =
            Converters.BorderRadiusConverter.OrDefault(Length.Zero);

        internal BorderEndStartRadiusProperty()
            : base(PropertyNames.BorderEndStartRadius, PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
