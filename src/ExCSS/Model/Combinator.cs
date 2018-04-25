﻿
namespace ExCSS
{
    public abstract class Combinator
    {
        public virtual ISelector Change(ISelector selector)
        {
            return selector;
        }
        public static readonly Combinator Child = new ChildCombinator();
        public static readonly Combinator Deep = new DeepCombinator();
        public static readonly Combinator Descendent = new DescendentCombinator();
        public static readonly Combinator AdjacentSibling = new AdjacentSiblingCombinator();
        public static readonly Combinator Sibling = new SiblingCombinator();
        public static readonly Combinator Namespace = new NamespaceCombinator();
        public static readonly Combinator Column = new ColumnCombinator();
        public string Delimiter { get; protected set; }
        private sealed class ChildCombinator : Combinator
        {
            public ChildCombinator()
            {
                Delimiter = Combinators.Child;
            }
        }
        private sealed class DeepCombinator : Combinator
        {
            public DeepCombinator()
            {
                Delimiter = Combinators.Deep;
            }
        }
        private sealed class DescendentCombinator : Combinator
        {
            public DescendentCombinator()
            {
                Delimiter = Combinators.Descendent;
            }
        }
        private sealed class AdjacentSiblingCombinator : Combinator
        {
            public AdjacentSiblingCombinator()
            {
                Delimiter = Combinators.Adjacent;
            }
        }
        private sealed class SiblingCombinator : Combinator
        {
            public SiblingCombinator()
            {
                Delimiter = Combinators.Sibling;
            }
        }
        private sealed class NamespaceCombinator : Combinator
        {
            public NamespaceCombinator()
            {
                Delimiter = Combinators.Pipe;
            }
            public override ISelector Change(ISelector selector)
            {
                var prefix = selector.Text;
                return new SimpleSelector( /*el => el.MatchesCssNamespace(prefix),*/ Priority.Zero, prefix);
            }
        }
        private sealed class ColumnCombinator : Combinator
        {
            public ColumnCombinator()
            {
                Delimiter = Combinators.Column;
            }
        }
    }
}