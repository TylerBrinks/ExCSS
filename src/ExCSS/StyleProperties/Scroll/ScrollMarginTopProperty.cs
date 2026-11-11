namespace ExCSS
{
    internal sealed class ScrollMarginTopProperty : Property
    {
        private static readonly IValueConverter StyleConverter = ScrollConverters.MarginConverter.OrDefault();

        internal ScrollMarginTopProperty()
            : base(PropertyNames.ScrollMarginTop, PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
