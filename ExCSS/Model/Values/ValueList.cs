using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a list of values in the CSS context.
    /// </summary>
    sealed class ValueList : Value
    {
        #region Members

        List<Value> _items;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new CSS value list.
        /// </summary>
        internal ValueList()
        {
            _items = new List<Value>();
            _type = RuleValueType.ValueList;
        }

        /// <summary>
        /// Creates a new CSS value list.
        /// </summary>
        /// <param name="items">The list of values to consider.</param>
        internal ValueList(List<Value> items)
        {
            _items = items;
            _type = RuleValueType.ValueList;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the number of CSSValues in the list.
        /// </summary>
        public int Length
        {
            get { return _items.Count; }
        }

        /// <summary>
        /// Used to retrieve a Value by ordinal index. The order in this collection represents the order of the values in the CSS style property.
        /// </summary>
        /// <param name="index">If index is greater than or equal to the number of values in the list, this returns null.</param>
        /// <returns>The value at the given index or null.</returns>
        [IndexerName("ListItems")]
        public Value this[int index]
        {
            get { return index >= 0 && index < _items.Count ? _items[index] : null; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Used to retrieve a Value by ordinal index. The order in this collection represents the order of the values in the CSS style property.
        /// </summary>
        /// <param name="index">If index is greater than or equal to the number of values in the list, this returns null.</param>
        /// <returns>The value at the given index or null.</returns>
        public Value Item(int index)
        {
            return this[index];
        }

        /// <summary>
        /// Returns a CSS code representation of the stylesheet.
        /// </summary>
        /// <returns>A string that contains the code.</returns>
        public override string ToString()
        {
            var values = new String[_items.Count];

            for (int i = 0; i < _items.Count; i++)
                values[i] = _items[i].ToString();

            return String.Join(" ", values);
        }

        #endregion
    }
}
