using System;

namespace ExCSS.Model
{
    /// <summary>
    /// The bracket token that contains the opening or closing of a bracket.
    /// </summary>
    sealed class BracketBlock : Block
    {
        readonly static BracketBlock RoundOpen= new BracketBlock { Type = GrammarSegment.ParenOpen, _mirror = GrammarSegment.ParenClose };
        readonly static BracketBlock RoundClose= new BracketBlock { Type = GrammarSegment.ParenClose, _mirror = GrammarSegment.ParenOpen };
        readonly static BracketBlock CurlyOpen=new BracketBlock { Type = GrammarSegment.CurlyBraceOpen, _mirror = GrammarSegment.CurlyBracketClose };
        readonly static BracketBlock CurlyClose= new BracketBlock { Type = GrammarSegment.CurlyBracketClose, _mirror = GrammarSegment.CurlyBraceOpen };
        readonly static BracketBlock SquareOpen = new BracketBlock { Type = GrammarSegment.SquareBraceOpen, _mirror = GrammarSegment.SquareBracketClose };
        readonly static BracketBlock SquareClose= new BracketBlock { Type = GrammarSegment.SquareBracketClose, _mirror = GrammarSegment.SquareBraceOpen };

        GrammarSegment _mirror;

        BracketBlock()
        {
        }
 
        public Char Open
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

        public Char Close
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

        public GrammarSegment Mirror
        {
            get { return _mirror; }
        }

        public static BracketBlock OpenRound
        {
            get { return RoundOpen; }
        }

        public static BracketBlock CloseRound
        {
            get { return RoundClose; }
        }

        public static BracketBlock OpenCurly
        {
            get { return CurlyOpen; }
        }

        public static BracketBlock CloseCurly
        {
            get { return CurlyClose; }
        }

        public static BracketBlock OpenSquare
        {
            get { return SquareOpen; }
        }

        public static BracketBlock CloseSquare
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
