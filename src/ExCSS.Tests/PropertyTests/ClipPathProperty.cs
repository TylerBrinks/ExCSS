using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class ClipPathPropertyTests : CssConstructionFunctions
    {
        [Theory]
        // clip-path: none | <basic-shape> (CSS Masking 1 5.1 / CSS Shapes 1 3).
        [InlineData("none")]
        [InlineData("circle()")]
        [InlineData("circle(50%)")]
        [InlineData("circle(5em at center)")]
        [InlineData("circle(closest-side at 30% 40%)")]
        [InlineData("circle(farthest-side)")]
        [InlineData("ellipse(50% 40%)")]
        [InlineData("ellipse(closest-side farthest-side at center)")]
        [InlineData("ellipse()")]
        [InlineData("inset(10px)")]
        [InlineData("inset(10px 20px)")]
        [InlineData("inset(10px 20px 30px 40px)")]
        [InlineData("inset(10px round 5px)")]
        [InlineData("polygon(0 0, 100% 0, 100% 100%)")]
        [InlineData("polygon(evenodd, 0 0, 100% 0, 50% 100%)")]
        [InlineData("polygon(nonzero, 0% 0%, 50% 100%, 100% 0%)")]
        public void ClipPathLegalValues(string value)
        {
            var property = ParseDeclaration($"clip-path: {value}");

            Assert.Equal("clip-path", property.Name);
            Assert.IsType<ClipPathProperty>(property);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("clip-path: banana")]
        [InlineData("clip-path: circle(-5px)")]            // negative radius
        [InlineData("clip-path: ellipse(50%)")]            // ellipse needs two radii
        [InlineData("clip-path: ellipse(10% 20% 30%)")]    // ...not three
        [InlineData("clip-path: inset()")]                 // needs at least one offset
        [InlineData("clip-path: inset(10px round)")]       // round needs a radius
        [InlineData("clip-path: inset(1px 2px 3px 4px 5px)")]
        [InlineData("clip-path: polygon(0 0, 100%)")]      // incomplete vertex
        [InlineData("clip-path: polygon()")]               // needs at least one vertex
        [InlineData("clip-path: circle(at)")]              // at needs a position
        [InlineData("clip-path: circle(50% 50%)")]         // circle takes one radius, not two
        public void ClipPathIllegalValues(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.IsType<ClipPathProperty>(property);
            Assert.False(property.HasValue);
        }

        [Fact]
        public void ClipPathPreservesAuthoredText()
        {
            var property = ParseDeclaration("clip-path: polygon(0 0, 100% 0, 50% 100%)");

            Assert.Equal("polygon(0 0, 100% 0, 50% 100%)", property.Value);
        }
    }
}
