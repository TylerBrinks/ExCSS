using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class ShapePropertiesTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("none")]
        [InlineData("margin-box")]
        [InlineData("border-box")]
        [InlineData("padding-box")]
        [InlineData("content-box")]
        [InlineData("circle(50%)")]
        [InlineData("circle(closest-side at 30% 40%)")]
        [InlineData("ellipse(50% 40%)")]
        [InlineData("inset(10px round 5px)")]
        [InlineData("polygon(0 0, 100% 0, 50% 100%)")]
        [InlineData("circle(50%) margin-box")]
        [InlineData("margin-box circle(50%)")]
        [InlineData("polygon(0 0, 100% 0, 50% 100%) content-box")]
        [InlineData("url(shape.png)")]
        [InlineData("linear-gradient(transparent, black)")]
        [InlineData("image-set(url(a.png) 1x, url(b.png) 2x)")]
        public void ShapeOutsideLegalValues(string value)
        {
            var property = ParseDeclaration($"shape-outside: {value}");

            Assert.IsType<ShapeOutsideProperty>(property);
            Assert.True(property.HasValue, value);
            Assert.False(property.IsInherited);
        }

        [Fact]
        public void ShapeOutsidePreservesAuthoredText()
        {
            Assert.Equal("circle(50%) margin-box", ParseDeclaration("shape-outside: circle(50%) margin-box").Value);
        }

        [Theory]
        [InlineData("banana")]
        [InlineData("none none")]
        [InlineData("none circle(50%)")]
        [InlineData("margin-box border-box")]            // two boxes
        [InlineData("circle(50%) ellipse(1px 2px)")]     // two shapes
        [InlineData("circle(50%) margin-box margin-box")]
        [InlineData("url(a.png) margin-box")]            // an image takes no box
        [InlineData("circle(-5px)")]
        [InlineData("polygon()")]
        [InlineData("text")]
        [InlineData("fill-box")]
        public void ShapeOutsideIllegalValues(string value)
        {
            var property = ParseDeclaration($"shape-outside: {value}");

            Assert.IsType<ShapeOutsideProperty>(property);
            Assert.False(property.HasValue, value);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("10px")]
        [InlineData("2.5em")]
        [InlineData("50%")]
        [InlineData("calc(1px + 5%)")]
        public void ShapeMarginLegalValues(string value)
        {
            var property = ParseDeclaration($"shape-margin: {value}");

            Assert.IsType<ShapeMarginProperty>(property);
            Assert.True(property.HasValue, value);
            Assert.True(property.IsAnimatable);
            Assert.False(property.IsInherited);
        }

        [Theory]
        [InlineData("-1px")]
        [InlineData("-5%")]
        [InlineData("auto")]
        [InlineData("1")]
        [InlineData("1px 2px")]
        [InlineData("banana")]
        public void ShapeMarginIllegalValues(string value)
        {
            Assert.False(ParseDeclaration($"shape-margin: {value}").HasValue, value);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("0.5")]
        [InlineData("1")]
        [InlineData("50%")]
        [InlineData("2")]            // out-of-range values are legal and clamped at use
        public void ShapeImageThresholdLegalValues(string value)
        {
            var property = ParseDeclaration($"shape-image-threshold: {value}");

            Assert.IsType<ShapeImageThresholdProperty>(property);
            Assert.True(property.HasValue, value);
            Assert.True(property.IsAnimatable);
            Assert.False(property.IsInherited);
        }

        [Theory]
        [InlineData("-0.5")]
        [InlineData("10px")]
        [InlineData("auto")]
        [InlineData("0.5 0.5")]
        public void ShapeImageThresholdIllegalValues(string value)
        {
            Assert.False(ParseDeclaration($"shape-image-threshold: {value}").HasValue, value);
        }

        [Fact]
        public void ShapePropertiesAcceptGlobalKeywords()
        {
            Assert.True(ParseDeclaration("shape-outside: inherit").HasValue);
            Assert.True(ParseDeclaration("shape-margin: initial").HasValue);
            Assert.True(ParseDeclaration("shape-image-threshold: unset").HasValue);
        }

        [Fact]
        public void StyleDeclarationExposesShapeAccessors()
        {
            var style = ((StyleRule)ParseStyleSheet(
                ".a { shape-outside: circle(50%); shape-margin: 4px; shape-image-threshold: 0.3 }").Rules[0]).Style;

            Assert.Equal("circle(50%)", style.ShapeOutside);
            Assert.Equal("4px", style.ShapeMargin);
            Assert.Equal("0.3", style.ShapeImageThreshold);
        }
    }
}
