using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class GridBoxAlignmentPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("justify-items", "center")]
        [InlineData("justify-items", "start")]
        [InlineData("justify-items", "end")]
        [InlineData("justify-items", "stretch")]
        [InlineData("justify-self", "center")]
        [InlineData("justify-self", "auto")]
        [InlineData("place-items", "center")]
        [InlineData("place-items", "start end")]
        [InlineData("place-content", "center")]
        [InlineData("place-content", "space-between center")]
        [InlineData("place-self", "center")]
        [InlineData("place-self", "start end")]
        public void BoxAlignmentLegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
        }

        [Fact]
        public void PlaceItems_ExpandsToAlignAndJustifyItems()
        {
            var declaration = ParseDeclarations("place-items: center start");
            Assert.Equal("center", declaration.GetPropertyValue("align-items"));
            Assert.Equal("start", declaration.GetPropertyValue("justify-items"));
        }

        [Fact]
        public void PlaceSelf_SingleValueAppliesToBothAxes()
        {
            var declaration = ParseDeclarations("place-self: center");
            Assert.Equal("center", declaration.GetPropertyValue("align-self"));
            Assert.Equal("center", declaration.GetPropertyValue("justify-self"));
        }

        [Theory]
        [InlineData("justify-items", "banana")]
        [InlineData("place-content", "center start end")]
        public void BoxAlignmentIllegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.False(property.HasValue);
        }
    }
}
