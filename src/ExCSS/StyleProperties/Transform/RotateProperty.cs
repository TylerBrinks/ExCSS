namespace ExCSS
{
    internal sealed class RotateProperty : Property
    {
        private static readonly IValueConverter StyleConverter =
            Converters.RotateIndividualConverter.OrNone();

        internal RotateProperty()
            : base(PropertyNames.Rotate, PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
