using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class StringSetPropertyTests : CssConstructionFunctions
    {
        [Theory]
        // string-set: [ <custom-ident> <content-list> ]# | none (GCPM 7.3).
        [InlineData("chapter content()", "chapter content()")]
        [InlineData("title \"x\"", "title \"x\"")]
        [InlineData("heading content(text)", "heading content()")]
        [InlineData("h string(chapter)", "h string(chapter)")]
        [InlineData("chapter counter(page)", "chapter counter(page)")]
        [InlineData("chapter content(), section content()", "chapter content(), section content()")]
        [InlineData("chapter \"pre \" content()", "chapter \"pre \" content()")]
        [InlineData("none", "none")]
        public void StringSetLegalValues(string value, string expected)
        {
            var property = ParseDeclaration($"string-set: {value}");

            Assert.Equal("string-set", property.Name);
            Assert.IsType<StringSetProperty>(property);
            var concrete = (StringSetProperty)property;
            Assert.True(concrete.HasValue);
            Assert.Equal(expected, concrete.Value);
        }

        [Theory]
        [InlineData("string-set: chapter")]              // a name with no content-list
        [InlineData("string-set: \"x\"")]                // a content-list with no name
        [InlineData("string-set: chapter content(bogus)")]
        public void StringSetIllegalValues(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.IsType<StringSetProperty>(property);
            Assert.False(property.HasValue);
        }

        [Theory]
        // string() and content() are also valid inside the content property.
        [InlineData("content: string(chapter)", "string(chapter)")]
        [InlineData("content: string(chapter, first)", "string(chapter)")]
        [InlineData("content: string(chapter, last)", "string(chapter, last)")]
        [InlineData("content: content(before)", "content(before)")]
        [InlineData("content: content(text)", "content()")]
        [InlineData("content: \"Chapter \" content()", "\"Chapter \" content()")]
        public void ContentAcceptsStringAndContentFunctions(string declaration, string expected)
        {
            var property = ParseDeclaration(declaration);

            Assert.Equal("content", property.Name);
            Assert.IsType<ContentProperty>(property);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("content: string()")]                 // string() needs a name
        [InlineData("content: string(a, bogus)")]         // invalid keyword
        [InlineData("content: content(bogus)")]           // invalid mode
        public void ContentRejectsMalformedFunctions(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.IsType<ContentProperty>(property);
            Assert.False(property.HasValue);
        }
    }
}
