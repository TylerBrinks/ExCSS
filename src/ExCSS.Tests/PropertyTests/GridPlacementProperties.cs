using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class GridPlacementPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("grid-column-start", "auto", "auto")]
        [InlineData("grid-column-start", "3", "3")]
        [InlineData("grid-column-start", "-1", "-1")]
        [InlineData("grid-column-end", "span 2", "span 2")]
        [InlineData("grid-row-start", "1", "1")]
        [InlineData("grid-row-end", "span 3", "span 3")]
        public void PlacementLonghandLegalValues(string name, string value, string expected)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("grid-column-start", "0")]
        [InlineData("grid-column-start", "span 0")]
        [InlineData("grid-row-end", "none")]
        [InlineData("grid-row-end", "1.5")]
        public void PlacementLonghandIllegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("grid-column", "1 / 3")]
        [InlineData("grid-column", "2 / span 2")]
        [InlineData("grid-row", "1 / -1")]
        [InlineData("grid-area", "1 / 1 / 3 / 3")]
        [InlineData("grid-area", "2 / 2")]
        public void PlacementShorthandLegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
        }

        [Fact]
        public void GridColumnShorthand_ExpandsToLonghands()
        {
            var declaration = ParseDeclarations("grid-column: 2 / span 3");
            Assert.Equal("2", declaration.GetPropertyValue("grid-column-start"));
            Assert.Equal("span 3", declaration.GetPropertyValue("grid-column-end"));
        }

        [Fact]
        public void GridArea_ExpandsToFourLonghands()
        {
            var declaration = ParseDeclarations("grid-area: 1 / 2 / 3 / 4");
            Assert.Equal("1", declaration.GetPropertyValue("grid-row-start"));
            Assert.Equal("2", declaration.GetPropertyValue("grid-column-start"));
            Assert.Equal("3", declaration.GetPropertyValue("grid-row-end"));
            Assert.Equal("4", declaration.GetPropertyValue("grid-column-end"));
        }

        [Theory]
        [InlineData("grid-column", "1 / 2 / 3")]
        [InlineData("grid-area", "1 / 2 / 3 / 4 / 5")]
        [InlineData("grid-column", "none / 2")]
        public void PlacementShorthandIllegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.False(property.HasValue);
        }
    }
}
