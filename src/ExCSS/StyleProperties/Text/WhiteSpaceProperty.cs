namespace ExCSS
{
    internal sealed class WhiteSpaceProperty : ShorthandProperty
    {
        private static readonly IValueConverter StyleConverter = TextFlowConverters.WhiteSpaceConverter;

        internal WhiteSpaceProperty()
            : base(PropertyNames.WhiteSpace, PropertyFlags.Inherited)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}