
namespace ExCSS.Model
{
    internal class MatchBlock : Block
    {
        internal readonly static MatchBlock Include = new MatchBlock { Type = GrammarSegment.IncludeMatch };
        internal readonly static MatchBlock Dash = new MatchBlock { Type = GrammarSegment.DashMatch };
        internal readonly static Block Prefix = new MatchBlock { Type = GrammarSegment.PrefixMatch };
        internal readonly static Block Substring = new MatchBlock { Type = GrammarSegment.SubstringMatch };
        internal readonly static Block Suffix = new MatchBlock { Type = GrammarSegment.SuffixMatch };
        internal readonly static Block Not = new MatchBlock { Type = GrammarSegment.NegationMatch };

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
