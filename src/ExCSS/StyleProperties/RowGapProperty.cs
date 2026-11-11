namespace ExCSS
{
    internal sealed class RowGapProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.GapConverter.OrGlobalValue();

        internal RowGapProperty()
            : base(PropertyNames.RowGap, PropertyFlags.Animatable)
        { }

        internal override IValueConverter Converter => StyleConverter;
    }
}
