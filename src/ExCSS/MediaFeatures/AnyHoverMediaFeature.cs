namespace ExCSS
{
    internal sealed class AnyHoverMediaFeature : MediaFeature
    {
        public AnyHoverMediaFeature() : base(FeatureNames.AnyHover)
        {
        }

        internal override IValueConverter Converter => Converters.Any;
    }
}
