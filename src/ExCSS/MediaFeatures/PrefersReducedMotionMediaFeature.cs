namespace ExCSS
{
    internal sealed class PrefersReducedMotionMediaFeature : MediaFeature
    {
        public PrefersReducedMotionMediaFeature() : base(FeatureNames.PrefersReducedMotion)
        {
        }

        internal override IValueConverter Converter => Converters.Any;
    }
}
