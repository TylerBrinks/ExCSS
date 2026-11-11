namespace ExCSS
{
    internal sealed class CornerStartStartShapeProperty : Property
    {
        private static readonly IValueConverter StyleConverter =
            Converters.CornerShapeConverter.OrDefault(Keywords.Round);

        internal CornerStartStartShapeProperty()
            : base(PropertyNames.CornerStartStartShape)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
