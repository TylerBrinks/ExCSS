using System;

namespace ExCSS
{
    public struct CombinatorSelector
    {
        public SimpleSelector Selector;
        public Combinator Delimiter;

        public CombinatorSelector(SimpleSelector selector, Combinator delimiter)
        {
            this.Selector = selector;
            this.Delimiter = delimiter;
        }

        public char Char
        {
            get{
                switch (Delimiter)
                {
                    case Combinator.Child:
                        return Specification.GreaterThan;

                    case Combinator.AdjacentSibling:
                        return Specification.PlusSign;

                    case Combinator.Descendent:
                        return Specification.Space;

                    case Combinator.Sibling:
                        return Specification.Tilde;

                    default:
                        throw new NotImplementedException("Unknown combinator: " + Delimiter);
                }
            }
        }
    }

}

