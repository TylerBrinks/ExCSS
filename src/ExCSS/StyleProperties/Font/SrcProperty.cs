namespace ExCSS
{
    public sealed class SrcProperty : Property
    {
        public SrcProperty()
            : base(PropertyNames.Src)
        {
        }

        internal override IValueConverter Converter => Converters.Any;
    }
}
