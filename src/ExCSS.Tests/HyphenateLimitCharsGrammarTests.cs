using System.Collections.Generic;
using Xunit;

namespace ExCSS.Tests
{
    /// <summary>Tests for the shared <see cref="HyphenateLimitCharsGrammar"/> (the <c>hyphenate-limit-chars</c>
    /// value grammar <c>[ auto | &lt;integer&gt; ]{1,3}</c>), <see cref="HangingPunctuationGrammar"/> and
    /// <see cref="InitialLetterGrammar"/>.</summary>
    public class HyphenateLimitCharsGrammarTests : CssConstructionFunctions
    {
        private static IReadOnlyList<Token> Tokens(string value)
        {
            var lexer = new Lexer(new TextSource(value));
            var tokens = new List<Token>();
            var token = lexer.Get();
            while (token.Type != TokenType.EndOfFile)
            {
                tokens.Add(token);
                token = lexer.Get();
            }
            return tokens;
        }

        private static bool TryParse(string value, out int? word, out int? before, out int? after) =>
            HyphenateLimitCharsGrammar.TryParse(Tokens(value), out word, out before, out after);

        [Fact]
        public void Auto_AllThreeComponentsAreNull()
        {
            Assert.True(TryParse("auto", out var word, out var before, out var after));
            Assert.Null(word);
            Assert.Null(before);
            Assert.Null(after);
        }

        [Fact]
        public void ThreeIntegers_EachComponentParsed()
        {
            Assert.True(TryParse("5 2 3", out var word, out var before, out var after));
            Assert.Equal(5, word);
            Assert.Equal(2, before);
            Assert.Equal(3, after);
        }

        [Fact]
        public void TwoValues_AfterDefaultsToBefore()
        {
            Assert.True(TryParse("10 4", out var word, out var before, out var after));
            Assert.Equal(10, word);
            Assert.Equal(4, before);
            Assert.Equal(4, after);
        }

        [Fact]
        public void OneValue_BeforeAndAfterAreAuto()
        {
            Assert.True(TryParse("10", out var word, out var before, out var after));
            Assert.Equal(10, word);
            Assert.Null(before);
            Assert.Null(after);
        }

        [Fact]
        public void MixedAutoAndIntegers_EachComponentIndependent()
        {
            Assert.True(TryParse("auto 3 4", out var word, out var before, out var after));
            Assert.Null(word);
            Assert.Equal(3, before);
            Assert.Equal(4, after);
        }

        [Theory]
        [InlineData("")]
        [InlineData("1 2 3 4")] // more than 3 values
        [InlineData("-1")] // negative integer
        [InlineData("banana")]
        [InlineData("1.5")] // not an integer
        [InlineData("5px")]
        public void Invalid_ReturnsFalse(string value)
        {
            Assert.False(TryParse(value, out _, out _, out _));
        }

        [Theory]
        [InlineData("hyphenate-limit-chars: auto", true)]
        [InlineData("hyphenate-limit-chars: 5 2 2", true)]
        [InlineData("hyphenate-limit-chars: auto 3", true)]
        [InlineData("hyphenate-limit-chars: banana", false)]
        [InlineData("hyphenate-limit-chars: 1 2 3 4", false)]
        [InlineData("hyphenate-limit-chars: -1", false)]
        public void ParserAcceptsValid_RejectsInvalid(string declaration, bool shouldApply)
        {
            var style = ParseDeclarations(declaration);
            Assert.Equal(shouldApply, !string.IsNullOrEmpty(style.GetPropertyValue("hyphenate-limit-chars")));
        }

        [Theory]
        [InlineData("none", (int)(HangingPunctuationFlags.None))]
        [InlineData("first", (int)(HangingPunctuationFlags.First))]
        [InlineData("last", (int)(HangingPunctuationFlags.Last))]
        [InlineData("force-end", (int)(HangingPunctuationFlags.ForceEnd))]
        [InlineData("allow-end", (int)(HangingPunctuationFlags.AllowEnd))]
        [InlineData("first last", (int)(HangingPunctuationFlags.First | HangingPunctuationFlags.Last))]
        [InlineData("last force-end first", (int)(HangingPunctuationFlags.First | HangingPunctuationFlags.Last | HangingPunctuationFlags.ForceEnd))]
        [InlineData("allow-end first", (int)(HangingPunctuationFlags.First | HangingPunctuationFlags.AllowEnd))]
        public void HangingPunctuation_ParsesFlags(string value, int expected)
        {
            Assert.True(HangingPunctuationGrammar.TryParse(Tokens(value), out var flags));
            Assert.Equal((HangingPunctuationFlags)expected, flags);
        }

        [Theory]
        [InlineData("")]
        [InlineData("none first")]
        [InlineData("first first")]
        [InlineData("force-end allow-end")]
        [InlineData("middle")]
        [InlineData("5")]
        public void HangingPunctuation_RejectsInvalid(string value)
        {
            Assert.False(HangingPunctuationGrammar.TryParse(Tokens(value), out _));
        }

        [Theory]
        [InlineData("normal", 0d, 0)]
        [InlineData("3", 3d, 3)]
        [InlineData("2.5", 2.5d, 2)]
        [InlineData("3 2", 3d, 2)]
        [InlineData("3 drop", 3d, 3)]
        [InlineData("3 raise", 3d, 1)]
        public void InitialLetter_ParsesSizeAndSink(string value, double size, int sink)
        {
            Assert.True(InitialLetterGrammar.TryParse(Tokens(value), out var parsedSize, out var parsedSink));
            Assert.Equal(size, parsedSize);
            Assert.Equal(sink, parsedSink);
        }

        [Theory]
        [InlineData("")]
        [InlineData("0.5")]
        [InlineData("0")]
        [InlineData("3 0")]
        [InlineData("3 1.5")]
        [InlineData("3 2 1")]
        [InlineData("drop")]
        [InlineData("normal 2")]
        public void InitialLetter_RejectsInvalid(string value)
        {
            Assert.False(InitialLetterGrammar.TryParse(Tokens(value), out _, out _));
        }
    }
}
