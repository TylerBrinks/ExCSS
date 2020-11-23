using System.Linq;

namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    public class JustifyContentPropertyTests : CssConstructionFunctions
    {
        [Fact]
        public void JustifyContentCenter()
        {
            var snippet = "justify-content: center";
            var property = ParseDeclaration(snippet);
            Assert.Equal("justify-content", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<JustifyContentProperty>(property);
            var concrete = (JustifyContentProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("center", concrete.Value);
        }

        [Fact]
        public void JustifyContentStart()
        {
            var snippet = "justify-content: start";
            var property = ParseDeclaration(snippet);
            Assert.Equal("justify-content", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<JustifyContentProperty>(property);
            var concrete = (JustifyContentProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("start", concrete.Value);
        }

        [Fact]
        public void JustifyContentSpaceAround()
        {
            var snippet = "justify-content: space-around";
            var property = ParseDeclaration(snippet);
            Assert.Equal("justify-content", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<JustifyContentProperty>(property);
            var concrete = (JustifyContentProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("space-around", concrete.Value);
        }

        [Fact]
        public void JustifyContentSpaceBetween()
        {
            var snippet = "justify-content: space-between";
            var property = ParseDeclaration(snippet);
            Assert.Equal("justify-content", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<JustifyContentProperty>(property);
            var concrete = (JustifyContentProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("space-between", concrete.Value);
        }

        [Fact]
        public void JustifyContentSpaceEvenly()
        {
            var snippet = "justify-content: space-evenly";
            var property = ParseDeclaration(snippet);
            Assert.Equal("justify-content", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<JustifyContentProperty>(property);
            var concrete = (JustifyContentProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("space-evenly", concrete.Value);
        }

        [Fact]
        public void ParseJustifyContent()
        {
            var parser = new StylesheetParser();
            var sheet = parser.Parse("a {justify-content: space-evenly; }");
            var style = ((StyleRule)sheet.StyleRules.First()).Style;
            Assert.Equal("space-evenly", style.JustifyContent);
        }
    }
}
