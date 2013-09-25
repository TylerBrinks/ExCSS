using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExCSS.Model.Factories;
using ExCSS.Model.TextBlocks;

namespace ExCSS
{
    public class StyleSheet
    {
        private readonly Lexer _lexer;

        internal StyleSheet(Lexer lexer)
        {
            _lexer = lexer;
            Rulesets = new List<StyleRule>();
            AtRules = new List<RuleSet>();

            ActiveRules = new Stack<RuleSet>();
            ReadBuffer = new StringBuilder();
            Errors = new List<StylesheetParseError>();
        }

        internal void BuildRules()
        {
            BuildRulesets(Rulesets.ToArray());
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

                switch (reader.Current.GrammarSegment)
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

        public List<CharacterSetRule> CharsetDirectives
        {
            get { return GetDirectives<CharacterSetRule>(); }
        }

        public List<FontFaceRule> FontFaceDirectives
        {
            get { return GetDirectives<FontFaceRule>(); }
        }

        public List<ImportRule> ImportDirectives
        {
            get { return GetDirectives<ImportRule>(); }
        } 

        public List<KeyframesRule> KeyframeDirectives
        {
            get { return GetDirectives<KeyframesRule>(); }
        }

        public List<MediaRule> MediaDirectives
        {
            get { return GetDirectives<MediaRule>(); }
        }

        public List<NamespaceRule> NamespaceDirectives
        {
            get { return GetDirectives<NamespaceRule>(); }
        }

        public List<PageRule> PageDirectives
        {
            get { return GetDirectives<PageRule>(); }
        }

        public List<SupportsRule> SupportsDirectives
        {
            get { return GetDirectives<SupportsRule>(); }
        }

        public List<StylesheetParseError> Errors { get; internal set; } 

        public List<StyleRule> Rulesets { get; set; }

        internal Lexer Lexer { get { return _lexer; } }
        internal List<RuleSet> AtRules { get; set; }
        internal Stack<RuleSet> ActiveRules { get; set; }
        internal StringBuilder ReadBuffer { get; set; }
        internal List<T> GetDirectives<T>()
        {
            return AtRules.OfType<T>().ToList();
        } 

        internal RuleSet CurrentRule
        {
            get { return ActiveRules.Count > 0 ? ActiveRules.Peek() : null; }
        }        

        internal void AppendStyleToActiveRule(StyleRule ruleSet)
        {
            if (ActiveRules.Count == 0)
            {
                Rulesets.Add(ruleSet);
                return;
            }

            var rule = ActiveRules.Peek();
            
            if (rule is IRuleContainer)
            {
                (rule as IRuleContainer).Declarations.Add(ruleSet);
            }
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool friendlyFormat)
        {
            var builder = new StringBuilder();

            foreach (var atRule in AtRules)
            {
                builder.Append(atRule.ToString(friendlyFormat));

                if (friendlyFormat)
                {
                    builder.Append(Environment.NewLine);
                }
            }
        
            foreach (var rule in Rulesets)
            {
                builder.Append(rule.ToString(friendlyFormat));
            }

            return builder.ToString();
        }
    }
}