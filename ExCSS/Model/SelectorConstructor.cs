using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ExCSS.Model
{
    class SelectorConstructor
    {
        #region Constants

        private const string NthChildOdd = "odd";
        private const string NthChildEven = "even";
        private const string NthChildN = "n";

        //const string PSEUDOCLASS_ROOT = "root";
        //const string PSEUDOCLASS_FIRSTOFTYPE = "first-of-type";
        //const string PSEUDOCLASS_LASTOFTYPE = "last-of-type";
        //const string PSEUDOCLASS_ONLYCHILD = "only-child";
        const string PseudoclassFirstchild = "first-child";
        const string PseudoclassLastchild = "last-child";
        //const string PSEUDOCLASS_EMPTY = "empty";
        //const string PSEUDOCLASS_LINK = "link";
        //const string PSEUDOCLASS_VISITED = "visited";
        //const string PSEUDOCLASS_ACTIVE = "active";
        //const string PSEUDOCLASS_HOVER = "hover";
        //const string PSEUDOCLASS_FOCUS = "focus";
        //const string PSEUDOCLASS_TARGET = "target";
        //const string PSEUDOCLASS_ENABLED = "enabled";
        //const string PSEUDOCLASS_DISABLED = "disabled";
        //const string PSEUDOCLASS_CHECKED = "checked";
        //const string PSEUDOCLASS_UNCHECKED = "unchecked";
        //const string PSEUDOCLASS_INDETERMINATE = "indeterminate";
        //const string PSEUDOCLASS_DEFAULT = "default";

        //const string PSEUDOCLASS_VALID = "valid";
        //const string PSEUDOCLASS_INVALID = "invalid";
        //const string PSEUDOCLASS_REQUIRED = "required";
        //const string PSEUDOCLASS_INRANGE = "in-range";
        //const string PSEUDOCLASS_OUTOFRANGE = "out-of-range";
        //const string PSEUDOCLASS_OPTIONAL = "optional";
        //const string PSEUDOCLASS_READONLY = "read-only";
        //const string PSEUDOCLASS_READWRITE = "read-write";

        //const string PSEUDOCLASSFUNCTION_DIR = "dir";
        const string PseudoclassfunctionNthchild = "nth-child";
        const string PseudoclassfunctionNthlastchild = "nth-last-child";
        //const string PSEUDOCLASSFUNCTION_NOT = "not";
        //const string PSEUDOCLASSFUNCTION_LANG = "lang";
        //const string PSEUDOCLASSFUNCTION_CONTAINS = "contains";

        const string PseudoelementBefore = "before";
        const string PseudoelementAfter = "after";
        const string PseudoelementSelection = "selection";
        const string PseudoelementFirstline = "first-line";
        const string PseudoelementFirstletter = "first-letter";

        #endregion

       
        private Selector _temp;
        private ListSelector _group;
        private bool _hasCombinator;
        private bool _ignoreErrors;
        private Combinator _combinator;
        private ComplexSelector _complex;

        public SelectorConstructor()
        {
            _combinator = Combinator.Descendent;
            _hasCombinator = false;
        }

        public bool IgnoreErrors
        {
            get { return _ignoreErrors; }
            set { _ignoreErrors = value; }
        }

        public Selector Result
        {
            get
            {
                if (_complex != null)
                {
                    _complex.ConcludeSelector(_temp);
                    _temp = _complex;
                }

                if (_group == null || _group.Length == 0)
                {
                    return _temp ?? SimpleSelector.All;
                }
                
                if (_temp == null && _group.Length == 1)
                {
                    return _group[0];
                }
                
                if (_temp != null)
                {
                    _group.AppendSelector(_temp);
                    _temp = null;
                }

                return _group;
            }
        }

        public void PickSelector(IEnumerator<Block> tokens)
        {
            switch (tokens.Current.Type)
            {
                //Begin of attribute [A]
                case GrammarSegment.SquareBraceOpen:
                    OnAttribute(tokens);
                    break;

                //Begin of Pseudo :P
                case GrammarSegment.Colon:
                    OnColon(tokens);
                    break;

                //Begin of ID #I
                case GrammarSegment.Hash:
                    Insert(SimpleSelector.Id(((SymbolBlock)tokens.Current).Value));
                    break;

                //Begin of Type E
                case GrammarSegment.Ident:
                    Insert(SimpleSelector.Type(((SymbolBlock)tokens.Current).Value));
                    break;

                //Whitespace could be significant
                case GrammarSegment.Whitespace:
                    Insert(Combinator.Descendent);
                    break;

                //Various
                case GrammarSegment.Delimiter:
                    OnDelim(tokens);
                    break;

                case GrammarSegment.Comma:
                    InsertOr();
                    break;

                default:
                    //if (!ignoreErrors) 
                        //throw new DOMException(ErrorCode.SyntaxError);
                    var a = 1;
                    break;
            }
        }

        public void InsertOr()
        {
            if (_temp == null)
            {
                return;
            }

            if (_group == null)
            {
                _group = new ListSelector();
            }

            if (_complex != null)
            {
                _complex.ConcludeSelector(_temp);
                _group.AppendSelector(_complex);
                _complex = null;
            }
            else
            {
                _group.AppendSelector(_temp);
            }

            _temp = null;
        }

        public void Insert(Selector selector)
        {
            if (_temp != null)
            {
                if (!_hasCombinator)
                {
                    var compound = _temp as CompoundSelector;

                    if (compound == null)
                    {
                        compound = new CompoundSelector();
                        compound.AppendSelector(_temp);
                    }

                    compound.AppendSelector(selector);
                    _temp = compound;
                }
                else
                {
                    if (_complex == null)
                    {
                        _complex = new ComplexSelector();
                    }

                    _complex.AppendSelector(_temp, _combinator);
                    _combinator = Combinator.Descendent;
                    _hasCombinator = false;
                    _temp = selector;
                }
            }
            else
            {
                _combinator = Combinator.Descendent;
                _hasCombinator = false;
                _temp = selector;
            }
        }
        
        public void Insert(Combinator combinator)
        {
            _hasCombinator = true;

            if (combinator != Combinator.Descendent)
            {
                _combinator = combinator;
            }
        }

        public void OnDelim(IEnumerator<Block> tokens)
        {
            var chr = ((DelimBlock)tokens.Current).Value;

            switch (chr)
            {
                case Specification.COMMA:
                    InsertOr();
                    break;

                case Specification.GT:
                    Insert(Combinator.Child);
                    break;

                case Specification.PLUS:
                    Insert(Combinator.AdjacentSibling);
                    break;

                case Specification.TILDE:
                    Insert(Combinator.Sibling);
                    break;

                case Specification.ASTERISK:
                    Insert(SimpleSelector.All);
                    break;

                case Specification.DOT:
                    if (tokens.MoveNext() && tokens.Current.Type == GrammarSegment.Ident)
                    {
                        var cls = (SymbolBlock)tokens.Current;
                        Insert(SimpleSelector.Class(cls.Value));
                    }
                    else if (!_ignoreErrors)
                    {
                        var b = 1;
                        //throw new DOMException(ErrorCode.SyntaxError);
                    }
    
                    break;
            }
        }

        public void OnAttribute(IEnumerator<Block> tokens)
        {
            var selector = GetAttributeSelector(tokens);

            if (selector != null)
            {
                Insert(selector);
            }
        }
        
        public void OnColon(IEnumerator<Block> tokens)
        {
            var selector = GetPseudoSelector(tokens);
            
            if (selector != null)
            {
                Insert(selector);
            }
        }

        public SimpleSelector GetSimpleSelector(IEnumerator<Block> tokens)
        {
            while (tokens.MoveNext())
            {
                switch (tokens.Current.Type)
                {
                    //Begin of attribute [A]
                    case GrammarSegment.SquareBraceOpen:
                        {
                            var sel = GetAttributeSelector(tokens);
                            if (sel != null)
                            {
                                return sel;
                            }
                        }
                        break;

                    //Begin of Pseudo :P
                    case GrammarSegment.Colon:
                        {
                            var sel = GetPseudoSelector(tokens);
                            if (sel != null)
                            {
                                return sel;
                            }
                        }
                        break;

                    //Begin of ID #I
                    case GrammarSegment.Hash:
                        return SimpleSelector.Id(((SymbolBlock)tokens.Current).Value);

                    //Begin of Type E
                    case GrammarSegment.Ident:
                        return SimpleSelector.Type(((SymbolBlock)tokens.Current).Value);

                    //Various
                    case GrammarSegment.Delimiter:
                        if (((DelimBlock)tokens.Current).Value == Specification.DOT && tokens.MoveNext() && tokens.Current.Type == GrammarSegment.Ident)
                            return SimpleSelector.Class(((SymbolBlock)tokens.Current).Value);
                        break;

                    ////All others are being ignored
                    //case GrammarSegment.Whitespace:
                    //case GrammarSegment.Comma:
                    //default:
                    //    break;
                }
            }

            return null;
        }

        public SimpleSelector GetPseudoSelector(IEnumerator<Block> tokens)
        {
            SimpleSelector sel = null;

            if (tokens.MoveNext())
            {
                if (tokens.Current.Type == GrammarSegment.Colon)
                {
                    sel = GetPseudoElement(tokens);
                }
                else if (tokens.Current.Type == GrammarSegment.Function)
                {
                    sel = GetPseudoClassFunction(tokens);
                }
                else if (tokens.Current.Type == GrammarSegment.Ident)
                {
                    sel = GetPseudoClassIdentifier(tokens);
                }
            }

            if (sel == null && !_ignoreErrors)
            {
                var a = 1;
            
                //throw new DOMException(ErrorCode.SyntaxError);
            }
            return sel;
        }

        public SimpleSelector GetPseudoClassIdentifier(IEnumerator<Block> tokens)
        {
            //switch (((SymbolBlock)tokens.Current).Value)
            //{
            //    case PSEUDOCLASS_ROOT:
            //        return SimpleSelector.PseudoClass(el => el.OwnerDocument.DocumentElement == el, PSEUDOCLASS_ROOT);

            //    case PSEUDOCLASS_FIRSTOFTYPE:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            var parent = el.ParentElement;

            //            if (parent == null)
            //                return true;

            //            for (int i = 0; i < parent.ChildNodes.Length; i++)
            //            {
            //                if (parent.ChildNodes[i].NodeName == el.NodeName)
            //                    return parent.ChildNodes[i] == el;
            //            }

            //            return false;
            //        }, PSEUDOCLASS_FIRSTOFTYPE);

            //    case PSEUDOCLASS_LASTOFTYPE:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            var parent = el.ParentElement;

            //            if (parent == null)
            //                return true;

            //            for (int i = parent.ChildNodes.Length - 1; i >= 0; i--)
            //            {
            //                if (parent.ChildNodes[i].NodeName == el.NodeName)
            //                    return parent.ChildNodes[i] == el;
            //            }

            //            return false;
            //        }, PSEUDOCLASS_LASTOFTYPE);

            //    case PSEUDOCLASS_ONLYCHILD:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            var parent = el.ParentElement;

            //            if (parent == null)
            //                return false;

            //            var elements = 0;

            //            for (int i = 0; i < parent.ChildNodes.Length; i++)
            //            {
            //                if (parent.ChildNodes[i] is Element && ++elements == 2)
            //                    return false;
            //            }

            //            return true;
            //        }, PSEUDOCLASS_ONLYCHILD);

            //    case PSEUDOCLASS_FIRSTCHILD:
            //        return FirstChildSelector.Instance;

            //    case PSEUDOCLASS_LASTCHILD:
            //        return LastChildSelector.Instance;

            //    case PSEUDOCLASS_EMPTY:
            //        return SimpleSelector.PseudoClass(el => el.ChildNodes.Length == 0, PSEUDOCLASS_EMPTY);

            //    case PSEUDOCLASS_LINK:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLAnchorElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && !((HTMLAnchorElement)el).IsVisited;
            //            else if (el is HTMLAreaElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && !((HTMLAreaElement)el).IsVisited;
            //            else if (el is HTMLLinkElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && !((HTMLLinkElement)el).IsVisited;

            //            return false;
            //        }, PSEUDOCLASS_LINK);

            //    case PSEUDOCLASS_VISITED:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLAnchorElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && ((HTMLAnchorElement)el).IsVisited;
            //            else if (el is HTMLAreaElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && ((HTMLAreaElement)el).IsVisited;
            //            else if (el is HTMLLinkElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && ((HTMLLinkElement)el).IsVisited;

            //            return false;
            //        }, PSEUDOCLASS_VISITED);

            //    case PSEUDOCLASS_ACTIVE:
            //        return SimpleSelector.PseudoClass(el => 
            //        {
            //            if (el is HTMLAnchorElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && ((HTMLAnchorElement)el).IsActive;
            //            else if (el is HTMLAreaElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && ((HTMLAreaElement)el).IsActive;
            //            else if (el is HTMLLinkElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href")) && ((HTMLLinkElement)el).IsActive;
            //            else if (el is HTMLButtonElement)
            //                return !((HTMLButtonElement)el).Disabled && ((HTMLButtonElement)el).IsActive;
            //            else if (el is HTMLInputElement)
            //            {
            //                var inp = (HTMLInputElement)el;
            //                return (inp.Type == HTMLInputElement.InputType.Submit || inp.Type == HTMLInputElement.InputType.Image ||
            //                    inp.Type == HTMLInputElement.InputType.Reset || inp.Type == HTMLInputElement.InputType.Button) && 
            //                    inp.IsActive;
            //            }
            //            else if (el is HTMLMenuItemElement)
            //                return string.IsNullOrEmpty(el.GetAttribute("disabled")) && ((HTMLMenuItemElement)el).IsActive;

            //            return false;
            //        }, PSEUDOCLASS_ACTIVE);

            //    case PSEUDOCLASS_HOVER:
            //        return SimpleSelector.PseudoClass(el => el.IsHovered, PSEUDOCLASS_HOVER);

            //    case PSEUDOCLASS_FOCUS:
            //        return SimpleSelector.PseudoClass(el => el.IsFocused, PSEUDOCLASS_FOCUS);

            //    case PSEUDOCLASS_TARGET:
            //        return SimpleSelector.PseudoClass(el => el.OwnerDocument != null && el.Id == el.OwnerDocument.Location.Hash, PSEUDOCLASS_TARGET);

            //    case PSEUDOCLASS_ENABLED:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLAnchorElement || el is HTMLAreaElement || el is HTMLLinkElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("href"));
            //            else if (el is HTMLButtonElement)
            //                return !((HTMLButtonElement)el).Disabled;
            //            else if (el is HTMLInputElement)
            //                return !((HTMLInputElement)el).Disabled;
            //            else if (el is HTMLSelectElement)
            //                return !((HTMLSelectElement)el).Disabled;
            //            else if (el is HTMLTextAreaElement)
            //                return !((HTMLTextAreaElement)el).Disabled;
            //            else if (el is HTMLOptionElement)
            //                return !((HTMLOptionElement)el).Disabled;
            //            else if (el is HTMLOptGroupElement || el is HTMLMenuItemElement || el is HTMLFieldSetElement)
            //                return string.IsNullOrEmpty(el.GetAttribute("disabled"));

            //            return false;
            //        }, PSEUDOCLASS_ENABLED);

            //    case PSEUDOCLASS_DISABLED:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLButtonElement)
            //                return ((HTMLButtonElement)el).Disabled;
            //            else if (el is HTMLInputElement)
            //                return ((HTMLInputElement)el).Disabled;
            //            else if (el is HTMLSelectElement)
            //                return ((HTMLSelectElement)el).Disabled;
            //            else if (el is HTMLTextAreaElement)
            //                return ((HTMLTextAreaElement)el).Disabled;
            //            else if (el is HTMLOptionElement)
            //                return ((HTMLOptionElement)el).Disabled;
            //            else if (el is HTMLOptGroupElement || el is HTMLMenuItemElement || el is HTMLFieldSetElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("disabled"));

            //            return false;
            //        }, PSEUDOCLASS_DISABLED);

            //    case PSEUDOCLASS_DEFAULT:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLButtonElement)
            //            {
            //                var bt = (HTMLButtonElement)el;
            //                var form = bt.Form;

            //                if (form != null)//TODO Check if button is form def. button
            //                    return true;
            //            }
            //            else if (el is HTMLInputElement)
            //            {
            //                var input = (HTMLInputElement)el;

            //                if (input.Type == HTMLInputElement.InputType.Submit || input.Type == HTMLInputElement.InputType.Image)
            //                {
            //                    var form = input.Form;

            //                    if (form != null)//TODO Check if input is form def. button
            //                        return true;
            //                }
            //                else
            //                {
            //                    //TODO input that are checked and can be checked ...
            //                }
            //            }
            //            else if (el is HTMLOptionElement)
            //                return !string.IsNullOrEmpty(el.GetAttribute("selected"));

            //            return false;
            //        }, PSEUDOCLASS_DEFAULT);

            //    case PSEUDOCLASS_CHECKED:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLInputElement)
            //            {
            //                var inp = (HTMLInputElement)el;
            //                return (inp.Type == HTMLInputElement.InputType.Checkbox || inp.Type == HTMLInputElement.InputType.Radio)
            //                    && inp.Checked;
            //            }
            //            else if (el is HTMLMenuItemElement)
            //            {
            //                var mi = (HTMLMenuItemElement)el;
            //                return (mi.Type == HTMLMenuItemElement.ItemType.Checkbox || mi.Type == HTMLMenuItemElement.ItemType.Radio) 
            //                    && mi.Checked;
            //            }
            //            else if (el is HTMLOptionElement)
            //                return ((HTMLOptionElement)el).Selected;

            //            return false;
            //        }, PSEUDOCLASS_CHECKED);

            //    case PSEUDOCLASS_INDETERMINATE:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLInputElement)
            //            {
            //                var inp = (HTMLInputElement)el;
            //                return inp.Type == HTMLInputElement.InputType.Checkbox && inp.Indeterminate;
            //            }
            //            else if (el is HTMLProgressElement)
            //                return string.IsNullOrEmpty(el.GetAttribute("value"));

            //            return false;
            //        }, PSEUDOCLASS_INDETERMINATE);

            //    case PSEUDOCLASS_UNCHECKED:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLInputElement)
            //            {
            //                var inp = (HTMLInputElement)el;
            //                return (inp.Type == HTMLInputElement.InputType.Checkbox || inp.Type == HTMLInputElement.InputType.Radio)
            //                    && !inp.Checked;
            //            }
            //            else if (el is HTMLMenuItemElement)
            //            {
            //                var mi = (HTMLMenuItemElement)el;
            //                return (mi.Type == HTMLMenuItemElement.ItemType.Checkbox || mi.Type == HTMLMenuItemElement.ItemType.Radio)
            //                    && !mi.Checked;
            //            }
            //            else if (el is HTMLOptionElement)
            //                return !((HTMLOptionElement)el).Selected;

            //            return false;
            //        }, PSEUDOCLASS_UNCHECKED);

            //    case PSEUDOCLASS_VALID:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is IValidation)
            //                return ((IValidation)el).CheckValidity();
            //            else if (el is HTMLFormElement)
            //                return ((HTMLFormElement)el).IsValid;

            //            return false;
            //        }, PSEUDOCLASS_VALID);

            //    case PSEUDOCLASS_INVALID:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is IValidation)
            //                return !((IValidation)el).CheckValidity();
            //            else if (el is HTMLFormElement)
            //                return !((HTMLFormElement)el).IsValid;
            //            else if (el is HTMLFieldSetElement)
            //                return ((HTMLFieldSetElement)el).IsInvalid;

            //            return false;
            //        }, PSEUDOCLASS_INVALID);

            //    case PSEUDOCLASS_REQUIRED:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLInputElement)
            //                return ((HTMLInputElement)el).Required;
            //            else if (el is HTMLSelectElement)
            //                return ((HTMLSelectElement)el).Required;
            //            else if (el is HTMLTextAreaElement)
            //                return ((HTMLTextAreaElement)el).Required;

            //            return false;
            //        }, PSEUDOCLASS_REQUIRED);

            //    case PSEUDOCLASS_READONLY:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLInputElement)
            //                return !((HTMLInputElement)el).IsMutable;
            //            else if (el is HTMLTextAreaElement)
            //                return !((HTMLTextAreaElement)el).IsMutable;

            //            return !el.IsContentEditable;
            //        }, PSEUDOCLASS_READONLY);

            //    case PSEUDOCLASS_READWRITE:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLInputElement)
            //                return ((HTMLInputElement)el).IsMutable;
            //            else if (el is HTMLTextAreaElement)
            //                return ((HTMLTextAreaElement)el).IsMutable;

            //            return el.IsContentEditable;
            //        }, PSEUDOCLASS_READWRITE);

            //    case PSEUDOCLASS_INRANGE:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is IValidation)
            //            {
            //                var state = ((IValidation)el).Validity;
            //                return !state.RangeOverflow && !state.RangeUnderflow;
            //            }

            //            return false;
            //        }, PSEUDOCLASS_INRANGE);

            //    case PSEUDOCLASS_OUTOFRANGE:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is IValidation)
            //            {
            //                var state = ((IValidation)el).Validity;
            //                return state.RangeOverflow || state.RangeUnderflow;
            //            }

            //            return false;
            //        }, PSEUDOCLASS_OUTOFRANGE);

            //    case PSEUDOCLASS_OPTIONAL:
            //        return SimpleSelector.PseudoClass(el =>
            //        {
            //            if (el is HTMLInputElement)
            //                return !((HTMLInputElement)el).Required;
            //            else if (el is HTMLSelectElement)
            //                return !((HTMLSelectElement)el).Required;
            //            else if (el is HTMLTextAreaElement)
            //                return !((HTMLTextAreaElement)el).Required;

            //            return false;
            //        }, PSEUDOCLASS_OPTIONAL);

            //    // LEGACY STYLE OF DEFINING PSEUDO ELEMENTS - AS PSEUDO CLASS!
            //    case PSEUDOELEMENT_BEFORE:
            //        return SimpleSelector.PseudoClass(MatchBefore, PSEUDOELEMENT_BEFORE);

            //    case PSEUDOELEMENT_AFTER:
            //        return SimpleSelector.PseudoClass(MatchAfter, PSEUDOELEMENT_AFTER);

            //    case PSEUDOELEMENT_FIRSTLINE:
            //        return SimpleSelector.PseudoClass(MatchFirstLine, PSEUDOELEMENT_FIRSTLINE);

            //    case PSEUDOELEMENT_FIRSTLETTER:
            //        return SimpleSelector.PseudoClass(MatchFirstLetter, PSEUDOELEMENT_FIRSTLETTER);
            //}

            return null;
        }

        public SimpleSelector GetPseudoClassFunction(IEnumerator<Block> tokens)
        {
            var name = ((SymbolBlock)tokens.Current).Value;
            var args = new List<Block>();

            while (tokens.MoveNext())
            {
                if (tokens.Current.Type == GrammarSegment.ParenClose)
                {
                    break;
                }
               
                args.Add(tokens.Current);
            }

            if (args.Count == 0)
            {
                return null;
            }

            //switch (name)
            //{
            //    case PSEUDOCLASSFUNCTION_NTHCHILD:
            //        return GetArguments<NthChildSelector>(args.GetEnumerator());

            //    case PSEUDOCLASSFUNCTION_NTHLASTCHILD:
            //        return GetArguments<NthLastChildSelector>(args.GetEnumerator());

            //    case PSEUDOCLASSFUNCTION_DIR:
            //        if (args.Count == 1 && args[0].Type == GrammarSegment.Ident)
            //        {
            //            var dir = ((SymbolBlock)args[0]).Value;
            //            var code = string.Format("{0}({1})", PSEUDOCLASSFUNCTION_DIR, dir);
            //            var dirCode = dir == "ltr" ? DirectionMode.Ltr : DirectionMode.Rtl;
            //            return SimpleSelector.PseudoClass(el => el.Dir == dirCode, code);
            //        }

            //        break;

            //    case PSEUDOCLASSFUNCTION_LANG:
            //        if (args.Count == 1 && args[0].Type == GrammarSegment.Ident)
            //        {
            //            var lang = ((SymbolBlock)args[0]).Value;
            //            var code = string.Format("{0}({1})", PSEUDOCLASSFUNCTION_LANG, lang);
            //            return SimpleSelector.PseudoClass(el => el.Lang.Equals(lang, StringComparison.OrdinalIgnoreCase), code);
            //        }

            //        break;

            //    case PSEUDOCLASSFUNCTION_CONTAINS:
            //        if (args.Count == 1 && args[0].Type == GrammarSegment.String)
            //        {
            //            var str = ((StringBlock)args[0]).Value;
            //            var code = string.Format("{0}({1})", PSEUDOCLASSFUNCTION_CONTAINS, str);
            //            return SimpleSelector.PseudoClass(el => el.TextContent.Contains(str), code);
            //        }
            //        else if (args.Count == 1 && args[0].Type == GrammarSegment.Ident)
            //        {
            //            var str = ((SymbolBlock)args[0]).Value;
            //            var code = string.Format("{0}({1})", PSEUDOCLASSFUNCTION_CONTAINS, str);
            //            return SimpleSelector.PseudoClass(el => el.TextContent.Contains(str), code);
            //        }

            //        break;

            //    case PSEUDOCLASSFUNCTION_NOT:
            //        {
            //            var sel = GetSimpleSelector(args.GetEnumerator());
            //            if (sel != null)
            //            {
            //                var code = string.Format("{0}({1})", PSEUDOCLASSFUNCTION_NOT, sel.ToCss());
            //                return SimpleSelector.PseudoClass(el => !sel.Match(el), code);
            //            }
            //        }
            //        break;
            //}

            if (!_ignoreErrors)
            {
                var a = 1;
            
                //throw new DOMException(ErrorCode.SyntaxError);
}
            return null;
        }

        /// <summary>
        /// Invoked once two colons has been found in the token enumerator.
        /// </summary>
        /// <param name="tokens">The token source.</param>
        /// <returns>The created selector.</returns>
        public SimpleSelector GetPseudoElement(IEnumerator<Block> tokens)
        {
            if (tokens.MoveNext() && tokens.Current.Type == GrammarSegment.Ident)
            {
                var data = ((SymbolBlock)tokens.Current).Value;

                switch (data)
                {
                    case PseudoelementBefore:
                        return SimpleSelector.PseudoElement(MatchBefore, PseudoelementBefore);
                    
                    case PseudoelementAfter:
                        return SimpleSelector.PseudoElement(MatchAfter, PseudoelementAfter);
                    
                    case PseudoelementSelection:
                        return SimpleSelector.PseudoElement(el => true, PseudoelementSelection);
                    
                    case PseudoelementFirstline:
                        return SimpleSelector.PseudoElement(MatchFirstLine, PseudoelementFirstline);
                    
                    case PseudoelementFirstletter:
                        return SimpleSelector.PseudoElement(MatchFirstLetter, PseudoelementFirstletter);
                }
            }

            return null;
        }

        public SimpleSelector GetAttributeSelector(IEnumerator<Block> tokens)
        {
            var values = new List<string>();
            Block op = null;

            while (tokens.MoveNext())
            {
                if (tokens.Current.Type == GrammarSegment.SquareBracketClose)
                {
                    break;
                }

                if (tokens.Current.Type == GrammarSegment.Ident)
                {
                    values.Add(((SymbolBlock)tokens.Current).Value);
                }
                
                else if (tokens.Current.Type == GrammarSegment.String)
                {
                    values.Add(((StringBlock)tokens.Current).Data);
                }

                else if (tokens.Current.Type == GrammarSegment.Number)
                {
                    values.Add(((NumericBlock)tokens.Current).Data.ToString());
                }

                else if (op == null && (tokens.Current is MatchBlock || tokens.Current.Type == GrammarSegment.Delimiter))
                {
                    op = tokens.Current;
                }
                else if (tokens.Current.Type != GrammarSegment.Whitespace)
                {
                    //if (!ignoreErrors) throw new DOMException(ErrorCode.SyntaxError);
                    return null;
                }
            }

            if ((op == null || values.Count != 2) && (op != null || values.Count != 1))
            {
                if (!_ignoreErrors)
                {
                    var a = 1;
                    //throw new DOMException(ErrorCode.SyntaxError);
                }
                return null;
            }

            if (op == null)
            {
                return SimpleSelector.AttrAvailable(values[0]);
            }

            switch (op.ToValue())
            {
                case "=":
                    return SimpleSelector.AttrMatch(values[0], values[1]);
                case "~=":
                    return SimpleSelector.AttrList(values[0], values[1]);
                case "|=":
                    return SimpleSelector.AttrHyphen(values[0], values[1]);
                case "^=":
                    return SimpleSelector.AttrBegins(values[0], values[1]);
                case "$=":
                    return SimpleSelector.AttrEnds(values[0], values[1]);
                case "*=":
                    return SimpleSelector.AttrContains(values[0], values[1]);
                case "!=":
                    return SimpleSelector.AttrNotMatch(values[0], values[1]);
            }

            if (!_ignoreErrors)
            {
                var a = 1;
 
                //throw new DOMException(ErrorCode.SyntaxError);
            }
            return null;
        }

        T GetArguments<T>(IEnumerator<Block> it) where T : NthChildSelector, new()
        {
            var f = new T();
            var repr = string.Empty;

            while (it.MoveNext())
            {
                switch (it.Current.Type)
                {
                    case GrammarSegment.Ident:
                    case GrammarSegment.Number:
                    case GrammarSegment.Dimension:
                    case GrammarSegment.Whitespace:
                        repr += it.Current.ToValue();
                        break;

                    case GrammarSegment.Delimiter:
                        var chr = ((DelimBlock)it.Current).Value;

                        if (chr == Specification.PLUS || chr == Specification.MINUS)
                        {
                            repr += chr;
                            break;
                        }

                        goto default;

                    default:
                        if (!_ignoreErrors)
                        {
                            var a = 1;
                            //throw new DOMException(ErrorCode.SyntaxError);
                        }
                        return f;
                }
            }

            repr = repr.Trim();

            if (repr.Equals(NthChildOdd, StringComparison.OrdinalIgnoreCase))
            {
                f.step = 2;
                f.offset = 1;
            }
            else if (repr.Equals(NthChildEven, StringComparison.OrdinalIgnoreCase))
            {
                f.step = 2;
                f.offset = 0;
            }
            else if (!int.TryParse(repr, out f.offset))
            {
                var index = repr.IndexOf(NthChildN, StringComparison.OrdinalIgnoreCase);

                if (repr.Length > 0 && index != -1)
                {
                    var first = repr.Substring(0, index).Replace(" ", "");
                    var second = repr.Substring(index + 1).Replace(" ", "");

                    if (first == string.Empty || (first.Length == 1 && first[0] == Specification.PLUS))
                    {
                        f.step = 1;
                    }
                    else if (first.Length == 1 && first[0] == Specification.MINUS)
                    {
                        f.step = -1;
                    }
                    else if (!int.TryParse(first, out f.step))
                    {
                        var a = 1;
                        //throw new DOMException(ErrorCode.SyntaxError);
                    }

                    if (second == string.Empty)
                    {
                        f.offset = 0;
                    }
                    else if (!int.TryParse(second, out f.offset) && !_ignoreErrors)
                    {
                        var a = 1;
                        //throw new DOMException(ErrorCode.SyntaxError);
                    }
                }
                else if (!_ignoreErrors)
                {
                    var a = 1;
                    //throw new DOMException(ErrorCode.SyntaxError);
                }
            }

            return f;
        }

        
        static bool MatchBefore(Element element)
        {
            //TODO
            return true;
        }

        static bool MatchAfter(Element element)
        {
            //TODO
            return true;
        }

        static bool MatchFirstLine(Element element)
        {
            //TODO
            return true;
        }

        static bool MatchFirstLetter(Element element)
        {
            //TODO
            return true;
        }

        class NthChildSelector : SimpleSelector
        {
            public int step;
            public int offset;

            public override int Specifity
            {
                get { return 10; }
            }

            public override bool Match(Element element)
            {
                var parent = element.ParentNode;

                if (parent == null)
                {
                    return false;
                }

                Debugger.Break();

                //for (int i = 0; i < parent.ChildNodes.Length; i++)
                //{
                //    if (parent.ChildNodes[i] == element)
                //        return step == 0 ? n == offset : (n - offset) % step == 0;
                //    else if (parent.ChildNodes[i] is Element)
                //        n++;
                //}

                return true;
            }

            public override string ToString()
            {
                return string.Format(":{0}({1}n+{2})", PseudoclassfunctionNthchild, step, offset);
            }
        }

        class NthLastChildSelector : NthChildSelector
        {
            public override bool Match(Element element)
            {
                var parent = element.ParentElement;

                if (parent == null)
                {
                    return false;
                }

                Debugger.Break();

                //for (int i = parent.ChildNodes.Length - 1; i >= 0; i--)
                //{
                //    if (parent.ChildNodes[i] == element)
                //        return step == 0 ? n == offset : (n - offset) % step == 0;
                //    else if (parent.ChildNodes[i] is Element)
                //        n++;
                //}

                return true;
            }

            public override string ToString()
            {
                return string.Format(":{0}({1}n+{2})", PseudoclassfunctionNthlastchild, step, offset);
            }
        }

        class FirstChildSelector : SimpleSelector
        {
            private FirstChildSelector()
            { }

            static FirstChildSelector instance;

            public static FirstChildSelector Instance
            {
                get { return instance ?? (instance = new FirstChildSelector()); }
            }

            public override int Specifity
            {
                get { return 10; }
            }

            public override bool Match(Element element)
            {
                var parent = element.ParentNode;

                if (parent == null)
                {
                    return false;
                }

                Debugger.Break();
                //for (int i = 0; i <= parent.ChildNodes.Length; i++)
                //{
                //    if (parent.ChildNodes[i] == element)
                //        return true;
                //    else if (parent.ChildNodes[i] is Element)
                //        return false;
                //}

                return false;
            }

            public override string ToString()
            {
                return ":" + PseudoclassFirstchild;
            }
        }

        class LastChildSelector : SimpleSelector
        {
            private LastChildSelector()
            { }

            static LastChildSelector instance;

            public static LastChildSelector Instance
            {
                get { return instance ?? (instance = new LastChildSelector()); }
            }

            public override int Specifity
            {
                get { return 10; }
            }

            public override bool Match(Element element)
            {
                var parent = element.ParentElement;

                if (parent == null)
                {
                    return false;
                }

                Debugger.Break();
                //for (int i = parent.ChildNodes.Length - 1; i >= 0; i--)
                //{
                //    if (parent.ChildNodes[i] == element)
                //        return true;
                //    else if (parent.ChildNodes[i] is Element)
                //        return false;
                //}

                return false;
            }

            public override string ToString()
            {
                return ":" + PseudoclassLastchild;
            }
        }
    }
}
