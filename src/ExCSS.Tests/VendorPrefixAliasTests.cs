using Xunit;

namespace ExCSS.Tests
{
    public class VendorPrefixAliasTests : CssConstructionFunctions
    {
        private static Property Parse(string declaration) =>
            ParseDeclaration(declaration, enableVendorPrefixAliases: true);

        private static StyleDeclaration ParseStyle(string css, bool enable = true)
        {
            var sheet = ParseStyleSheet("div { " + css + " }", enableVendorPrefixAliases: enable);
            return ((StyleRule)sheet.Rules[0]).Style;
        }

        private static string NameOf(string declaration) => declaration.Substring(0, declaration.IndexOf(':'));

        [Theory]
        [InlineData("-webkit-box-shadow: 1px 1px red", "box-shadow", typeof(BoxShadowProperty))]
        [InlineData("-moz-box-shadow: 1px 1px red", "box-shadow", typeof(BoxShadowProperty))]
        [InlineData("-webkit-transform: none", "transform", typeof(TransformProperty))]
        [InlineData("-webkit-transform-origin: 10px 10px", "transform-origin", null)]
        [InlineData("-webkit-box-sizing: border-box", "box-sizing", typeof(BoxSizingProperty))]
        [InlineData("-moz-box-sizing: border-box", "box-sizing", typeof(BoxSizingProperty))]
        [InlineData("-webkit-border-top-left-radius: 4px", "border-top-left-radius", typeof(BorderTopLeftRadiusProperty))]
        [InlineData("-webkit-column-count: 2", "column-count", null)]
        [InlineData("-moz-column-gap: 10px", "column-gap", null)]
        [InlineData("-webkit-hyphens: auto", "hyphens", null)]
        [InlineData("-webkit-flex-direction: column", "flex-direction", null)]
        [InlineData("-webkit-justify-content: center", "justify-content", null)]
        [InlineData("-webkit-align-items: center", "align-items", null)]
        [InlineData("-webkit-order: 2", "order", null)]
        [InlineData("-webkit-box-flex: 2", "flex-grow", null)]
        [InlineData("-moz-box-ordinal-group: 3", "order", null)]
        [InlineData("-webkit-text-decoration-line: underline", "text-decoration-line", null)]
        [InlineData("-webkit-opacity: 0.5", "opacity", null)]
        [InlineData("-webkit-animation-name: spin", "animation-name", null)]
        [InlineData("-webkit-transition-duration: 1s", "transition-duration", null)]
        [InlineData("-webkit-column-break-before: always", "break-before", null)]
        public void LonghandAlias_IsCreatedAsTheStandardProperty(string declaration, string standardName, System.Type expectedType)
        {
            var property = Parse(declaration);

            Assert.NotNull(property);
            Assert.Equal(standardName, property.Name);
            Assert.True(property.HasValue);
            if (expectedType != null) Assert.IsType(expectedType, property);
        }

        [Theory]
        [InlineData("-webkit-border-radius: 5px", "border-top-left-radius")]
        [InlineData("-webkit-flex: 1 1 auto", "flex-grow")]
        [InlineData("-webkit-flex-flow: row wrap", "flex-wrap")]
        [InlineData("-moz-columns: 2 100px", "column-count")]
        [InlineData("-webkit-text-decoration: underline", "text-decoration-line")]
        [InlineData("-webkit-border-image: url(a.png) 10 stretch", "border-image-source")]
        public void ShorthandAlias_ExpandsToTheStandardLonghands(string declaration, string expectedLonghand)
        {
            var style = ParseStyle(declaration);

            Assert.NotNull(style.GetProperty(expectedLonghand));
            Assert.Null(style.GetProperty(NameOf(declaration)));
            Assert.DoesNotContain(style, p => p.Name.StartsWith("-"));
        }

        [Fact]
        public void TheAliasAndTheStandardSpelling_AddressTheSameSlotInADeclarationBlock()
        {
            var style = ParseStyle("-webkit-box-shadow: 1px 1px red; box-shadow: 2px 2px blue");

            Assert.Single(style, p => p.Name == "box-shadow");
            Assert.StartsWith("2px 2px", style.GetPropertyValue("-webkit-box-shadow"));
            Assert.Equal(style.GetPropertyValue("box-shadow"), style.GetPropertyValue("-moz-box-shadow"));
        }

        [Fact]
        public void SetAndRemove_ThroughThePrefixedName_UseTheStandardSlot()
        {
            var style = ParseStyle("box-sizing: content-box");

            style.SetProperty("-webkit-box-sizing", "border-box");
            Assert.Equal("border-box", style.GetPropertyValue("box-sizing"));
            Assert.Single(style, p => p.Name == "box-sizing");

            Assert.Equal("border-box", style.RemoveProperty("-moz-box-sizing"));
            Assert.Equal(string.Empty, style.GetPropertyValue("box-sizing"));
        }

        [Fact]
        public void ShorthandAlias_ReadsBackThroughThePrefixedName()
        {
            var style = ParseStyle("border-radius: 5px");

            Assert.Equal(style.GetPropertyValue("border-radius"), style.GetPropertyValue("-webkit-border-radius"));
            Assert.NotEqual(string.Empty, style.GetPropertyValue("-webkit-border-radius"));
        }

        [Fact]
        public void LaterStandardDeclarationBeatsEarlierPrefixedOneAndViceVersa()
        {
            Assert.Equal("border-box",
                ParseStyle("-webkit-box-sizing: content-box; box-sizing: border-box").GetPropertyValue("box-sizing"));
            Assert.Equal("content-box",
                ParseStyle("box-sizing: border-box; -webkit-box-sizing: content-box").GetPropertyValue("box-sizing"));
        }

        [Fact]
        public void PrefixesWithNoSupportedStandardGrammar_AreStillDropped()
        {
            var style = ParseStyle("-webkit-box-lines: multiple; color: red");

            Assert.Single(style);
        }

        [Fact]
        public void InvalidValueOnAnAlias_IsRejectedLikeTheStandardProperty()
        {
            Assert.False(Parse("-webkit-box-sizing: bogus")?.HasValue ?? false);
        }

        [Fact]
        public void AliasedDeclarations_CoexistWithTheirStandardFallbacks()
        {
            var style = ParseStyle(@"
  -o-border-radius: 5px;
  -webkit-border-radius: 5px;
  border-radius: 5px;
  display: -webkit-box;
  display: -webkit-flex;
  display: -ms-flexbox;
  display: flex;");

            Assert.NotNull(style.GetProperty("border-top-left-radius"));
            Assert.Equal("flex", style.GetPropertyValue("display"));
        }

        [Fact]
        public void SupportsCondition_WithAPrefixedName_BehavesLikeTheStandardName()
        {
            var sheet = ParseStyleSheet("@supports (-webkit-box-shadow: 0 0 2px black) { div { color: red } }",
                enableVendorPrefixAliases: true);
            var css = sheet.Rules[0].ToCss();

            Assert.Contains("(box-shadow", css);
            Assert.DoesNotContain("-webkit-", css);
        }

        [Theory]
        [InlineData("display: -webkit-flex", "display", "flex")]
        [InlineData("display: -webkit-inline-flex", "display", "inline-flex")]
        [InlineData("display: -webkit-box", "display", "flex")]
        [InlineData("display: -moz-box", "display", "flex")]
        [InlineData("display: -webkit-inline-box", "display", "inline-flex")]
        [InlineData("display: -ms-flexbox", "display", "flex")]
        [InlineData("display: -ms-inline-flexbox", "display", "inline-flex")]
        [InlineData("position: -webkit-sticky", "position", "sticky")]
        [InlineData("-webkit-column-break-before: always", "break-before", "column")]
        [InlineData("-webkit-column-break-before: auto", "break-before", "auto")]
        [InlineData("-webkit-column-break-after: avoid", "break-after", "avoid-column")]
        [InlineData("-webkit-column-break-inside: avoid", "break-inside", "avoid-column")]
        [InlineData("transition-property: -webkit-transform", "transition-property", "transform")]
        public void PrefixedKeywordValue_IsNormalizedToTheStandardKeyword(string declaration, string name, string expected)
        {
            var property = Parse(declaration);

            Assert.NotNull(property);
            Assert.Equal(name, property.Name);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("-webkit-box-pack: center")]
        [InlineData("-webkit-box-pack: justify")]
        [InlineData("-moz-box-pack: start")]
        [InlineData("-webkit-box-orient: vertical")]
        [InlineData("-webkit-box-orient: inline-axis")]
        [InlineData("-webkit-box-direction: reverse")]
        [InlineData("-webkit-box-align: baseline")]
        [InlineData("-webkit-box-pack: inherit")]
        public void LegacyBoxContainerProperty_IsAcceptedUnderItsOwnName(string declaration)
        {
            var property = Parse(declaration);

            Assert.NotNull(property);
            Assert.True(property.HasValue);
            Assert.Equal(NameOf(declaration), property.Name);
        }

        [Theory]
        [InlineData("-webkit-box-pack: bogus")]
        [InlineData("-webkit-box-orient: diagonal")]
        [InlineData("-webkit-box-align: middle")]
        public void LegacyBoxContainerProperty_WithAnInvalidKeyword_IsDropped(string declaration)
        {
            Assert.False(Parse(declaration)?.HasValue ?? false);
        }

        [Theory]
        [InlineData("-webkit-font-smoothing: antialiased")]
        [InlineData("-moz-osx-font-smoothing: grayscale")]
        [InlineData("-webkit-text-size-adjust: 100%")]
        [InlineData("-webkit-tap-highlight-color: rgba(0,0,0,0)")]
        [InlineData("-webkit-touch-callout: none")]
        [InlineData("-webkit-overflow-scrolling: touch")]
        [InlineData("-webkit-user-drag: none")]
        [InlineData("-webkit-text-stroke: 1px red")]
        [InlineData("-webkit-text-fill-color: transparent")]
        [InlineData("-webkit-text-security: disc")]
        [InlineData("-webkit-box-reflect: below 2px")]
        [InlineData("-webkit-user-select: none")]
        public void AcceptedOnlyVendorProperty_IsKeptWithItsRawValue(string declaration)
        {
            var property = Parse(declaration);

            Assert.NotNull(property);
            Assert.True(property.HasValue);
            Assert.Equal(NameOf(declaration), property.Name);
        }

        [Fact]
        public void WordWrap_IsUnaffectedByTheFlag()
        {
            Assert.Equal(ParseDeclaration("word-wrap: break-word").Name, Parse("word-wrap: break-word").Name);
        }

        // The flag is off by default: nothing changes.

        [Theory]
        [InlineData("-webkit-box-shadow: 1px 1px red")]
        [InlineData("-webkit-transform: none")]
        [InlineData("-webkit-border-radius: 5px")]
        [InlineData("-webkit-box-pack: center")]
        [InlineData("-webkit-font-smoothing: antialiased")]
        [InlineData("-webkit-column-break-before: always")]
        public void FlagOff_PrefixedPropertiesAreStillUnknownAndDropped(string declaration)
        {
            Assert.Null(ParseDeclaration(declaration));

            var style = ParseStyle(declaration + "; color: red", enable: false);
            Assert.Single(style);
            Assert.Equal(string.Empty, style.GetPropertyValue("-webkit-box-shadow"));
        }

        [Theory]
        [InlineData("display: -webkit-flex")]
        [InlineData("display: -webkit-box")]
        [InlineData("position: -webkit-sticky")]
        [InlineData("transition-property: -webkit-transform")]
        public void FlagOff_PrefixedKeywordValuesAreStillRejected(string declaration)
        {
            Assert.False(ParseDeclaration(declaration)?.HasValue ?? false);
        }

        [Fact]
        public void FlagOff_PrefixedNamesDoNotAliasInTheCssom()
        {
            var style = ParseStyle("box-sizing: border-box", enable: false);

            Assert.Equal(string.Empty, style.GetPropertyValue("-webkit-box-sizing"));
        }

        [Fact]
        public void FlagOff_IncludeUnknownDeclarations_KeepsThePrefixedNameVerbatim()
        {
            var parser = new StylesheetParser(includeUnknownDeclarations: true);
            var style = ((StyleRule)parser.Parse("div { -webkit-box-shadow: 1px 1px red }").Rules[0]).Style;

            Assert.Equal("-webkit-box-shadow", System.Linq.Enumerable.Single(style).Name);
        }
    }
}
