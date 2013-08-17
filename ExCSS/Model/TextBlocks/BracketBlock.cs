
namespace ExCSS.Model
{
    internal sealed class BracketBlock : Block
    {
        private readonly static BracketBlock RoundOpen= new BracketBlock { Type = GrammarSegment.ParenOpen, _mirror = GrammarSegment.ParenClose };
        private readonly static BracketBlock RoundClose = new BracketBlock { Type = GrammarSegment.ParenClose, _mirror = GrammarSegment.ParenOpen };
        private readonly static BracketBlock CurlyOpen = new BracketBlock { Type = GrammarSegment.CurlyBraceOpen, _mirror = GrammarSegment.CurlyBracketClose };
        private readonly static BracketBlock CurlyClose = new BracketBlock { Type = GrammarSegment.CurlyBracketClose, _mirror = GrammarSegment.CurlyBraceOpen };
        private readonly static BracketBlock SquareOpen = new BracketBlock { Type = GrammarSegment.SquareBraceOpen, _mirror = GrammarSegment.SquareBracketClose };
        private readonly static BracketBlock SquareClose = new BracketBlock { Type = GrammarSegment.SquareBracketClose, _mirror = GrammarSegment.SquareBraceOpen };

        private GrammarSegment _mirror;

        BracketBlock()
        {
        }
 
        internal char Open
        {
            get
            {
                switch (Type)
                {
                    case GrammarSegment.ParenOpen:
                        return '(';
                       
                    case GrammarSegment.SquareBraceOpen:
                        return '[';
                       
                    default:
                        return '{';
                       
                }
            }
        }

        internal char Close
        {
            get
            {
                switch (Type)
                {
                    case GrammarSegment.ParenOpen:
                        return ')';
                      
                    case GrammarSegment.SquareBraceOpen:
                        return ']';
                       
                    default:
                        return '}';
                      
                }
            }
        }

        internal GrammarSegment Mirror
        {
            get { return _mirror; }
        }

        internal static BracketBlock OpenRound
        {
            get { return RoundOpen; }
        }

        internal static BracketBlock CloseRound
        {
            get { return RoundClose; }
        }

        internal static BracketBlock OpenCurly
        {
            get { return CurlyOpen; }
        }

        internal static BracketBlock CloseCurly
        {
            get { return CurlyClose; }
        }

        internal static BracketBlock OpenSquare
        {
            get { return SquareOpen; }
        }

        internal static BracketBlock CloseSquare
        {
            get { return SquareClose; }
        }

        public override string ToString()
        {
            switch (Type)
            {
                case GrammarSegment.CurlyBraceOpen:
                    return "{";

                case GrammarSegment.CurlyBracketClose:
                    return "}";

                case GrammarSegment.ParenClose:
                    return ")";

                case GrammarSegment.ParenOpen:
                    return "(";

                case GrammarSegment.SquareBracketClose:
                    return "]";

                case GrammarSegment.SquareBraceOpen:
                    return "[";
            }

            return string.Empty;
        }
    }
}
