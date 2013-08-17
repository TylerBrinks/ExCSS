using System;
using System.Collections.Generic;
using System.Text;
using ExCSS.Model.Factories;
using ExCSS.Model.Factories.AtRuleFactories;
using ExCSS.Model.Factories.StyleRuleFactories;
using ExCSS.Model.Rules;

namespace ExCSS.Model
{
    public class StyleSheetContext
    {
        private readonly Lexer _lexer;

        internal StyleSheetContext(Lexer lexer)
        {
            _lexer = lexer;
            Ruleset = new List<RuleSet>();
            AtRules = new List<RuleSet>();

            ActiveRules = new Stack<RuleSet>();
            ReadBuffer = new StringBuilder();
        }

        internal void BuildRules()
        {
            BuildRulesets(Ruleset);
        }

        internal void BuildRulesets(ICollection<RuleSet> rules)
        {
            var reader = _lexer.Tokens.GetEnumerator();

            BuildRulesets(reader, rules);
        }

        internal void BuildRulesets(IEnumerator<Block> reader, ICollection<RuleSet> rules)
        {
            while (reader.MoveNext())
            {
                RuleFactory factory = null;

                switch (reader.Current.Type)
                {
                    case GrammarSegment.CommentClose:
                    case GrammarSegment.CommentOpen:
                    case GrammarSegment.Whitespace:
                        break;

                    case GrammarSegment.AtRule:
                        factory = new AtRuleFactory(this);
                        break;

                    default:
                        factory = new StyleRuleFactory(this);
                        break;
                }

                if (factory == null)
                {
                    continue;
                }

                factory.Parse(reader);
            }
        }

        internal List<RuleSet> Ruleset { get; set; }
        internal List<RuleSet> AtRules { get; set; }
        internal Stack<RuleSet> ActiveRules { get; set; }
        internal StringBuilder ReadBuffer { get; set; }
        internal RuleSet CurrentRule
        {
            get { return ActiveRules.Count > 0 ? ActiveRules.Peek() : null; }
        }
        internal Lexer Lexer { get { return _lexer; } }

        internal void AppendStyleToActiveRule(RuleSet ruleSet)
        {
            var rule = ActiveRules.Peek();
            
            if (rule is IRuleContainer)
            {
                (rule as IRuleContainer).Rules.Add(ruleSet);
            }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var rule in Ruleset)
            {
                sb.AppendLine(rule.ToString());
            }

            return sb.ToString();
        }
    }
}