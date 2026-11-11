namespace ExCSS
{
    internal sealed class PrefersReducedTransparencyMediaFeature : MediaFeature
    {
        public PrefersReducedTransparencyMediaFeature() : base(FeatureNames.PrefersReducedTransparency)
        {
        }

        internal override IValueConverter Converter => Converters.Any;
    }
}
