namespace ExCSS
{
    internal class GapProperty : ShorthandProperty
    {
        private static readonly IValueConverter StyleConverter = Converters.GapConverter;

        internal GapProperty()
            : base(PropertyNames.Gap, PropertyFlags.Animatable)
        { }

        internal override IValueConverter Converter => StyleConverter;
    }
}
