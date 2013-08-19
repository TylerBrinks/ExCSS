using System.Collections.Generic;

namespace ExCSS.Model.TextBlocks
{
    internal class RangeBlock : Block
    {
        public RangeBlock()
        {
            GrammarSegment = GrammarSegment.Range;
        }

        internal bool IsEmpty
        {
            get { return SelectedRange == null || SelectedRange.Length == 0; }
        }

        internal string[] SelectedRange { get; private set; }

        internal RangeBlock SetRange(string start, string end)
        {
            var i = int.Parse(start, System.Globalization.NumberStyles.HexNumber);

            if (i <= Specification.MaxPoint)
            {
                if (end == null)
                {
                    SelectedRange = new [] { char.ConvertFromUtf32(i) };
                }
                else
                {
                    var list = new List<string>();
                    var f = int.Parse(end, System.Globalization.NumberStyles.HexNumber);

                    if (f > Specification.MaxPoint)
                    {
                        f = Specification.MaxPoint;
                    }

                    for (; i <= f; i++)
                    {
                        list.Add(char.ConvertFromUtf32(i));
                    }

                    SelectedRange = list.ToArray();
                }
            }

            return this;
        }

        public override string ToString()
        {
            if (IsEmpty)
            {
                return string.Empty;
            }

            if (SelectedRange.Length == 1)
            {
                return "#" + char.ConvertToUtf32(SelectedRange[0], 0).ToString("x");
            }

            return "#" + char.ConvertToUtf32(SelectedRange[0], 0).ToString("x") + "-#" + 
                char.ConvertToUtf32(SelectedRange[SelectedRange.Length - 1], 0).ToString("x");
        }
    }
}
