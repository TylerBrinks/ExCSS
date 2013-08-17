
namespace ExCSS.Model
{
    internal sealed class PipeBlock : Block
    {
        private readonly static PipeBlock tokenBlock;

        static PipeBlock()
        {
            tokenBlock = new PipeBlock();
        }

        PipeBlock()
        {
            Type = GrammarSegment.Column;
        }

        public static PipeBlock Token
        {
            get { return tokenBlock; }
        }

        public override string ToString()
        {
            return "||";
        }
    }
}
