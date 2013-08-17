using System;
using System.Globalization;

namespace ExCSS.Model
{
    internal sealed class UnitBlock : Block
    {
        private string _data;

        UnitBlock(GrammarSegment type)
        {
            Type = type;
        }

        public Single Data
        {
            get { return Single.Parse(_data, CultureInfo.InvariantCulture); }
        }

        public string Unit { get; private set; }

        public static UnitBlock Percentage(string value)
        {
            return new UnitBlock(GrammarSegment.Percentage) { _data = value, Unit = "%" };
        }

        public static UnitBlock Dimension(string value, string dimension)
        {
            return new UnitBlock(GrammarSegment.Dimension) { _data = value, Unit = dimension };
        }

        public override string ToString()
        {
            return _data + Unit;
        }
    }
}
