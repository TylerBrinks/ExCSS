namespace ExCSS
{
    internal sealed class AlignItemsProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.AlignItemsConverter;

        internal AlignItemsProperty()
            : base(PropertyNames.AlignItems)
        { }

        internal override IValueConverter Converter => StyleConverter;
    }
}