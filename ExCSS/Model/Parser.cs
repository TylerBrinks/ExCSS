using System.Collections.Generic;
using System.Text;
using System.IO;
using ExCSS.Model.Factories.AtRuleFactories;

namespace ExCSS.Model
{
    public class Parser
    {
        private readonly Lexer _lexer;
        private StyleSheetContext _stylesheetContext;

        public Parser(string source) : this(new StylesheetStreamReader(source))
        {
        }

        public Parser(Stream stream) : this(new StylesheetStreamReader(stream))
        {
        }

        internal Parser(StylesheetStreamReader reader)
        {
            //ReadBuffer = new StringBuilder();
            _lexer = new Lexer(reader);

            //ActiveRules = new Stack<Ruleset>();

            //_lexer.ErrorOccurred += (s, ev) =>
            //{
            //    if (ErrorOccurred != null)
            //        ErrorOccurred(this, ev);
            //};
        }

        public void Parse()
        {
            _stylesheetContext = new StyleSheetContext(_lexer);
            _stylesheetContext.BuildRules();
        }

        internal Lexer Lexer { get { return _lexer; } }

        public bool IsQuirksMode { get; set; }

        
        /*
        private void AppendDeclarations(IEnumerator<Block> reader, ICollection<Property> declarations)
        {
            while (reader.MoveNext())
            {
                switch (reader.Current.Type)
                {
                    case GrammarSegment.Whitespace:
                    case GrammarSegment.Semicolon:
                        break;

                    case GrammarSegment.Ident:
                        var tokens = LimitToSemicolon(reader);
                        var it = tokens.GetEnumerator();
                        it.MoveNext();
                        var decl = CreateDeclaration(it);

                        if (decl != null)
                            declarations.Add(decl);

                        break;

                    default:
                        //RaiseErrorOccurred(ErrorCode.InvalidCharacter);
                        SkipToNextSemicolon(reader);
                        break;
                }
            }
        }
        
        private List<ValueList> CreateMultipleValues(IEnumerator<Block> reader)
        {
            var values = new List<ValueList>();

            do
            {
                var list = CreateValueList(reader);

                if (list.Length > 0)
                {
                    values.Add(list);
                }
            }
            while (reader.Current != null && reader.Current.Type == GrammarSegment.Comma);

            return values;
        }

        private ValueList CreateValueList(IEnumerator<Block> reader)
        {
            var list = new List<Value>();

            while (SkipToNextNonWhitespace(reader))
            {
                if (reader.Current.Type == GrammarSegment.Semicolon)
                {
                    break;
                }

                if (reader.Current.Type == GrammarSegment.Comma)
                {
                    break;
                }

                var value = CreateValue(reader);

                if (value == null)
                {
                    SkipToNextSemicolon(reader);
                    break;
                }

                list.Add(value);
            }

            return new ValueList(list);
        }

        private Value CreateValue(IEnumerator<Block> reader)
        {
            Value value = null;

            switch (reader.Current.Type)
            {
                case GrammarSegment.String:// 'i am a string'
                    value = new PrimitiveValue(UnitType.String, ((StringBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Url:// url('this is a valid URL')
                    value = new PrimitiveValue(UnitType.Uri, ((StringBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Ident: // ident
                    value = new PrimitiveValue(UnitType.Ident, ((SymbolBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Percentage: // 5%
                    value = new PrimitiveValue(UnitType.Percentage, ((UnitBlock)reader.Current).Data);
                    break;

                case GrammarSegment.Dimension: // 3px
                    value = new PrimitiveValue(((UnitBlock)reader.Current).Unit, ((UnitBlock)reader.Current).Data);
                    break;

                case GrammarSegment.Number: // 173
                    value = new PrimitiveValue(UnitType.Number, ((NumericBlock)reader.Current).Data);
                    break;

                case GrammarSegment.Hash: // #string
                    HtmlColor color;

                    if (HtmlColor.TryFromHex(((SymbolBlock)reader.Current).Value, out color))
                    {
                        value = new PrimitiveValue(color);
                    }

                    break;

                case GrammarSegment.Delimiter: // e.g. #0F3, #012345, ...
                    if (((DelimBlock)reader.Current).Value == '#')
                    {
                        var hash = string.Empty;

                        while (reader.MoveNext())
                        {
                            var stop = false;

                            switch (reader.Current.Type)
                            {
                                case GrammarSegment.Number:
                                case GrammarSegment.Dimension:
                                case GrammarSegment.Ident:
                                    var rest = reader.Current.ToString();

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
                    value = CreateFunction(reader);
                    break;
            }

            return value;
        }

        private Function CreateFunction(IEnumerator<Block> reader)
        {
            var name = ((SymbolBlock)reader.Current).Value;
            var args = new ValueList();

            while (reader.MoveNext())
            {
                if (reader.Current.Type == GrammarSegment.ParenClose)
                    break;
            }

            return Function.Create(name, args);
        }

        

        
        private Property CreateDeclaration(IEnumerator<Block> reader)
        {
            string name = ((SymbolBlock)reader.Current).Value;
            Property property = null;
            Value value = Value.Inherit;
            bool hasValue = SkipToNextNonWhitespace(reader) && reader.Current.Type == GrammarSegment.Colon;

            if (hasValue)
                value = CreateValueList(reader);

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
                //case "border-image-reader":
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
                    property = new Property(name) { Value = value };
                    break;
            }

            if (hasValue && reader.Current.Type == GrammarSegment.Delimiter && ((DelimBlock)reader.Current).Value == Specification.Em && SkipToNextNonWhitespace(reader))
                property.Important = reader.Current.Type == GrammarSegment.Ident && ((SymbolBlock)reader.Current).Value.Equals("important", StringComparison.OrdinalIgnoreCase);

            SkipBehindNextSemicolon(reader);
            return property;
        }

        private GenericRule CreateUnknownRule(string name, IEnumerator<Block> reader)
        {
            var rule = new GenericRule();
            var endCurly = 0;

            ActiveRules.Push(rule);
            ReadBuffer.Append(name).Append(" ");

            do
            {
                if (reader.Current.Type == GrammarSegment.Semicolon && endCurly == 0)
                {
                    reader.MoveNext();
                    break;
                }

                ReadBuffer.Append(reader.Current.ToString());

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

            rule.SetText(ReadBuffer.ToString());
            ReadBuffer.Clear();
            ActiveRules.Pop();
            return rule;
        }

        

        
        private SupportsRule CreateSupportsRule(IEnumerator<Block> reader)
        {
            var supports = new SupportsRule();

            ActiveRules.Push(supports);

            do
            {
                if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
                {
                    if (SkipToNextNonWhitespace(reader))
                    {
                        var tokens = LimitToCurrentBlock(reader);
                        AppendRules(supports.Rules);//.List);
                    }

                    break;
                }

                ReadBuffer.Append(reader.Current);
            }
            while (reader.MoveNext());

            supports.ConditionText = ReadBuffer.ToString();
            ReadBuffer.Clear();
            ActiveRules.Pop();
            return supports;
        }

        private NamespaceRule CreateNamespaceRule(IEnumerator<Block> reader)
        {
            var ns = new NamespaceRule();


            if (reader.Current.Type == GrammarSegment.Ident)
            {
                ns.Prefix = reader.Current.ToString();
                SkipToNextNonWhitespace(reader);

                if (reader.Current.Type == GrammarSegment.String)
                {
                    ns.NamespaceURI = reader.Current.ToString();
                }
            }

            SkipToNextSemicolon(reader);
            return ns;
        }

        private CharsetRule CreateCharsetRule(IEnumerator<Block> reader)
        {
            var charset = new CharsetRule();

            if (reader.Current.Type == GrammarSegment.String)
            {
                charset.Encoding = ((StringBlock)reader.Current).Value;
            }

            SkipToNextSemicolon(reader);
            return charset;
        }

        private FontFaceRule CreateFontFaceRule(IEnumerator<Block> reader)
        {
            var fontface = new FontFaceRule();
            ActiveRules.Push(fontface);

            if (reader.Current.Type == GrammarSegment.CurlyBraceOpen)
            {
                if (SkipToNextNonWhitespace(reader))
                {
                    var tokens = LimitToCurrentBlock(reader);
                    AppendDeclarations(tokens.GetEnumerator(), fontface.CssRules.List);
                }
            }

            ActiveRules.Pop();
            return fontface;
        }

        private ImportRule CreateImportRule(IEnumerator<Block> reader)
        {
            var import = new ImportRule();

            ActiveRules.Push(import);

            switch (reader.Current.Type)
            {
                case GrammarSegment.Semicolon:
                    reader.MoveNext();
                    break;

                case GrammarSegment.String:
                case GrammarSegment.Url:
                    import.Href = ((StringBlock)reader.Current).Value;
                    AppendMediaList(reader, import.Media);

                    break;

                default:
                    SkipToNextSemicolon(reader);
                    break;
            }

            ActiveRules.Pop();
            return import;
        }

        

        private static bool SkipToNextSemicolon(IEnumerator<Block> reader)
        {
            do
            {
                if (reader.Current.Type == GrammarSegment.Semicolon)
                {
                    return true;
                }
            }
            while (reader.MoveNext());

            return false;
        }

        private static bool SkipBehindNextSemicolon(IEnumerator<Block> reader)
        {
            do
            {
                if (reader.Current.Type != GrammarSegment.Semicolon)
                {
                    continue;
                }

                reader.MoveNext();
                return true;
            }
            while (reader.MoveNext());

            return false;
        }

        private static IEnumerable<Block> LimitToSemicolon(IEnumerator<Block> reader)
        {
            do
            {
                if (reader.Current.Type == GrammarSegment.Semicolon)
                {
                    yield break;
                }

                yield return reader.Current;
            }
            while (reader.MoveNext());
        }

        
        internal static Selector ParseSelector(string selector, bool quirksMode = false)
        {
            var parser = new Parser(selector)
                {
                    IsQuirksMode = quirksMode
                };

            var tokens = parser._lexer.Tokens.GetEnumerator();
            var ctor = new SelectorConstructor();

            while (tokens.MoveNext())
            {
                ctor.PickSelector(tokens);
            }

            return ctor.Result;
        }

        internal static StyleDeclaration ParseDeclarations(string declarations, bool quirksMode = false)
        {
            var parser = new Parser(declarations)
                {
                    IsQuirksMode = quirksMode,
                    _ignore = false
                };

            var it = parser._lexer.Tokens.GetEnumerator();
            var decl = new StyleDeclaration();
            parser.AppendDeclarations(it, decl.List);
            return decl;
        }

        internal static Value ParseValue(string reader, bool quirksMode = false)
        {
            var parser = new Parser(reader)
                {
                    IsQuirksMode = quirksMode,
                    _ignore = false
                };

            var it = parser._lexer.Tokens.GetEnumerator();
            SkipToNextNonWhitespace(it);
            return parser.CreateValue(it);
        }

        internal static ValueList ParseValueList(string reader, bool quirksMode = false)
        {
            var parser = new Parser(reader)
                {
                    IsQuirksMode = quirksMode,
                    _ignore = false
                };

            var it = parser._lexer.Tokens.GetEnumerator();
            return parser.CreateValueList(it);
        }

        internal static List<ValueList> ParseMultipleValues(string reader, bool quirksMode = false)
        {
            var parser = new Parser(reader)
                {
                    IsQuirksMode = quirksMode,
                    _ignore = false
                };

            var it = parser._lexer.Tokens.GetEnumerator();
            return parser.CreateMultipleValues(it);
        }

        
        */
        #region Event-Helpers

        //void RaiseErrorOccurred(ErrorCode code)
        //{
        //    if (ErrorOccurred != null)
        //    {
        //        var pck = new ParseErrorEventArgs((int)code, Errors.GetError(code));
        //        pck.Line = _lexer.Reader.Line;
        //        pck.Column = _lexer.Reader.Column;
        //        ErrorOccurred(this, pck);
        //    }
        //}

        #endregion
    }
}
