using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class GridNamedLinesPropertyTests : CssConstructionFunctions
    {
        [Theory]
        // End-to-end: the [name] line-name groups must survive strict value parsing (ValueBuilder must
        // accept the square-bracket tokens) and then validate through the track-list grammar.
        [InlineData("grid-template-columns", "[sidebar-start] 200px [sidebar-end] 1fr")]
        [InlineData("grid-template-columns", "[a b] 100px [c]")]
        [InlineData("grid-template-rows", "[start] repeat(3, 100px) [end]")]
        public void NamedLineTrackLists_AreAccepted(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("grid-template-columns", "[unclosed 100px")]
        [InlineData("grid-template-columns", "repeat(2, [x] 1fr)")]
        public void InvalidNamedLineTrackLists_AreDropped(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("grid-column-start", "sidebar")]
        [InlineData("grid-row-end", "main-start")]
        [InlineData("grid-column-start", "col 2")]
        public void NamedGridLinePlacement_IsAccepted(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Fact]
        public void GridColumn_BareCustomIdent_CopiesToEnd()
        {
            // CSS Grid 8.3.1: a bare <custom-ident> propagates to the omitted end edge.
            var declaration = ParseDeclarations("grid-column: main");
            Assert.Equal("main", declaration.GetPropertyValue("grid-column-start"));
            Assert.Equal("main", declaration.GetPropertyValue("grid-column-end"));
        }

        [Fact]
        public void GridArea_BareCustomIdent_CopiesToAllFourEdges()
        {
            var declaration = ParseDeclarations("grid-area: main");
            Assert.Equal("main", declaration.GetPropertyValue("grid-row-start"));
            Assert.Equal("main", declaration.GetPropertyValue("grid-column-start"));
            Assert.Equal("main", declaration.GetPropertyValue("grid-row-end"));
            Assert.Equal("main", declaration.GetPropertyValue("grid-column-end"));
        }
    }
}
