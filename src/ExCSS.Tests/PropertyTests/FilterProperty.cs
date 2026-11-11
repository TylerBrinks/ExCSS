using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class FilterPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("none")]
        [InlineData("blur()")]
        [InlineData("blur(5px)")]
        [InlineData("blur(0)")]
        [InlineData("brightness()")]
        [InlineData("brightness(150%)")]
        [InlineData("contrast(0.5)")]
        [InlineData("grayscale(50%)")]
        [InlineData("invert(1)")]
        [InlineData("opacity(80%)")]
        [InlineData("saturate(2)")]
        [InlineData("sepia(1)")]
        [InlineData("hue-rotate(45deg)")]
        [InlineData("hue-rotate(0.5turn)")]
        [InlineData("hue-rotate(100grad)")]
        [InlineData("hue-rotate(1rad)")]
        [InlineData("hue-rotate(0)")]
        [InlineData("BLUR(2PX)")]
        [InlineData("blur(calc(1px + 2px))")]
        [InlineData("hue-rotate(calc(45deg + 45deg))")]
        [InlineData("url(#effect)")]
        [InlineData("url(filters.svg#effect)")]
        [InlineData("drop-shadow(2px 3px)")]
        [InlineData("drop-shadow(2px 3px 5px red)")]
        [InlineData("drop-shadow(red 2px 3px 5px)")]
        [InlineData("drop-shadow(2px 3px 5px #08f)")]
        [InlineData("drop-shadow(2px 3px 5px rgba(0,0,0,.5))")]
        [InlineData("drop-shadow(0 0 4px currentColor)")]
        [InlineData("blur(2px) brightness(1.2) url(#x) drop-shadow(1px 1px 2px black)")]
        public void FilterLegalValues(string value)
        {
            foreach (var name in new[] { "filter", "backdrop-filter" })
            {
                var property = ParseDeclaration($"{name}: {value}");

                Assert.Equal(name, property.Name);
                Assert.True(property.HasValue, $"{name}: {value}");
                Assert.False(property.IsInherited);
            }
        }

        [Theory]
        [InlineData("")]
        [InlineData("banana")]
        [InlineData("blur")]                         // a bare name is not a function
        [InlineData("banana()")]
        [InlineData("opacity(0.5) banana")]
        [InlineData("opacity(0.5), brightness(1)")]  // no commas between filter functions
        [InlineData("none blur(1px)")]
        [InlineData("hue-rotate(45)")]               // missing unit
        [InlineData("hue-rotate(45%)")]
        [InlineData("hue-rotate(45px)")]
        [InlineData("brightness(-1)")]
        [InlineData("opacity(-10%)")]
        [InlineData("blur(-5px)")]
        [InlineData("blur(50%)")]
        [InlineData("blur(5)")]                      // unitless non-zero length
        [InlineData("brightness(1 2)")]
        [InlineData("brightness(10px)")]
        [InlineData("drop-shadow()")]
        [InlineData("drop-shadow(2px)")]
        [InlineData("drop-shadow(1px 1px 1px 1px)")] // no spread
        [InlineData("drop-shadow(inset 2px 3px)")]
        [InlineData("drop-shadow(2px 3px -5px)")]
        [InlineData("drop-shadow(2px 3px red blue)")]
        [InlineData("drop-shadow(2px red 3px)")]     // lengths must be adjacent
        [InlineData("drop-shadow(2foo 3px)")]
        public void FilterIllegalValues(string value)
        {
            foreach (var name in new[] { "filter", "backdrop-filter" })
            {
                var property = ParseDeclaration($"{name}: {value}");

                Assert.False(property.HasValue, $"{name}: {value}");
            }
        }

        private static IReadOnlyList<Token> Tokenize(string value)
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

        [Fact]
        public void FilterPropertyTypes()
        {
            Assert.IsType<FilterProperty>(ParseDeclaration("filter: none"));
            Assert.IsType<BackdropFilterProperty>(ParseDeclaration("backdrop-filter: none"));
        }

        [Fact]
        public void FilterPreservesAuthoredText()
        {
            var property = ParseDeclaration("filter: blur(2px) drop-shadow(1px 1px 2px black)");

            Assert.Equal("blur(2px) drop-shadow(1px 1px 2px black)", property.Value);
        }

        [Fact]
        public void FilterAcceptsGlobalKeywords()
        {
            Assert.True(ParseDeclaration("filter: inherit").HasValue);
            Assert.True(ParseDeclaration("backdrop-filter: initial").HasValue);
        }

        [Fact]
        public void FilterIsAnimatable()
        {
            Assert.True(ParseDeclaration("filter: none").IsAnimatable);
            Assert.True(ParseDeclaration("backdrop-filter: none").IsAnimatable);
        }

        [Fact]
        public void FilterGrammarParsesFunctions()
        {
            var tokens = Tokenize("blur(2px) opacity(50%) drop-shadow(red 1px 2px)");
            var functions = FilterGrammar.TryParse(tokens);

            Assert.NotNull(functions);
            Assert.Equal(new[] { "blur", "opacity", "drop-shadow" }, functions.Select(f => f.Name).ToArray());
            Assert.Equal(new[] { "2px" }, functions[0].Arguments);
            Assert.Empty(FilterGrammar.TryParse(Tokenize("none")));
            Assert.Null(FilterGrammar.TryParse(Tokenize("banana()")));
        }

        [Fact]
        public void StyleDeclarationExposesFilterAccessors()
        {
            var style = ParseStyleSheet(".a { filter: blur(2px); backdrop-filter: blur(4px) }").Rules[0] as StyleRule;

            Assert.Equal("blur(2px)", style.Style.Filter);
            Assert.Equal("blur(4px)", style.Style.BackdropFilter);
        }
    }
}
