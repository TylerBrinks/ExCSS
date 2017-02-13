using System.IO;

namespace ExCSS
{
    internal sealed class UnknownSelector : StylesheetNode, ISelector
    {
        public Priority Specifity => Priority.Zero;

        public string Text => this.ToCss();

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            writer.Write(StylesheetText?.Text);
        }
    }
}