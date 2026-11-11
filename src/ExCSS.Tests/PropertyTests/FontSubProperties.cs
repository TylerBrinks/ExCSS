using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class FontSubPropertyTests : CssConstructionFunctions
    {
        // A single-declaration parse: the property as written (shorthands stay unexpanded here).
        private static Property Parse(string name, string value) => ParseDeclaration($"{name}: {value}");

        private static string Computed(string declarations, string name) =>
            ParseDeclarations(declarations).GetPropertyValue(name);

        // --- simple keyword longhands ---------------------------------------------------------------

        [Theory]
        [InlineData("font-kerning", "auto")]
        [InlineData("font-kerning", "normal")]
        [InlineData("font-kerning", "none")]
        [InlineData("font-optical-sizing", "auto")]
        [InlineData("font-optical-sizing", "none")]
        [InlineData("font-synthesis-weight", "auto")]
        [InlineData("font-synthesis-weight", "none")]
        [InlineData("font-synthesis-style", "auto")]
        [InlineData("font-synthesis-style", "none")]
        [InlineData("font-synthesis-small-caps", "auto")]
        [InlineData("font-synthesis-small-caps", "none")]
        [InlineData("font-synthesis-position", "auto")]
        [InlineData("font-synthesis-position", "none")]
        [InlineData("font-variant-caps", "normal")]
        [InlineData("font-variant-caps", "small-caps")]
        [InlineData("font-variant-caps", "all-small-caps")]
        [InlineData("font-variant-caps", "petite-caps")]
        [InlineData("font-variant-caps", "all-petite-caps")]
        [InlineData("font-variant-caps", "unicase")]
        [InlineData("font-variant-caps", "titling-caps")]
        [InlineData("font-variant-position", "normal")]
        [InlineData("font-variant-position", "sub")]
        [InlineData("font-variant-position", "super")]
        [InlineData("font-variant-emoji", "normal")]
        [InlineData("font-variant-emoji", "text")]
        [InlineData("font-variant-emoji", "emoji")]
        [InlineData("font-variant-emoji", "unicode")]
        public void KeywordLonghandsAcceptTheirKeywords(string name, string value)
        {
            var property = Parse(name, value);

            Assert.Equal(name, property.Name);
            Assert.False(property.IsImportant);
            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("font-kerning", "bold")]
        [InlineData("font-kerning", "auto none")]
        [InlineData("font-kerning", "1")]
        [InlineData("font-optical-sizing", "normal")]
        [InlineData("font-synthesis-weight", "weight")]
        [InlineData("font-synthesis-style", "normal")]
        [InlineData("font-synthesis-small-caps", "small-caps")]
        [InlineData("font-synthesis-position", "sub")]
        [InlineData("font-variant-caps", "smallcaps")]
        [InlineData("font-variant-caps", "small-caps unicase")]
        [InlineData("font-variant-position", "small-caps")]
        [InlineData("font-variant-position", "sub super")]
        [InlineData("font-variant-emoji", "none")]
        public void KeywordLonghandsRejectOtherValues(string name, string value)
        {
            var property = Parse(name, value);

            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("font-kerning")]
        [InlineData("font-optical-sizing")]
        [InlineData("font-synthesis-weight")]
        [InlineData("font-synthesis-style")]
        [InlineData("font-synthesis-small-caps")]
        [InlineData("font-synthesis-position")]
        [InlineData("font-variant-caps")]
        [InlineData("font-variant-position")]
        [InlineData("font-variant-emoji")]
        [InlineData("font-variant-ligatures")]
        [InlineData("font-variant-numeric")]
        [InlineData("font-variant-east-asian")]
        [InlineData("font-variant-alternates")]
        [InlineData("font-variation-settings")]
        [InlineData("font-feature-settings")]
        [InlineData("font-language-override")]
        public void LonghandsAreInheritedAndNotAnimatable(string name)
        {
            var property = Parse(name, "inherit");

            Assert.True(property.HasValue);
            Assert.Equal("inherit", property.Value);
            Assert.True(property.CanBeInherited);
            Assert.False(property.IsAnimatable);
        }

        [Fact]
        public void KeywordsAreCaseInsensitiveAndSerializeLowercase()
        {
            Assert.Equal("none", Parse("font-kerning", "NONE").Value);
            Assert.Equal("small-caps", Parse("font-variant-caps", "Small-Caps").Value);
        }

        [Fact]
        public void FontSizeAdjustAndStretchStayAnimatable()
        {
            Assert.True(Parse("font-size-adjust", "0.5").IsAnimatable);
            Assert.True(Parse("font-stretch", "50%").IsAnimatable);
        }

        // --- font-variant-ligatures / numeric / east-asian -----------------------------------------

        [Theory]
        [InlineData("font-variant-ligatures", "normal")]
        [InlineData("font-variant-ligatures", "none")]
        [InlineData("font-variant-ligatures", "common-ligatures")]
        [InlineData("font-variant-ligatures", "no-common-ligatures contextual")]
        [InlineData("font-variant-ligatures", "contextual no-discretionary-ligatures historical-ligatures common-ligatures")]
        [InlineData("font-variant-numeric", "normal")]
        [InlineData("font-variant-numeric", "oldstyle-nums tabular-nums slashed-zero")]
        [InlineData("font-variant-numeric", "ordinal diagonal-fractions lining-nums proportional-nums")]
        [InlineData("font-variant-east-asian", "normal")]
        [InlineData("font-variant-east-asian", "jis78-forms full-width ruby")]
        [InlineData("font-variant-east-asian", "traditional proportional-width")]
        public void ComponentListLonghandsAcceptValidCombinations(string name, string value)
        {
            var property = Parse(name, value);

            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("font-variant-ligatures", "common-ligatures no-common-ligatures")]
        [InlineData("font-variant-ligatures", "common-ligatures common-ligatures")]
        [InlineData("font-variant-ligatures", "none common-ligatures")]
        [InlineData("font-variant-ligatures", "sparkly")]
        [InlineData("font-variant-ligatures", "5px")]
        [InlineData("font-variant-numeric", "lining-nums oldstyle-nums")]
        [InlineData("font-variant-numeric", "none")]
        [InlineData("font-variant-numeric", "ordinal ordinal")]
        [InlineData("font-variant-east-asian", "jis78-forms jis83-forms")]
        [InlineData("font-variant-east-asian", "full-width proportional-width")]
        [InlineData("font-variant-east-asian", "none")]
        public void ComponentListLonghandsRejectInvalidCombinations(string name, string value)
        {
            Assert.False(Parse(name, value).HasValue);
        }

        // --- font-variant-alternates ---------------------------------------------------------------

        [Theory]
        [InlineData("normal")]
        [InlineData("historical-forms")]
        [InlineData("stylistic(foo)")]
        [InlineData("swash(bar)")]
        [InlineData("ornaments(baz)")]
        [InlineData("annotation(qux)")]
        [InlineData("styleset(nice-style)")]
        [InlineData("styleset(nice-style, other-style)")]
        [InlineData("character-variant(flourish, swoosh)")]
        [InlineData("stylistic(foo) historical-forms")]
        [InlineData("historical-forms styleset(a, b) swash(c)")]
        public void AlternatesAcceptsValidValues(string value)
        {
            var property = Parse("font-variant-alternates", value);

            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("5px")]
        [InlineData("bogus")]
        [InlineData("stylistic()")]
        [InlineData("stylistic(foo, bar)")]
        [InlineData("styleset()")]
        [InlineData("styleset(1)")]
        [InlineData("styleset(a,)")]
        [InlineData("stylistic(foo) stylistic(bar)")]
        [InlineData("historical-forms historical-forms")]
        [InlineData("normal historical-forms")]
        public void AlternatesRejectsInvalidValues(string value)
        {
            Assert.False(Parse("font-variant-alternates", value).HasValue);
        }

        // --- font-feature-settings / font-variation-settings / font-language-override ----------------

        [Theory]
        [InlineData("normal", "normal")]
        [InlineData("\"smcp\"", "\"smcp\"")]
        [InlineData("\"smcp\" 1, \"onum\" on, \"ss01\" 2", "\"smcp\" 1, \"onum\" on, \"ss01\" 2")]
        [InlineData("\"liga\" off", "\"liga\" off")]
        [InlineData("\"liga\" OFF", "\"liga\" off")]
        [InlineData("\"liga\" 0,\"kern\" 1", "\"liga\" 0, \"kern\" 1")]
        public void FeatureSettingsAcceptsValidValues(string value, string expected)
        {
            var property = Parse("font-feature-settings", value);

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("smcp")]
        [InlineData("\"smc\"")]
        [InlineData("\"smcpp\"")]
        [InlineData("\"smcp\" -1")]
        [InlineData("\"smcp\" 1.5")]
        [InlineData("\"smcp\" yes")]
        [InlineData("\"smcp\" 1 2")]
        [InlineData("\"smcp\", ")]
        [InlineData("\"smcp\" \"liga\"")]
        [InlineData("normal, \"liga\"")]
        [InlineData("1")]
        public void FeatureSettingsRejectsInvalidValues(string value)
        {
            Assert.False(Parse("font-feature-settings", value).HasValue);
        }

        [Theory]
        [InlineData("normal", "normal")]
        [InlineData("\"wght\" 700", "\"wght\" 700")]
        [InlineData("\"wght\" 400.5, \"wdth\" 75", "\"wght\" 400.5, \"wdth\" 75")]
        [InlineData("\"slnt\" -10", "\"slnt\" -10")]
        public void VariationSettingsAcceptsValidValues(string value, string expected)
        {
            var property = Parse("font-variation-settings", value);

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("\"wght\"")]
        [InlineData("wght 700")]
        [InlineData("\"weight\" 700")]
        [InlineData("\"wght\" bold")]
        [InlineData("\"wght\" 1, ")]
        [InlineData("700")]
        public void VariationSettingsRejectsInvalidValues(string value)
        {
            Assert.False(Parse("font-variation-settings", value).HasValue);
        }

        [Theory]
        [InlineData("normal", "normal")]
        [InlineData("\"TRK\"", "\"TRK\"")]
        [InlineData("\"ENG \"", "\"ENG \"")]
        [InlineData("\"A\"", "\"A\"")]
        public void LanguageOverrideAcceptsValidValues(string value, string expected)
        {
            var property = Parse("font-language-override", value);

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("TRK")]
        [InlineData("\"\"")]
        [InlineData("\"TOOLONG\"")]
        [InlineData("\"TRK\" \"ENG\"")]
        [InlineData("1")]
        public void LanguageOverrideRejectsInvalidValues(string value)
        {
            Assert.False(Parse("font-language-override", value).HasValue);
        }

        // --- font-size-adjust / font-stretch ---------------------------------------------------------

        [Theory]
        [InlineData("none", "none")]
        [InlineData("NONE", "none")]
        [InlineData("0.5", "0.5")]
        [InlineData("1", "1")]
        [InlineData("0", "0")]
        [InlineData("from-font", "from-font")]
        [InlineData("ex-height 0.5", "ex-height 0.5")]
        [InlineData("cap-height 0.7", "cap-height 0.7")]
        [InlineData("CH-WIDTH 0.4", "ch-width 0.4")]
        [InlineData("ic-width 1", "ic-width 1")]
        [InlineData("ic-height 0.9", "ic-height 0.9")]
        [InlineData("cap-height from-font", "cap-height from-font")]
        public void FontSizeAdjustAcceptsValidValues(string value, string expected)
        {
            var property = Parse("font-size-adjust", value);

            Assert.IsType<FontSizeAdjustProperty>(property);
            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("1.1em")]
        [InlineData("0.5 0.5")]
        [InlineData("-1")]
        [InlineData("cap-height")]
        [InlineData("bogus 0.5")]
        [InlineData("none 0.5")]
        [InlineData("0.5 ex-height")]
        [InlineData("ex-height cap-height 0.5")]
        [InlineData("50%")]
        public void FontSizeAdjustRejectsInvalidValues(string value)
        {
            Assert.False(Parse("font-size-adjust", value).HasValue);
        }

        [Theory]
        [InlineData("50%", "50%")]
        [InlineData("100%", "100%")]
        [InlineData("62.5%", "62.5%")]
        [InlineData("0%", "0%")]
        [InlineData("200%", "200%")]
        [InlineData("semi-condensed", "semi-condensed")]
        public void FontStretchAcceptsPercentagesAndKeywords(string value, string expected)
        {
            var property = Parse("font-stretch", value);

            Assert.IsType<FontStretchProperty>(property);
            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("-10%")]
        [InlineData("50")]
        [InlineData("50px")]
        [InlineData("50% 60%")]
        [InlineData("wide")]
        public void FontStretchRejectsInvalidValues(string value)
        {
            Assert.False(Parse("font-stretch", value).HasValue);
        }

        [Fact]
        public void FontShorthandDoesNotAcceptStretchPercentage()
        {
            Assert.False(Parse("font", "50% 12px serif").HasValue);
            Assert.True(Parse("font", "condensed 12px serif").HasValue);
        }

        // --- font-synthesis shorthand -----------------------------------------------------------------

        [Theory]
        [InlineData("none", "none", "none", "none", "none")]
        [InlineData("weight style small-caps position", "auto", "auto", "auto", "auto")]
        [InlineData("weight", "auto", "none", "none", "none")]
        [InlineData("style position", "none", "auto", "none", "auto")]
        [InlineData("small-caps weight", "auto", "none", "auto", "none")]
        public void FontSynthesisExpandsListedToAutoAndOmittedToNone(string value, string weight, string style,
            string smallCaps, string position)
        {
            var declarations = ParseDeclarations($"font-synthesis: {value}");

            Assert.Equal(weight, declarations.GetPropertyValue("font-synthesis-weight"));
            Assert.Equal(style, declarations.GetPropertyValue("font-synthesis-style"));
            Assert.Equal(smallCaps, declarations.GetPropertyValue("font-synthesis-small-caps"));
            Assert.Equal(position, declarations.GetPropertyValue("font-synthesis-position"));
        }

        [Theory]
        [InlineData("weight weight")]
        [InlineData("none weight")]
        [InlineData("bold")]
        [InlineData("1")]
        [InlineData("auto")]
        [InlineData("weight style small-caps position weight")]
        public void FontSynthesisRejectsInvalidValues(string value)
        {
            var property = Parse("font-synthesis", value);

            Assert.IsType<FontSynthesisProperty>(property);
            Assert.False(property.HasValue);
            Assert.Equal(string.Empty, Computed($"font-synthesis: {value}", "font-synthesis-weight"));
        }

        [Theory]
        [InlineData("weight style", "weight style")]
        [InlineData("position weight", "weight position")]
        [InlineData("none", "none")]
        public void FontSynthesisIsCanonicalizedAndRebuiltFromLonghands(string value, string expected)
        {
            Assert.Equal(expected, Parse("font-synthesis", value).Value);
            Assert.Equal(expected, ParseDeclarations($"font-synthesis: {value}").GetPropertyValue("font-synthesis"));
            Assert.Equal(expected, ParseDeclarations($"font-synthesis: {value}").FontSynthesis);
        }

        [Fact]
        public void FontSynthesisLonghandsSerializeBackToTheShorthand()
        {
            var declarations = ParseDeclarations("font-synthesis: weight style");

            Assert.Equal("font-synthesis: weight style", declarations.CssText);
        }

        [Fact]
        public void FontSynthesisAcceptsGlobalKeywords()
        {
            var declarations = ParseDeclarations("font-synthesis: inherit");

            Assert.Equal("inherit", declarations.GetPropertyValue("font-synthesis-weight"));
            Assert.Equal("inherit", declarations.GetPropertyValue("font-synthesis-position"));
        }

        // --- font-variant shorthand -------------------------------------------------------------------

        [Fact]
        public void FontVariantIsAShorthandOfSevenLonghands()
        {
            var property = Parse("font-variant", "small-caps");

            Assert.IsType<FontVariantProperty>(property);
            Assert.True(property.IsShorthand);
            Assert.Equal(new[]
            {
                "font-variant-ligatures", "font-variant-alternates", "font-variant-caps", "font-variant-numeric",
                "font-variant-east-asian", "font-variant-position", "font-variant-emoji"
            }, PropertyFactory.Instance.GetLonghands("font-variant"));
        }

        [Fact]
        public void FontVariantExpandsComponentsInAnyOrder()
        {
            var declarations = ParseDeclarations(
                "font-variant: tabular-nums small-caps no-common-ligatures styleset(nice-style) sub emoji ruby historical-forms;");

            Assert.Equal("small-caps", declarations.GetPropertyValue("font-variant-caps"));
            Assert.Equal("no-common-ligatures", declarations.GetPropertyValue("font-variant-ligatures"));
            Assert.Equal("tabular-nums", declarations.GetPropertyValue("font-variant-numeric"));
            Assert.Equal("ruby", declarations.GetPropertyValue("font-variant-east-asian"));
            Assert.Equal("sub", declarations.GetPropertyValue("font-variant-position"));
            Assert.Equal("emoji", declarations.GetPropertyValue("font-variant-emoji"));
            Assert.Equal("styleset(nice-style) historical-forms", declarations.GetPropertyValue("font-variant-alternates"));
        }

        [Fact]
        public void FontVariantInterleavedComponentsOfOneLonghandAreMerged()
        {
            var declarations = ParseDeclarations("font-variant: common-ligatures small-caps no-contextual lining-nums contextual-unknown;");

            Assert.Equal(string.Empty, declarations.GetPropertyValue("font-variant-caps"));

            declarations = ParseDeclarations("font-variant: common-ligatures small-caps no-contextual lining-nums;");

            Assert.Equal("common-ligatures no-contextual", declarations.GetPropertyValue("font-variant-ligatures"));
            Assert.Equal("small-caps", declarations.GetPropertyValue("font-variant-caps"));
            Assert.Equal("lining-nums", declarations.GetPropertyValue("font-variant-numeric"));
        }

        [Fact]
        public void FontVariantNormalSetsEveryLonghandToNormal()
        {
            var declarations = ParseDeclarations(
                "font-variant-caps: small-caps; font-variant-ligatures: none; font-variant: normal;");

            foreach (var name in PropertyFactory.Instance.GetLonghands("font-variant"))
            {
                Assert.Equal("normal", declarations.GetPropertyValue(name));
            }

            Assert.Equal("normal", declarations.FontVariant);
        }

        [Fact]
        public void FontVariantNoneSetsLigaturesToNoneAndTheRestToNormal()
        {
            var declarations = ParseDeclarations("font-variant: none;");

            Assert.Equal("none", declarations.GetPropertyValue("font-variant-ligatures"));
            Assert.Equal("normal", declarations.GetPropertyValue("font-variant-caps"));
            Assert.Equal("normal", declarations.GetPropertyValue("font-variant-numeric"));
            Assert.Equal("normal", declarations.GetPropertyValue("font-variant-alternates"));
            Assert.Equal("none", declarations.FontVariant);
        }

        [Fact]
        public void FontVariantResetsLonghandsItDoesNotMention()
        {
            var declarations = ParseDeclarations(
                "font-variant-alternates: styleset(nice-style); font-variant-numeric: ordinal; font-variant: small-caps;");

            Assert.Equal("small-caps", declarations.GetPropertyValue("font-variant-caps"));
            Assert.Equal("initial", declarations.GetPropertyValue("font-variant-alternates"));
            Assert.Equal("initial", declarations.GetPropertyValue("font-variant-numeric"));
        }

        [Theory]
        [InlineData("normal")]
        [InlineData("none")]
        [InlineData("small-caps")]
        [InlineData("all-petite-caps")]
        [InlineData("sub")]
        [InlineData("text")]
        [InlineData("common-ligatures")]
        public void FontVariantAcceptsSingleComponents(string value)
        {
            var property = Parse("font-variant", value);

            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
            Assert.Equal(value, ParseDeclarations($"font-variant: {value}").FontVariant);
        }

        [Theory]
        [InlineData("small-caps common-ligatures", "common-ligatures small-caps")]
        [InlineData("super oldstyle-nums unicase", "unicase oldstyle-nums super")]
        [InlineData("emoji text-unknown", null)]
        [InlineData("normal small-caps", null)]
        [InlineData("none small-caps", null)]
        [InlineData("small-caps normal", null)]
        [InlineData("small-caps all-small-caps", null)]
        [InlineData("sub super", null)]
        [InlineData("text emoji", null)]
        [InlineData("common-ligatures no-common-ligatures", null)]
        [InlineData("lining-nums oldstyle-nums", null)]
        [InlineData("jis78-forms jis83-forms", null)]
        [InlineData("stylistic(a) stylistic(b)", null)]
        [InlineData("smallcaps", null)]
        [InlineData("1", null)]
        [InlineData("5px", null)]
        public void FontVariantSerializesInLonghandOrderAndRejectsInvalidValues(string value, string expected)
        {
            var property = Parse("font-variant", value);

            Assert.Equal(expected != null, property.HasValue);

            if (expected != null) Assert.Equal(expected, property.Value);
        }

        [Fact]
        public void FontVariantIsReconstructedFromLonghands()
        {
            var declarations = ParseDeclarations(
                "font-variant-caps: small-caps; font-variant-numeric: tabular-nums; font-variant-ligatures: normal; " +
                "font-variant-alternates: normal; font-variant-east-asian: normal; font-variant-position: normal; " +
                "font-variant-emoji: normal;");

            Assert.Equal("small-caps tabular-nums", declarations.FontVariant);
            Assert.Equal("font-variant: small-caps tabular-nums", declarations.CssText);
        }

        [Fact]
        public void FontVariantWithNonNormalLigaturesNoneCannotBeRebuilt()
        {
            var declarations = ParseDeclarations(
                "font-variant-caps: small-caps; font-variant-numeric: normal; font-variant-ligatures: none; " +
                "font-variant-alternates: normal; font-variant-east-asian: normal; font-variant-position: normal; " +
                "font-variant-emoji: normal;");

            Assert.Null(declarations.FontVariant);
            Assert.Contains("font-variant-ligatures: none", declarations.CssText);
        }

        [Fact]
        public void FontVariantAcceptsGlobalKeywords()
        {
            var declarations = ParseDeclarations("font-variant: inherit;");

            Assert.Equal("inherit", declarations.GetPropertyValue("font-variant-caps"));
            Assert.Equal("inherit", declarations.FontVariant);
            Assert.Equal("font-variant: inherit", declarations.CssText);
        }

        [Fact]
        public void FontVariantAccessorCanBeSet()
        {
            var declarations = ParseDeclarations(string.Empty);

            declarations.FontVariant = "small-caps oldstyle-nums";

            Assert.Equal("small-caps", declarations.GetPropertyValue("font-variant-caps"));
            Assert.Equal("oldstyle-nums", declarations.GetPropertyValue("font-variant-numeric"));
            Assert.Equal("small-caps oldstyle-nums", declarations.FontVariant);
        }

        [Fact]
        public void FontVariantImportantAppliesToEveryLonghand()
        {
            var declarations = ParseDeclarations("font-variant: small-caps !important;");

            Assert.Equal("important", declarations.GetPropertyPriority("font-variant"));
            Assert.Equal("important", declarations.GetPropertyPriority("font-variant-caps"));
            Assert.Equal("important", declarations.GetPropertyPriority("font-variant-emoji"));
        }

        [Fact]
        public void FontVariantInAStyleSheet()
        {
            var sheet = ParseStyleSheet(".a { font-variant: small-caps; } .b { font-variant: small-caps styleset(x) }");
            var rules = sheet.Rules.OfType<StyleRule>().ToArray();

            Assert.Equal("small-caps", rules[0].Style.FontVariant);
            Assert.Equal("small-caps", rules[0].Style.GetPropertyValue("font-variant-caps"));
            Assert.Equal("styleset(x) small-caps", rules[1].Style.FontVariant);
        }

        // --- the font shorthand ------------------------------------------------------------------------

        [Theory]
        [InlineData("small-caps 12px serif", "small-caps")]
        [InlineData("italic small-caps bold 12px serif", "small-caps")]
        public void FontShorthandAcceptsOnlyTheCss2VariantSubset(string value, string caps)
        {
            var declarations = ParseDeclarations($"font: {value}");

            Assert.Equal(caps, declarations.GetPropertyValue("font-variant-caps"));
            Assert.Equal(caps, declarations.FontVariant);
        }

        [Theory]
        [InlineData("all-small-caps 12px serif")]
        [InlineData("none 12px serif")]
        [InlineData("common-ligatures 12px serif")]
        [InlineData("small-caps tabular-nums 12px serif")]
        [InlineData("sub 12px serif")]
        public void FontShorthandRejectsTheFullVariantGrammar(string value)
        {
            Assert.False(Parse("font", value).HasValue);
        }

        [Fact]
        public void FontShorthandResetsTheOtherFontVariantLonghandsAndFriends()
        {
            var declarations = ParseDeclarations(
                "font-variant-ligatures: none; font-variant-numeric: ordinal; font-variant-east-asian: ruby; " +
                "font-variant-alternates: swash(a); font-variant-position: sub; font-variant-emoji: text; " +
                "font-kerning: none; font-optical-sizing: none; font-size-adjust: 0.5; font-variation-settings: \"wght\" 700; " +
                "font-feature-settings: \"liga\" 0; font-language-override: \"TRK\"; font-synthesis-weight: none; " +
                "font: small-caps 12px serif;");

            Assert.Equal("small-caps", declarations.GetPropertyValue("font-variant-caps"));

            foreach (var name in new[]
            {
                "font-variant-ligatures", "font-variant-numeric", "font-variant-east-asian", "font-variant-alternates",
                "font-variant-position", "font-variant-emoji", "font-kerning", "font-optical-sizing",
                "font-size-adjust", "font-variation-settings", "font-feature-settings", "font-language-override"
            })
            {
                Assert.Equal("initial", declarations.GetPropertyValue(name));
            }

            // font-synthesis is not reset by the font shorthand.
            Assert.Equal("none", declarations.GetPropertyValue("font-synthesis-weight"));
        }

        [Fact]
        public void FontShorthandIsRebuiltFromItsLonghands()
        {
            var declarations = ParseDeclarations("font: italic small-caps bold 12px/14px serif");

            Assert.Equal("italic small-caps bold 12px / 14px serif", declarations.GetPropertyValue("font"));
        }

        [Fact]
        public void FontShorthandIsNotRebuiltOverNonInitialResetLonghands()
        {
            var declarations = ParseDeclarations("font: small-caps 12px serif; font-kerning: none;");

            Assert.Null(declarations.GetPropertyValue("font"));
            Assert.Contains("font-kerning: none", declarations.CssText);
            Assert.Contains("font-family: serif", declarations.CssText);
        }

        [Fact]
        public void FontShorthandIsNotRebuiltOverNonCss2Caps()
        {
            var declarations = ParseDeclarations("font: 12px serif; font-variant-caps: all-small-caps;");

            Assert.Null(declarations.GetPropertyValue("font"));
            Assert.Equal("all-small-caps", declarations.GetPropertyValue("font-variant-caps"));
        }

        // --- @font-face descriptor --------------------------------------------------------------------

        [Fact]
        public void FontFaceVariantDescriptorStaysFlatAndCss2Only()
        {
            var sheet = ParseStyleSheet("@font-face{font-family:x;font-variant:small-caps}");
            var fontFace = (IFontFaceRule)sheet.Rules[0];

            Assert.Equal("small-caps", fontFace.Variant);

            sheet = ParseStyleSheet("@font-face{font-family:x;font-variant:all-small-caps}");
            fontFace = (IFontFaceRule)sheet.Rules[0];

            Assert.Equal(string.Empty, fontFace.Variant);
        }

        // --- Map.FontVariants / stylesheet round trip ----------------------------------------------------

        [Fact]
        public void LonghandsRoundTripThroughAStyleSheet()
        {
            var sheet = ParseStyleSheet(
                "p { font-kerning: none; font-optical-sizing: auto; font-variant-east-asian: full-width; " +
                "font-feature-settings: \"liga\" 0, \"smcp\"; font-variation-settings: \"wght\" 600; " +
                "font-language-override: \"TRK\"; font-size-adjust: cap-height 0.7; font-stretch: 85%; }");
            var style = sheet.Rules.OfType<StyleRule>().Single().Style;

            Assert.Equal("none", style.FontKerning);
            Assert.Equal("auto", style.FontOpticalSizing);
            Assert.Equal("full-width", style.FontVariantEastAsian);
            Assert.Equal("\"liga\" 0, \"smcp\"", style.FontFeatureSettings);
            Assert.Equal("\"wght\" 600", style.FontVariationSettings);
            Assert.Equal("\"TRK\"", style.FontLanguageOverride);
            Assert.Equal("cap-height 0.7", style.FontSizeAdjust);
            Assert.Equal("85%", style.FontStretch);
        }

        [Fact]
        public void VarReferencesAreAcceptedInTheNewProperties()
        {
            var declarations = ParseDeclarations("font-variant: var(--v); font-kerning: var(--k);");

            Assert.Equal("var(--v)", declarations.FontVariant);
            Assert.Equal("var(--k)", declarations.FontKerning);
        }
    }
}
