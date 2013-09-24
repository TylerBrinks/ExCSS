using System;
using System.Collections.Generic;
using ExCSS.Model.TextBlocks;
using ExCSS.Model.Values;

namespace ExCSS.Model.Extensions
{
    internal static class BlockExtensions
    {
        internal static IEnumerable<Block> LimitToCurrentBlock(this IEnumerator<Block> reader)
        {
            var open = 1;

            do
            {
                if (reader.Current.GrammarSegment == GrammarSegment.CurlyBraceOpen)
                {
                    open++;
                }
                else if (reader.Current.GrammarSegment == GrammarSegment.CurlyBracketClose && --open == 0)
                {
                    yield break;
                }

                yield return reader.Current;
            }
            while (reader.MoveNext());
        }

        internal static bool SkipToNextNonWhitespace(this IEnumerator<Block> reader)
        {
            while (reader.MoveNext())
            {
                if (reader.Current.GrammarSegment != GrammarSegment.Whitespace)
                {
                    return true;
                }
            }

            return false;
        }

        internal static IEnumerable<Block> LimitToSemicolon(this IEnumerator<Block> reader)
        {
            do
            {
                if (reader.Current.GrammarSegment == GrammarSegment.Semicolon)
                {
                    yield break;
                }

                yield return reader.Current;
            }
            while (reader.MoveNext());
        }

        internal static Property CreateDeclaration(this IEnumerator<Block> reader)
        {
            var name = ((SymbolBlock)reader.Current).Value;
            Property property;
            var value = Term.Inherit;

            var hasValue = SkipToNextNonWhitespace(reader) && reader.Current.GrammarSegment == GrammarSegment.Colon;

            if (hasValue)
            {
                value = CreateValueList(reader);
            }

            switch (name)
            {
                default:
                    property = new Property(name) { Term = value };
                    break;
            }

            if (hasValue && reader.Current.GrammarSegment == GrammarSegment.Delimiter &&
                ((DelimiterBlock)reader.Current).Value == Specification.Em && 
                SkipToNextNonWhitespace(reader))
            {
                property.Important = reader.Current.GrammarSegment == GrammarSegment.Ident && 
                    ((SymbolBlock)reader.Current).Value.Equals("important", StringComparison.OrdinalIgnoreCase);
            }

            SkipBehindNextSemicolon(reader);
            return property;
        }

        internal static void AppendDeclarations(this IEnumerator<Block> reader, ICollection<Property> declarations, 
            Action<ParserError, string> errorHandler)
        {
            while (reader.MoveNext())
            {
                switch (reader.Current.GrammarSegment)
                {
                    case GrammarSegment.Whitespace:
                    case GrammarSegment.Semicolon:
                        break;

                    case GrammarSegment.Ident:
                        var tokens = LimitToSemicolon(reader);
                        var enumerator = tokens.GetEnumerator();
                        enumerator.MoveNext();
                        var declaration = CreateDeclaration(enumerator);

                        if (declaration != null)
                        {
                            declarations.Add(declaration);
                        }

                        break;

                    default:
                        errorHandler(ParserError.InvalidCharacter, "Invalid character in declaration.");
                        SkipToNextSemicolon(reader);
                        break;
                }
            }
        }

        internal static bool SkipToNextSemicolon(this IEnumerator<Block> reader)
        {
            do
            {
                if (reader.Current.GrammarSegment == GrammarSegment.Semicolon)
                {
                    return true;
                }
            }
            while (reader.MoveNext());

            return false;
        }

        internal static TermList CreateValueList(this IEnumerator<Block> reader)
        {
            var list = new List<Term>();
            var commaDelimited = false;

            while (SkipToNextNonWhitespace(reader))
            {
                if (reader.Current.GrammarSegment == GrammarSegment.Semicolon)
                {
                    break;
                }

                if (reader.Current.GrammarSegment == GrammarSegment.Comma)
                {
                    //break;
                    commaDelimited = true;
                    continue;
                }

                var value = CreateValue(reader);

                if (value == null)
                {
                    SkipToNextSemicolon(reader);
                    break;
                }

                list.Add(value);
            }

            return new TermList(list, commaDelimited);
        }

        internal static Term CreateValue(this IEnumerator<Block> reader)
        {
            Term value = null;

            switch (reader.Current.GrammarSegment)
            {
                case GrammarSegment.String:
                    value = new PrimitiveTerm(UnitType.String, ((StringBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Url:
                    value = new PrimitiveTerm(UnitType.Uri, ((StringBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Ident:
                    value = new PrimitiveTerm(UnitType.Ident, ((SymbolBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Percentage:
                    value = new PrimitiveTerm(UnitType.Percentage, ((UnitBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Dimension:
                    value = new PrimitiveTerm(((UnitBlock)reader.Current).Unit, ((UnitBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Number:
                    value = new PrimitiveTerm(UnitType.Number, ((NumericBlock)reader.Current).Value);
                    break;

                case GrammarSegment.Hash:
                    HtmlColor color;

                    if (HtmlColor.TryFromHex(((SymbolBlock)reader.Current).Value, out color))
                    {
                        value = new PrimitiveTerm(color);
                    }

                    break;

                case GrammarSegment.Delimiter: // e.g. #0F3, #012345, ...
                    if (((DelimiterBlock)reader.Current).Value == '#')
                    {
                        var hash = string.Empty;

                        while (reader.MoveNext())
                        {
                            var stop = false;

                            switch (reader.Current.GrammarSegment)
                            {
                                case GrammarSegment.Number:
                                case GrammarSegment.Dimension:
                                case GrammarSegment.Ident:
                                    var remainingText = reader.Current.ToString();

                                    if (hash.Length + remainingText.Length <= 6)
                                    {
                                        hash += remainingText;
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
                            value = new PrimitiveTerm(color);
                        }
                    }
                    break;

                case GrammarSegment.Function: // rgba(255, 255, 20, 0.5)
                    value = CreateFunction(reader);
                    break;
            }

            return value;
        }

        internal static Function CreateFunction(this IEnumerator<Block> reader)
        {
            var name = ((SymbolBlock)reader.Current).Value;
            var args = new TermList();

            while (reader.MoveNext())
            {
                if (reader.Current.GrammarSegment == GrammarSegment.ParenClose)
                {
                    break;
                }
            }

            return Function.Create(name, args);
        }

        internal static bool SkipBehindNextSemicolon(IEnumerator<Block> reader)
        {
            do
            {
                if (reader.Current.GrammarSegment != GrammarSegment.Semicolon)
                {
                    continue;
                }

                reader.MoveNext();
                return true;
            }
            while (reader.MoveNext());

            return false;
        }
    }
}