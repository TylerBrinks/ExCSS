using System.Text;

namespace ExCSS
{
    internal class MultipleSelectorList : SelectorList
    {
        internal static MultipleSelectorList Create(params SimpleSelector[] selectors)
        {
            var multiple = new MultipleSelectorList();

            foreach (var selector in selectors)
            {
                multiple.Selectors.Add(selector);
            }

            return multiple;
        }

        public bool IsInvalid { get; internal set;}

        public override string ToString()
        {
            var builder = new StringBuilder();

            if (Selectors.Count > 0)
            {
                builder.Append(Selectors[0]);

                for (var i = 1; i < Selectors.Count; i++)
                {
                    builder.Append(',').Append(Selectors[i]);
                }
            }

            return builder.ToString();
        }
    }
}
