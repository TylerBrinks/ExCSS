namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    public class CssContentPropertyTests
    {
        static StyleRule Parse(string source)
        {
            var parser = new StylesheetParser();
            var rule = parser.Parse(source).Rules[0];
            return rule as StyleRule;
        }

        [Fact]
        public void CssContentParseStringWithDoubleQuoteEscape()
        {
            var source = "a{content:\"\\\"\"}";
            var parsed = Parse(source);
            Assert.Equal("\"\\\"\"", parsed.Style.Content);
        }

        [Fact]
        public void CssContentParseStringWithSingleQuoteEscape()
        {
            var source = "a{content:'\\''}";
            var parsed = Parse(source);
            Assert.Equal("\"'\"", parsed.Style.Content);
        }

        [Fact]
        public void CssContentParseStringWithDoubleQuoteMultipleEscapes()
        {
            var source = "a{content:\"abc\\\"\\\"d\\\"ef\"}";
            var parsed = Parse(source);
            Assert.Equal("\"abc\\\"\\\"d\\\"ef\"", parsed.Style.Content);
        }

        [Fact]
        public void CssContentParseStringWithSingleQuoteMultipleEscapes()
        {
            var source = "a{content:'abc\\'\\'d\\'ef'}";
            var parsed = Parse(source);
            Assert.Equal("\"abc''d'ef\"", parsed.Style.Content);
        }

        [Theory]
        [InlineData("counter(item)")]
        [InlineData("counter(item, upper-roman)")]
        [InlineData("counters(item, \".\")")]
        [InlineData("counters(item, \".\", lower-alpha)")]
        public void CssContentParsesCounterFunctions(string value)
        {
            var parsed = Parse("a{content:" + value + "}");
            Assert.False(string.IsNullOrEmpty(parsed.Style.Content));
        }

        [Fact]
        public void CssContentCountersInList()
        {
            var parsed = Parse("a{content:\"(\" counters(item, \".\") \")\"}");
            Assert.False(string.IsNullOrEmpty(parsed.Style.Content));
        }

        [Theory]
        [InlineData("rect(auto, 10px, auto, 0)")]
        [InlineData("rect(1px, auto, 3px, auto)")]
        [InlineData("rect(1px 2px 3px 4px)")]
        [InlineData("rect(auto auto auto auto)")]
        public void CssClipRectAcceptsAuto(string value)
        {
            var parsed = Parse("a{clip:" + value + "}");
            Assert.False(string.IsNullOrEmpty(parsed.Style.Clip));
        }

        [Fact]
        public void CssClipRectRejectsInvalidEdge()
        {
            var parsed = Parse("a{clip:rect(auto, foo, auto, 0)}");
            Assert.True(string.IsNullOrEmpty(parsed.Style.Clip));
        }
    }
}
