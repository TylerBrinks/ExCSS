
namespace ExCSS.Model
{
    internal sealed class CommentBlock : Block
    {
        private readonly static CommentBlock OpenBlock;
        private readonly static CommentBlock CloseBlock;

        static CommentBlock()
        {
            OpenBlock = new CommentBlock { Type = GrammarSegment.CommentOpen };
            CloseBlock = new CommentBlock { Type = GrammarSegment.CommentClose };
        }

        CommentBlock()
        {
        }

       
        public static CommentBlock Open
        {
            get { return OpenBlock; }
        }

        
        public static CommentBlock Close
        {
            get { return CloseBlock; }
        }

        public override string ToString()
        {
            return Type == GrammarSegment.CommentOpen ? "<!--" : "-->";
        }
    }
}
