using System;
using System.IO;
using System.Linq;

namespace ExCSS
{
    internal sealed class SupportsRule : ConditionRule, ISupportsRule
    {
        internal SupportsRule(StylesheetParser parser)
            : base(RuleType.Supports, parser)
        {
        }

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            var rules = formatter.Block(Rules);
            writer.Write(formatter.Rule("@supports", ConditionText, rules));
        }


        public string ConditionText
        {
            get { return Condition.ToCss(); }
            set
            {
                var condition = Parser.ParseCondition(value);

                if (condition == null)
                {
                    throw new ParseException("Unable to parse condition");
                }

                Condition = condition;
            }
        }

        public IConditionFunction Condition
        {
            get { return Children.OfType<IConditionFunction>().FirstOrDefault() ?? new EmptyCondition(); }
            set
            {
                if (value == null)
                {
                    return;
                }

                RemoveChild(Condition);
                AppendChild(value);
            }
        }
    }
}