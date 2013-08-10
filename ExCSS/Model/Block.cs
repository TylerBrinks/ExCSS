using System;

namespace ExCSS.Model
{
    //[DebuggerStepThrough]
    abstract class Block
    {
       // protected GrammarSegment Type;

        public GrammarSegment Type
        {
            get;set;// { return _type; }
        }

        public static PipeBlock Column
        {
            get { return PipeBlock.Token; }
        }

        public static DelimBlock Delim(Char c)
        {
            return new DelimBlock(c);
        }

        public static NumericBlock Number(string value)
        {
            return new NumericBlock(value);
        }

        public static RangeBlock Range(string start, string end)
        {
            return new RangeBlock().SetRange(start, end);
        }
    }
}
