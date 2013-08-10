using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace ExCSS.Model
{
    public class Parser
    {
        //private bool _started;
        private readonly Lexer _lexer;
        private StyleSheet _stylesheet;
        //private TaskCompletionSource<bool> tcs;
        private readonly StringBuilder _readBuffer;
        private Stack<Ruleset> _activeRules;
        private bool ignore;
        //public event EventHandler<ParseErrorEventArgs> ErrorOccurred;
        
        public Parser(string source) : this(new SourceManager(source))
        {
        }
        
        public Parser(Stream stream) : this(new SourceManager(stream))
        {
        }
        
        internal Parser(SourceManager source)
        {
            ignore = true;
            _readBuffer = new StringBuilder();
            _lexer = new Lexer(source);

            //_lexer.ErrorOccurred += (s, ev) =>
            //{
            //    if (ErrorOccurred != null)
            //        ErrorOccurred(this, ev);
            //};

            //_started = false;
            
            _activeRules = new Stack<Ruleset>();
        }

        public bool IsQuirksMode { get; set; }

        internal Ruleset CurrentRule
        {
            get { return _activeRules.Count > 0 ? _activeRules.Peek() : null; }
        }

        public void Parse()
        {
            _stylesheet = new StyleSheet();
            AppendRules(_stylesheet.Ruleset); 
        }

        private void AppendRules(List<Ruleset> rules)
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
                        _stylesheet.AtRules.Add(CreateAtRule(source));
                        break;

                    default:
                        rules.Add(CreateRuleset(source));
                        break;
                }
            }
        }

        private void AppendDeclarations(IEnumerator<Block> source, List<Property> declarations)
        {
            while (source.MoveNext())
            {
                switch (source.Current.Type)
                {
                    case GrammarSegment.Whitespace:
                    case GrammarSegment.Semicolon:
                        break;

                    case GrammarSegment.Ident:
                        var tokens = LimitToSemicolon(source);
                        var it = tokens.GetEnumerator();
                        it.MoveNext();
                        var decl = CreateDeclaration(it);

                        if (decl != null)
                            declarations.Add(decl);

                        break;

                    default:
                        //RaiseErrorOccurred(ErrorCode.InvalidCharacter);
                        SkipToNextSemicolon(source);
                        break;
                }
            }
        }
     
        private void AppendMediaList(IEnumerator<Block> source, MediaList media, GrammarSegment endToken = GrammarSegment.Semicolon)
        {
            do
            {
                if (source.Current.Type == GrammarSegment.Whitespace)
                {
                    continue;
                }
                 
                if (source.Current.Type == endToken)
                 {
                     break;
                 }

                do
                {
                    if (source.Current.Type == GrammarSegment.Comma || source.Current.Type == endToken)
                    {
                        break;
                    }
                    
                    if (source.Current.Type == GrammarSegment.Whitespace)
                    {
                        _readBuffer.Append(' ');
                    }
                    else
                    {
                        _readBuffer.Append(source.Current.ToValue());
                    }
                }
                while (source.MoveNext());

                media.AppendMedium(_readBuffer.ToString());
                _readBuffer.Clear();

                if (source.Current.Type == endToken)
                {
                    break;
                }
            }
            while (source.MoveNext());
        }

        private List<ValueList> CreateMultipleValues(IEnumerator<Block> source)
        {
            var values = new List<ValueList>();

            do
            {
                var list = CreateValueList(source);

                if (list.Length > 0)
                {
                    values.Add(list);
                }
            }
            while (source.Current != null && source.Current.Type == GrammarSegment.Comma);

            return values;
        }

        private ValueList CreateValueList(IEnumerator<Block> source)
        {
            var list = new List<Value>();

            while (SkipToNextNonWhitespace(source))
            {
                if (source.Current.Type == GrammarSegment.Semicolon)
                {
                    break;
                }
                
                if (source.Current.Type == GrammarSegment.Comma)
                {
                    break;
                }

                var value = CreateValue(source);

                if (value == null)
                {
                    SkipToNextSemicolon(source);
                    break;
                }

                list.Add(value);
            }

            return new ValueList(list);
        }

        private Value CreateValue(IEnumerator<Block> source)
        {
            Value value = null;

            switch (source.Current.Type)
            {
                case GrammarSegment.String:// 'i am a string'
                    value = new PrimitiveValue(UnitType.String, ((StringBlock)source.Current).Data);
                    break;

                case GrammarSegment.Url:// url('this is a valid URL')
                    value = new PrimitiveValue(UnitType.Uri, ((StringBlock)source.Current).Data);
                    break;

                case GrammarSegment.Ident: // ident
                    value = new PrimitiveValue(UnitType.Ident, ((SymbolBlock)source.Current).Value);
                    break;

                case GrammarSegment.Percentage: // 5%
                    value = new PrimitiveValue(UnitType.Percentage, ((UnitBlock)source.Current).Data);
                    break;

                case GrammarSegment.Dimension: // 3px
                    value = new PrimitiveValue(((UnitBlock)source.Current).Unit, ((UnitBlock)source.Current).Data);
                    break;

                case GrammarSegment.Number: // 173
                    value = new PrimitiveValue(UnitType.Number, ((NumericBlock)source.Current).Data);
                    break;

                case GrammarSegment.Hash: // #string
                    HtmlColor color;

                    if (HtmlColor.TryFromHex(((SymbolBlock) source.Current).Value, out color))
                    {
                        value = new PrimitiveValue(color);
                    }

                    break;

                case GrammarSegment.Delimiter: // e.g. #0F3, #012345, ...
                    if (((DelimBlock)source.Current).Value == '#')
                    {
                        string hash = String.Empty;

                        while (source.MoveNext())
                        {
                            var stop = false;

                            switch (source.Current.Type)
                            {
                                case GrammarSegment.Number:
                                case GrammarSegment.Dimension:
                                case GrammarSegment.Ident:
                                    var rest = source.Current.ToValue();

                                    if (hash.Length + rest.Length <= 6)
                                    {
                                        hash += rest;
                                    }
                                    else
                                    {
                                        stop = true;
                                    }

                                    break;

                                default:
                                    stop = true;
                                    break;
                            }

                            if (stop || hash.Length == 6)
                            {
                                break;
                            }
                        }

                        if (HtmlColor.TryFromHex(hash, out color))
                        {
                            value = new PrimitiveValue(color);
                        }
                    }
                    break;

                case GrammarSegment.Function: // rgba(255, 255, 20, 0.5)
                    value = CreateFunction(source);
                    break;
            }

            return value;
        }
      
        Function CreateFunction(IEnumerator<Block> source)
        {
            var name = ((SymbolBlock)source.Current).Value;
            var args = new ValueList();

            while (source.MoveNext())
            {
                if (source.Current.Type == GrammarSegment.ParenClose)
                    break;
            }

            return Function.Create(name, args);
        }

        StyleRule CreateRuleset(IEnumerator<Block> source)
        {
            var style = new StyleRule();
            var ctor = new SelectorConstructor {IgnoreErrors = ignore};
            
            style.ParentRule = CurrentRule;
            _activeRules.Push(style);

            do
            {
                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (SkipToNextNonWhitespace(source))
                    {
                        var tokens = LimitToCurrentBlock(source);
                        AppendDeclarations(tokens.GetEnumerator(), style.Style.List);
                    }

                    break;
                }

                ctor.PickSelector(source);
            }
            while (source.MoveNext());

            style.Selector = ctor.Result;
            _activeRules.Pop();
            return style;
        }

        /// <summary>
        /// Creates a @-rule from the given source.
        /// </summary>
        /// <param name="source">The token iterator.</param>
        /// <returns>The @-rule.</returns>
        Ruleset CreateAtRule(IEnumerator<Block> source)
        {
            var name = ((SymbolBlock)source.Current).Value;
            SkipToNextNonWhitespace(source);

            switch (name)
            {
                case MediaRule.RuleName:
                    return CreateMediaRule(source);

                case PageRule.RuleName: 
                    return CreatePageRule(source);

                case ImportRule.RuleName: 
                    return CreateImportRule(source);

                case FontFaceRule.RuleName: 
                    return CreateFontFaceRule(source);

                case CharsetRule.RuleName: 
                    return CreateCharsetRule(source);

                case NamespaceRule.RuleName: 
                    return CreateNamespaceRule(source);

                case SupportsRule.RuleName: 
                    return CreateSupportsRule(source);

                case KeyframesRule.RuleName: 
                    return CreateKeyframesRule(source);

                default: 
                    return CreateUnknownRule(name, source);
            }
        }

        /// <summary>
        /// Creates a new property from the given source.
        /// </summary>
        /// <param name="source">The token iterator starting at the name of the property.</param>
        /// <returns>The new property.</returns>
        Property CreateDeclaration(IEnumerator<Block> source)
        {
            string name = ((SymbolBlock)source.Current).Value;
            Property property = null;
            Value value = Value.Inherit;
            bool hasValue = SkipToNextNonWhitespace(source) && source.Current.Type == GrammarSegment.Colon;

            if (hasValue)
                value = CreateValueList(source);

            //TODO
            switch (name)
            {
                //case "azimuth":
                //case "animation":
                //case "animation-delay":
                //case "animation-direction":
                //case "animation-duration":
                //case "animation-fill-mode":
                //case "animation-iteration-count":
                //case "animation-name":
                //case "animation-play-state":
                //case "animation-timing-function":
                //case "background-attachment":
                //case "background-color":
                //case "background-clip":
                //case "background-origin":
                //case "background-size":
                //case "background-image":
                //case "background-position":
                //case "background-repeat":
                //case "background":
                //case "border-color":
                //case "border-spacing":
                //case "border-collapse":
                //case "border-style":
                //case "border-radius":
                //case "box-shadow":
                //case "box-decoration-break":
                //case "break-after":
                //case "break-before":
                //case "break-inside":
                //case "backface-visibility":
                //case "border-top-left-radius":
                //case "border-top-right-radius":
                //case "border-bottom-left-radius":
                //case "border-bottom-right-radius":
                //case "border-image":
                //case "border-image-outset":
                //case "border-image-repeat":
                //case "border-image-source":
                //case "border-image-slice":
                //case "border-image-width":
                //case "border-top":
                //case "border-right":
                //case "border-bottom":
                //case "border-left":
                //case "border-top-color":
                //case "border-left-color":
                //case "border-right-color":
                //case "border-bottom-color":
                //case "border-top-style":
                //case "border-left-style":
                //case "border-right-style":
                //case "border-bottom-style":
                //case "border-top-width":
                //case "border-left-width":
                //case "border-right-width":
                //case "border-bottom-width":
                //case "border-width":
                //case "border":
                //case "bottom":
                //case "columns":
                //case "column-count":
                //case "column-fill":
                //case "column-gap":
                //case "column-rule-color":
                //case "column-rule-style":
                //case "column-rule-width":
                //case "column-span":
                //case "column-width":
                //case "caption-side":
                //case "clear":
                //case "clip":
                //case "color":
                //case "content":
                //case "counter-increment":
                //case "counter-reset":
                //case "cue-after":
                //case "cue-before":
                //case "cue":
                //case "cursor":
                //case "direction":
                //case "display":
                //case "elevation":
                //case "empty-cells":
                //case "float":
                //case "font-family":
                //case "font-size":
                //case "font-style":
                //case "font-variant":
                //case "font-weight":
                //case "font":
                //case "height":
                //case "left":
                //case "letter-spacing":
                //case "line-height":
                //case "list-style-image":
                //case "list-style-position":
                //case "list-style-type":
                //case "list-style":
                //case "marquee-direction":
                //case "marquee-play-count":
                //case "marquee-speed":
                //case "marquee-style":
                //case "margin-right":
                //case "margin-left":
                //case "margin-top":
                //case "margin-bottom":
                //case "margin":
                //case "max-height":
                //case "max-width":
                //case "min-height":
                //case "min-width":
                //case "opacity":
                //case "orphans":
                //case "outline-color":
                //case "outline-style":
                //case "outline-width":
                //case "outline":
                //case "overflow":
                //case "padding-top":
                //case "padding-right":
                //case "padding-left":
                //case "padding-bottom":
                //case "padding":
                //case "page-break-after":
                //case "page-break-before":
                //case "page-break-inside":
                //case "pause-after":
                //case "pause-before":
                //case "pause":
                //case "perspective":
                //case "perspective-origin":
                //case "pitch-range":
                //case "pitch":
                //case "play-during":
                //case "position":
                //case "quotes":
                //case "richness":
                //case "right":
                //case "speak-header":
                //case "speak-numeral":
                //case "speak-punctuation":
                //case "speak":
                //case "speech-rate":
                //case "stress":
                //case "table-layout":
                //case "text-align":
                //case "text-decoration":
                //case "text-indent":
                //case "text-transform":
                //case "transform":
                //case "transform-origin":
                //case "transform-style":
                //case "transition":
                //case "transition-delay":
                //case "transition-duration":
                //case "transition-timing-function":
                //case "transition-property":
                //case "top":
                //case "unicode-bidi":
                //case "vertical-align":
                //case "visibility":
                //case "voice-family":
                //case "volume":
                //case "white-space":
                //case "widows":
                //case "width":
                //case "word-spacing":
                //case "z-index":
                default:
                    property = new Property(name) {Value = value};
                    break;
            }

            if (hasValue && source.Current.Type == GrammarSegment.Delimiter && ((DelimBlock)source.Current).Value == Specification.EM && SkipToNextNonWhitespace(source))
                property.Important = source.Current.Type == GrammarSegment.Ident && ((SymbolBlock)source.Current).Value.Equals("important", StringComparison.OrdinalIgnoreCase);

            SkipBehindNextSemicolon(source);
            return property;
        }
      
        GenericRule CreateUnknownRule(string name, IEnumerator<Block> source)
        {
            var rule = new GenericRule();
            var endCurly = 0;
         
            rule.ParentRule = CurrentRule;
            _activeRules.Push(rule);
            _readBuffer.Append(name).Append(" ");

            do
            {
                if (source.Current.Type == GrammarSegment.Semicolon && endCurly == 0)
                {
                    source.MoveNext();
                    break;
                }

                _readBuffer.Append(source.Current.ToString());

                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                    endCurly++;
                else if (source.Current.Type == GrammarSegment.CurlyBracketClose && --endCurly == 0)
                    break;
            }
            while (source.MoveNext());

            rule.SetText(_readBuffer.ToString());
            _readBuffer.Clear();
            _activeRules.Pop();
            return rule;
        }

       
        KeyframesRule CreateKeyframesRule(IEnumerator<Block> source)
        {
            var keyframes = new KeyframesRule();
           
            keyframes.ParentRule = CurrentRule;
            _activeRules.Push(keyframes);

            if (source.Current.Type == GrammarSegment.Ident)
            {
                keyframes.Name = ((SymbolBlock)source.Current).Value;
                SkipToNextNonWhitespace(source);

                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    SkipToNextNonWhitespace(source);
                    var tokens = LimitToCurrentBlock(source).GetEnumerator();

                    while (SkipToNextNonWhitespace(tokens))
                    {
                        //keyframes.Rules.List.Add(CreateKeyframeRule(tokens));
                        keyframes.Rules.Add(CreateKeyframeRule(tokens));
                    }
                }
            }

            _activeRules.Pop();
            return keyframes;
        }

       
        KeyframeRule CreateKeyframeRule(IEnumerator<Block> source)
        {
            var keyframe = new KeyframeRule {ParentRule = CurrentRule};

            _activeRules.Push(keyframe);

            do
            {
                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (SkipToNextNonWhitespace(source))
                    {
                        var tokens = LimitToCurrentBlock(source);
                        AppendDeclarations(tokens.GetEnumerator(), keyframe.Style.List);
                    }

                    break;
                }

                _readBuffer.Append(source.Current);
            }
            while (source.MoveNext());

            keyframe.KeyText = _readBuffer.ToString();
            _readBuffer.Clear();
            _activeRules.Pop();
            return keyframe;
        }

        SupportsRule CreateSupportsRule(IEnumerator<Block> source)
        {
            var supports = new SupportsRule();
        
            supports.ParentRule = CurrentRule;
            _activeRules.Push(supports);

            do
            {
                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (SkipToNextNonWhitespace(source))
                    {
                        var tokens = LimitToCurrentBlock(source);
                        AppendRules(/*tokens,*/ supports.Rules);//.List);
                    }

                    break;
                }

                _readBuffer.Append(source.Current);
            }
            while (source.MoveNext());

            supports.ConditionText = _readBuffer.ToString();
            _readBuffer.Clear();
            _activeRules.Pop();
            return supports;
        }

      
        NamespaceRule CreateNamespaceRule(IEnumerator<Block> source)
        {
            var ns = new NamespaceRule();
           

            if (source.Current.Type == GrammarSegment.Ident)
            {
                ns.Prefix = source.Current.ToValue();
                SkipToNextNonWhitespace(source);

                if (source.Current.Type == GrammarSegment.String)
                    ns.NamespaceURI = source.Current.ToValue();
            }

            SkipToNextSemicolon(source);
            return ns;
        }

      
        CharsetRule CreateCharsetRule(IEnumerator<Block> source)
        {
            var charset = new CharsetRule();

            if (source.Current.Type == GrammarSegment.String)
            {
                charset.Encoding = ((StringBlock)source.Current).Data;
            }

            SkipToNextSemicolon(source);
            return charset;
        }


        FontFaceRule CreateFontFaceRule(IEnumerator<Block> source)
        {
            var fontface = new FontFaceRule();
            fontface.ParentRule = CurrentRule;
            _activeRules.Push(fontface);

            if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
            {
                if (SkipToNextNonWhitespace(source))
                {
                    var tokens = LimitToCurrentBlock(source);
                    AppendDeclarations(tokens.GetEnumerator(), fontface.CssRules.List);
                }
            }

            _activeRules.Pop();
            return fontface;
        }

      
        ImportRule CreateImportRule(IEnumerator<Block> source)
        {
            var import = new ImportRule();
          
            import.ParentRule = CurrentRule;
            _activeRules.Push(import);

            switch (source.Current.Type)
            {
                case GrammarSegment.Semicolon:
                    source.MoveNext();
                    break;

                case GrammarSegment.String:
                case GrammarSegment.Url:
                    import.Href = ((StringBlock)source.Current).Data;
                    AppendMediaList(source, import.Media);
                  
                    break;

                default:
                    SkipToNextSemicolon(source);
                    break;
            }

            _activeRules.Pop();
            return import;
        }

       
        PageRule CreatePageRule(IEnumerator<Block> source)
        {
            var page = new PageRule {ParentRule = CurrentRule};

            _activeRules.Push(page);

            var ctor = new SelectorConstructor {IgnoreErrors = ignore};

            do
            {
                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (SkipToNextNonWhitespace(source))
                    {
                        var tokens = LimitToCurrentBlock(source);
                        AppendDeclarations(tokens.GetEnumerator(), page.Style.List);
                        break;
                    }
                }

                ctor.PickSelector(source);
            }
            while (source.MoveNext());

            page.Selector = ctor.Result;
            _activeRules.Pop();
            return page;
        }

    
        MediaRule CreateMediaRule(IEnumerator<Block> source)
        {
            var media = new MediaRule {ParentRule = CurrentRule};

            _activeRules.Push(media);
            AppendMediaList(source, media.Media, GrammarSegment.CurlyBraceOpen);

            if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
            {
                if (SkipToNextNonWhitespace(source))
                {
                    var tokens = LimitToCurrentBlock(source);
                    AppendRules(/*tokens, */media.Rules);//.List);
                }
            }

            _activeRules.Pop();
            return media;
        }

     


        static bool SkipToNextNonWhitespace(IEnumerator<Block> source)
        {
            while (source.MoveNext())
            {
                if (source.Current.Type != GrammarSegment.Whitespace)
                {
                    return true;
                }
            }

            return false;
        }

        static bool SkipToNextSemicolon(IEnumerator<Block> source)
        {
            do
            {
                if (source.Current.Type == GrammarSegment.Semicolon)
                {
                    return true;
                }
            }
            while (source.MoveNext());

            return false;
        }

        
        static bool SkipBehindNextSemicolon(IEnumerator<Block> source)
        {
            do
            {
                if (source.Current.Type != GrammarSegment.Semicolon)
                {
                    continue;
                }

                source.MoveNext();
                return true;
            }
            while (source.MoveNext());

            return false;
        }

        
        static IEnumerable<Block> LimitToSemicolon(IEnumerator<Block> source)
        {
            do
            {
                if (source.Current.Type == GrammarSegment.Semicolon)
                {
                    yield break;
                }

                yield return source.Current;
            }
            while (source.MoveNext());
        }

        
        static IEnumerable<Block> LimitToCurrentBlock(IEnumerator<Block> source)
        {
            var open = 1;

            do
            {
                if (source.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    open++;
                }
                else if (source.Current.Type == GrammarSegment.CurlyBracketClose && --open == 0)
                {
                    yield break;
                }

                yield return source.Current;
            }
            while (source.MoveNext());
        }


        
        public static Selector ParseSelector(string selector, bool quirksMode = false)
        {
            var parser = new Parser(selector);
            parser.IsQuirksMode = quirksMode;
            var tokens = parser._lexer.Tokens.GetEnumerator();
            var ctor = new SelectorConstructor();

            while (tokens.MoveNext())
            {
                ctor.PickSelector(tokens);
            }

            return ctor.Result;
        }

        

        
        public static StyleDeclaration ParseDeclarations(string declarations, bool quirksMode = false)
        {
            var parser = new Parser(declarations);
            parser.IsQuirksMode = quirksMode;
            parser.ignore = false;
            var it = parser._lexer.Tokens.GetEnumerator();
            var decl = new StyleDeclaration();
            parser.AppendDeclarations(it, decl.List);
            return decl;
        }

        
        public static Value ParseValue(string source, bool quirksMode = false)
        {
            var parser = new Parser(source);
            parser.IsQuirksMode = quirksMode;
            parser.ignore = false;
            var it = parser._lexer.Tokens.GetEnumerator();
            SkipToNextNonWhitespace(it);
            return parser.CreateValue(it);
        }

        
        internal static ValueList ParseValueList(string source, bool quirksMode = false)
        {
            var parser = new Parser(source);
            parser.IsQuirksMode = quirksMode;
            parser.ignore = false;
            var it = parser._lexer.Tokens.GetEnumerator();
            return parser.CreateValueList(it);
        }

        
        internal static List<ValueList> ParseMultipleValues(string source, bool quirksMode = false)
        {
            var parser = new Parser(source);
            parser.IsQuirksMode = quirksMode;
            parser.ignore = false;
            var it = parser._lexer.Tokens.GetEnumerator();
            return parser.CreateMultipleValues(it);
        }

        
        internal static KeyframeRule ParseKeyframeRule(string rule, bool quirksMode = false)
        {
            var parser = new Parser(rule);
            parser.IsQuirksMode = quirksMode;
            parser.ignore = false;
            var it = parser._lexer.Tokens.GetEnumerator();

            if (SkipToNextNonWhitespace(it))
            {
                //if (it.Current.Type == GrammarSegment.CommentOpen || it.Current.Type == GrammarSegment.CommentClose)
                // throw new DOMException(ErrorCode.SyntaxError);

                return parser.CreateKeyframeRule(it);
            }

            return null;
        }


        #region Event-Helpers

        //void RaiseErrorOccurred(ErrorCode code)
        //{
        //    if (ErrorOccurred != null)
        //    {
        //        var pck = new ParseErrorEventArgs((int)code, Errors.GetError(code));
        //        pck.Line = _lexer.Stream.Line;
        //        pck.Column = _lexer.Stream.Column;
        //        ErrorOccurred(this, pck);
        //    }
        //}

        #endregion
    }

    public abstract class BaseCollection<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }


    public sealed class NodeList : BaseCollection<object>
    {
    }


    public class Element
    {
        public NodeList ChildNodes
        {
            get { return null; }
        }

        public Element ParentNode
        {
            get
            {
                return null;
            }
        }

        public Element ParentElement
        {
            get
            {
                return null;
            }
        }

        public string TagName
        {
            get { return "tagname"; }
        }

        public string ClassList
        {
            get { return "cl"; }
        }

        public string Id
        {
            get { return "id"; }
        }


        public bool HasAttribute(string x)
        {
            return true;
        }
        public string GetAttribute(string x)
        {
            return "ga";
        }
        public DirectionMode Dir
        {
            get { return DirectionMode.Ltr; }
            set { }
            //get { return ToEnum(GetAttribute("dir"), DirectionMode.Ltr); }
            //set { SetAttribute("dir", value.ToString()); }
        }
    }


    [StructLayout(LayoutKind.Explicit, Pack = 1, CharSet = CharSet.Unicode)]
    struct HtmlColor : IEquatable<HtmlColor>
    {
        //TODO
        //http://en.wikipedia.org/wiki/Alpha_compositing

        #region Members

        [FieldOffset(0)]
        Byte alpha;
        [FieldOffset(1)]
        Byte red;
        [FieldOffset(2)]
        Byte green;
        [FieldOffset(3)]
        Byte blue;
        [FieldOffset(0)]
        Int32 hashcode;

        #endregion

        #region ctor

        /// <summary>
        /// Creates an Html color type without any transparency (alpha = 100%).
        /// </summary>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public HtmlColor(Byte r, Byte g, Byte b)
        {
            hashcode = 0;
            alpha = 255;
            red = r;
            blue = b;
            green = g;
        }

        /// <summary>
        /// Creates an Html color type.
        /// </summary>
        /// <param name="a">The alpha value.</param>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public HtmlColor(Byte a, Byte r, Byte g, Byte b)
        {
            hashcode = 0;
            alpha = a;
            red = r;
            blue = b;
            green = g;
        }

        /// <summary>
        /// Creates an Html color type.
        /// </summary>
        /// <param name="a">The alpha value between 0 and 1.</param>
        /// <param name="r">The red value.</param>
        /// <param name="g">The green value.</param>
        /// <param name="b">The blue value.</param>
        public HtmlColor(Double a, Byte r, Byte g, Byte b)
        {
            hashcode = 0;
            alpha = (Byte)Math.Max(Math.Min(Math.Ceiling(255 * a), 255), 0);
            red = r;
            blue = b;
            green = g;
        }

        #endregion

        #region Static constructors

        /// <summary>
        /// Returns the color from the given primitives.
        /// </summary>
        /// <param name="r">The value for red.</param>
        /// <param name="g">The value for green.</param>
        /// <param name="b">The value for blue.</param>
        /// <param name="a">The value for alpha.</param>
        /// <returns>The HTML color value.</returns>
        public static HtmlColor FromRgba(Byte r, Byte g, Byte b, Single a)
        {
            return new HtmlColor(a, r, g, b);
        }

        /// <summary>
        /// Returns the color from the given primitives.
        /// </summary>
        /// <param name="r">The value for red.</param>
        /// <param name="g">The value for green.</param>
        /// <param name="b">The value for blue.</param>
        /// <param name="a">The value for alpha.</param>
        /// <returns>The HTML color value.</returns>
        public static HtmlColor FromRgba(Byte r, Byte g, Byte b, Double a)
        {
            return new HtmlColor(a, r, g, b);
        }

        /// <summary>
        /// Returns the color from the given primitives without any alpha (non-transparent).
        /// </summary>
        /// <param name="r">The value for red.</param>
        /// <param name="g">The value for green.</param>
        /// <param name="b">The value for blue.</param>
        /// <returns>The HTML color value.</returns>
        public static HtmlColor FromRgb(Byte r, Byte g, Byte b)
        {
            return new HtmlColor(r, g, b);
        }

        /// <summary>
        /// Returns the color from the given hex string.
        /// </summary>
        /// <param name="color">The hex string like fff or abc123 or AA126B etc.</param>
        /// <returns>The HTML color value.</returns>
        public static HtmlColor FromHex(string color)
        {
            if (color.Length == 3)
            {
                int r = color[0].FromHex();
                r += r * 16;
                int g = color[1].FromHex();
                g += g * 16;
                int b = color[2].FromHex();
                b += b * 16;

                return new HtmlColor((byte)r, (byte)g, (byte)b);
            }
            else if (color.Length == 6)
            {
                int r = 16 * color[0].FromHex();
                int g = 16 * color[2].FromHex();
                int b = 16 * color[4].FromHex();
                r += color[1].FromHex();
                g += color[3].FromHex();
                b += color[5].FromHex();

                return new HtmlColor((byte)r, (byte)g, (byte)b);
            }

            return new HtmlColor();
        }

        /// <summary>
        /// Returns the color from the given hex string if it can be converted, otherwise
        /// the color is not set.
        /// </summary>
        /// <param name="color">The hexadecimal reresentation of the color.</param>
        /// <param name="htmlColor">The color value to be created.</param>
        /// <returns>The status if the string can be converted.</returns>
        public static bool TryFromHex(string color, out HtmlColor htmlColor)
        {
            htmlColor = new HtmlColor();
            htmlColor.alpha = 255;

            if (color.Length == 3)
            {
                if (!color[0].IsHex() || !color[1].IsHex() || !color[2].IsHex())
                    return false;

                var r = color[0].FromHex();
                r += r * 16;
                var g = color[1].FromHex();
                g += g * 16;
                var b = color[2].FromHex();
                b += b * 16;

                htmlColor.red = (Byte)r;
                htmlColor.green = (Byte)g;
                htmlColor.blue = (Byte)b;
                return true;
            }
            else if (color.Length == 6)
            {
                if (!color[0].IsHex() || !color[1].IsHex() || !color[2].IsHex() ||
                    !color[3].IsHex() || !color[4].IsHex() || !color[5].IsHex())
                    return false;

                var r = 16 * color[0].FromHex();
                var g = 16 * color[2].FromHex();
                var b = 16 * color[4].FromHex();
                r += color[1].FromHex();
                g += color[3].FromHex();
                b += color[5].FromHex();

                htmlColor.red = (Byte)r;
                htmlColor.green = (Byte)g;
                htmlColor.blue = (Byte)b;
                return true;
            }

            return false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the Int32 value of the color.
        /// </summary>
        public Int32 Value
        {
            get { return hashcode; }
        }

        /// <summary>
        /// Gets the alpha part of the color.
        /// </summary>
        public Byte A
        {
            get { return alpha; }
        }

        /// <summary>
        /// Gets the alpha part of the color in percent (0..1).
        /// </summary>
        public Double Alpha
        {
            get { return alpha / 255.0; }
        }

        /// <summary>
        /// Gets the red part of the color.
        /// </summary>
        public Byte R
        {
            get { return red; }
        }

        /// <summary>
        /// Gets the green part of the color.
        /// </summary>
        public Byte G
        {
            get { return green; }
        }

        /// <summary>
        /// Gets the blue part of the color.
        /// </summary>
        public Byte B
        {
            get { return blue; }
        }

        #endregion

        #region Operators

        public static bool operator ==(HtmlColor a, HtmlColor b)
        {
            return a.hashcode == b.hashcode;
        }

        public static bool operator !=(HtmlColor a, HtmlColor b)
        {
            return a.hashcode != b.hashcode;
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Tests if another object is equal to this object.
        /// </summary>
        /// <param name="obj">The object to test with.</param>
        /// <returns>True if the two objects are equal, otherwise false.</returns>
        public override bool Equals(Object obj)
        {
            if (obj is HtmlColor)
                return this.Equals((HtmlColor)obj);

            return false;
        }

        /// <summary>
        /// Returns a hash code that defines the current color.
        /// </summary>
        /// <returns>The integer value of the hashcode.</returns>
        public override Int32 GetHashCode()
        {
            return hashcode;
        }

        /// <summary>
        /// Returns a string representing the color.
        /// </summary>
        /// <returns>The RGBA string.</returns>
        public override string ToString()
        {
            return String.Format("rgba({0}, {1}, {2}, {3})", red, green, blue, alpha / 255.0);
        }

        #endregion

        #region Implementing Interface

        public bool Equals(HtmlColor other)
        {
            return this.hashcode == other.hashcode;
        }

        #endregion

        #region string representation

        /// <summary>
        /// Returns a string containing the CSS code.
        /// </summary>
        /// <returns>The string with the rgb or rgba code.</returns>
        public string ToCss()
        {
            if (alpha == 255)
                return "rgb(" + red.ToString() + ", " + green.ToString() + ", " + blue.ToString() + ")";

            return "rgba(" + red.ToString() + ", " + green.ToString() + ", " + blue.ToString() + ", " + Alpha.ToString() + ")";
        }

        /// <summary>
        /// Returns a string containing the HTML (hex) code.
        /// </summary>
        /// <returns>The string with the hex code color.</returns>
        public string ToHtml()
        {
            return "#" + red.ToHex() + green.ToHex() + blue.ToHex();
        }

        #endregion
    }

    static class CharacterExtensions
    {
        /// <summary>
        /// Examines if a the given list of characters contains a certain element.
        /// </summary>
        /// <param name="list">The list of characters.</param>
        /// <param name="element">The element to search for.</param>
        /// <returns>The status of the check.</returns>
        //[DebuggerStepThrough]
        public static bool Contains(this IEnumerable<Char> list, Char element)
        {
            foreach (var entry in list)
                if (entry == element)
                    return true;

            return false;
        }

        /// <summary>
        /// Returns a value indicating whether the specified object occurs within this string.
        /// This method might seem obsolete, but it is quite useful in case of porting
        /// AngleSharp to a PCL, where string instances to not have a Contains method.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <param name="content">The string to seek.</param>
        /// <returns>True if the value parameter occurs within this string, or if value is the empty string.</returns>
        //[DebuggerStepThrough]
        public static bool Contains(this string str, string content)
        {
            return str.IndexOf(content) >= 0;
        }

        /// <summary>
        /// Collapses and strips all spaces in the given string.
        /// </summary>
        /// <param name="str">The string to collapse and strip.</param>
        /// <returns>The modified string with collapsed and stripped spaces.</returns>
        //[DebuggerStepThrough]
        public static string CollapseAndStrip(this string str)
        {
            var chars = new List<Char>();
            var hasSpace = true;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].IsSpaceCharacter())
                {
                    if (hasSpace)
                        continue;

                    chars.Add(Specification.SPACE);
                    hasSpace = true;
                }
                else
                {
                    hasSpace = false;
                    chars.Add(str[i]);
                }
            }

            if (hasSpace && chars.Count > 0)
                chars.RemoveAt(chars.Count - 1);

            return new String(chars.ToArray());
        }

        /// <summary>
        /// Collapses all spaces in the given string.
        /// </summary>
        /// <param name="str">The string to collapse.</param>
        /// <returns>The modified string with collapsed spaces.</returns>
        //[DebuggerStepThrough]
        public static string Collapse(this string str)
        {
            var chars = new List<Char>();
            var hasSpace = false;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i].IsSpaceCharacter())
                {
                    if (hasSpace)
                        continue;

                    chars.Add(Specification.SPACE);
                    hasSpace = true;
                }
                else
                {
                    hasSpace = false;
                    chars.Add(str[i]);
                }
            }

            return new String(chars.ToArray());
        }

        /// <summary>
        /// Examines if a the given list of string contains a certain element.
        /// </summary>
        /// <param name="list">The list of strings.</param>
        /// <param name="element">The element to search for.</param>
        /// <returns>The status of the check.</returns>
        //[DebuggerStepThrough]
        public static bool Contains(this String[] list, string element)
        {
            for (int i = 0; i < list.Length; i++)
                if (list[i] == element)
                    return true;

            return false;
        }

        /// <summary>
        /// Examines if the given element is equal to one of the given elements.
        /// </summary>
        /// <param name="element">The element to check for equality.</param>
        /// <param name="elements">The allowed (equal) elements.</param>
        /// <returns>True if the element is equal to one of the elements, otherwise false.</returns>
        //[DebuggerStepThrough]
        public static bool IsOneOf(this string element, params String[] elements)
        {
            for (var i = 0; i != elements.Length; i++)
                if (element.Equals(elements[i]))
                    return true;

            return false;
        }

        /// <summary>
        /// Examines if the given element is one of the table elements (table, tbody, tfoot, thead, tr).
        /// </summary>
        /// <param name="node">The node to examine</param>
        /// <returns>True if the element is equal to one of the elements, otherwise false.</returns>
        ////[DebuggerStepThrough]
        //public static bool IsTableElement(this Node node)
        //{
        //    return (node is HTMLTableElement || node is HTMLTableSectionElement || node is HTMLTableRowElement);
        //}

        /// <summary>
        /// Examines if the given element is one of the table elements (table, tbody, tfoot, thead, tr).
        /// </summary>
        /// <param name="tagName">The tag name to examine</param>
        /// <returns>True if the element is equal to one of the elements, otherwise false.</returns>
        ////[DebuggerStepThrough]
        //public static bool IsTableElement(this string tagName)
        //{
        //    return (tagName == HTMLTableElement.Tag || tagName == HTMLTableSectionElement.BodyTag ||
        //        tagName == HTMLTableSectionElement.FootTag || tagName == HTMLTableSectionElement.HeadTag ||
        //        tagName == HTMLTableRowElement.Tag);
        //}

        /// <summary>
        /// Examines if the given tag name matches one of the elements (tbody, tfoot, thead).
        /// </summary>
        /// <param name="tagName">The tag name to examine</param>
        /// <returns>True if the element is equal to one of the elements, otherwise false.</returns>
        ////[DebuggerStepThrough]
        //public static bool IsTableSectionElement(this string tagName)
        //{
        //    return (tagName == HTMLTableSectionElement.BodyTag || tagName == HTMLTableSectionElement.FootTag ||
        //        tagName == HTMLTableSectionElement.HeadTag);
        //}

        /// <summary>
        /// Examines if the given tag name matches one of the elements (td, th).
        /// </summary>
        /// <param name="tagName">The tag name to examine</param>
        /// <returns>True if the element is equal to one of the elements, otherwise false.</returns>
        ////[DebuggerStepThrough]
        //public static bool IsTableCellElement(this string tagName)
        //{
        //    return (tagName == HTMLTableCellElement.NormalTag || tagName == HTMLTableCellElement.HeadTag);
        //}

        /// <summary>
        /// Examines if the given tag name matches one of the elements (caption, col, colgroup, tbody, tfoot, thead).
        /// </summary>
        /// <param name="tagName">The tag name to examine</param>
        /// <param name="includeRow">True if the tr element should also be tested.</param>
        /// <returns>True if the element is equal to one of the elements, otherwise false.</returns>
        ////[DebuggerStepThrough]
        //public static bool IsGeneralTableElement(this string tagName, bool includeRow = false)
        //{
        //    return (tagName == HTMLTableCaptionElement.Tag || tagName == HTMLTableColElement.ColTag ||
        //        tagName == HTMLTableColElement.ColgroupTag || tagName == HTMLTableSectionElement.BodyTag ||
        //        tagName == HTMLTableSectionElement.FootTag || tagName == HTMLTableSectionElement.HeadTag) || (includeRow && tagName == HTMLTableRowElement.Tag);
        //}

        /// <summary>
        /// Examines if the given tag name matches one of the elements (body, caption, col, colgroup, html, td, th).
        /// </summary>
        /// <param name="tagName">The tag name to examine</param>
        /// <param name="includeRow">True if the tr element should also be tested.</param>
        /// <returns>True if the element is equal to one of the elements, otherwise false.</returns>
        ////[DebuggerStepThrough]
        //public static bool IsSpecialTableElement(this string tagName, bool includeRow = false)
        //{
        //    return (tagName == HTMLBodyElement.Tag || tagName == HTMLHtmlElement.Tag ||
        //        tagName == HTMLTableColElement.ColgroupTag || tagName == HTMLTableColElement.ColTag ||
        //        tagName == HTMLTableCellElement.HeadTag || tagName == HTMLTableCellElement.NormalTag ||
        //        tagName == HTMLTableCaptionElement.Tag) || (includeRow && tagName == HTMLTableRowElement.Tag);
        //}

        /// <summary>
        /// Examines if the given tag name matches one of the elements (html, body, br).
        /// </summary>
        /// <param name="tagName">The tag name to examine</param>
        /// <param name="includeHead">True if the head element should also be tested.</param>
        /// <returns>True if the element is equal to one of the elements, otherwise false.</returns>
        ////[DebuggerStepThrough]
        //public static bool IsHtmlBodyOrBreakRowElement(this string tagName, bool includeHead = false)
        //{
        //    return (tagName == HTMLHtmlElement.Tag || tagName == HTMLBodyElement.Tag ||
        //        tagName == HTMLBRElement.Tag) || (includeHead && tagName == HTMLHeadElement.Tag);
        //}

        /// <summary>
        /// Converts the given char (should be A-Z) to a lowercase version.
        /// </summary>
        /// <param name="chr">The uppercase char.</param>
        /// <returns>The lowercase char of A-Z, otherwise undefined.</returns>
        //[DebuggerStepThrough]
        public static Char ToLower(this Char chr)
        {
            return (char)(chr + 0x20);
        }

        /// <summary>
        /// Converts a given character from the hex representation (0-9A-Fa-f) to an integer.
        /// </summary>
        /// <param name="c">The character to convert.</param>
        /// <returns>The integer value or undefined behavior if invalid.</returns>
        //[DebuggerStepThrough]
        public static Int32 FromHex(this Char c)
        {
            return c.IsDigit() ? c - 0x30 : c - (c.IsLowercaseAscii() ? 0x57 : 0x37);
        }

        /// <summary>
        /// Strips all line breaks from the given string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns>A new string, which excludes the line breaks.</returns>
        //[DebuggerStepThrough]
        public static string StripLineBreaks(this string str)
        {
            var array = str.ToCharArray();
            var shift = 0;
            var length = array.Length;

            for (var i = 0; i < length; )
            {
                array[i] = array[i + shift];

                if (array[i].IsLineBreak())
                {
                    shift++;
                    length--;
                }
                else
                    i++;
            }

            return new String(array, 0, length);
        }

        /// <summary>
        /// Strips all leading and tailing space characters from the given string.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns>A new string, which excludes the leading and tailing spaces.</returns>
        //[DebuggerStepThrough]
        public static string StripLeadingTailingSpaces(this string str)
        {
            return StripLeadingTailingSpaces(str.ToCharArray());
        }

        /// <summary>
        /// Strips all leading and tailing space characters from the given char array.
        /// </summary>
        /// <param name="array">The array of characters to examine.</param>
        /// <returns>A new string, which excludes the leading and tailing spaces.</returns>
        //[DebuggerStepThrough]
        public static string StripLeadingTailingSpaces(this Char[] array)
        {
            var start = 0;
            var end = array.Length - 1;

            while (start < array.Length && array[start].IsSpaceCharacter())
                start++;

            while (end > start && array[end].IsSpaceCharacter())
                end--;

            return new String(array, start, 1 + end - start);
        }

        /// <summary>
        /// Splits the string with the given char delimiter.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <param name="c">The delimiter character.</param>
        /// <returns>The list of tokens.</returns>
        //[DebuggerStepThrough]
        public static String[] SplitWithoutTrimming(this string str, Char c)
        {
            return SplitWithoutTrimming(str.ToCharArray(), c);
        }

        /// <summary>
        /// Splits the char array with the given char delimiter.
        /// </summary>
        /// <param name="chars">The char array to examine.</param>
        /// <param name="c">The delimiter character.</param>
        /// <returns>The list of tokens.</returns>
        //[DebuggerStepThrough]
        public static String[] SplitWithoutTrimming(this Char[] chars, Char c)
        {
            var list = new List<String>();
            var index = 0;

            for (var i = 0; i < chars.Length; i++)
            {
                if (chars[i] == c)
                {
                    if (i > index)
                        list.Add(new String(chars, index, i - index));

                    index = i + 1;
                }
            }

            if (chars.Length > index)
                list.Add(new String(chars, index, chars.Length - index));

            return list.ToArray();
        }

        /// <summary>
        /// Splits the string on commas.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns>The list of tokens.</returns>
        //[DebuggerStepThrough]
        public static String[] SplitCommas(this string str)
        {
            return str.SplitWithTrimming(',');
        }

        /// <summary>
        /// Splits the string on dash characters.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns>The list of tokens.</returns>
        //[DebuggerStepThrough]
        public static String[] SplitHyphens(this string str)
        {
            return SplitWithTrimming(str, Specification.MINUS);
        }

        /// <summary>
        /// Splits the string on space characters.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <returns>The list of tokens.</returns>
        //[DebuggerStepThrough]
        public static String[] SplitSpaces(this string str)
        {
            var list = new List<String>();
            var buffer = new List<Char>();
            var chars = str.ToCharArray();

            for (var i = 0; i <= chars.Length; i++)
            {
                if (i == chars.Length || chars[i].IsSpaceCharacter())
                {
                    if (buffer.Count > 0)
                    {
                        var token = buffer.ToArray().StripLeadingTailingSpaces();

                        if (token.Length != 0)
                            list.Add(token);

                        buffer.Clear();
                    }
                }
                else
                    buffer.Add(chars[i]);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Splits the string with the given char delimiter and trims the leading and tailing spaces.
        /// </summary>
        /// <param name="str">The string to examine.</param>
        /// <param name="c">The delimiter character.</param>
        /// <returns>The list of tokens.</returns>
        //[DebuggerStepThrough]
        public static String[] SplitWithTrimming(this string str, Char c)
        {
            var list = new List<String>();
            var buffer = new List<Char>();
            var chars = str.ToCharArray();

            for (var i = 0; i <= chars.Length; i++)
            {
                if (i == chars.Length || chars[i] == c)
                {
                    if (buffer.Count > 0)
                    {
                        var token = buffer.ToArray().StripLeadingTailingSpaces();

                        if (token.Length != 0)
                            list.Add(token);

                        buffer.Clear();
                    }
                }
                else
                    buffer.Add(chars[i]);
            }

            return list.ToArray();
        }

        /// <summary>
        /// Transforms the given number to a hexadecimal string.
        /// </summary>
        /// <param name="num">The number (0-255).</param>
        /// <returns>A 2 digit upper case hexadecimal string.</returns>
        //[DebuggerStepThrough]
        public static string ToHex(this Byte num)
        {
            Char[] chrs = new Char[2];
            var rem = num >> 4;
            chrs[0] = (Char)(rem + (rem < 10 ? 48 : 55));
            rem = num - 16 * rem;
            chrs[1] = (Char)(rem + (rem < 10 ? 48 : 55));
            return new String(chrs);
        }
    }

    static class Specification
    {
        #region Constants

        /// <summary>
        /// Gets the XML annotation string annotation-xml
        /// </summary>
        public const string XML_ANNOTATION = "annotation-xml";

        /// <summary>
        /// The end of file character 26.
        /// </summary>
        public const Char EOF = (char)0x1a;

        /// <summary>
        /// The tilde character ( ~ ).
        /// </summary>
        public const Char TILDE = (char)0x7e;

        /// <summary>
        /// The pipe character ( | ).
        /// </summary>
        public const Char PIPE = (char)0x7c;

        /// <summary>
        /// The null character.
        /// </summary>
        public const Char NULL = (char)0x0;

        /// <summary>
        /// The ampersand character ( &amp; ).
        /// </summary>
        public const Char AMPERSAND = (char)0x26;

        /// <summary>
        /// The number sign character ( # ).
        /// </summary>
        public const Char NUM = (char)0x23;

        /// <summary>
        /// The dollar sign character ( $ ).
        /// </summary>
        public const Char DOLLAR = (char)0x24;

        /// <summary>
        /// The semicolon sign ( ; ).
        /// </summary>
        public const Char SC = (char)0x3b;

        /// <summary>
        /// The asterisk character ( * ).
        /// </summary>
        public const Char ASTERISK = (char)0x2a;

        /// <summary>
        /// The equals sign ( = ).
        /// </summary>
        public const Char EQ = (char)0x3d;

        /// <summary>
        /// The plus sign ( + ).
        /// </summary>
        public const Char PLUS = (char)0x2b;

        /// <summary>
        /// The comma character ( , ).
        /// </summary>
        public const Char COMMA = (char)0x2c;

        /// <summary>
        /// The full stop ( . ).
        /// </summary>
        public const Char DOT = (char)0x2e;

        /// <summary>
        /// The circumflex accent ( ^ ) character.
        /// </summary>
        public const Char ACCENT = (char)0x5e;

        /// <summary>
        /// The commercial at ( @ ) character.
        /// </summary>
        public const Char AT = (char)0x40;

        /// <summary>
        /// The opening angle bracket ( LESS-THAN-SIGN ).
        /// </summary>
        public const Char LT = (char)0x3c;

        /// <summary>
        /// The closing angle bracket ( GREATER-THAN-SIGN ).
        /// </summary>
        public const Char GT = (char)0x3e;

        /// <summary>
        /// The single quote / quotation mark ( ' ).
        /// </summary>
        public const Char SQ = (char)0x27;

        /// <summary>
        /// The (double) quotation mark ( " ).
        /// </summary>
        public const Char DQ = (char)0x22;

        /// <summary>
        /// The (curved) quotation mark ( ` ).
        /// </summary>
        public const Char CQ = (char)0x60;

        /// <summary>
        /// The question mark ( ? ).
        /// </summary>
        public const Char QM = (char)0x3f;

        /// <summary>
        /// The tab character.
        /// </summary>
        public const Char TAB = (char)0x09;

        /// <summary>
        /// The line feed character.
        /// </summary>
        public const Char LF = (char)0x0a;

        /// <summary>
        /// The carriage return character.
        /// </summary>
        public const Char CR = (char)0x0d;

        /// <summary>
        /// The form feed character.
        /// </summary>
        public const Char FF = (char)0x0c;

        /// <summary>
        /// The space character.
        /// </summary>
        public const Char SPACE = (char)0x20;

        /// <summary>
        /// The slash (solidus, /) character.
        /// </summary>
        public const Char SOLIDUS = (char)0x2f;

        /// <summary>
        /// The backslash ( reverse-solidus, \ ) character.
        /// </summary>
        public const Char RSOLIDUS = (char)0x5c;

        /// <summary>
        /// The colon ( : ) character.
        /// </summary>
        public const Char COLON = (char)0x3a;

        /// <summary>
        /// The exlamation mark ( ! ) character.
        /// </summary>
        public const Char EM = (char)0x21;

        /// <summary>
        /// The dash ( hypen minus, - ) character.
        /// </summary>
        public const Char MINUS = (char)0x2d;

        /// <summary>
        /// The replacement character in case of errors.
        /// </summary>
        public const Char REPLACEMENT = (char)0xfffd;

        /// <summary>
        /// The low line ( _ ) character.
        /// </summary>
        public const Char UNDERSCORE = (char)0x5f;

        /// <summary>
        /// The round bracket open ( ( ) character.
        /// </summary>
        public const Char RBO = (char)0x28;

        /// <summary>
        /// The round bracket close ( ) ) character.
        /// </summary>
        public const Char RBC = (char)0x29;

        /// <summary>
        /// The square bracket open ( [ ) character.
        /// </summary>
        public const Char SBO = (char)0x5b;

        /// <summary>
        /// The square bracket close ( ] ) character.
        /// </summary>
        public const Char SBC = (char)0x5d;

        /// <summary>
        /// The percent ( % ) character.
        /// </summary>
        public const Char PERCENT = (char)0x25;

        /// <summary>
        /// The maximum allowed codepoint (defined in Unicode).
        /// </summary>
        public const Int32 MAXIMUM_CODEPOINT = 0x10FFFF;

        #endregion

        #region Methods

        /// <summary>
        /// Gets if the character is actually a non-ascii character.
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsNonAscii(this Char c)
        {
            return c >= 0x80;
        }

        /// <summary>
        /// Gets if the character is actually a non-printable (special) character.
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsNonPrintable(this Char c)
        {
            return (c >= 0x0 && c <= 0x8) || (c >= 0xe && c <= 0x1f) || (c >= 0x7f && c <= 0x9f);
        }

        /// <summary>
        /// Gets if the character is actually a (A-Z,a-z) letter.
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsLetter(this Char c)
        {
            return IsUppercaseAscii(c) || IsLowercaseAscii(c);
        }

        /// <summary>
        /// Gets if the character is actually a name character.
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsName(this Char c)
        {
            return c >= 0x80 || c.IsLetter() || c == UNDERSCORE || c == MINUS || IsDigit(c);
        }

        /// <summary>
        /// Determines if the given character is a valid character for starting an identifier.
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsNameStart(this Char c)
        {
            return c >= 0x80 || IsUppercaseAscii(c) || IsLowercaseAscii(c) || c == UNDERSCORE;
        }

        /// <summary>
        /// Determines if the given character is a line break character as specified here:
        /// http://www.w3.org/TR/html401/struct/text.html#h-9.3.2
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsLineBreak(this Char c)
        {
            //line feed, carriage return
            return c == LF || c == CR;
        }

        /// <summary>
        /// Determines if the given character is a space character as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#space-character
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsSpaceCharacter(this Char c)
        {
            //white space, tab, line feed, form feed, carriage return
            return c == SPACE || c == TAB || c == LF || c == FF || c == CR;
        }

        /// <summary>
        /// Determines if the given character is a white-space character as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#white_space
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsWhiteSpaceCharacter(this Char c)
        {
            return (c >= 0x0009 && c <= 0x000d) || c == 0x0020 || c == 0x0085 || c == 0x00a0 ||
                    c == 0x1680 || c == 0x180e || (c >= 0x2000 && c <= 0x200a) || c == 0x2028 ||
                    c == 0x2029 || c == 0x202f || c == 0x205f || c == 0x3000;
        }

        /// <summary>
        /// Determines if the given character is a digit (0-9) as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#ascii-digits
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsDigit(this Char c)
        {
            return c >= 0x30 && c <= 0x39;
        }

        /// <summary>
        /// Determines if the given string consists only of digits (0-9) as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#ascii-digits
        /// </summary>
        /// <param name="s">The characters to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsDigit(this string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!IsDigit(s[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if the given character is a uppercase character (A-Z) as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#uppercase-ascii-letters
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsUppercaseAscii(this Char c)
        {
            return c >= 0x41 && c <= 0x5a;
        }

        /// <summary>
        /// Determines if the given character is a lowercase character (a-z) as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#lowercase-ascii-letters
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsLowercaseAscii(this Char c)
        {
            return c >= 0x61 && c <= 0x7a;
        }

        /// <summary>
        /// Determines if the given character is a alphanumeric character (0-9a-zA-z) as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#alphanumeric-ascii-characters
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsAlphanumericAscii(this Char c)
        {
            return IsDigit(c) || IsUppercaseAscii(c) || IsLowercaseAscii(c);
        }

        /// <summary>
        /// Determines if the given character is a hexadecimal (0-9a-fA-F) as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#ascii-hex-digits
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsHex(this Char c)
        {
            return IsDigit(c) || (c >= 0x41 && c <= 0x46) || (c >= 0x61 && c <= 0x66);
        }

        /// <summary>
        /// Determines if the given string only contains characters, which are hexadecimal (0-9a-fA-F) as specified here:
        /// http://www.whatwg.org/specs/web-apps/current-work/multipage/common-microsyntaxes.html#ascii-hex-digits
        /// </summary>
        /// <param name="s">The string to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsHex(this string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (!IsHex(s[i]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if the given character is a legal character for the public id field:
        /// http://www.w3.org/TR/REC-xml/#NT-PubidChar
        /// </summary>
        /// <param name="c">The character to examine.</param>
        /// <returns>The result of the test.</returns>
        //[DebuggerStepThrough]
        public static bool IsPubidChar(this Char c)
        {
            return IsAlphanumericAscii(c) || c == MINUS || c == SQ || c == PLUS || c == COMMA || c == DOT ||
                   c == SOLIDUS || c == COLON || c == QM || c == EQ || c == EM || c == ASTERISK || c == NUM ||
                   c == AT || c == DOLLAR || c == UNDERSCORE || c == RBO || c == RBC || c == SC || c == PERCENT ||
                   IsSpaceCharacter(c);
        }

        #endregion
    }
}
