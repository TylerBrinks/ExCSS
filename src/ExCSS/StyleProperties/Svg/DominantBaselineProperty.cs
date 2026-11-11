namespace ExCSS
{
    internal sealed class DominantBaselineProperty : Property
    {
        private static readonly IValueConverter StyleConverter = SvgConverters.DominantBaselineConverter.OrDefault();

        internal DominantBaselineProperty()
            : base(PropertyNames.DominantBaseline, PropertyFlags.Inherited)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
