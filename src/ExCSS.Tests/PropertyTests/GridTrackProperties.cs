using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class GridTrackPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("grid-template-columns", "none", "none")]
        [InlineData("grid-template-columns", "100px 200px", "100px 200px")]
        [InlineData("grid-template-columns", "1fr 2fr", "1fr 2fr")]
        [InlineData("grid-template-columns", "repeat(3, 1fr)", "repeat(3, 1fr)")]
        [InlineData("grid-template-columns", "minmax(100px, 1fr)", "minmax(100px, 1fr)")]
        [InlineData("grid-template-columns", "repeat(auto-fill, minmax(200px, 1fr))", "repeat(auto-fill, minmax(200px, 1fr))")]
        [InlineData("grid-template-columns", "subgrid", "subgrid")]
        [InlineData("grid-template-columns", "subgrid [a] [b]", "subgrid [a] [b]")]
        [InlineData("grid-template-rows", "auto 1fr auto", "auto 1fr auto")]
        [InlineData("grid-auto-columns", "min-content", "min-content")]
        [InlineData("grid-auto-rows", "100px 200px", "100px 200px")]
        [InlineData("grid-auto-flow", "row", "row")]
        [InlineData("grid-auto-flow", "column dense", "column dense")]
        [InlineData("grid-auto-flow", "dense", "dense")]
        public void GridTrackLegalValues(string name, string value, string expected)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("grid-template-columns", "banana")]
        [InlineData("grid-template-columns", "minmax(1fr, 2fr)")]
        [InlineData("grid-template-columns", "repeat(0, 100px)")]
        [InlineData("grid-auto-columns", "repeat(2, 100px)")]
        [InlineData("grid-auto-flow", "row column")]
        [InlineData("grid-auto-flow", "dense dense")]
        public void GridTrackIllegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.False(property.HasValue);
        }
    }
}
