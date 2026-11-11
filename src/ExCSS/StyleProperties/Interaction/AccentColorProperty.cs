namespace ExCSS
{
    internal sealed class AccentColorProperty : Property
    {
        private static readonly IValueConverter StyleConverter =
            Converters.CurrentColorConverter.OrAuto().OrDefault();

        internal AccentColorProperty()
            : base(PropertyNames.AccentColor, PropertyFlags.Inherited | PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
