using System.Collections.Generic;
using System.Linq;
using ExCSS.Model;
using ExCSS.Model.TextBlocks;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    internal class SelectorConstructor
    {
        private SimpleSelector _testSelector;
        private MultipleSelectorList _multipleSelectorList;
        private bool _hasCombinator;
        private Combinator _combinator;
        private ComplexSelector _complexSelector;

        internal SelectorConstructor()
        {
            _combinator = Combinator.Descendent;
            _hasCombinator = false;
        }

        internal SimpleSelector Result
        {
            get
            {
                if (_complexSelector != null)
                {
                    _complexSelector.ConcludeSelector(_testSelector);
                    _testSelector = _complexSelector;
                }

                if (_multipleSelectorList == null || _multipleSelectorList.Length == 0)
                {
                    return _testSelector ?? SimpleSelector.Global;
                }

                if (_testSelector == null && _multipleSelectorList.Length == 1)
                {
                    return _multipleSelectorList[0];
                }

                if (_testSelector != null)
                {
                    _multipleSelectorList.AppendSelector(_testSelector);
                    _testSelector = null;
                }

                return _multipleSelectorList;
            }
        }

        internal void AssignSelector(IEnumerator<Block> tokens)
        {
            switch (tokens.Current.GrammarSegment)
            {
                case GrammarSegment.SquareBraceOpen: // [Attribute]
                    ParseAttribute(tokens);
                    break;

                case GrammarSegment.Colon:  // :pseudo
                    ParseColon(tokens);
                    break;

                case GrammarSegment.Hash: // #identifier
                    Insert(SimpleSelector.Id(((SymbolBlock)tokens.Current).Value));
                    break;

                case GrammarSegment.Ident: // element
                    Insert(SimpleSelector.Type(((SymbolBlock)tokens.Current).Value));
                    break;

                case GrammarSegment.Whitespace:
                    Insert(Combinator.Descendent);
                    break;

                case GrammarSegment.Delimiter:
                    ParseDelimiter(tokens);
                    break;

                case GrammarSegment.Comma:
                    InsertCommaDelimiter();
                    break;

                default:
                    //if (!ignoreErrors) 
                    //throw new DOMException(ErrorCode.SyntaxError);
                    break;
            }
        }

        internal void InsertCommaDelimiter()
        {
            if (_testSelector == null)
            {
                return;
            }

            if (_multipleSelectorList == null)
            {
                _multipleSelectorList = new MultipleSelectorList();
            }

            if (_complexSelector != null)
            {
                _complexSelector.ConcludeSelector(_testSelector);
                _multipleSelectorList.AppendSelector(_complexSelector);
                _complexSelector = null;
            }
            else
            {
                _multipleSelectorList.AppendSelector(_testSelector);
            }

            _testSelector = null;
        }

        internal void Insert(SimpleSelector selector)
        {
            if (_testSelector != null)
            {
                if (!_hasCombinator)
                {
                    var compound = _testSelector as AggregateSelectorList;

                    if (compound == null)
                    {
                        compound = new AggregateSelectorList();
                        compound.AppendSelector(_testSelector);
                    }

                    compound.AppendSelector(selector);
                    _testSelector = compound;
                }
                else
                {
                    if (_complexSelector == null)
                    {
                        _complexSelector = new ComplexSelector();
                    }

                    _complexSelector.AppendSelector(_testSelector, _combinator);
                    _combinator = Combinator.Descendent;
                    _hasCombinator = false;
                    _testSelector = selector;
                }
            }
            else
            {
                _combinator = Combinator.Descendent;
                _hasCombinator = false;
                _testSelector = selector;
            }
        }

        internal void Insert(Combinator combinator)
        {
            _hasCombinator = true;

            if (combinator != Combinator.Descendent)
            {
                _combinator = combinator;
            }
        }

        internal void ParseDelimiter(IEnumerator<Block> tokens)
        {
            var delimiter = ((DelimiterBlock)tokens.Current).Value;

            switch (delimiter)
            {
                case Specification.Comma:
                    InsertCommaDelimiter();
                    break;

                case Specification.GreaterThan:
                    Insert(Combinator.Child);
                    break;

                case Specification.PlusSign:
                    Insert(Combinator.AdjacentSibling);
                    break;

                case Specification.Tilde:
                    Insert(Combinator.Sibling);
                    break;

                case Specification.Asterisk:
                    Insert(SimpleSelector.Global);
                    break;

                case Specification.Pipe:
                    Insert(SimpleSelector.Namespace);
                    break;

                case Specification.Period:
                    if (tokens.MoveNext() && tokens.Current.GrammarSegment == GrammarSegment.Ident)
                    {
                        var classBlock = (SymbolBlock)tokens.Current;
                        Insert(SimpleSelector.Class(classBlock.Value));
                    }
                    
                    break;
            }
        }

        internal void ParseAttribute(IEnumerator<Block> tokens)
        {
            var selector = GetAttributeSelector(tokens);

            if (selector != null)
            {
                Insert(selector);
            }
        }

        internal void ParseColon(IEnumerator<Block> tokens)
        {
            var selector = GetPseudoSelector(tokens);

            if (selector != null)
            {
                Insert(selector);
            }
        }

        internal SimpleSelector GetSimpleSelector(IEnumerator<Block> tokens)
        {
            while (tokens.MoveNext())
            {
                switch (tokens.Current.GrammarSegment)
                {
                    case GrammarSegment.SquareBraceOpen: // [Attribute]
                        {
                            var sel = GetAttributeSelector(tokens);
                            if (sel != null)
                            {
                                return sel;
                            }
                        }
                        break;

                    case GrammarSegment.Colon: // :pseudo-class
                        {
                            var sel = GetPseudoSelector(tokens);
                            if (sel != null)
                            {
                                return sel;
                            }
                        }
                        break;

                    case GrammarSegment.Hash: // #identifier
                        return SimpleSelector.Id(((SymbolBlock)tokens.Current).Value);

                    case GrammarSegment.Ident: // element
                        return SimpleSelector.Type(((SymbolBlock)tokens.Current).Value);

                    case GrammarSegment.Delimiter:
                        if (((DelimiterBlock) tokens.Current).Value == Specification.Period && tokens.MoveNext() &&
                            tokens.Current.GrammarSegment == GrammarSegment.Ident)
                        {
                            return SimpleSelector.Class(((SymbolBlock)tokens.Current).Value);
                        }
                        break;
                }
            }

            return null;
        }

        internal SimpleSelector GetPseudoSelector(IEnumerator<Block> tokens)
        {
            SimpleSelector selector = null;

            if (tokens.MoveNext())
            {
                switch (tokens.Current.GrammarSegment)
                {
                    case GrammarSegment.Colon:
                        selector = GetPseudoElement(tokens);
                        break;
                    case GrammarSegment.Function:
                        selector = GetPseudoClassFunction(tokens);
                        break;
                    case GrammarSegment.Ident:
                        selector = SimpleSelector.PseudoClass(((SymbolBlock)tokens.Current).Value);
                        break;
                }
            }

            if (selector == null)// && !_ignoreErrors)
            {
                //throw new DOMException(ErrorCode.SyntaxError);
            }
            return selector;
        }

        internal SimpleSelector GetPseudoClassFunction(IEnumerator<Block> tokens)
        {
            var name = ((SymbolBlock)tokens.Current).Value;
            var blocks = new List<Block>();

            while (tokens.MoveNext())
            {
                if (tokens.Current.GrammarSegment == GrammarSegment.ParenClose)
                {
                    break;
                }

                blocks.Add(tokens.Current);
            }

            if (blocks.Count == 0)
            {
                return null;
            }

            //if (!_ignoreErrors)
            //{
            //throw new DOMException(ErrorCode.SyntaxError);
            //}

            var functionValue = string.Join("", blocks.Select(b => b.ToString()));
            return SimpleSelector.Function(name, functionValue);
        }

        internal SimpleSelector GetPseudoElement(IEnumerator<Block> tokens)
        {
            if (tokens.MoveNext() && tokens.Current.GrammarSegment == GrammarSegment.Ident)
            {
                var pseudo = ((SymbolBlock)tokens.Current).Value;

                return SimpleSelector.PseudoElement(pseudo);
            }

            return null;
        }

        internal SimpleSelector GetAttributeSelector(IEnumerator<Block> tokens)
        {
            var values = new List<string>();
            Block operatorBlock = null;

            while (tokens.MoveNext())
            {
                if (tokens.Current.GrammarSegment == GrammarSegment.SquareBracketClose)
                {
                    break;
                }

                switch (tokens.Current.GrammarSegment)
                {
                    case GrammarSegment.Ident:
                        values.Add(((SymbolBlock)tokens.Current).Value);
                        break;
                    case GrammarSegment.String:
                        values.Add(((StringBlock)tokens.Current).Value);
                        break;
                    case GrammarSegment.Number:
                        values.Add(((NumericBlock)tokens.Current).Value.ToString());
                        break;
                    default:
                        if (operatorBlock == null && (tokens.Current is MatchBlock || tokens.Current.GrammarSegment == GrammarSegment.Delimiter))
                        {
                            operatorBlock = tokens.Current;
                        }
                       
                        break;
                }
            }

            if ((operatorBlock == null || values.Count != 2) && (operatorBlock != null || values.Count != 1))
            {
                return null;
            }

            if (operatorBlock == null)
            {
                return SimpleSelector.AttributeUnmatched(values[0]);
            }

            switch (operatorBlock.ToString())
            {
                case "=":
                    return SimpleSelector.AttributeMatch(values[0], values[1]);

                case "~=":
                    return SimpleSelector.AttributeSpaceSeparated(values[0], values[1]);

                case "|=":
                    return SimpleSelector.AttributeDashSeparated(values[0], values[1]);

                case "^=":
                    return SimpleSelector.AttributeStartsWith(values[0], values[1]);

                case "$=":
                    return SimpleSelector.AttributeEndsWith(values[0], values[1]);

                case "*=":
                    return SimpleSelector.AttributeContains(values[0], values[1]);

                case "!=":
                    return SimpleSelector.AttributeNegatedMatch(values[0], values[1]);
            }

            return null;
        }
    }
}
