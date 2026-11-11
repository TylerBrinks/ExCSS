using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    /// <summary>Tests for <see cref="ColorInterpolationMethodGrammar"/>, which validates only the
    /// <c>in ...</c> slice of a gradient prelude and returns the rest as the remainder.</summary>
    public class ColorInterpolationMethodGrammarTests : CssConstructionFunctions
    {
        private static List<Token> Tokens(string value)
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

        private static (bool Ok, bool HasIn, string Remainder) Extract(string value)
        {
            var ok = ColorInterpolationMethodGrammar.TryExtractInterpolationMethod(
                Tokens(value), out var remainder, out var hasIn);
            var text = ok && remainder != null
                ? string.Join(" ", remainder.Where(t => t.Type != TokenType.Whitespace).Select(t => t.ToValue()))
                : null;
            return (ok, hasIn, text);
        }

        [Fact]
        public void NoInClausePassesThroughUnchanged()
        {
            var (ok, hasIn, remainder) = Extract("to right");
            Assert.True(ok);
            Assert.False(hasIn);
            Assert.Equal("to right", remainder);
        }

        [Theory]
        [InlineData("in srgb")]
        [InlineData("in srgb-linear")]
        [InlineData("in display-p3")]
        [InlineData("in a98-rgb")]
        [InlineData("in prophoto-rgb")]
        [InlineData("in rec2020")]
        [InlineData("in lab")]
        [InlineData("in oklab")]
        [InlineData("in xyz")]
        [InlineData("in xyz-d65")]
        [InlineData("in xyz-d50")]
        [InlineData("in hsl")]
        [InlineData("in hwb")]
        [InlineData("in lch")]
        [InlineData("in oklch")]
        [InlineData("in oklch shorter hue")]
        [InlineData("in oklch longer hue")]
        [InlineData("in hwb increasing hue")]
        [InlineData("in LCH Decreasing HUE")]
        public void ValidMethodOnlyHasInWithEmptyRemainder(string value)
        {
            var (ok, hasIn, remainder) = Extract(value);
            Assert.True(ok);
            Assert.True(hasIn);
            Assert.Equal("", remainder);
        }

        [Theory]
        [InlineData("in oklab to right", "to right")]
        [InlineData("to right in oklab", "to right")]
        [InlineData("in oklch longer hue to right", "to right")]
        // A hue method on a rectangular space is not rejected here: the leftover "longer hue" is returned
        // as the remainder for the caller's own validation to reject.
        [InlineData("in oklab longer hue", "longer hue")]
        public void ValidMethodWithRemainderExtractsRemainder(string value, string expectedRemainder)
        {
            var (ok, hasIn, remainder) = Extract(value);
            Assert.True(ok);
            Assert.True(hasIn);
            Assert.Equal(expectedRemainder, remainder);
        }

        [Theory]
        [InlineData("in nonsense")]
        [InlineData("in")]
        [InlineData("in oklch longer")]
        public void InvalidMethodReturnsFalse(string value)
        {
            var ok = ColorInterpolationMethodGrammar.TryExtractInterpolationMethod(Tokens(value), out _, out _);
            Assert.False(ok);
        }

        [Theory]
        [InlineData("oklch", true)]
        [InlineData("hsl", true)]
        [InlineData("oklab", false)]
        [InlineData("srgb", false)]
        public void PolarColorSpacesAreClassified(string name, bool polar)
        {
            Assert.True(ColorInterpolationMethodGrammar.IsColorSpace(name));
            Assert.Equal(polar, ColorInterpolationMethodGrammar.IsPolarColorSpace(name));
        }

        [Fact]
        public void UnknownSpaceIsNotAColorSpace()
        {
            Assert.False(ColorInterpolationMethodGrammar.IsColorSpace("banana"));
        }
    }
}
