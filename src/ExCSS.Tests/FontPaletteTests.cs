using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    public class FontPaletteTests : CssConstructionFunctions
    {
        private string FontPalette(string value)
        {
            var sheet = ParseStyleSheet($".x {{ font-palette: {value}; }}");
            var rule = sheet.Rules.OfType<StyleRule>().Single();
            return rule.Style.GetPropertyValue("font-palette");
        }

        [Theory]
        [InlineData("normal")]
        [InlineData("light")]
        [InlineData("dark")]
        [InlineData("--my-palette")]
        public void FontPaletteAcceptsValidValues(string value)
        {
            Assert.Equal(value, FontPalette(value));
        }

        [Theory]
        [InlineData("5px")]
        [InlineData("#fff")]
        [InlineData("rgb(0,0,0)")]
        public void FontPaletteDropsInvalidValues(string value)
        {
            Assert.Equal(string.Empty, FontPalette(value));
        }

        [Fact]
        public void FontPaletteRoundTrips()
        {
            var sheet = ParseStyleSheet(".x { font-palette: --my-palette; }");
            var rule = sheet.Rules.OfType<StyleRule>().Single();

            Assert.Equal("--my-palette", rule.Style.GetPropertyValue("font-palette"));
            Assert.Contains("font-palette: --my-palette", rule.ToCss());
        }

        [Fact]
        public void FontPaletteValuesParsesNameAndDescriptors()
        {
            var src = "@font-palette-values --brand { font-family: \"Nabla\"; base-palette: 2; override-colors: 0 #ff0000, 1 rgb(0, 255, 0); }";
            var sheet = ParseStyleSheet(src);

            var rule = sheet.Rules.OfType<FontPaletteValuesRule>().Single();
            Assert.Equal(RuleType.FontPaletteValues, rule.Type);
            Assert.Equal("--brand", rule.Name);
            Assert.Contains("Nabla", rule.Family);
            Assert.Equal("2", rule.BasePalette);
            Assert.Contains("#ff0000", rule.OverrideColors);
            Assert.Contains("rgb(0, 255, 0)", rule.OverrideColors);
        }

        [Fact]
        public void FontPaletteValuesLightDarkBasePalette()
        {
            var sheet = ParseStyleSheet("@font-palette-values --d { font-family: Nabla; base-palette: dark; }");
            var rule = sheet.Rules.OfType<FontPaletteValuesRule>().Single();
            Assert.Equal("dark", rule.BasePalette);
        }

        [Fact]
        public void FontPaletteValuesRoundTrips()
        {
            var src = "@font-palette-values --p { font-family: Nabla; base-palette: 1; }";
            var sheet = ParseStyleSheet(src);
            var rule = sheet.Rules.OfType<FontPaletteValuesRule>().Single();

            var css = rule.ToCss();
            Assert.StartsWith("@font-palette-values --p {", css);
            Assert.Contains("font-family: Nabla", css);
            Assert.Contains("base-palette: 1", css);
        }

        [Fact]
        public void FontPaletteValuesIsExposedAsIFontPaletteValuesRule()
        {
            var sheet = ParseStyleSheet("@font-palette-values --c { font-family: Nabla; base-palette: 1; }");
            var rule = Assert.IsAssignableFrom<IFontPaletteValuesRule>(sheet.Rules.First());

            Assert.Equal("--c", rule.Name);
        }

        [Fact]
        public void FontPaletteValuesAmongOtherRules()
        {
            var src = ".a { color: red } @font-palette-values --p { font-family: Nabla; base-palette: 1; } .b { color: blue }";
            var sheet = ParseStyleSheet(src);

            Assert.Equal(3, sheet.Rules.Length);
            Assert.IsType<StyleRule>(sheet.Rules[0]);
            Assert.IsType<FontPaletteValuesRule>(sheet.Rules[1]);
            Assert.IsType<StyleRule>(sheet.Rules[2]);
        }

        [Fact]
        public void FontPaletteValuesDoesNotDerailFollowingRules()
        {
            var src = "@font-palette-values --p { font-family: Nabla; base-palette: 1; } .after { color: red; }";
            var sheet = ParseStyleSheet(src);

            Assert.Single(sheet.Rules.OfType<FontPaletteValuesRule>());
            var styleRule = sheet.Rules.OfType<StyleRule>().Single();
            Assert.Equal(".after", styleRule.SelectorText);
            Assert.Equal("rgb(255, 0, 0)", styleRule.Style.GetPropertyValue("color"));
        }

        [Fact]
        public void FontPaletteValuesNoDeclarationBlockDoesNotCrashAndFollowingRuleApplies()
        {
            var sheet = ParseStyleSheet("@font-palette-values --x; .after { color: red; }");
            var styleRule = sheet.Rules.OfType<StyleRule>().Single();
            Assert.Equal(".after", styleRule.SelectorText);
        }
    }
}
