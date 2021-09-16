using System.IO;

namespace ExCSS
{
    internal sealed class CompoundSelector : Selectors, ISelector
    {
        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            foreach (var selector in _selectors) writer.Write(selector.Text);
        }
    }
}