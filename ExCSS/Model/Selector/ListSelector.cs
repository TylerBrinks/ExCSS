using System.Text;

namespace ExCSS.Model
{
    internal class ListSelector : Selectors
    {
        internal static ListSelector Create(params Selector[] selectors)
        {
            var list = new ListSelector();

            foreach (var t in selectors)
            {
                list.SelectorList.Add(t);
            }

            return list;
        }

        public bool IsInvalid
        {
            get;
            internal set;
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (SelectorList.Count > 0)
            {
                sb.Append(SelectorList[0]);

                for (var i = 1; i < SelectorList.Count; i++)
                {
                    sb.Append(',').Append(SelectorList[i]);
                }
            }

            return sb.ToString();
        }

    }
}
