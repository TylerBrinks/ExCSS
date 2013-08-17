
namespace ExCSS.Model
{
    internal sealed class MatchBlock : Block
    {
        private readonly static MatchBlock IncludeBlock;
        private readonly static MatchBlock DashBlock;
        private readonly static Block PrefixBlock;
        private readonly static Block SubstringBlock;
        private readonly static Block SuffixBlock;
        private readonly static Block NotBlock;

        static MatchBlock()
        {
            IncludeBlock = new MatchBlock { Type = GrammarSegment.IncludeMatch };
            DashBlock = new MatchBlock { Type = GrammarSegment.DashMatch };
            PrefixBlock = new MatchBlock { Type = GrammarSegment.PrefixMatch };
            SubstringBlock = new MatchBlock { Type = GrammarSegment.SubstringMatch };
            SuffixBlock = new MatchBlock { Type = GrammarSegment.SuffixMatch };
            NotBlock = new MatchBlock { Type = GrammarSegment.NegationMatch };
        }
        
        MatchBlock()
        {
        }
        
        public static MatchBlock Include
        {
            get { return IncludeBlock; }
        }
        
        public static MatchBlock Dash
        {
            get { return DashBlock; }
        }
        
        public static Block Prefix
        {
            get { return PrefixBlock; }
        }

        
        public static Block Substring
        {
            get { return SubstringBlock; }
        }

        
        public static Block Suffix
        {
            get { return SuffixBlock; }
        }

        
        public static Block Not
        {
            get { return NotBlock; }
        }

        
        public override string ToString()
        {
            switch (Type)
            {
                case GrammarSegment.SubstringMatch:
                    return "*=";

                case GrammarSegment.SuffixMatch:
                    return "$=";

                case GrammarSegment.PrefixMatch:
                    return "^=";

                case GrammarSegment.IncludeMatch:
                    return "~=";

                case GrammarSegment.DashMatch:
                    return "|=";

                case GrammarSegment.NegationMatch:
                    return "!=";
            }

            return string.Empty;
        }
    }
}
