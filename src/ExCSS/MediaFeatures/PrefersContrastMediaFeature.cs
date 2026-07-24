namespace ExCSS
{
    internal sealed class PrefersContrastMediaFeature : MediaFeature
    {
        public PrefersContrastMediaFeature() : base(FeatureNames.PrefersContrast)
        {
        }

        internal override IValueConverter Converter => Converters.Any;
    }
}
