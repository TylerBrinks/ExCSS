namespace ExCSS
{
    internal sealed class ClipPathProperty : Property
    {
        private static readonly IValueConverter StyleConverter = new ClipPathValueConverter().OrDefault();

        internal ClipPathProperty()
            : base(PropertyNames.ClipPath, PropertyFlags.Animatable)
        {
        }

        internal override IValueConverter Converter => StyleConverter;
    }
}
