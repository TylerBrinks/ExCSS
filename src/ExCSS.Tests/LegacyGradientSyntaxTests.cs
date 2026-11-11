using Xunit;

namespace ExCSS.Tests
{
    public class LegacyGradientSyntaxTests : CssConstructionFunctions
    {
        private static Property Parse(string declaration) =>
            ParseDeclaration(declaration, enableVendorPrefixAliases: true);

        [Theory]
        [InlineData("-webkit-linear-gradient(red, blue)", "linear-gradient(red, blue)")]
        [InlineData("-moz-linear-gradient(top, red, blue)", "linear-gradient(to bottom, red, blue)")]
        [InlineData("-webkit-linear-gradient(left, red, blue)", "linear-gradient(to right, red, blue)")]
        [InlineData("-webkit-linear-gradient(bottom right, red, blue)", "linear-gradient(to left top, red, blue)")]
        [InlineData("-webkit-linear-gradient(right top, red, blue)", "linear-gradient(to left bottom, red, blue)")]
        [InlineData("-webkit-linear-gradient(45deg, red, blue)", "linear-gradient(45deg, red, blue)")]
        [InlineData("-webkit-linear-gradient(0deg, red, blue)", "linear-gradient(90deg, red, blue)")]
        [InlineData("-webkit-linear-gradient(90deg, red, blue)", "linear-gradient(0deg, red, blue)")]
        [InlineData("-webkit-linear-gradient(180deg, red, blue)", "linear-gradient(270deg, red, blue)")]
        [InlineData("-webkit-linear-gradient(-90deg, red, blue)", "linear-gradient(180deg, red, blue)")]
        [InlineData("-webkit-linear-gradient(0.25turn, red, blue)", "linear-gradient(0deg, red, blue)")]
        [InlineData("-webkit-linear-gradient(100grad, red, blue)", "linear-gradient(0deg, red, blue)")]
        [InlineData("-webkit-linear-gradient(1.5707963rad, red, blue)", "linear-gradient(0deg, red, blue)")]
        [InlineData("-webkit-linear-gradient(0, red, blue)", "linear-gradient(90deg, red, blue)")]
        [InlineData("-o-linear-gradient(left, red, blue)", "linear-gradient(to right, red, blue)")]
        [InlineData("-ms-linear-gradient(left, red, blue)", "linear-gradient(to right, red, blue)")]
        [InlineData("-webkit-radial-gradient(farthest-side, red, blue)", "radial-gradient(farthest-side, red, blue)")]
        [InlineData("-webkit-repeating-linear-gradient(left, red 0, blue 10px)", "repeating-linear-gradient(to right, red 0, blue 10px)")]
        [InlineData("-webkit-linear-gradient(to right, red, blue)", "linear-gradient(to right, red, blue)")]
        [InlineData("-webkit-radial-gradient(red, blue)", "radial-gradient(red, blue)")]
        [InlineData("-webkit-radial-gradient(center, circle cover, red, blue)", "radial-gradient(circle farthest-corner at center, red, blue)")]
        [InlineData("-webkit-radial-gradient(top left, ellipse contain, red, blue)", "radial-gradient(ellipse closest-side at top left, red, blue)")]
        [InlineData("-moz-radial-gradient(30% 40%, red, blue)", "radial-gradient(at 30% 40%, red, blue)")]
        [InlineData("-webkit-radial-gradient(circle closest-corner, red, blue)", "radial-gradient(circle closest-corner, red, blue)")]
        [InlineData("-webkit-repeating-radial-gradient(center, circle, red 0, blue 10px)", "repeating-radial-gradient(circle at center, red 0, blue 10px)")]
        [InlineData("-webkit-linear-gradient(left, rgba(255, 0, 0, 0.5) 10%, blue 90%)", "linear-gradient(to right, rgba(255, 0, 0, 0.5) 10%, blue 90%)")]
        public void LegacyGradientFunction_IsRewrittenToStandardSyntax(string legacy, string standard)
        {
            var property = Parse("background-image: " + legacy);
            var expected = ParseDeclaration("background-image: " + standard);

            Assert.NotNull(property);
            Assert.True(property.HasValue);
            Assert.NotNull(expected);
            Assert.Equal(expected.Value, property.Value);
        }

        [Theory]
        [InlineData("-webkit-linear-gradient(top top, red, blue)")]
        [InlineData("-webkit-linear-gradient(left right, red, blue)")]
        [InlineData("-webkit-linear-gradient()")]
        [InlineData("-webkit-gradient(linear, left top, left bottom, from(red), to(blue))")]
        [InlineData("-webkit-conic-gradient(red, blue)")]
        [InlineData("-khtml-linear-gradient(left, red, blue)")]
        public void UnrewritableLegacyGradient_IsLeftAsWrittenAndRejectedByTheConverter(string value)
        {
            Assert.False(Parse("background-image: " + value)?.HasValue ?? false);
        }

        [Fact]
        public void LegacyGradient_InTheBackgroundShorthand_IsRewrittenToo()
        {
            var sheet = ParseStyleSheet("div { background: -webkit-linear-gradient(left, red, blue) no-repeat }",
                enableVendorPrefixAliases: true);
            var style = ((StyleRule)sheet.Rules[0]).Style;

            Assert.Contains("linear-gradient(to right,", style.GetPropertyValue("background-image"));
            Assert.DoesNotContain("-webkit-", style.CssText);
        }

        [Fact]
        public void LegacyGradient_AmongMultipleBackgroundLayers_IsRewrittenPerLayer()
        {
            var property = Parse("background-image: url(a.png), -webkit-linear-gradient(left, red, blue)");

            Assert.True(property.HasValue);
            Assert.Contains("linear-gradient(to right,", property.Value);
            Assert.DoesNotContain("-webkit-", property.Value);
        }

        [Fact]
        public void LegacyGradient_InASupportsCondition_BehavesLikeTheStandardOne()
        {
            var sheet = ParseStyleSheet(
                "@supports (background-image: -webkit-linear-gradient(left, red, blue)) { div { color: red } }",
                enableVendorPrefixAliases: true);

            Assert.DoesNotContain("-webkit-", sheet.Rules[0].ToCss());
        }

        [Fact]
        public void FlagOff_LegacyGradientIsStillRejected()
        {
            Assert.False(ParseDeclaration("background-image: -webkit-linear-gradient(left, red, blue)")?.HasValue ?? false);

            var sheet = ParseStyleSheet("div { background-image: -webkit-linear-gradient(left, red, blue); color: red }");
            var style = ((StyleRule)sheet.Rules[0]).Style;
            Assert.Equal(string.Empty, style.GetPropertyValue("background-image"));
        }
    }
}
