namespace ExCSS
{
    internal sealed class TransformBoxProperty : Property
    {
        private static readonly IValueConverter StyleConverter = SvgConverters.TransformBoxConverter.OrDefault();

        internal TransformBoxProperty()
            : base(PropertyNames.TransformBox, PropertyFlags.None)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
