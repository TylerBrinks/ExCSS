using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class PageRuleTests : CssConstructionFunctions
    {
        private static PageRule ParsePage(string source)
        {
            var sheet = ParseStyleSheet(source);
            return (PageRule)sheet.Rules.First();
        }

        [Theory]
        // CSS Paged Media 3 4.1: an @page selector is a comma-separated list of "[<name>]? [:<pseudo>]?"
        // entries.
        [InlineData("@page :left { margin: 1cm }", ":left")]
        [InlineData("@page :first { margin: 1cm }", ":first")]
        [InlineData("@page chapter { margin: 1cm }", "chapter")]
        [InlineData("@page chapter:left { margin: 1cm }", "chapter:left")]
        [InlineData("@page chapter1:left, chapter2:left { margin: 1cm }", "chapter1:left, chapter2:left")]
        [InlineData("@page a, b, c { margin: 1cm }", "a, b, c")]
        public void PageSelectorForms(string source, string expectedSelector)
        {
            var rule = ParsePage(source);

            Assert.Equal(RuleType.Page, rule.Type);
            Assert.Equal(expectedSelector, rule.SelectorText);
        }

        [Fact]
        public void PageSelectorListEntriesAreExposed()
        {
            var rule = ParsePage("@page chapter1:left, chapter2:right { margin: 1cm }");
            var selector = Assert.IsType<PageSelector>(rule.Selector);

            Assert.Equal(2, selector.Entries.Count);
            Assert.Equal("chapter1", selector.Entries[0].Name);
            Assert.Equal("left", selector.Entries[0].Pseudo);
            Assert.Equal("chapter2", selector.Entries[1].Name);
            Assert.Equal("right", selector.Entries[1].Pseudo);
        }

        [Theory]
        // The @page "size" descriptor (CSS Paged Media 3 6.3).
        [InlineData("size: A4", "A4")]
        [InlineData("size: letter", "letter")]
        [InlineData("size: landscape", "landscape")]
        [InlineData("size: 8.5in 11in", "8.5in 11in")]
        [InlineData("size: 210mm", "210mm")]
        public void PageSizeDescriptor(string declaration, string value)
        {
            var style = ParseDeclarations(declaration);

            Assert.Equal(value, style.Size);
        }

        [Theory]
        // The "page" property (CSS Paged Media 3 3.4).
        [InlineData("page: chapter", "chapter")]
        [InlineData("page: auto", "auto")]
        public void PageProperty(string declaration, string value)
        {
            var style = ParseDeclarations(declaration);

            Assert.Equal(value, style.PageName);
        }
    }
}
