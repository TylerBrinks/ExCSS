using System.Collections.Generic;
using System.Linq;

namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    /// <summary>Shared helpers for the CSS Color 4/5 tests: tokenize a bare value and resolve it to sRGB.</summary>
    internal static class ColorTestHelpers
    {
        public static List<Token> Tokenize(string value)
        {
            var lexer = new Lexer(new TextSource(value));
            var tokens = new List<Token>();
            Token token;
            do
            {
                token = lexer.Get();
                if (token.Type != TokenType.EndOfFile && token.Type != TokenType.Whitespace)
                {
                    tokens.Add(token);
                }
            } while (token.Type != TokenType.EndOfFile);

            return tokens;
        }

        public static Color? TryResolve(string value) => Tokenize(value).ToResolvedColor();

        public static Color Resolve(string value)
        {
            var color = TryResolve(value);
            Assert.True(color.HasValue, $"expected '{value}' to resolve to a color");
            return color.Value;
        }
    }

    /// <summary>The CSS Color 4 <c>color()</c> function and its predefined color spaces.</summary>
    public class ColorFunctionTests : CssConstructionFunctions
    {
        private static Color Resolve(string value) => ColorTestHelpers.Resolve(value);

        [Theory]
        [InlineData("color(srgb 1 0 0)", 255, 0, 0, 255)]
        [InlineData("color(srgb 100% 0% 50% / 50%)", 255, 0, 128, 128)]
        [InlineData("COLOR(SRGB 0 1 0 / 0.5)", 0, 255, 0, 128)]
        [InlineData("color(srgb-linear 0.5 0.5 0.5)", 188, 188, 188, 255)]
        [InlineData("color(display-p3 0 1 0)", 0, 255, 0, 255)]
        public void ColorFunction_ReadsThePredefinedSpaces(string text, int r, int g, int b, int a)
        {
            var color = Resolve(text);

            Assert.InRange((int)color.R, r - 2, r + 2);
            Assert.InRange((int)color.G, g - 2, g + 2);
            Assert.InRange((int)color.B, b - 2, b + 2);
            Assert.InRange((int)color.A, a - 1, a + 1);
        }

        [Theory]
        [InlineData("color(srgb 1 1 1)")]
        [InlineData("color(srgb-linear 1 1 1)")]
        [InlineData("color(display-p3 1 1 1)")]
        [InlineData("color(a98-rgb 1 1 1)")]
        [InlineData("color(prophoto-rgb 1 1 1)")]
        [InlineData("color(rec2020 1 1 1)")]
        [InlineData("color(xyz 0.95047 1 1.08883)")]
        [InlineData("color(xyz-d65 0.95047 1 1.08883)")]
        [InlineData("color(xyz-d50 0.96422 1 0.82521)")]
        public void ColorFunction_WhiteIsWhiteInEverySpace(string text)
        {
            var color = Resolve(text);

            Assert.InRange((int)color.R, 253, 255);
            Assert.InRange((int)color.G, 253, 255);
            Assert.InRange((int)color.B, 253, 255);
        }

        [Theory]
        [InlineData("color(a98-rgb 0.5 0.5 0.5)")]
        [InlineData("color(prophoto-rgb 0.5 0.5 0.5)")]
        [InlineData("color(rec2020 0.5 0.5 0.5)")]
        [InlineData("color(display-p3 0.5 0.5 0.5)")]
        public void ColorFunction_AGrayStaysGray(string text)
        {
            var color = Resolve(text);

            Assert.InRange((int)color.R - color.G, -2, 2);
            Assert.InRange((int)color.B - color.G, -2, 2);
            Assert.InRange((int)color.G, 100, 150);
        }

        [Fact]
        public void ColorFunction_WideGamutPrimaryIsClippedToSrgb()
        {
            // Pure display-p3 green is outside sRGB: it clips to the sRGB green corner.
            var color = Resolve("color(display-p3 0 1 0)");

            Assert.Equal(0, color.R);
            Assert.Equal(255, color.G);
        }

        [Fact]
        public void ColorFunction_PredefinedSpacesAgreeWithTheirDefinitions()
        {
            // srgb-linear 0.21404 is sRGB 0.5, and xyz-d65 of sRGB red is its matrix column.
            var linear = Resolve("color(srgb-linear 0.21404 0.21404 0.21404)");
            Assert.InRange((int)linear.R, 126, 129);

            var xyz = Resolve("color(xyz-d65 0.4124564 0.2126729 0.0193339)");
            Assert.InRange((int)xyz.R, 253, 255);
            Assert.InRange((int)xyz.G, 0, 2);
            Assert.InRange((int)xyz.B, 0, 2);
        }

        [Theory]
        [InlineData("color(srgb 1 0 0 0.5)")]
        [InlineData("color(srgb 1 0)")]
        [InlineData("color(nonsense 1 0 0)")]
        [InlineData("color(1 0 0)")]
        [InlineData("color(srgb red 0 0)")]
        public void ColorFunction_RejectsWhatItDoesNotRead(string text) =>
            Assert.False(ColorTestHelpers.TryResolve(text).HasValue);

        [Fact]
        public void ColorMix_AcceptsTheNewSpaces()
        {
            // Two identical operands mix to themselves in any interpolation space.
            foreach (var space in new[] { "a98-rgb", "prophoto-rgb", "rec2020" })
            {
                var mixed = Resolve($"color-mix(in {space}, rgb(200 100 50), rgb(200 100 50))");
                Assert.InRange((int)mixed.R, 198, 202);
                Assert.InRange((int)mixed.G, 98, 102);
                Assert.InRange((int)mixed.B, 48, 52);
            }
        }

        [Fact]
        public void ColorFunction_InsideColorMix_IsAnOperand()
        {
            var mixed = Resolve("color-mix(in srgb, color(srgb 1 0 0), color(srgb 0 0 1))");

            Assert.InRange((int)mixed.R, 126, 129);
            Assert.InRange((int)mixed.B, 126, 129);
        }

        // -- Parse level: accepted wherever a color is, and the authored text round-trips --

        [Theory]
        [InlineData("color(srgb 1 0 0)")]
        [InlineData("color(display-p3 0.5 0.25 1 / 0.5)")]
        [InlineData("color(a98-rgb 1 1 1)")]
        [InlineData("color(prophoto-rgb 0.5 0.5 0.5)")]
        [InlineData("color(rec2020 0.1 0.2 0.3)")]
        [InlineData("color(xyz-d50 0.96422 1 0.82521)")]
        public void ColorFunction_IsAcceptedAndRoundTrips_AsBackgroundColor(string value)
        {
            var sheet = ParseStyleSheet($"div {{ background-color: {value}; }}");
            var rule = sheet.StyleRules.OfType<StyleRule>().Single();

            Assert.Equal(value, rule.Style.GetPropertyValue("background-color"));
        }

        [Fact]
        public void ColorFunction_IsAcceptedByColorAndBorderColorAndShorthands()
        {
            var sheet = ParseStyleSheet(
                "div { color: color(display-p3 1 0 0); border-color: color(srgb 0 1 0); outline: 1px solid color(rec2020 0 0 1); background: color(a98-rgb 1 0 0); }");
            var style = sheet.StyleRules.OfType<StyleRule>().Single().Style;

            Assert.Equal("color(display-p3 1 0 0)", style.GetPropertyValue("color"));
            Assert.Equal("color(srgb 0 1 0)", style.GetPropertyValue("border-top-color"));
            Assert.Equal("color(rec2020 0 0 1)", style.GetPropertyValue("outline-color"));
            Assert.Equal("color(a98-rgb 1 0 0)", style.GetPropertyValue("background-color"));
        }

        [Fact]
        public void ColorFunction_IsAcceptedAsGradientStop()
        {
            var sheet = ParseStyleSheet("div { background-image: linear-gradient(color(display-p3 1 0 0), color(srgb 0 0 1)); }");
            var style = sheet.StyleRules.OfType<StyleRule>().Single().Style;

            Assert.False(string.IsNullOrEmpty(style.GetPropertyValue("background-image")));
        }
    }
}
