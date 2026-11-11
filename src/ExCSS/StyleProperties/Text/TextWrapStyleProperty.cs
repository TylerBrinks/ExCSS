namespace ExCSS
{
    internal sealed class TextWrapStyleProperty : Property
    {
        private static readonly IValueConverter StyleConverter = TextFlowConverters.TextWrapStyleConverter;

        internal TextWrapStyleProperty()
            : base(PropertyNames.TextWrapStyle, PropertyFlags.Inherited)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
