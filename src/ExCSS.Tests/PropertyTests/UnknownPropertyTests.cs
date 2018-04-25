namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    //[TestFixture]
    public class UnknownPropertyTests : CssConstructionFunctions
    {
        [Fact]
        public void Test()
        {
            var snippet = "BackgroundColour: #ff0000;";
            var property = ParseDeclaration(snippet);
            Assert.Equal("background-attachment", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<BackgroundAttachmentProperty>(property);
            var concrete = (BackgroundAttachmentProperty)property;
            Assert.False(concrete.IsInherited);
            Assert.True(concrete.HasValue);
            Assert.Equal("scroll", concrete.Value);
        }

    }
}
