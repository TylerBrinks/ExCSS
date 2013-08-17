using System;

namespace ExCSS.Model
{
    internal sealed class DelimiterBlock : CharacterBlock
    {
        internal DelimiterBlock()
        {
            Type = GrammarSegment.Delimiter;
        }

        internal DelimiterBlock(Char value) : base(value)
        {
            Type = GrammarSegment.Delimiter;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
