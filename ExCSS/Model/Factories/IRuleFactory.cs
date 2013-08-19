using System.Collections.Generic;
using ExCSS.Model.TextBlocks;

namespace ExCSS.Model.Factories
{
    internal interface IRuleFactory
    {
        void Parse(IEnumerator<Block> reader);
    }
}