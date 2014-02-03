using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class ComplexSelector : SimpleSelector, IEnumerable<CombinatorSelector>
    {
        private readonly List<CombinatorSelector> _selectors;

        public ComplexSelector()
        {
            _selectors = new List<CombinatorSelector>();
        }

        public ComplexSelector AppendSelector(SimpleSelector selector, Combinator combinator)
        {
            if (IsReady)
            {
                throw new InvalidOperationException("Last selector already added");
            }

            _selectors.Add(new CombinatorSelector(selector, combinator));
            return this;
        }

        public IEnumerator<CombinatorSelector> GetEnumerator()
        {
            return _selectors.GetEnumerator();
        }
       
        internal void ConcludeSelector(SimpleSelector selector)
        {
            if (IsReady)
            {
                throw new InvalidOperationException("Last selector already added.");
            }

            _selectors.Add(new CombinatorSelector { Selector = selector });
            IsReady = true;
        }

        internal ComplexSelector ClearSelectors()
        {
            IsReady = false;
            _selectors.Clear();
            return this;
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

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_selectors).GetEnumerator();
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public new string ToString(bool friendlyFormat, int indentation = 0)
        {
            var builder = new StringBuilder();

            if (_selectors.Count <= 0)
            {
                return builder.ToString();
            }

            var n = _selectors.Count - 1;

            for (var i = 0; i < n; i++)
            {
                builder.Append(_selectors[i].Selector);
                builder.Append(_selectors[i].Character);
            }

            builder.Append(_selectors[n].Selector);

            return builder.ToString();
        }
    }
}
