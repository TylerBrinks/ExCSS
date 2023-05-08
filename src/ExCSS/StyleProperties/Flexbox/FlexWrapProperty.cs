namespace ExCSS
{
    internal sealed class FlexWrapProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.FlexWrapConverter.OrDefault(FlexWrap.NoWrap);

        internal FlexWrapProperty()
            : base(PropertyNames.FlexWrap)
        { }

        internal override IValueConverter Converter => StyleConverter;
    }
}
