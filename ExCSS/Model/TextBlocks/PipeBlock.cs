
namespace ExCSS.Model
{
    internal sealed class PipeBlock : Block
    {
        private readonly static PipeBlock TokenBlock;

        static PipeBlock()
        {
            TokenBlock = new PipeBlock();
        }

        PipeBlock()
        {
            Type = GrammarSegment.Column;
        }

        internal static PipeBlock Token
        {
            get { return TokenBlock; }
        }

        public override string ToString()
        {
            return "||";
        }
    }
}
