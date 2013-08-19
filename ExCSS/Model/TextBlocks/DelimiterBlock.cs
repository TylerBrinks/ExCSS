using System;

namespace ExCSS.Model
{
    internal sealed class DelimiterBlock : CharacterBlock
    {
        internal DelimiterBlock()
        {
            GrammarSegment = GrammarSegment.Delimiter;
        }

        internal DelimiterBlock(Char value) : base(value)
        {
            GrammarSegment = GrammarSegment.Delimiter;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
