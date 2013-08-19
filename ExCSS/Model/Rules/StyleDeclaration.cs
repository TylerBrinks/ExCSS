using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ExCSS.Model;
using ExCSS.Model.Factories;

namespace ExCSS
{
    public sealed class StyleDeclaration : IEnumerable<Property>
    {
        private readonly List<Property> _properties;
        private readonly Func<string> _getter;
        private readonly Action<string> _setter;
        private bool _blocking;

        internal StyleDeclaration()
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

        public int Length
        {
            get { return _properties.Count; }
        }

        public string this[int index]
        {
            get { return index >= 0 && index < Length ? _properties[index].Name : null; }
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

            var rules = RuleFactory.ParseDeclarations(value ?? string.Empty)._properties;
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
            var sb = new StringBuilder();

            foreach (var t in _properties)
            {
                sb.Append(t).Append(';');
            }

            return sb.ToString();
        }
        
        public IEnumerator<Property> GetEnumerator()
        {
            return _properties.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_properties).GetEnumerator();
        }
    }
}