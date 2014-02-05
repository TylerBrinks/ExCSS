using System.Collections;
using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public abstract class SelectorList : SimpleSelector, IEnumerable<SimpleSelector>
    {
        protected List<SimpleSelector> Selectors;

        protected SelectorList()
        {
            Selectors = new List<SimpleSelector>();
        }

        public int Length 
        {
            get { return Selectors.Count; } 
        }

        public SimpleSelector this[int index]
        {
            get { return Selectors[index]; }
            set { Selectors[index] = value; }
        }

        public SelectorList AppendSelector(SimpleSelector selector)
        {
            Selectors.Add(selector);
            return this;
        }

        public SelectorList RemoveSelector(SimpleSelector selector)
        {
            Selectors.Remove(selector);
            return this;
        }

        public SelectorList ClearSelectors()
        {
            Selectors.Clear();
            return this;
        }

        public IEnumerator<SimpleSelector> GetEnumerator()
        {
            return Selectors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Selectors).GetEnumerator();
        }

        public override abstract string ToString(bool friendlyFormat, int indentation = 0);
    }
}
