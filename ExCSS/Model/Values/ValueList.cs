using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExCSS.Model
{
    internal class ValueList : Value
    {
        private readonly List<Value> _items;

        internal ValueList()
        {
            _items = new List<Value>();
            RuleValueType = RuleValueType.ValueList;
        }

        
        internal ValueList(List<Value> items)
        {
            _items = items;
            RuleValueType = RuleValueType.ValueList;
        }

        public int Length
        {
            get { return _items.Count; }
        }

        
        [IndexerName("ListItems")]
        public Value this[int index]
        {
            get { return index >= 0 && index < _items.Count ? _items[index] : null; }
        }

        public Value Item(int index)
        {
            return this[index];
        }
        
        public override string ToString()
        {
            var values = new String[_items.Count];

            for (var i = 0; i < _items.Count; i++)
            {
                values[i] = _items[i].ToString();
            }

            return String.Join(" ", values);
        }
    }
}
