using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    public class CounterStyleRuleTests : CssConstructionFunctions
    {
        private static CounterStyleRule Parse(string css) =>
            ParseStyleSheet(css).Rules.OfType<CounterStyleRule>().Single();

        [Fact]
        public void ParsesNameAndAllDescriptors()
        {
            var rule = Parse("@counter-style thumbs { system: cyclic; negative: \"-\" \"\"; prefix: \"<\"; suffix: \"> \"; " +
                             "range: 1 infinite; pad: 2 \"0\"; fallback: decimal; symbols: \"A\" \"B\"; " +
                             "additive-symbols: 2 \"B\", 1 \"A\"; speak-as: auto; }");

            Assert.Equal(RuleType.CounterStyle, rule.Type);
            Assert.Equal("thumbs", rule.Name);
            Assert.Equal("cyclic", rule.System);
            Assert.Contains("-", rule.Negative);
            Assert.Contains("<", rule.Prefix);
            Assert.Contains(">", rule.Suffix);
            Assert.Equal("1 infinite", rule.Range);
            Assert.Contains("2", rule.Pad);
            Assert.Equal("decimal", rule.Fallback);
            Assert.Contains("\"A\"", rule.Symbols);
            Assert.Contains("2 \"B\"", rule.AdditiveSymbols);
            Assert.Equal("auto", rule.SpeakAs);
            Assert.Equal(10, rule.Length);
        }

        [Fact]
        public void RoundTripsToCss()
        {
            var rule = Parse("@counter-style x { system: fixed 3; symbols: \"a\" \"b\"; }");
            var css = rule.ToCss();

            Assert.StartsWith("@counter-style x {", css);
            Assert.Contains("system: fixed 3", css);
            Assert.Contains("symbols:", css);

            var again = Parse(css);
            Assert.Equal("x", again.Name);
            Assert.Equal("fixed 3", again.System);
        }

        [Theory]
        [InlineData("none")]
        [InlineData("inherit")]
        [InlineData("initial")]
        [InlineData("unset")]
        public void ReservedNameInvalidatesRule(string name)
        {
            var sheet = ParseStyleSheet($"@counter-style {name} {{ system: cyclic; symbols: \"a\"; }} .a {{ color: red }}");

            Assert.Empty(sheet.Rules.OfType<CounterStyleRule>());
            Assert.Single(sheet.Rules.OfType<StyleRule>());
        }

        [Fact]
        public void MissingNameInvalidatesRule()
        {
            var sheet = ParseStyleSheet("@counter-style { system: cyclic; } .a { color: red }");

            Assert.Empty(sheet.Rules.OfType<CounterStyleRule>());
            Assert.Single(sheet.Rules.OfType<StyleRule>());
        }

        [Fact]
        public void UnknownDescriptorIsDropped()
        {
            var rule = Parse("@counter-style x { symbols: \"a\"; color: red; bogus: 1; }");

            Assert.Equal(1, rule.Length);
            Assert.Equal(string.Empty, rule["color"]);
            Assert.Equal(string.Empty, rule["bogus"]);
        }

        [Fact]
        public void MultipleRules()
        {
            var sheet = ParseStyleSheet("@counter-style a { symbols: \"x\"; } @counter-style b { system: extends a; }");
            var rules = sheet.Rules.OfType<CounterStyleRule>().ToList();

            Assert.Equal(new[] { "a", "b" }, rules.Select(r => r.Name));
            Assert.Equal("extends a", rules[1].System);
        }

        [Fact]
        public void NestedInMedia()
        {
            var sheet = ParseStyleSheet("@media print { @counter-style p { system: cyclic; symbols: \"*\"; } }");
            var media = sheet.Rules.OfType<MediaRule>().Single();

            var rule = media.Children.OfType<CounterStyleRule>().Single();
            Assert.Equal("p", rule.Name);
        }

        [Theory]
        [InlineData("cyclic")]
        [InlineData("numeric")]
        [InlineData("alphabetic")]
        [InlineData("symbolic")]
        [InlineData("additive")]
        [InlineData("fixed")]
        [InlineData("fixed 5")]
        [InlineData("fixed -2")]
        [InlineData("extends decimal")]
        [InlineData("extends my-style")]
        public void SystemAcceptsValid(string value)
        {
            Assert.Equal(value, Parse($"@counter-style x {{ system: {value}; }}").System);
        }

        [Theory]
        [InlineData("bogus")]
        [InlineData("fixed 1.5")]
        [InlineData("fixed foo")]
        [InlineData("extends")]
        [InlineData("extends none")]
        [InlineData("extends 3")]
        [InlineData("cyclic 3")]
        [InlineData("\"cyclic\"")]
        [InlineData("fixed 1 2")]
        public void SystemRejectsInvalid(string value)
        {
            Assert.Equal(string.Empty, Parse($"@counter-style x {{ system: {value}; symbols: \"a\"; }}").System);
        }
    }
}
