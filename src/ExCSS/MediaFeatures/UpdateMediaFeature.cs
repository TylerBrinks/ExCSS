namespace ExCSS
{
    internal sealed class UpdateMediaFeature : MediaFeature
    {
        public UpdateMediaFeature() : base(FeatureNames.Update)
        {
        }

        internal override IValueConverter Converter => Converters.Any;
    }
}
