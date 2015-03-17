// ReSharper disable once CheckNamespace

using System;
using System.Linq;
using System.Text;
using ExCSS.Model;

namespace ExCSS
{
    public class GenericRule : AggregateRule, ISupportsDeclarations
    {
        private string _text;
        private bool _stopped;


        public StyleDeclaration Declarations { get; private set; }

        public GenericRule()
        {
            Declarations = new StyleDeclaration();
        }

        internal void SetInstruction(string text)
        {
            _text = text;
            _stopped = true;
        }

        internal void SetCondition(string text)
        {
            _text = text;
            _stopped = false;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public override string ToString(bool friendlyFormat, int indentation = 0)
        {
            if (_stopped)
            {
                return _text + ";";
            }
            var sb = new StringBuilder();
            sb.Append(_text).Append("{");
            sb.Append(Declarations.ToString(friendlyFormat, indentation));
            foreach (var rule in  RuleSets)
            {
                if (friendlyFormat)
                    sb.AppendLine();
                sb.Append(rule.ToString(friendlyFormat, indentation));
            }
            if (friendlyFormat)
                sb.AppendLine();
            sb.Append("}");
            return sb.ToString();
        }
    }
}
