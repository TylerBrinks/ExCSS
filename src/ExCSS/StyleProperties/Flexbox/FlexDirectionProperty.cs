namespace ExCSS
{
    internal sealed class FlexDirectionProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.FlexDirectionConverter.OrDefault(FlexDirection.Row);

        internal FlexDirectionProperty()
            : base(PropertyNames.FlexDirection)
        { }

        internal override IValueConverter Converter => StyleConverter;
    }
}