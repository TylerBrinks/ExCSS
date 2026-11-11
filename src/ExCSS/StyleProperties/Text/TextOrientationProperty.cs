namespace ExCSS
{
    internal sealed class TextOrientationProperty : Property
    {
        private static readonly IValueConverter StyleConverter = TextFlowConverters.TextOrientationConverter;

        internal TextOrientationProperty()
            : base(PropertyNames.TextOrientation, PropertyFlags.Inherited)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
