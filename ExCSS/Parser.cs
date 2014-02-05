﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExCSS.Model;
using ExCSS.Model.TextBlocks;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    internal delegate void ParseErrorEventHandler(StylesheetParseError e);

    public sealed partial class Parser
    {
		private SelectorFactory _selectorFactory;
		private Stack<FunctionBuffer> _functionBuffers;
        private Lexer _lexer;
		private bool _isFraction;
		private Property _property;
		private List<Term> _terms = new List<Term>();
        private StyleSheet _styleSheet;
        private Stack<RuleSet> _activeRuleSets;
		private StringBuilder _buffer;
		private ParsingContext _parsingContext;
        private GrammarSegment _termDelimiter = GrammarSegment.Whitespace;

        public StyleSheet Parse(string css)
        {
            _selectorFactory = new SelectorFactory();
            _functionBuffers = new Stack<FunctionBuffer>();
            _styleSheet = new StyleSheet();
            _activeRuleSets = new Stack<RuleSet>();
            _lexer = new Lexer(new StylesheetReader(css)) {ErrorHandler = HandleLexerError };

            SetParsingContext(ParsingContext.DataBlock);

            var tokens = _lexer.Tokens;

            foreach (var token in tokens)
            {
                if (ParseTokenBlock(token))
                {
                    continue;
                }

                HandleLexerError(ParserError.UnexpectedLineBreak, ErrorMessages.Default);
            }

            if (_property != null)
            {
                ParseTokenBlock(SpecialCharacter.Semicolon);
            }

            return _styleSheet;
        }
        
        internal static BaseSelector ParseSelector(string selector)
        {
			var tokenizer = new Lexer(new StylesheetReader(selector));
			var tokens = tokenizer.Tokens;
			var selctor = new SelectorFactory();

            foreach (var token in tokens)
            {
                selctor.Apply(token);
            }

			var result = selctor.Result;

			return result;
        }

        internal static RuleSet ParseRule(string css)
        {
            var parser = new Parser();//new StyleSheet(), new StylesheetReader(rule))
            

            var styleSheet = parser.Parse(css);

            return styleSheet.Rules.Count > 0
                ? styleSheet.Rules[0] 
                : null;
        }

        internal static StyleDeclaration ParseDeclarations(string declarations, bool quirksMode = false)
        {
            var decl = new StyleDeclaration();
            AppendDeclarations(decl, declarations, quirksMode);

            return decl;
        }

		internal static void AppendDeclarations(StyleDeclaration list, string css, bool quirksMode = false)
		{
		    var parser = new Parser();//(new StyleSheet(), new StylesheetReader(declarations))
           

		    parser.AddRuleSet(list.ParentRule ?? new StyleRule(list));

		    parser._parsingContext = ParsingContext.InDeclaration;
			parser.Parse(css);
		}

        internal void HandleLexerError(ParserError error, string message)
        {
            _styleSheet.Errors.Add(new StylesheetParseError(error, message, _lexer.Stream.Line, _lexer.Stream.Column));
        }

        private bool AddTerm(Term value)
        {
            if (_isFraction)
            {
                if (_terms.Any())
                {
                    value = new PrimitiveTerm(UnitType.Unknown, _terms[0] + "/" + value);
                    _terms = new List<Term>();
                }

                _isFraction = false;
            }

            if (_functionBuffers.Count > 0)
            {
                _functionBuffers.Peek().TermList.Add(value);
            }
            else if (!_terms.Any())
            {
                _terms.Add(value);
            }
            else if (_parsingContext == ParsingContext.InSingleValue)
            {
                // Fonts delimited by a comma
                if (CurrentRule is FontFaceRule)
                {
                    _termDelimiter = GrammarSegment.Comma;
                }

                _terms.Add(value);
            }
            else
            {
                return false;
            }

            return true;
        }

        private void FinalizeProperty()
        {
            if (_property != null)
            {
                if (_terms.Count > 1)
                {
                    var termList = new TermList();
                    _property.Term = termList;

                    _terms.ForEach(t => termList.AddTerm(_termDelimiter, t));
                }
                else
                {
                    _property.Term = _terms[0];
                }
            }

            _terms.Clear();
            _termDelimiter = GrammarSegment.Whitespace;
            _property = null;
        }

        private bool FinalizeRule()
        {
            if (_activeRuleSets.Count <= 0)
            {
                return false;
            }

            _activeRuleSets.Pop();
            return true;
        }

        private void AddRuleSet(RuleSet rule)
        {
            //rule.ParentStyleSheet = _styleSheet;

            if (_activeRuleSets.Count > 0)
            {
                var container = _activeRuleSets.Peek() as ISupportsRuleSets;

                if (container != null)
                {
                    container.RuleSets.Add(rule);
                }
            }
            else
            {
                _styleSheet.Rules.Add(rule);
            }

            _activeRuleSets.Push(rule);
        }

        private void AddProperty(Property property)
        {
            _property = property;
            var rule = CurrentRule as ISupportsDeclarations;

            if (rule != null)
            {
                rule.Declarations.Add(property);
            }
        }

        private T CastRuleSet<T>() where T : RuleSet
        {
            if (_activeRuleSets.Count > 0)
            {
                return _activeRuleSets.Peek() as T;
            }

            return default(T);
        }

        private void SetParsingContext(ParsingContext newState)
        {
            switch (newState)
            {
                case ParsingContext.InSelector:
                    _lexer.IgnoreComments = true;
                    _lexer.IgnoreWhitespace = false;
                    _selectorFactory.ResetFactory();
                    break;

                case ParsingContext.InHexValue:
                case ParsingContext.InUnknown:
                case ParsingContext.InCondition:
                case ParsingContext.InSingleValue:
                case ParsingContext.InMediaValue:
                    _lexer.IgnoreComments = true;
                    _lexer.IgnoreWhitespace = false;
                    break;

                default:
                    _lexer.IgnoreComments = true;
                    _lexer.IgnoreWhitespace = true;
                    break;
            }

            _parsingContext = newState;
        }

        internal RuleSet CurrentRule
        {
            get
            {
                return _activeRuleSets.Count > 0
                    ? _activeRuleSets.Peek()
                    : null;
            }
        }
    }
}
