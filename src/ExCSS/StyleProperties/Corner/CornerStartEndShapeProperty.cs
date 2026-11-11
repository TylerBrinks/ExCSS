namespace ExCSS
{
    internal sealed class CornerStartEndShapeProperty : Property
    {
        private static readonly IValueConverter StyleConverter =
            Converters.CornerShapeConverter.OrDefault(Keywords.Round);

        internal CornerStartEndShapeProperty()
            : base(PropertyNames.CornerStartEndShape)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
