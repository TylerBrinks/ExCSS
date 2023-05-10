namespace ExCSS
{
    internal sealed class FlexGrowProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.NumberConverter
                                                                           .OrGlobalValue()
                                                                           .OrDefault(0);

        internal FlexGrowProperty()
            : base(PropertyNames.FlexGrow)
        { }

        internal override IValueConverter Converter => StyleConverter;
    }
}
