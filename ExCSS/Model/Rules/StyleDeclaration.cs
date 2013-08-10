using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace ExCSS.Model
{
    public sealed class StyleDeclaration : IEnumerable<Property>
    {
        #region Members

        List<Property> _rules;
        Ruleset _parent;
        Func<string> _getter;
        Action<String> _setter;
        bool _blocking;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new CSS style declaration. Here a local string
        /// variable is used to cache the text representation.
        /// </summary>
        internal StyleDeclaration()
        {
            string text = String.Empty;
            _getter = () => text;
            _setter = value => text = value;
            _rules = new List<Property>();
        }

        /// <summary>
        /// Creates a new CSS style declaration with pre-defined getter
        /// and setter functions (use-case: HTML element).
        /// </summary>
        /// <param name="host">The element to host this representation.</param>
        //internal CSSStyleDeclaration(Element host)
        //{
        //    _getter = () => host.GetAttribute("style");
        //    _setter = value => host.SetAttribute("style", value);
        //    _rules = new List<Property>();
        //}

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the textual representation of the declaration block.
        /// </summary>
        //[DOM("cssText")]
        public string CssText
        {
            get { return _getter(); }
            set
            {
                Update(value);
                _setter(value);
            }
        }

        /// <summary>
        /// Gets the number of properties in the declaration.
        /// </summary>
        //[DOM("length")]
        public Int32 Length
        {
            get { return _rules.Count; }
        }

        /// <summary>
        /// Gets the containing Ruleset.
        /// </summary>
        //[DOM("parentRule")]
        public Ruleset ParentRule
        {
            get { return _parent; }
            internal set { _parent = value; }
        }

        /// <summary>
        /// Returns a property name.
        /// </summary>
        /// <param name="index">The index of the property to retrieve.</param>
        /// <returns>The name of the property at the given index.</returns>
        //[DOM("item")]
        public string this[Int32 index]
        {
            get { return index >= 0 && index < Length ? _rules[index].Name : null; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Returns the value deleted.
        /// </summary>
        /// <param name="propertyName">The name of the property to be removed.</param>
        /// <returns>The value of the deleted property.</returns>
        //[DOM("removeProperty")]
        public string RemoveProperty(string propertyName)
        {
            for (int i = 0; i < _rules.Count; i++)
            {
                if (_rules[i].Name.Equals(propertyName))
                {
                    var value = _rules[i].Value;
                    _rules.RemoveAt(i);
                    Propagate();
                    return value.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the optional priority, "important".
        /// </summary>
        /// <param name="propertyName">The name of the property to get the priority of.</param>
        /// <returns>A priority or null.</returns>
        //[DOM("getPropertyPriority")]
        public string GetPropertyPriority(string propertyName)
        {
            for (int i = 0; i < _rules.Count; i++)
            {
                if (_rules[i].Name.Equals(propertyName))
                    return _rules[i].Important ? "important" : null;
            }

            return null;
        }

        /// <summary>
        /// Returns the value of a property.
        /// </summary>
        /// <param name="propertyName">The name of the property to get the priority of.</param>
        /// <returns>A value or null if nothing has been set.</returns>
        //[DOM("getPropertyValue")]
        public string GetPropertyValue(string propertyName)
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                if (_rules[i].Name.Equals(propertyName))
                {
                    return _rules[i].Value.ToString();
                }
            }

            return null;
        }

        /// <summary>
        /// Sets a property with the given name and value.
        /// </summary>
        /// <param name="propertyName">The property's name.</param>
        /// <param name="propertyValue">The value of the property.</param>
        /// <returns>The current style declaration.</returns>
        //[DOM("setProperty")]
        public StyleDeclaration SetProperty(string propertyName, string propertyValue)
        {
            //_rules.Add(CssParser.ParseDeclaration(propertyName + ":" + propertyValue));
            //TODO
            Propagate();
            return this;
        }

        #endregion

        #region Internal properties

        /// <summary>
        /// Gets the list of CSS declarations.
        /// </summary>
        internal List<Property> List
        {
            get { return _rules; }
        }

        #endregion

        #region Internal methods

        /// <summary>
        /// Updates the CSSStyleDeclaration with the given value.
        /// </summary>
        /// <param name="value">The new value.</param>
        internal void Update(string value)
        {
            if (!_blocking)
            {
                var rules = Parser.ParseDeclarations(value ?? String.Empty)._rules;
                _rules.Clear();
                _rules.AddRange(rules);
            }
        }

        #endregion

        #region Helpers

        void Propagate()
        {
            _blocking = true;
            _setter(ToCss());
            _blocking = false;
        }

        #endregion

        #region string Representation

        /// <summary>
        /// Returns the CSS representation of the list of rules.
        /// </summary>
        /// <returns></returns>
        public string ToCss()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < _rules.Count; i++)
                sb.Append(_rules[i]).Append(';');

            return sb.ToString();
        }

        #endregion

        #region Interface implementation

        /// <summary>
        /// Returns an ienumerator that enumerates over all entries.
        /// </summary>
        /// <returns>The iterator.</returns>
        public IEnumerator<Property> GetEnumerator()
        {
            return _rules.GetEnumerator();
        }

        /// <summary>
        /// Returns a common ienumerator to enumerate all entries.
        /// </summary>
        /// <returns>The iterator.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_rules).GetEnumerator();
        }

        #endregion
    }
}