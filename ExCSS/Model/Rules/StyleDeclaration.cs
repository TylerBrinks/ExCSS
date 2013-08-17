using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ExCSS.Model.Factories;

namespace ExCSS.Model
{
    public sealed class StyleDeclaration : IEnumerable<Property>
    {
        private readonly List<Property> _rules;
        private readonly Func<string> _getter;
        private readonly Action<String> _setter;
        private bool _blocking;

        internal StyleDeclaration()
        {
            var text = string.Empty;
            _getter = () => text;
            _setter = value => text = value;
            _rules = new List<Property>();
        }

        public string CssText
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
            get { return _rules.Count; }
        }

        public string this[int index]
        {
            get { return index >= 0 && index < Length ? _rules[index].Name : null; }
        }

        public string RemoveProperty(string propertyName)
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                if (!_rules[i].Name.Equals(propertyName))
                {
                    continue;
                }

                var value = _rules[i].Value;

                _rules.RemoveAt(i);
                Propagate();

                return value.ToString();
            }

            return null;
        }

        
        public string GetPropertyPriority(string propertyName)
        {
            for (var i = 0; i < _rules.Count; i++)
            {
                if (_rules[i].Name.Equals(propertyName))
                {
                    return _rules[i].Important ? "important" : null;
                }
            }

            return null;
        }

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

        public StyleDeclaration SetProperty(string propertyName, string propertyValue)
        {
            //_rules.Add(CssParser.ParseDeclaration(propertyName + ":" + propertyValue));
            //TODO
            Propagate();
            return this;
        }

        internal List<Property> List
        {
            get { return _rules; }
        }

        internal void Update(string value)
        {
            if (_blocking)
            {
                return;
            }

            var rules = RuleFactory.ParseDeclarations(value ?? string.Empty)._rules;
            _rules.Clear();
            _rules.AddRange(rules);
        }

        private void Propagate()
        {
            _blocking = true;
            _setter(ToCss());
            _blocking = false;
        }

        public string ToCss()
        {
            var sb = new StringBuilder();

            foreach (var t in _rules)
            {
                sb.Append(t).Append(';');
            }

            return sb.ToString();
        }
        
        public IEnumerator<Property> GetEnumerator()
        {
            return _rules.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_rules).GetEnumerator();
        }
    }
}