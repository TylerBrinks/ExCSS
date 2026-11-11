using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class ClipPathPropertyTests : CssConstructionFunctions
    {
        [Theory]
        // clip-path: none | <basic-shape> (CSS Masking 1 5.1 / CSS Shapes 1 3).
        [InlineData("none")]
        [InlineData("border-box")]
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
        [InlineData("padding-box")]
        [InlineData("margin-box")]
        [InlineData("fill-box")]
        [InlineData("stroke-box")]
        [InlineData("view-box")]
        [InlineData("circle(50%) border-box")]
        [InlineData("content-box inset(10px)")]
        [InlineData("inset(10px round 5px 10px / 1px 2px 3px 4px)")]
        [InlineData("rect(10px auto 50% 0)")]
        [InlineData("rect(0 10px 20px 5px round 3px)")]
        [InlineData("xywh(0 0 100% 50%)")]
        [InlineData("xywh(10px 20px 30px 40px round 5px / 2px) fill-box")]
        [InlineData("path('M0 0 L10 10 Z')")]
        [InlineData("path(evenodd, 'M0,0 L10,0 L10,10 Z')")]
        [InlineData("path('M0 0h10v10H0z')")]
        [InlineData("path('M10 10 20 20 30 10')")]
        [InlineData("path('M0 0 C 10 0 10 10 0 10 S -10 20 0 20 Q 5 25 10 20 T 20 20')")]
        [InlineData("path('M0 0 a5 5 0 1,1 10 0')")]
        [InlineData("path('M0 0a5 5 0 1110 0')")]      // compact arc flags
        [InlineData("path('M1e2-3.5.5.5L.5 1E-1')")]
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
        [InlineData("clip-path: border-box padding-box")]  // one geometry-box only
        [InlineData("clip-path: circle(50%) inset(10px)")] // one basic-shape only
        [InlineData("clip-path: circle(50%) none")]
        [InlineData("clip-path: border-box border-box")]
        [InlineData("clip-path: inset(10px round 5px /)")]
        [InlineData("clip-path: inset(10px round -5px)")]
        [InlineData("clip-path: inset(10px round 1px 2px 3px 4px 5px)")]
        [InlineData("clip-path: rect(1px 2px 3px)")]
        [InlineData("clip-path: rect(1px 2px 3px 4px 5px)")]
        [InlineData("clip-path: rect(1px auto)")]
        [InlineData("clip-path: xywh(0 0 auto 10px)")]    // auto not allowed in xywh
        [InlineData("clip-path: xywh(0 0 -10px 10px)")]   // negative size
        [InlineData("clip-path: xywh(0 0 10px)")]
        [InlineData("clip-path: path()")]
        [InlineData("clip-path: path('')")]
        [InlineData("clip-path: path('   ')")]
        [InlineData("clip-path: path(evenodd)")]
        [InlineData("clip-path: path(banana, 'M0 0')")]
        [InlineData("clip-path: path('M0 0', 'M1 1')")]
        [InlineData("clip-path: path(M0 0)")]              // must be a string
        [InlineData("clip-path: path('L10 10')")]          // must start with moveto
        [InlineData("clip-path: path('M0')")]              // too few arguments
        [InlineData("clip-path: path('M0 0 L10')")]
        [InlineData("clip-path: path('M0 0 X10 10')")]    // unknown command
        [InlineData("clip-path: path('M0 0 Z 10 10')")]   // Z takes no arguments
        [InlineData("clip-path: path('M0 0 a5 5 0 2 1 10 0')")] // arc flag must be 0 or 1
        [InlineData("clip-path: path('M0 0 L1 x')")]
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
