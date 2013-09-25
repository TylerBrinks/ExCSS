using System.Text;
using System.Collections.Generic;
using ExCSS.Model;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class ComplexSelector : SimpleSelector
    {
        private readonly List<CombinatorSelector> _selectors;
        
        public ComplexSelector()
        {
            _selectors = new List<CombinatorSelector>();
        }

        public int Length
        {
            get { return _selectors.Count; }
        }

        internal bool IsReady
        {
            get;
            private set;
        }

        internal ComplexSelector ConcludeSelector(SimpleSelector selector)
        {
            if (!IsReady)
            {
                _selectors.Add(new CombinatorSelector { Selector = selector});
                IsReady = true;
            }

            return this;
        }

        internal ComplexSelector AppendSelector(SimpleSelector selector, Combinator combinator)
        {
            if (IsReady)
            {
                return this;
            }

            char delim;

            switch (combinator)
            {
                case Combinator.Child:
                    delim = Specification.GreaterThan;
                    break;

                case Combinator.AdjacentSibling:
                    delim = Specification.PlusSign;
                    break;

                case Combinator.Descendent:
                    delim = Specification.Space;
                    break;

                case Combinator.Sibling:
                    delim = Specification.Tilde;
                    break;

                default:
                    return this;
            }

            _selectors.Add(new CombinatorSelector
                {
                    Selector = selector,
                    Delimiter = delim
                });
            return this;
        }

        internal ComplexSelector ClearSelectors()
        {
            IsReady = false;
            _selectors.Clear();
            return this;
        }

        internal struct CombinatorSelector
        {
            public char Delimiter;
            public SimpleSelector Selector;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat, int indentation = 0)
        {
            var builder = new StringBuilder();

            if (_selectors.Count > 0)
            {
                var n = _selectors.Count - 1;

                for (var i = 0; i < n; i++)
                {
                    builder.Append(_selectors[i].Selector).Append(_selectors[i].Delimiter);
                }

                builder.Append(_selectors[n].Selector);
            }

            return builder.ToString();
        }
    }
}
