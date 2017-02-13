
namespace ExCSS
{
    internal sealed class OverflowWrapProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.OverflowWrapConverter;

        public OverflowWrapProperty()
            : base(PropertyNames.OverflowWrap)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}