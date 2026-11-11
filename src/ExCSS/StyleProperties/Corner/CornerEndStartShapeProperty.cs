namespace ExCSS
{
    internal sealed class CornerEndStartShapeProperty : Property
    {
        private static readonly IValueConverter StyleConverter =
            Converters.CornerShapeConverter.OrDefault(Keywords.Round);

        internal CornerEndStartShapeProperty()
            : base(PropertyNames.CornerEndStartShape)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
