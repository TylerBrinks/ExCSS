using System;
using System.Globalization;

namespace ExCSS.Model
{
    internal sealed class NumericBlock : Block
    {
        private readonly string _data;

        public NumericBlock(string number)
        {
            _data = number;
            Type = GrammarSegment.Number;
        }

        public Single Data
        {
            get { return Single.Parse(_data, CultureInfo.InvariantCulture); }
        }
        
        public override string ToString()
        {
            return _data;
        }
    }
}
