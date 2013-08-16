using System;
using System.Collections.Generic;
using System.Linq;

namespace ExCSS.Model.Rules
{
    public sealed class KeyframesRule : Ruleset
    {
        internal const string RuleName = "keyframes";

        private readonly RuleList _rules;
        private string _name;

        internal KeyframesRule(StyleSheetContext context)
            : base( context)
        {
            _rules = new RuleList();
            _type = RuleType.Keyframes;
        }
       
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public RuleList Rules
        {
            get { return _rules; }
        }

        public KeyframesRule AppendRule(string rule)
        {
            var obj = ParseKeyframeRule(rule);
     
            //_rules.InsertAt(_rules.Length, obj);
            _rules.Add(obj);
            return this;
        }

        internal KeyframeRule ParseKeyframeRule(string rule, bool quirksMode = false)
        {
            var parser = new Parser(rule)
            {
                IsQuirksMode = quirksMode,
                //_ignore = false
            };

            var it = parser.Lexer.Tokens.GetEnumerator();

            if (it.SkipToNextNonWhitespace())
            {
                //if (it.Current.Type == GrammarSegment.CommentOpen || it.Current.Type == GrammarSegment.CommentClose)
                // throw new DOMException(ErrorCode.SyntaxError);

                return CreateKeyframeRule(it);
            }

            return null;
        }

        private  KeyframeRule CreateKeyframeRule(IEnumerator<Block> source)
        {
            var keyframe = new KeyframeRule(Context);

            Context.ActiveRules.Push(keyframe);

            do
            {
                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (source.SkipToNextNonWhitespace())
                    {
                        var tokens = source.LimitToCurrentBlock();
                        tokens.GetEnumerator().AppendDeclarations(keyframe.Style.List);
                    }

                    break;
                }

                Context.ReadBuffer.Append(source.Current);
            }
            while (source.MoveNext());

            keyframe.KeyText = Context.ReadBuffer.ToString();
            Context.ReadBuffer.Clear();
            Context.ActiveRules.Pop();
            return keyframe;
        }


        private KeyframesRule CreateKeyframesRule(IEnumerator<Block> source)
        {
            var keyframes = new KeyframesRule(Context);

            Context.ActiveRules.Push(keyframes);

            if (source.Current.Type == GrammarSegment.Ident)
            {
                keyframes.Name = ((SymbolBlock)source.Current).Value;
                source.SkipToNextNonWhitespace();

                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    source.SkipToNextNonWhitespace();
                    var tokens = source.LimitToCurrentBlock().GetEnumerator();

                    while (tokens.SkipToNextNonWhitespace())
                    {
                        //keyframes.Rules.List.Add(CreateKeyframeRule(tokens));
                        keyframes.Rules.Add(CreateKeyframeRule(tokens));
                    }
                }
            }

            Context.ActiveRules.Pop();
            return keyframes;
        }

        public KeyframesRule DeleteRule(string key)
        {
            //for (int i = 0; i < _rules.Length; i++)
            for (var i = 0; i < _rules.Count; i++)
            {
                if ((_rules[i] as KeyframeRule).KeyText.Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    _rules.RemoveAt(i);
                    break;
                }
            }

            return this;
        }

        public KeyframeRule FindRule(string key)
        {
            //for (int i = 0; i < _rules.Length; i++)
            return _rules.Select(t => t as KeyframeRule).FirstOrDefault(rule => rule.KeyText.Equals(key, StringComparison.OrdinalIgnoreCase));
        }

        public override string ToString()
        {
            return String.Format("@keyframes {0} {{{1}{2}}}", _name, Environment.NewLine, _rules);
        }
    }
}
