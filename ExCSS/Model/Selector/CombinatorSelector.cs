using System;
using ExCSS.Model;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public struct CombinatorSelector
    {
        public SimpleSelector Selector;
        public Combinator Delimiter;

        public CombinatorSelector(SimpleSelector selector, Combinator delimiter)
        {
            Selector = selector;
            Delimiter = delimiter;
        }

        public char Character
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

                    case Combinator.Namespace:
                        return Specification.Pipe;

                    default:
                        throw new NotImplementedException("Unknown combinator: " + Delimiter);
                }
            }
        }
    }

}

