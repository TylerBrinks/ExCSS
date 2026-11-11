using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    public class FontFeatureValuesTests : CssConstructionFunctions
    {
        private static IFontFeatureValuesRule Parse(string css)
        {
            return ParseStyleSheet(css).Rules.OfType<IFontFeatureValuesRule>().Single();
        }

        [Fact]
        public void ParsesSingleFamilyAndBlock()
        {
            var rule = Parse("@font-feature-values MyFont { @styleset { nice-style: 12; } }");

            Assert.Equal(RuleType.FontFeatureValues, rule.Type);
            Assert.Equal(new[] {"MyFont"}, rule.FontFamilies);

            var block = Assert.IsAssignableFrom<IFontFeatureValueSetRule>(rule.Rules.Single());
            Assert.Equal(RuleType.FontFeatureValueSet, block.Type);
            Assert.Equal("styleset", block.Name);

            var declaration = block.Single();
            Assert.Equal("nice-style", declaration.Name);
            Assert.Equal("12", declaration.Value);
            Assert.Equal(new[] {12}, block.GetValues("nice-style"));
        }

        [Fact]
        public void ParsesMultipleFamilies()
        {
            var rule = Parse("@font-feature-values Font One, \"Font Two\" { @swash { flowing: 1; } }");

            Assert.Equal(new[] {"Font One", "\"Font Two\""}, rule.FontFamilies);
        }

        [Theory]
        [InlineData("styleset")]
        [InlineData("character-variant")]
        [InlineData("swash")]
        [InlineData("ornaments")]
        [InlineData("annotation")]
        [InlineData("stylistic")]
        public void ParsesEachBlockKind(string blockName)
        {
            var rule = Parse($"@font-feature-values MyFont {{ @{blockName} {{ foo: 1; }} }}");

            var block = (IFontFeatureValueSetRule) rule.Rules.Single();
            Assert.Equal(blockName, block.Name);
            Assert.Equal("1", block.Single().Value);
        }

        [Fact]
        public void ParsesMultipleIntegerValues()
        {
            var rule = Parse("@font-feature-values MyFont { @styleset { nice-style: 1 7; } }");

            var block = (IFontFeatureValueSetRule) rule.Rules.Single();
            Assert.Equal("1 7", block.Single().Value);
            Assert.Equal(new[] {1, 7}, block.GetValues("nice-style"));
            Assert.Empty(block.GetValues("missing"));
        }

        [Fact]
        public void ParsesMultipleBlocksAndDeclarations()
        {
            var rule = Parse("@font-feature-values MyFont { " +
                             "@styleset { nice-style: 1; alternate-style: 2; } " +
                             "@character-variant { flourish: 12 3; } " +
                             "}");

            var blocks = rule.Rules.Cast<IFontFeatureValueSetRule>().ToList();
            Assert.Equal(2, blocks.Count);
            Assert.Equal(2, blocks.Single(b => b.Name == "styleset").Length);
            Assert.Equal("12 3", blocks.Single(b => b.Name == "character-variant").Single().Value);
        }

        [Theory]
        [InlineData("nice: -1")]
        [InlineData("nice: 1.5")]
        [InlineData("nice: red")]
        [InlineData("nice: 1 red")]
        [InlineData("nice:")]
        public void RejectsInvalidValues(string declaration)
        {
            var rule = Parse($"@font-feature-values MyFont {{ @styleset {{ {declaration}; ok: 2; }} }}");

            var block = (IFontFeatureValueSetRule) rule.Rules.Single();
            Assert.Equal("ok", block.Single().Name);
        }

        [Fact]
        public void UnknownNestedBlockIsIgnored()
        {
            var rule = Parse("@font-feature-values MyFont { @bogus { foo: 1; } @styleset { real: 1; } }");

            Assert.Equal("styleset", ((IFontFeatureValueSetRule) rule.Rules.Single()).Name);
        }

        [Fact]
        public void DoesNotDerailFollowingRules()
        {
            var sheet = ParseStyleSheet("@font-feature-values MyFont { @styleset { nice-style: 1; } } .after { color: red; }");

            Assert.Single(sheet.Rules.OfType<IFontFeatureValuesRule>());
            var styleRule = sheet.Rules.OfType<StyleRule>().Single();
            Assert.Equal(".after", styleRule.SelectorText);
            Assert.Equal("rgb(255, 0, 0)", styleRule.Style.GetPropertyValue("color"));
        }

        [Fact]
        public void NoDeclarationBlockDoesNotCrashAndFollowingRuleApplies()
        {
            var sheet = ParseStyleSheet("@font-feature-values MyFont; .after { color: red; }");

            Assert.Equal(".after", sheet.Rules.OfType<StyleRule>().Single().SelectorText);
        }

        [Fact]
        public void DoublyNestedStrayContentIsSkippedAsAWhole()
        {
            var rule = Parse("@font-feature-values MyFont { @bogus { @nested { x: 1; } } @styleset { real: 1; } }");

            var block = (IFontFeatureValueSetRule) rule.Rules.Single();
            Assert.Equal("styleset", block.Name);
            Assert.Equal("1", block.Single().Value);
        }

        [Fact]
        public void BareStrayContentBeforeClosingBraceDoesNotSwallowFollowingRules()
        {
            var sheet = ParseStyleSheet("@font-feature-values MyFont { @styleset { nice-style: 1; } bogus } .after { color: red; }");

            var block = (IFontFeatureValueSetRule) sheet.Rules.OfType<IFontFeatureValuesRule>().Single().Rules.Single();
            Assert.Equal("1", block.Single().Value);
            Assert.Equal(".after", sheet.Rules.OfType<StyleRule>().Single().SelectorText);
        }

        [Fact]
        public void NestedBlockWithNoDeclarationBlockDoesNotCrashAndFollowingRuleApplies()
        {
            var sheet = ParseStyleSheet("@font-feature-values MyFont { @styleset; @swash { real: 1; } } .after { color: red; }");

            Assert.Equal(".after", sheet.Rules.OfType<StyleRule>().Single().SelectorText);
        }

        [Fact]
        public void RoundTripsToCss()
        {
            var sheet = ParseStyleSheet("@font-feature-values Font One, \"Font Two\" { @styleset { nice-style: 1 7; } @swash { flowing: 2; } }");

            var css = sheet.Rules.Single().ToCss();

            Assert.Contains("@font-feature-values Font One, \"Font Two\"", css);
            Assert.Contains("@styleset", css);
            Assert.Contains("nice-style: 1 7", css);
            Assert.Contains("@swash", css);
            Assert.Contains("flowing: 2", css);

            var reparsed = ParseStyleSheet(css).Rules.OfType<IFontFeatureValuesRule>().Single();
            Assert.Equal(new[] {"Font One", "\"Font Two\""}, reparsed.FontFamilies);
            Assert.Equal(2, reparsed.Rules.Length);
        }

        [Fact]
        public void TextSetterReplacesRule()
        {
            var rule = (FontFeatureValuesRule) Parse("@font-feature-values MyFont { @styleset { nice-style: 1; } }");

            rule.Text = "@font-feature-values OtherFont { @swash { flowing: 2; } }";

            Assert.Equal(new[] {"OtherFont"}, rule.FontFamilies);
            Assert.Equal("swash", ((IFontFeatureValueSetRule) rule.Rules.Single()).Name);
        }

        [Fact]
        public void SetPropertyOnBlockUsesIntegerValidation()
        {
            var rule = Parse("@font-feature-values MyFont { @styleset { nice-style: 1; } }");
            var block = (IFontFeatureValueSetRule) rule.Rules.Single();

            block.SetProperty("another-style", "3");
            block.SetProperty("bad-style", "red");

            Assert.Equal("3", block.GetPropertyValue("another-style"));
            Assert.Equal(string.Empty, block.GetPropertyValue("bad-style"));
            Assert.Contains("another-style", block.ToCss());
        }
    }
}
