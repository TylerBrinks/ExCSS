namespace ExCSS
{
    internal sealed class OpacityProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.OptionalPercentOrFractionConverter;

        internal OpacityProperty() : base(PropertyNames.Opacity, PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}