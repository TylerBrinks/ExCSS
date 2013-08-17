using System.Text;

namespace ExCSS.Model
{
    internal class CompoundSelector : Selectors
    {
        internal static CompoundSelector Create(params SimpleSelector[] selectors)
        {
            var compound = new CompoundSelector();

            foreach (var t in selectors)
            {
                compound.SelectorList.Add(t);
            }

            return compound;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var t in SelectorList)
            {
                sb.Append(t);
            }

            return sb.ToString();
        }
    }
}
