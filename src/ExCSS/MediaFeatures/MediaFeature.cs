using System.IO;

namespace ExCSS
{
    public abstract class MediaFeature : StylesheetNode, IMediaFeature
    {
        private TokenValue _tokenValue;

        internal MediaFeature(string name)
        {
            Name = name;
            IsMinimum = name.StartsWith("min-");
            IsMaximum = name.StartsWith("max-");
        }

        internal abstract IValueConverter Converter { get; }

        public bool IsMinimum { get; }

        public bool IsMaximum { get; }

        public string Name { get; }

        public string Value => HasValue ? _tokenValue.Text : string.Empty;

        public bool HasValue => _tokenValue != null && _tokenValue.Count > 0;

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            var value = HasValue ? Value : null;
            writer.Write(formatter.Constraint(Name, value));
        }

        internal bool TrySetValue(TokenValue tokenValue)
        {
            bool result;

            if (tokenValue == null)
                result = !IsMinimum && !IsMaximum && Converter.ConvertDefault() != null;
            else
                result = Converter.Convert(tokenValue) != null;

            if (result) _tokenValue = tokenValue;

            return result;
        }
    }
}