using System;

namespace ExCSS.Model
{
    internal sealed class DelimiterBlock : CharacterBlock
    {
        public DelimiterBlock()
        {
            Type = GrammarSegment.Delimiter;
        }

        public DelimiterBlock(Char value) : base(value)
        {
            Type = GrammarSegment.Delimiter;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}
