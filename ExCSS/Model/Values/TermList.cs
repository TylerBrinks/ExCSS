using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class TermList : Term
    {
        private readonly List<GrammarSegment> _separator = new List<GrammarSegment>();
        private readonly List<Term> _items = new List<Term>();

        public TermList()
        {
        }

        internal void AddTerm(Term term)
        {
            if (_items.Count != _separator.Count)
            {
                throw new NotSupportedException("Must call AddTerm AddSeparator in that order");
            }

            _items.Add(term);
        }

        internal void AddSeparator(GrammarSegment termSepertor)
        {
            if (_items.Count != _separator.Count + 1)
            {
                throw new NotSupportedException("Must call AddTerm AddSeparator in that order");
            }

            _separator.Add(termSepertor);
        }

        public int Length
        {
            get { return _items.Count; }
        }

        [IndexerName("ListItems")]
        public Term this [int index]
        {
            //return index >= 0 && index < _items.Count ? _items[index] : null; 
            get { return _items[index]; }
        }

        public Term Item(int index)
        {
            return this[index];
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            for (var i = 0; i < _items.Count; i++)
            {
                builder.Append(_items[i]);

                if (i == _separator.Count)
                    break;

                switch (_separator[i])
                {
                    case GrammarSegment.Whitespace:
                        builder.Append(" ");
                        break;

                    case GrammarSegment.Comma:
                        builder.Append(",");
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return builder.ToString();
        }
    }
}
