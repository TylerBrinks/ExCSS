using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    /// <summary>
    /// Validates a <c>clip-path</c> value (CSS Masking 1 §5.1): the keyword <c>none</c> or a
    /// <c>&lt;basic-shape&gt;</c> - <c>polygon()</c>, <c>inset()</c>, <c>circle()</c> or <c>ellipse()</c>
    /// (CSS Shapes 1 §3). The authored text is preserved. The <c>&lt;geometry-box&gt;</c> and
    /// <c>&lt;url&gt;</c> forms are out of scope here.
    /// </summary>
    internal sealed class ClipPathValueConverter : IValueConverter
    {
        public IPropertyValue Convert(IEnumerable<Token> value)
        {
            var tokens = value.Where(t => t.Type != TokenType.Whitespace).ToArray();

            if (tokens.Length == 1 && tokens[0].Type == TokenType.Ident && tokens[0].Data.Isi(Keywords.None))
                return new ClipPathValue(value);

            return IsBasicShape(tokens) ? new ClipPathValue(value) : null;
        }

        public IPropertyValue Construct(Property[] properties)
        {
            return properties.Guard<ClipPathValue>();
        }

        private static bool IsBasicShape(IReadOnlyList<Token> tokens)
        {
            if (tokens.Count != 1 || tokens[0] is not FunctionToken function) return false;

            var args = function.ArgumentTokens.Where(t => t.Type != TokenType.Whitespace).ToArray();

            if (function.Data.Isi(FunctionNames.Polygon)) return IsPolygon(args);
            if (function.Data.Isi(FunctionNames.Inset)) return IsInset(args);
            if (function.Data.Isi(FunctionNames.Circle)) return IsCircle(args);
            if (function.Data.Isi(FunctionNames.Ellipse)) return IsEllipse(args);

            return false;
        }

        // polygon( <fill-rule>? , [ <length-percentage> <length-percentage> ]# )
        private static bool IsPolygon(IReadOnlyList<Token> args)
        {
            var groups = SplitByComma(args);
            if (groups == null || groups.Count == 0) return false;

            var first = 0;

            if (groups[0].Count == 1 && IsFillRule(groups[0][0]))
                first = 1;

            if (first >= groups.Count) return false;

            for (var i = first; i < groups.Count; i++)
            {
                var group = groups[i];
                if (group.Count != 2 || !IsLengthPercentage(group[0]) || !IsLengthPercentage(group[1]))
                    return false;
            }

            return true;
        }

        // inset( <length-percentage>{1,4} [ round <border-radius> ]? )
        private static bool IsInset(IReadOnlyList<Token> args)
        {
            if (args.Count == 0) return false;

            var roundIndex = -1;
            for (var i = 0; i < args.Count; i++)
            {
                if (args[i].Type == TokenType.Ident && args[i].Data.Isi(Keywords.Round))
                {
                    roundIndex = i;
                    break;
                }
            }

            var lengths = roundIndex >= 0 ? args.Take(roundIndex).ToArray() : args.ToArray();
            var roundCount = roundIndex >= 0 ? args.Count - roundIndex - 1 : 0;

            if (lengths.Length is < 1 or > 4 || lengths.Any(t => !IsLengthPercentage(t))) return false;

            // "round" must be followed by a radius, which must itself be all length-percentages.
            if (roundIndex >= 0)
            {
                if (roundCount == 0) return false;
                if (args.Skip(roundIndex + 1).Any(t => !IsLengthPercentage(t))) return false;
            }

            return true;
        }

        // circle( <shape-radius>? [ at <position> ]? )
        private static bool IsCircle(IReadOnlyList<Token> args)
        {
            if (!SplitAt(args, out var radius, out var position)) return false;
            return radius.Count switch
            {
                0 => true,
                1 => IsShapeRadius(radius[0]),
                _ => false
            } && IsPosition(position);
        }

        // ellipse( [ <shape-radius>{2} ]? [ at <position> ]? )
        private static bool IsEllipse(IReadOnlyList<Token> args)
        {
            if (!SplitAt(args, out var radius, out var position)) return false;
            return radius.Count switch
            {
                0 => true,
                2 => IsShapeRadius(radius[0]) && IsShapeRadius(radius[1]),
                _ => false
            } && IsPosition(position);
        }

        // Splits the arguments at a standalone "at" ident. Returns false when "at" has nothing after it.
        private static bool SplitAt(IReadOnlyList<Token> args, out IReadOnlyList<Token> before,
            out IReadOnlyList<Token> after)
        {
            for (var i = 0; i < args.Count; i++)
            {
                if (args[i].Type == TokenType.Ident && args[i].Data.Isi(Keywords.At))
                {
                    before = args.Take(i).ToArray();
                    after = args.Skip(i + 1).ToArray();
                    return after.Count > 0;
                }
            }

            before = args;
            after = new Token[0];
            return true;
        }

        // A permissive <position>: nothing (absent), or 1-4 tokens each an edge/center keyword or a
        // length-percentage. Absent is valid (the caller only reaches here with the post-"at" tokens).
        private static bool IsPosition(IReadOnlyList<Token> tokens)
        {
            if (tokens.Count == 0) return true;
            if (tokens.Count > 4) return false;

            return tokens.All(t => IsPositionKeyword(t) || IsLengthPercentage(t));
        }

        private static bool IsPositionKeyword(Token token)
        {
            if (token.Type != TokenType.Ident) return false;
            return token.Data.Isi(Keywords.Left) || token.Data.Isi(Keywords.Right) ||
                   token.Data.Isi(Keywords.Top) || token.Data.Isi(Keywords.Bottom) ||
                   token.Data.Isi(Keywords.Center);
        }

        private static bool IsShapeRadius(Token token)
        {
            if (token.Type == TokenType.Ident)
                return token.Data.Isi(Keywords.ClosestSide) || token.Data.Isi(Keywords.FarthestSide);

            // A <shape-radius> is a non-negative <length-percentage> (CSS Shapes 1 §3.2).
            if (token is UnitToken { Value: < 0f } or NumberToken { Value: < 0f }) return false;

            return IsLengthPercentage(token);
        }

        private static bool IsFillRule(Token token) =>
            token.Type == TokenType.Ident && (token.Data.Isi(Keywords.Nonzero) || token.Data.Isi(Keywords.Evenodd));

        private static bool IsLengthPercentage(Token token) =>
            token.Type is TokenType.Dimension or TokenType.Percentage || token is NumberToken { Value: 0f };

        private static List<List<Token>> SplitByComma(IReadOnlyList<Token> tokens)
        {
            var groups = new List<List<Token>>();
            var current = new List<Token>();

            foreach (var token in tokens)
            {
                if (token.Type == TokenType.Comma)
                {
                    groups.Add(current);
                    current = new List<Token>();
                }
                else
                {
                    current.Add(token);
                }
            }

            groups.Add(current);

            // A leading, trailing or doubled comma leaves an empty group - always malformed here.
            return tokens.Count > 0 && groups.Any(g => g.Count == 0) ? null : groups;
        }

        private sealed class ClipPathValue : IPropertyValue
        {
            public ClipPathValue(IEnumerable<Token> tokens)
            {
                Original = new TokenValue(tokens);
            }

            public string CssText => Original.Text;

            public TokenValue Original { get; }

            public TokenValue ExtractFor(string name) => Original;
        }
    }
}
