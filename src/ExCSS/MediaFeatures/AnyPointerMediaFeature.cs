namespace ExCSS
{
    internal sealed class AnyPointerMediaFeature : MediaFeature
    {
        public AnyPointerMediaFeature() : base(FeatureNames.AnyPointer)
        {
        }

        internal override IValueConverter Converter => Converters.Any;
    }
}
