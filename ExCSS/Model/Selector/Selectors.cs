using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ExCSS.Model
{
    internal abstract class Selectors : Selector, IEnumerable<Selector>
    {
        protected List<Selector> SelectorList;

        protected Selectors()
        {
            SelectorList = new List<Selector>();
        }

        public override int Specifity
        {
            get
            {
                return SelectorList.Sum(t => t.Specifity);
            }
        }

        public int Length 
        {
            get { return SelectorList.Count; } 
        }

        public Selector this[int index]
        {
            get { return SelectorList[index]; }
            set { SelectorList[index] = value; }
        }

        public Selectors AppendSelector(Selector selector)
        {
            SelectorList.Add(selector);
            return this;
        }

        public Selectors RemoveSelector(Selector selector)
        {
            SelectorList.Remove(selector);
            return this;
        }

        public Selectors ClearSelectors()
        {
            SelectorList.Clear();
            return this;
        }

        public IEnumerator<Selector> GetEnumerator()
        {
            return SelectorList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)SelectorList).GetEnumerator();
        }
    }
}
