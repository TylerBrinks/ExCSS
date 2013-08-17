using System;
using System.Collections.Generic;

namespace ExCSS.Model
{
    internal sealed class RangeBlock : Block
    {
        private String[] _range;

        public RangeBlock()
        {
            Type = GrammarSegment.Range;
        }

        public bool IsEmpty
        {
            get { return _range == null || _range.Length == 0; }
        }

        public String[] SelectedRange
        {
            get { return _range; }
        }

        public RangeBlock SetRange(string start, string end)
        {
            var i = int.Parse(start, System.Globalization.NumberStyles.HexNumber);

            if (i <= Specification.MaxPoint)
            {
                if (end == null)
                {
                    _range = new [] { char.ConvertFromUtf32(i) };
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

                    _range = list.ToArray();
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

            if (_range.Length == 1)
            {
                return "#" + char.ConvertToUtf32(_range[0], 0).ToString("x");
            }

            return "#" + char.ConvertToUtf32(_range[0], 0).ToString("x") + "-#" + 
                char.ConvertToUtf32(_range[_range.Length - 1], 0).ToString("x");
        }
    }
}
