using System;

namespace ExCSS.Model
{
    internal abstract class Block
    {
        internal GrammarSegment Type { get;set; }

        internal static PipeBlock Column
        {
            get { return PipeBlock.Token; }
        }

        internal static DelimiterBlock Delim(Char c)
        {
            return new DelimiterBlock(c);
        }

        internal static NumericBlock Number(string value)
        {
            return new NumericBlock(value);
        }

        internal static RangeBlock Range(string start, string end)
        {
            return new RangeBlock().SetRange(start, end);
        }
    }
}
