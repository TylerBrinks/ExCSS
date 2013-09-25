using System.Text;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class MultipleSelectorList : SelectorList
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

        internal bool IsInvalid { get; set; }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat, int indentation = 0)
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
