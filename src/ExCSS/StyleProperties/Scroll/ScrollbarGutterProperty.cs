namespace ExCSS
{
    internal sealed class ScrollbarGutterProperty : Property
    {
        private static readonly IValueConverter StyleConverter = ScrollConverters.ScrollbarGutterConverter.OrDefault();

        internal ScrollbarGutterProperty()
            : base(PropertyNames.ScrollbarGutter)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
