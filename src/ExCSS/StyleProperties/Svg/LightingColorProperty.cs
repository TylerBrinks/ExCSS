namespace ExCSS
{
    internal sealed class LightingColorProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.CurrentColorConverter.OrDefault();

        internal LightingColorProperty()
            : base(PropertyNames.LightingColor, PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
