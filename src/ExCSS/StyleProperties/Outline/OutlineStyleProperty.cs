namespace ExCSS
{
    internal sealed class OutlineStyleProperty : Property
    {
        private static readonly IValueConverter
            StyleConverter = Converters.LineStyleConverter.OrDefault(LineStyle.None);

        internal OutlineStyleProperty()
            : base(PropertyNames.OutlineStyle)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}