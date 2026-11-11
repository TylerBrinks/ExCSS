using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class PagedMediaBookmarkTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("bleed", "auto", "auto")]
        [InlineData("bleed", "6pt", "6pt")]
        [InlineData("bleed", "0", "0")]
        [InlineData("marks", "none", "none")]
        [InlineData("marks", "crop", "crop")]
        [InlineData("marks", "cross", "cross")]
        [InlineData("marks", "crop cross", "crop cross")]
        [InlineData("marks", "cross crop", "crop cross")]
        [InlineData("bookmark-level", "none", "none")]
        [InlineData("bookmark-level", "1", "1")]
        [InlineData("bookmark-level", "6", "6")]
        [InlineData("bookmark-state", "open", "open")]
        [InlineData("bookmark-state", "closed", "closed")]
        [InlineData("bookmark-target", "self", "self")]
        [InlineData("bookmark-target", "none", "none")]
        [InlineData("bookmark-target", "url(#ch1)", "url(\"#ch1\")")]
        [InlineData("bookmark-target", "attr(href)", "attr(href)")]
        [InlineData("bookmark-label", "content(text)", "content()")]
        [InlineData("bookmark-label", "\"Chapter \" counter(chapter) \": \" content()",
            "\"Chapter \" counter(chapter) \": \" content()")]
        [InlineData("bookmark-label", "attr(title)", "attr(title)")]
        [InlineData("bookmark-label", "target-text(attr(href))", "target-text(attr(href))")]
        [InlineData("counter-set", "chapter", "chapter")]
        [InlineData("counter-set", "chapter 3", "chapter 3")]
        [InlineData("counter-set", "a 1 b -2", "a 1 b -2")]
        [InlineData("counter-set", "none", "none")]
        [InlineData("position", "running(header)", "running(header)")]
        [InlineData("position", "absolute", "absolute")]
        public void LegalValues(string name, string value, string expected)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("bleed: 10%")]
        [InlineData("bleed: none")]
        [InlineData("marks: crop crop")]
        [InlineData("marks: crop cross extra")]
        [InlineData("marks: none crop")]
        [InlineData("marks: 3")]
        [InlineData("bookmark-level: 0")]
        [InlineData("bookmark-level: -1")]
        [InlineData("bookmark-level: 1.5")]
        [InlineData("bookmark-level: h1")]
        [InlineData("bookmark-state: maybe")]
        [InlineData("bookmark-target: 5")]
        [InlineData("bookmark-label: 12")]
        [InlineData("bookmark-label: url(a.png)")]
        [InlineData("counter-set: 3")]
        [InlineData("position: running()")]
        [InlineData("position: running(#id)")]
        [InlineData("position: running(a b)")]
        [InlineData("position: running(\"a\")")]
        public void IllegalValues(string declaration)
        {
            var property = ParseDeclaration(declaration);

            Assert.False(property.HasValue);
        }

        [Fact]
        public void PropertiesParseInPageRule()
        {
            var sheet = ParseStyleSheet("@page { size: A4; bleed: 3mm; marks: crop cross; }");
            var rule = (IPageRule)sheet.Rules[0];

            Assert.Equal("3mm", rule.Style.GetPropertyValue("bleed"));
            Assert.Equal("crop cross", rule.Style.GetPropertyValue("marks"));
        }

        [Fact]
        public void StyleDeclarationAccessors()
        {
            var sheet = ParseStyleSheet(
                "h1 { bookmark-level: 1; bookmark-state: closed; bookmark-label: content(); " +
                "bookmark-target: attr(href); counter-set: n 2; }");
            var style = ((IStyleRule)sheet.Rules[0]).Style;

            Assert.Equal("1", style.BookmarkLevel);
            Assert.Equal("closed", style.BookmarkState);
            Assert.Equal("content()", style.BookmarkLabel);
            Assert.Equal("attr(href)", style.BookmarkTarget);
            Assert.Equal("n 2", style.CounterSet);
        }

        // GCPM / Generated Content functions in <content-list> positions.
        [Theory]
        [InlineData("target-counter(url(#ch2), page)", "target-counter(url(\"#ch2\"), page)")]
        [InlineData("target-counter(attr(href), page)", "target-counter(attr(href), page)")]
        [InlineData("target-counter(\"ch2\", page)", "target-counter(\"ch2\", page)")]
        [InlineData("target-counter(url(#ch2), page, upper-roman)", "target-counter(url(\"#ch2\"), page, upper-roman)")]
        [InlineData("target-counters(attr(href), section, \".\")", "target-counters(attr(href), section, \".\")")]
        [InlineData("target-counters(attr(href), section, \".\", lower-alpha)",
            "target-counters(attr(href), section, \".\", lower-alpha)")]
        [InlineData("target-text(attr(href))", "target-text(attr(href))")]
        [InlineData("target-text(url(#a), before)", "target-text(url(\"#a\"), before)")]
        [InlineData("target-text(attr(href), after)", "target-text(attr(href), after)")]
        [InlineData("target-text(attr(href), first-letter)", "target-text(attr(href), first-letter)")]
        [InlineData("leader()", "leader()")]
        [InlineData("leader(dotted)", "leader(dotted)")]
        [InlineData("leader(solid)", "leader(solid)")]
        [InlineData("leader(space)", "leader(space)")]
        [InlineData("leader(\" . \")", "leader(\" . \")")]
        [InlineData("element(header)", "element(header)")]
        [InlineData("element(header, first)", "element(header)")]
        [InlineData("element(header, last)", "element(header, last)")]
        [InlineData("element(header, start)", "element(header, start)")]
        [InlineData("element(header, first-except)", "element(header, first-except)")]
        [InlineData("\"Chapter Two\" leader(dotted) target-counter(attr(href), page)",
            "\"Chapter Two\" leader(dotted) target-counter(attr(href), page)")]
        public void ContentAcceptsGcpmFunctions(string value, string expected)
        {
            var property = ParseDeclaration($"content: {value}");

            Assert.IsType<ContentProperty>(property);
            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("target-counter(url(#ch2))")]
        [InlineData("target-counter()")]
        [InlineData("target-counter(42, page)")]
        [InlineData("target-counter(url(#a), 5)")]
        [InlineData("target-counter(url(#a), page, 5)")]
        [InlineData("target-counter(url(#a), page, upper-roman, extra)")]
        [InlineData("target-counters(attr(href), section)")]
        [InlineData("target-counters(attr(href), section, 5)")]
        [InlineData("target-text()")]
        [InlineData("target-text(42)")]
        [InlineData("target-text(attr(href), bogus)")]
        [InlineData("target-text(attr(href), content, extra)")]
        [InlineData("leader(bogus)")]
        [InlineData("leader(dotted, solid)")]
        [InlineData("leader(5)")]
        [InlineData("element()")]
        [InlineData("element(#id)")]
        [InlineData("element(header, bogus)")]
        [InlineData("element(header, first, extra)")]
        [InlineData("element(\"header\")")]
        public void ContentRejectsInvalidGcpmFunctions(string value)
        {
            var property = ParseDeclaration($"content: {value}");

            Assert.IsType<ContentProperty>(property);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("chapter target-text(attr(href))", "chapter target-text(attr(href))")]
        [InlineData("page target-counter(attr(href), page) leader(dotted)",
            "page target-counter(attr(href), page) leader(dotted)")]
        [InlineData("running element(header)", "running element(header)")]
        public void StringSetAcceptsGcpmFunctions(string value, string expected)
        {
            var property = ParseDeclaration($"string-set: {value}");

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Fact]
        public void StringSetRejectsInvalidGcpmFunction()
        {
            Assert.False(ParseDeclaration("string-set: x target-text(42)").HasValue);
        }
    }
}
