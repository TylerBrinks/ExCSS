using System;
using System.Collections.Generic;
using ExCSS.Model;

namespace ExCSS
{
    public static class BlockExtensions
    {
        internal static IEnumerable<Block> LimitToCurrentBlock(this IEnumerator<Block> source)
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

        internal static bool SkipToNextNonWhitespace(this IEnumerator<Block> source)
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

        private static IEnumerable<Block> LimitToSemicolon(this IEnumerator<Block> source)
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

        internal static Property CreateDeclaration(this IEnumerator<Block> source)
        {
            var name = ((SymbolBlock)source.Current).Value;
            Property property = null;
            var value = Value.Inherit;
            var hasValue = SkipToNextNonWhitespace(source) && source.Current.Type == GrammarSegment.Colon;

            if (hasValue)
            {
                value = CreateValueList(source);
            }

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
                    property = new Property(name) { Value = value };
                    break;
            }

            if (hasValue && source.Current.Type == GrammarSegment.Delimiter &&
                ((DelimBlock)source.Current).Value == Specification.Em && SkipToNextNonWhitespace(source))
            {
                property.Important = source.Current.Type == GrammarSegment.Ident && ((SymbolBlock)source.Current).Value.Equals("important", StringComparison.OrdinalIgnoreCase);
            }

            SkipBehindNextSemicolon(source);
            return property;
        }

        internal static void AppendDeclarations(this IEnumerator<Block> source, ICollection<Property> declarations)
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

        internal static bool SkipToNextSemicolon(this IEnumerator<Block> source)
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

        internal static ValueList CreateValueList(this IEnumerator<Block> source)
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

        internal static Value CreateValue(this IEnumerator<Block> source)
        {
            Value value = null;

            switch (source.Current.Type)
            {
                case GrammarSegment.String:// 'i am a string'
                    value = new PrimitiveValue(UnitType.String, ((StringBlock)source.Current).Value);
                    break;

                case GrammarSegment.Url:// url('this is a valid URL')
                    value = new PrimitiveValue(UnitType.Uri, ((StringBlock)source.Current).Value);
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

                    if (HtmlColor.TryFromHex(((SymbolBlock)source.Current).Value, out color))
                    {
                        value = new PrimitiveValue(color);
                    }

                    break;

                case GrammarSegment.Delimiter: // e.g. #0F3, #012345, ...
                    if (((DelimBlock)source.Current).Value == '#')
                    {
                        var hash = string.Empty;

                        while (source.MoveNext())
                        {
                            var stop = false;

                            switch (source.Current.Type)
                            {
                                case GrammarSegment.Number:
                                case GrammarSegment.Dimension:
                                case GrammarSegment.Ident:
                                    var rest = source.Current.ToString();

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

        internal static Function CreateFunction(this IEnumerator<Block> source)
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

        internal static bool SkipBehindNextSemicolon(IEnumerator<Block> source)
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
    }
}