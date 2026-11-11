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

        [Fact]
        public void WhereRoundTripsAndHasZeroSpecificity()
        {
            var where = Assert.IsType<MatchesSelector>(Subject("a:where(.foo, #bar)"));

            Assert.Equal("where", where.Keyword);
            Assert.Equal("a:where(.foo,#bar)", ParseSelector("a:where(.foo, #bar)").Text);
            Assert.Equal(new Priority(0, 0, 0, 1), ParseSelector("a:where(.foo, #bar)").Specificity);
        }

        [Theory]
        [InlineData("-webkit-any")]
        [InlineData("-moz-any")]
        public void LegacyAnyAliasesParseLikeIs(string name)
        {
            var any = Assert.IsType<MatchesSelector>(Subject("a:" + name + "(.foo, #bar)"));

            Assert.Equal(name, any.Keyword);
            Assert.Equal("a:" + name + "(.foo,#bar)", ParseSelector("a:" + name + "(.foo, #bar)").Text);
            Assert.Equal(new Priority(0, 1, 0, 1), ParseSelector("a:" + name + "(.foo, #bar)").Specificity);
        }

        [Theory]
        [InlineData("p:is(.a, :nope-not-real)", "p:is(.a)")]
        [InlineData("p:is(:nope-not-real, .a)", "p:is(.a)")]
        [InlineData("p:is(.a, )", "p:is(.a)")]
        [InlineData("p:is(, .a)", "p:is(.a)")]
        [InlineData("p:is(.a, :unknown(x))", "p:is(.a)")]
        [InlineData("p:where(.a, :nope-not-real)", "p:where(.a)")]
        public void IsAndWhereDropInvalidAlternativesAndKeepTheOthers(string selector, string expected)
        {
            Assert.Equal(expected, ParseSelector(selector).Text);
        }

        [Theory]
        [InlineData("p:is(:nope-not-real)", "p:is()")]
        [InlineData("p:where(:nope-not-real)", "p:where()")]
        public void IsAndWhereWithOnlyInvalidAlternativesStayValidAndEmpty(string selector, string expected)
        {
            var sheet = ParseStyleSheet(selector + " { color: red } .b { color: blue }");

            Assert.Equal(2, sheet.Rules.Length);
            Assert.Equal(expected, ((StyleRule)sheet.Rules[0]).Selector.Text);
        }

        [Fact]
        public void EmptyForgivingListInASelectorListDoesNotInvalidateTheRule()
        {
            var sheet = ParseStyleSheet(".b, p:is(:nope-not-real) { color: blue }");

            Assert.Single(sheet.Rules);
        }

        [Fact]
        public void LegacyAnyFunctionsAreNotForgiving()
        {
            var sheet = ParseStyleSheet("p:-webkit-any(.a, :nope-not-real) { color: red }");

            Assert.Empty(sheet.Rules);
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
    }
}
