using Xunit;

namespace ExCSS.Tests
{
    public class Flexbox : CssConstructionFunctions
    {
        [Fact]
        public void FlexDirectionRowLegal()
        {
            var snippet = "flex-direction: row";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-direction", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexDirectionProperty>(property);
            var concrete = (FlexDirectionProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("row", concrete.Value);
        }

        [Fact]
        public void FlexDirectionRowReverseLegal()
        {
            var snippet = "flex-direction: row-reverse";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-direction", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexDirectionProperty>(property);
            var concrete = (FlexDirectionProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("row-reverse", concrete.Value);
        }

        [Fact]
        public void FlexDirectionColumnLegal()
        {
            var snippet = "flex-direction: column";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-direction", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexDirectionProperty>(property);
            var concrete = (FlexDirectionProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("column", concrete.Value);
        }

        [Fact]
        public void FlexDirectionColumnReverseLegal()
        {
            var snippet = "flex-direction: column-reverse";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-direction", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexDirectionProperty>(property);
            var concrete = (FlexDirectionProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("column-reverse", concrete.Value);
        }
    }
}
