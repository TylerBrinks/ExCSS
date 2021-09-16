namespace ExCSS
{
    internal sealed class AnimationFillModeProperty : Property
    {
        private static readonly IValueConverter ListConverter =
            Converters.AnimationFillStyleConverter.FromList().OrDefault(AnimationFillStyle.None);

        internal AnimationFillModeProperty()
            : base(PropertyNames.AnimationFillMode)
        {
        }

        internal override IValueConverter Converter => ListConverter;
    }
}