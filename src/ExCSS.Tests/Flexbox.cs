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

        [Fact]
        public void FlexWrapNoWrapLegal()
        {
            var snippet = "flex-wrap: nowrap";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-wrap", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexWrapProperty>(property);
            var concrete = (FlexWrapProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("nowrap", concrete.Value);
        }

        [Fact]
        public void FlexWrapWrapLegal()
        {
            var snippet = "flex-wrap: wrap";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-wrap", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexWrapProperty>(property);
            var concrete = (FlexWrapProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("wrap", concrete.Value);
        }

        [Fact]
        public void FlexWrapWrapReverseLegal()
        {
            var snippet = "flex-wrap: wrap-reverse";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-wrap", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexWrapProperty>(property);
            var concrete = (FlexWrapProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("wrap-reverse", concrete.Value);
        }

        [Fact]
        public void OrderLegal()
        {
            var snippet = "order: 1";
            var property = ParseDeclaration(snippet);
            Assert.Equal("order", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<OrderProperty>(property);
            var concrete = (OrderProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("1", concrete.Value);
        }
    }
}
