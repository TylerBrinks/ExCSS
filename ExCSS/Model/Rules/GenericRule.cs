using System;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents an unknown rule.
    /// </summary>
    sealed class GenericRule : Ruleset
    {
        private string _text;

        internal void SetText(string text)
        {
            _text = text;
        }

        public override string ToString()
        {
            return _text;
        }
    }
}
