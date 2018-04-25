using System.Collections;
using System.Collections.Generic;

namespace ExCSS
{
    public abstract class Selectors : StylesheetNode, IEnumerable<ISelector>
    {
        protected readonly List<ISelector> _selectors;

        protected Selectors()
        {
            _selectors = new List<ISelector>();
        }

        public Priority Specifity
        {
            get
            {
                var sum = new Priority();

                for (var i = 0; i < _selectors.Count; i++)
                {
                    sum += _selectors[i].Specifity;
                }

                return sum;
            }
        }

        public string Text => this.ToCss();
        public int Length => _selectors.Count;
        public ISelector this[int index]
        {
            get { return _selectors[index]; }
            set { _selectors[index] = value; }
        }

        public void Add(ISelector selector)
        {
            _selectors.Add(selector);
        }

        public void Remove(ISelector selector)
        {
            _selectors.Remove(selector);
        }

        public IEnumerator<ISelector> GetEnumerator()
        {
            return _selectors.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}