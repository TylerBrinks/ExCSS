using System;
using System.Collections.Generic;

namespace ExCSS
{
    public sealed class AttributeSelectorFactory
    {
        private static readonly Lazy<AttributeSelectorFactory> Lazy =
            new Lazy<AttributeSelectorFactory>(() => new AttributeSelectorFactory());

        internal static AttributeSelectorFactory Instance => Lazy.Value;

        private AttributeSelectorFactory()
        {
        }

        public delegate ISelector Creator(string name, string value, string prefix);

        private readonly Dictionary<string, Creator> _creators = new Dictionary<string, Creator>
        {
            {Combinators.Exactly, SimpleSelector.AttrMatch},
            {Combinators.InList, SimpleSelector.AttrList},
            {Combinators.InToken, SimpleSelector.AttrHyphen},
            {Combinators.Begins, SimpleSelector.AttrBegins},
            {Combinators.Ends, SimpleSelector.AttrEnds},
            {Combinators.InText, SimpleSelector.AttrContains},
            {Combinators.Unlike, SimpleSelector.AttrNotMatch}
        };

        public ISelector Create(string combinator, string name, string value, string prefix)
        {
            return _creators.TryGetValue(combinator, out Creator creator)
                ? creator.Invoke(name, value, prefix)
                : CreateDefault(name, value);
        }

        private ISelector CreateDefault(string name, string value)
        {
            return SimpleSelector.AttrAvailable(name, value);
        }
    }
}