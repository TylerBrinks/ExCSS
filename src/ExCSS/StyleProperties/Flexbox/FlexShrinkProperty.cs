namespace ExCSS
{
    internal sealed class FlexShrinkProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.NumberConverter
                                                                           .OrGlobalValue()
                                                                           .OrDefault(0);

        internal FlexShrinkProperty()
            : base(PropertyNames.FlexShrink)
        { }

        internal override IValueConverter Converter => StyleConverter;
    }
}
