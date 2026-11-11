using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    /// <summary>
    /// <c>@starting-style { … }</c> (CSS Transitions 2) grouping rule.
    /// </summary>
    public class StartingStyleTests : CssConstructionFunctions
    {
        [Fact]
        public void ParsesAsStartingStyleRuleWithNestedStyleRule()
        {
            var sheet = ParseStyleSheet("@starting-style { .a { opacity: 0 } }");
            var rule = Assert.IsAssignableFrom<IStartingStyleRule>(Assert.Single(sheet.Rules));

            Assert.Equal(RuleType.StartingStyle, rule.Type);
            var inner = Assert.IsType<StyleRule>(Assert.Single(rule.Rules));
            Assert.Equal(".a", inner.SelectorText);
            Assert.Equal("0", inner.Style.Opacity);
        }

        [Fact]
        public void MultipleRulesAreKept()
        {
            var sheet = ParseStyleSheet("@starting-style { .a { opacity: 0 } .b { color: red } }");
            var rule = Assert.IsAssignableFrom<IStartingStyleRule>(sheet.Rules.First());

            Assert.Equal(2, rule.Rules.Count());
        }

        [Fact]
        public void EmptyBlockParses()
        {
            var sheet = ParseStyleSheet("@starting-style { }");
            var rule = Assert.IsAssignableFrom<IStartingStyleRule>(Assert.Single(sheet.Rules));

            Assert.Empty(rule.Rules);
        }

        [Fact]
        public void RoundTripsToCss()
        {
            var sheet = ParseStyleSheet("@starting-style { .a { opacity: 0 } }");
            var css = sheet.ToCss();

            Assert.StartsWith("@starting-style", css);
            var reparsed = ParseStyleSheet(css);
            var rule = Assert.IsAssignableFrom<IStartingStyleRule>(Assert.Single(reparsed.Rules));
            Assert.Equal(".a", ((StyleRule)Assert.Single(rule.Rules)).SelectorText);
            Assert.Equal(css, reparsed.ToCss());
        }

        [Fact]
        public void NestedInsideMedia()
        {
            var sheet = ParseStyleSheet("@media screen { @starting-style { .a { opacity: 0 } } }");
            var media = Assert.IsAssignableFrom<IMediaRule>(Assert.Single(sheet.Rules));
            var rule = Assert.IsAssignableFrom<IStartingStyleRule>(Assert.Single(media.Rules));

            Assert.Single(rule.Rules);
        }

        [Fact]
        public void MediaInsideStartingStyle()
        {
            var sheet = ParseStyleSheet("@starting-style { @media screen { .a { opacity: 0 } } }");
            var rule = Assert.IsAssignableFrom<IStartingStyleRule>(Assert.Single(sheet.Rules));
            var media = Assert.IsAssignableFrom<IMediaRule>(Assert.Single(rule.Rules));

            Assert.Single(media.Rules);
        }

        [Fact]
        public void IsNotTreatedAsUnknownRule()
        {
            var sheet = ParseStyleSheet("@starting-style { .a { opacity: 0 } } .b { color: red }");

            Assert.Equal(new[] { RuleType.StartingStyle, RuleType.Style }, sheet.Rules.Select(r => r.Type));
        }
    }
}
