using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ExCSS.Model.Factories;
using ExCSS.Model.Values;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class StyleDeclaration : IList<Property>
    {
        private readonly List<Property> _properties;
        private readonly Func<string> _getter;
        private readonly Action<string> _setter;
        private bool _blocking;

        public StyleDeclaration()
        {
            var text = string.Empty;
            _getter = () => text;
            _setter = value => text = value;
            _properties = new List<Property>();
        }

        public string Value
        {
            get { return _getter(); }
            set
            {
                Update(value);
                _setter(value);
            }
        }

        public Property this[int index]
        {
            get { return _properties[index]; }
            set { _properties[index] = value; }
        }

        internal string RemoveProperty(string propertyName)
        {
            for (var i = 0; i < _properties.Count; i++)
            {
                if (!_properties[i].Name.Equals(propertyName))
                {
                    continue;
                }

                var value = _properties[i].Term;

                _properties.RemoveAt(i);
                Propagate();

                return value.ToString();
            }

            return null;
        }

        internal string GetPropertyPriority(string propertyName)
        {
            for (var i = 0; i < _properties.Count; i++)
            {
                if (_properties[i].Name.Equals(propertyName))
                {
                    return _properties[i].Important ? "important" : null;
                }
            }

            return null;
        }

        internal string GetPropertyValue(string propertyName)
        {
            for (var i = 0; i < _properties.Count; i++)
            {
                if (_properties[i].Name.Equals(propertyName))
                {
                    return _properties[i].Term.ToString();
                }
            }

            return null;
        }

        internal StyleDeclaration SetProperty(string propertyName, string propertyValue)
        {
            //_properties.Add(CssParser.ParseDeclaration(propertyName + ":" + propertyValue));
            //TODO
            Propagate();
            return this;
        }

        public List<Property> Properties
        {
            get { return _properties; }
        }

        internal void Update(string value)
        {
            if (_blocking)
            {
                return;
            }

            Lexer lexer;
            var rules = RuleFactory.ParseDeclarations(value ?? string.Empty, out lexer)._properties;

            //TODO: what to do with the temp lexer errors?

            _properties.Clear();
            _properties.AddRange(rules);
        }

        private void Propagate()
        {
            _blocking = true;
            _setter(ToString());
            _blocking = false;
        }

        public override string ToString()
        {
            var builder = new StringBuilder();

            foreach (var t in _properties)
            {
                builder.Append(t).Append(';');
            }

            return builder.ToString();
        }
        
        public IEnumerator<Property> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_properties).GetEnumerator();
        }

        public void Add(Property item)
        {
            _properties.Add(item);
        }

        public void Clear()
        {
            _properties.Clear();
        }

        public bool Contains(Property item)
        {
            return _properties.Contains(item);
        }

        public void CopyTo(Property[] array, int arrayIndex)
        {
            _properties.CopyTo(array, arrayIndex);
        }

        public bool Remove(Property item)
        {
            return _properties.Remove(item);
        }
 
        public int IndexOf(Property item)
        {
            return _properties.IndexOf(item);
        }

        public void Insert(int index, Property item)
        {
            _properties.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _properties.RemoveAt(index);
        }

        public int Count { get { return _properties.Count; } }
        public bool IsReadOnly { get { return false; } }
    }
}