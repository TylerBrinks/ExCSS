using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    public class SelectorObjectModelTests : CssConstructionFunctions
    {
        private static ISelector ParseSelector(string selector)
        {
            var sheet = ParseStyleSheet(selector + " { color: red }");
            return ((StyleRule)sheet.Rules[0]).Selector;
        }

        private static ISelector Subject(string selector)
        {
            var sel = ParseSelector(selector);
            return sel is CompoundSelector compound ? compound.Last() : sel;
        }

        [Fact]
        public void NotProducesNotSelectorRetainingInnerSelector()
        {
            var not = Assert.IsType<NotSelector>(Subject("a:not(.foo)"));

            Assert.IsType<ClassSelector>(not.Inner);
            Assert.Equal(".foo", not.Inner.Text);
            Assert.Equal("a:not(.foo)", ParseSelector("a:not(.foo)").Text);
        }

        [Fact]
        public void HasProducesHasSelectorRetainingInnerSelector()
        {
            var has = Assert.IsType<HasSelector>(Subject("a:has(.foo)"));

            Assert.Equal(".foo", has.Inner.Text);
            Assert.Equal("a:has(.foo)", ParseSelector("a:has(.foo)").Text);
        }

        [Fact]
        public void MatchesProducesMatchesSelectorRetainingInnerSelector()
        {
            var matches = Assert.IsType<MatchesSelector>(Subject("a:matches(.foo, #bar)"));

            Assert.IsType<ListSelector>(matches.Inner);
            Assert.Equal("matches", matches.Keyword);
            Assert.Equal("a:matches(.foo,#bar)", ParseSelector("a:matches(.foo, #bar)").Text);
        }

        [Fact]
        public void IsIsAnAliasOfMatchesAndRoundTripsAsIs()
        {
            var matches = Assert.IsType<MatchesSelector>(Subject("a:is(.foo, #bar)"));

            Assert.Equal("is", matches.Keyword);
            Assert.Equal("a:is(.foo,#bar)", ParseSelector("a:is(.foo, #bar)").Text);
        }

        [Theory]
        // CSS Selectors 4 16.1: the specificity of :is()/:not()/:has() is that of the most specific complex
        // selector in the argument. Priority is (inline, id, class, type).
        [InlineData(":is(em, #foo)", 0, 1, 0, 0)]          // spec's own example -> like #foo
        [InlineData(":not(em, strong#foo)", 0, 1, 0, 1)]   // spec's own example -> #foo + type
        [InlineData("a:not(.foo)", 0, 0, 1, 1)]
        [InlineData("a:has(.foo)", 0, 0, 1, 1)]
        [InlineData("a:is(.foo, #bar)", 0, 1, 0, 1)]
        public void ForgivingPseudoClassSpecificityIsMostSpecificArgument(string selector,
            int inline, int id, int cls, int type)
        {
            var specificity = ParseSelector(selector).Specificity;

            Assert.Equal(new Priority((byte)inline, (byte)id, (byte)cls, (byte)type), specificity);
        }

        [Fact]
        public void ListSelectorSpecificityIsMaxNotSum()
        {
            // A comma-separated list's specificity is the static max of its alternatives, not their sum.
            var list = Assert.IsType<ListSelector>(ParseSelector(".a, #b, c"));

            Assert.Equal(new Priority(0, 1, 0, 0), list.Specificity);
        }

        [Fact]
        public void CompoundSelectorSpecificityRemainsTheSum()
        {
            // A compound selector's members all constrain one element, so its specificity is still the sum.
            var compound = Assert.IsType<CompoundSelector>(ParseSelector("a.foo.bar"));

            Assert.Equal(new Priority(0, 0, 2, 1), compound.Specificity);
        }

        [Theory]
        [InlineData("p:lang(en)", ":lang(en)", new[] { "en" })]
        [InlineData("p:lang(\"en\")", ":lang(\"en\")", new[] { "en" })]
        [InlineData("p:lang(en, \"fr-CA\")", ":lang(en,\"fr-CA\")", new[] { "en", "fr-CA" })]
        public void LangProducesLangSelectorExposingItsLanguageRanges(string source, string text, string[] ranges)
        {
            var lang = Assert.IsType<LangSelector>(Subject(source));

            Assert.Equal(ranges, lang.LanguageRanges);
            Assert.Equal(ranges[0], lang.LanguageRange);
            Assert.Equal(text, lang.Text);
            Assert.Equal(Priority.OneClass, lang.Specificity);
            Assert.Equal("p" + text, ParseSelector(source).Text);
        }

        [Theory]
        [InlineData(":lang()")]
        [InlineData(":lang(en,)")]
        [InlineData(":lang(,en)")]
        [InlineData(":lang(en fr)")]
        [InlineData(":lang(1)")]
        public void MalformedLangArgumentIsNotALangSelector(string source)
        {
            var sheet = ParseStyleSheet(source + " { color: red }");

            Assert.Empty(sheet.Rules);
        }

        [Fact]
        public void OnlyChildProducesOnlyChildSelector()
        {
            var only = Assert.IsType<OnlyChildSelector>(Subject("li:only-child"));

            Assert.Equal(":only-child", only.Text);
            Assert.Equal(Priority.OneClass, only.Specificity);
            Assert.Equal("li:only-child", ParseSelector("li:only-child").Text);
            Assert.IsType<OnlyChildSelector>(Subject("li:ONLY-CHILD"));
        }

        [Fact]
        public void OnlyOfTypeProducesOnlyOfTypeSelector()
        {
            var only = Assert.IsType<OnlyOfTypeSelector>(Subject("li:only-of-type"));

            Assert.Equal(":only-of-type", only.Text);
            Assert.Equal(Priority.OneClass, only.Specificity);
            Assert.Equal("li:only-of-type", ParseSelector("li:only-of-type").Text);
        }

        [Theory]
        [InlineData(":first-child")]
        [InlineData(":last-child")]
        [InlineData(":first-of-type")]
        [InlineData(":last-of-type")]
        public void OtherStructuralPseudoClassesKeepTheirSerialization(string pseudo)
        {
            Assert.Equal("li" + pseudo, ParseSelector("li" + pseudo).Text);
        }

        [Fact]
        public void ComplexSelectorExposesEntriesIndexerAndSubject()
        {
            var complex = Assert.IsType<ComplexSelector>(ParseSelector("div > p span"));

            Assert.Equal(3, complex.Entries.Count);
            Assert.Equal("div", complex[0].Selector.Text);
            Assert.Equal(">", complex[0].Delimiter.Trim());
            Assert.Same(complex[2].Selector, complex.Subject);
            Assert.Equal("span", complex.Subject.Text);
            Assert.Null(new ComplexSelector().Subject);
        }
    }
}
