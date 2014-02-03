using System.Text;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class AggregateSelectorList : SelectorList
    {
        internal static AggregateSelectorList Create(params SimpleSelector[] selectors)
        {
            var compound = new AggregateSelectorList();

            foreach (var t in selectors)
            {
                compound.Selectors.Add(t);
            }

            return compound;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            var builder = new StringBuilder();

            foreach (var selector in Selectors)
            {
                if (selector is IToString)
                {

                    builder.Append((selector as IToString).ToString(friendlyFormat, indentation + 1));
                }
                else
                {
                    builder.Append(selector.ToString(friendlyFormat, indentation + 1));                    
                }
            }

            return builder.ToString();
        }
    }
}
