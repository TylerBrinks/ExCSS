using System.Collections.Generic;
using System.Text;
using ExCSS.Model.Factories.AtRuleFactories;

namespace ExCSS.Model
{
    public class StyleSheetContext
    {
        private readonly Lexer _lexer;

        internal StyleSheetContext(Lexer lexer)
        {
            _lexer = lexer;
            Ruleset = new RuleList();
            AtRules = new RuleList();

            ActiveRules = new Stack<Ruleset>();
            ReadBuffer = new StringBuilder();
        }

        internal void BuildRules()
        {
            AppendRules(Ruleset);
         }

        internal void AppendRules(ICollection<Ruleset> rules)
        {
            var source = _lexer.Tokens.GetEnumerator();
            while (source.MoveNext())
            {
                switch (source.Current.Type)
                {
                    case GrammarSegment.CommentClose:
                    case GrammarSegment.CommentOpen:
                    case GrammarSegment.Whitespace:
                        break;

                    case GrammarSegment.AtRule:
                        new AtRuleFactory(this).Parse(source);
                        break;

                    default:
                        rules.Add(CreateRuleset(source));
                        break;
                }
            }
        }

        internal StyleRule CreateRuleset(IEnumerator<Block> source)
        {
            var style = new StyleRule(this);
            //var ctor = new SelectorConstructor { IgnoreErrors = _ignore };

            //ActiveRules.Push(style);

            //do
            //{
            //    if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
            //    {
            //        if (SkipToNextNonWhitespace(reader))
            //        {
            //            var tokens = LimitToCurrentBlock(reader);
            //            AppendDeclarations(tokens.GetEnumerator(), style.Style.List);
            //        }

            //        break;
            //    }

            //    ctor.PickSelector(reader);
            //}
            //while (reader.MoveNext());

            //style.Selector = ctor.Result;
            //ActiveRules.Pop();
            return style;
        }

        internal RuleList Ruleset { get; set; }

        internal RuleList AtRules { get; set; }

        public Stack<Ruleset> ActiveRules { get; set; }

        internal Ruleset CurrentRule
        {
            get { return ActiveRules.Count > 0 ? ActiveRules.Peek() : null; }
        }

        internal StringBuilder ReadBuffer { get; set; }

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