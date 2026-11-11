using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class GridTemplateAreasPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("none")]
        [InlineData("\"a b\" \"c d\"")]
        [InlineData("\"header header\" \"nav main\" \"footer footer\"")]
        [InlineData("\"a . b\"")]
        public void GridTemplateAreasLegalValues(string value)
        {
            var property = ParseDeclaration($"grid-template-areas: {value}");
            Assert.Equal("grid-template-areas", property.Name);
            Assert.IsType<GridTemplateAreasProperty>(property);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("\"a a\" \"a a a\"")]
        [InlineData("\"a b a\"")]
        [InlineData("100px")]
        [InlineData("\"\"")]
        public void GridTemplateAreasIllegalValues(string value)
        {
            var property = ParseDeclaration($"grid-template-areas: {value}");
            Assert.IsType<GridTemplateAreasProperty>(property);
            Assert.False(property.HasValue);
        }
    }
}
