using System;
using System.Text;
using System.Collections.Generic;
using ExCSS.Model;

// ReSharper disable CheckNamespace
using System.Collections;


namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class ComplexSelector : SimpleSelector, IEnumerable<CombinatorSelector>
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

        internal void ConcludeSelector(SimpleSelector selector)
        {
            if (IsReady)
                throw new InvalidOperationException("Last selector already added");

            _selectors.Add(new CombinatorSelector { Selector = selector });
            IsReady = true;
        }

        public ComplexSelector AppendSelector(SimpleSelector selector, Combinator combinator)
        {
            if (IsReady)
                throw new InvalidOperationException("Last selector already added");
            _selectors.Add(new CombinatorSelector(selector, combinator));
            return this;
        }

        internal ComplexSelector ClearSelectors()
        {
            IsReady = false;
            _selectors.Clear();
            return this;
        }

        public IEnumerator<CombinatorSelector> GetEnumerator()
        {
            return _selectors.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_selectors).GetEnumerator();
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
                    builder.Append(_selectors[i].Selector);
                    builder.Append(_selectors[i].Char);
                }

                builder.Append(_selectors[n].Selector);
            }

            return builder.ToString();
        }
    }
}
