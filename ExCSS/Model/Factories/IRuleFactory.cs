using System.Collections.Generic;

namespace ExCSS.Model.Factories
{
    internal interface IRuleFactory
    {
        void Parse(IEnumerator<Block> reader);
    }
}