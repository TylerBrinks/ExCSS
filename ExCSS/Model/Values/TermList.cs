using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

// ReSharper disable CheckNamespace
using System.Text;

namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class TermList : Term
    {
        private readonly List<GrammarSegment> _separator = new List<GrammarSegment>();
        private readonly List<Term> _items = new List<Term>();

        public TermList()
        {
            RuleValueType = RuleValueType.ValueList;
        }

        internal void AddTerm(GrammarSegment sep, Term term)
        {
            if (sep != GrammarSegment.Whitespace && sep != GrammarSegment.Comma)
                throw new NotSupportedException("Only support comma and whitespace separator");
            _separator.Add(sep);
            _items.Add(term);
        }

        public int Length
        {
            get { return _items.Count; }
        }

        [IndexerName("ListItems")]
        public Term this [int index]
        {
            get { return index >= 0 && index < _items.Count ? _items[index] : null; }
        }

        public Term Item(int index)
        {
            return this[index];
        }

        public override string ToString()
        {
            var s = new StringBuilder();

            for (var i = 0; i < _items.Count; i++)
            {
                var sep = _separator[i];
                if (sep == GrammarSegment.Whitespace && i > 0)
                    s.Append(" ");
                if (sep == GrammarSegment.Comma)
                    s.Append(",");

                s.Append(_items[i]);
            }

            return s.ToString();
        }
    }
}
