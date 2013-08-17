using System;
using System.Collections.Generic;

namespace ExCSS.Model.Factories.AtRuleFactories
{
    internal class UnknownAtRuleFactory : RuleFactory
    {
        private string _name;

        public UnknownAtRuleFactory(string name, StyleSheetContext context) : base(context)
        {
            _name = name;
        }
        
        public override void Parse(IEnumerator<Block> reader)
        {
            var rule = new GenericRule(Context);
            var endCurly = 0;

            Context.ActiveRules.Push(rule);
            Context.ReadBuffer.Append(_name).Append(" ");

            do
            {
                if (reader.Current.Type == GrammarSegment.Semicolon && endCurly == 0)
                {
                    reader.MoveNext();
                    break;
                }

                Context.ReadBuffer.Append(reader.Current.ToString());

                if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    endCurly++;
                }
                else if (reader.Current.Type == GrammarSegment.CurlyBracketClose && --endCurly == 0)
                {
                    break;
                }
            }
            while (reader.MoveNext());

            rule.SetText(Context.ReadBuffer.ToString());
            Context.ReadBuffer.Clear();
            Context.ActiveRules.Pop();
   
            Context.AtRules.Add(rule);
        }
    }
}