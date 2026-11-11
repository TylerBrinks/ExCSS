using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class TextDecorationEmphasisPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("text-decoration-thickness", "auto")]
        [InlineData("text-decoration-thickness", "from-font")]
        [InlineData("text-decoration-thickness", "2px")]
        [InlineData("text-decoration-thickness", "10%")]
        [InlineData("text-decoration-thickness", "calc(1px + 10%)")]
        [InlineData("text-decoration-skip-ink", "auto")]
        [InlineData("text-decoration-skip-ink", "none")]
        [InlineData("text-decoration-skip-ink", "all")]
        [InlineData("text-decoration-skip", "none")]
        [InlineData("text-decoration-skip", "objects")]
        [InlineData("text-decoration-skip", "objects edges")]
        [InlineData("text-decoration-skip", "spaces ink")]
        [InlineData("text-decoration-skip", "leading-spaces trailing-spaces")]
        [InlineData("text-decoration-inset", "auto")]
        [InlineData("text-decoration-inset", "4px")]
        [InlineData("text-decoration-inset", "4px 2px")]
        [InlineData("text-underline-offset", "auto")]
        [InlineData("text-underline-offset", "3px")]
        [InlineData("text-underline-offset", "0.1em")]
        [InlineData("text-underline-offset", "20%")]
        [InlineData("text-underline-position", "auto")]
        [InlineData("text-underline-position", "from-font")]
        [InlineData("text-underline-position", "under")]
        [InlineData("text-underline-position", "under left")]
        [InlineData("text-underline-position", "right from-font")]
        [InlineData("text-underline-position", "left")]
        [InlineData("text-emphasis-style", "none")]
        [InlineData("text-emphasis-style", "filled")]
        [InlineData("text-emphasis-style", "open sesame")]
        [InlineData("text-emphasis-style", "triangle filled")]
        [InlineData("text-emphasis-style", "double-circle")]
        [InlineData("text-emphasis-style", "\"*\"")]
        [InlineData("text-emphasis-color", "red")]
        [InlineData("text-emphasis-position", "over")]
        [InlineData("text-emphasis-position", "under left")]
        [InlineData("text-emphasis-position", "right over")]
        public void LonghandLegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.False(property.IsImportant);
        }

        [Theory]
        [InlineData("text-decoration-thickness", "none")]
        [InlineData("text-decoration-thickness", "1")]
        [InlineData("text-decoration-thickness", "auto 2px")]
        [InlineData("text-decoration-skip-ink", "from-font")]
        [InlineData("text-decoration-skip-ink", "auto none")]
        [InlineData("text-decoration-skip", "spaces leading-spaces")]
        [InlineData("text-decoration-skip", "objects objects")]
        [InlineData("text-decoration-skip", "bogus")]
        [InlineData("text-decoration-inset", "1px 2px 3px")]
        [InlineData("text-decoration-inset", "10%")]
        [InlineData("text-decoration-inset", "auto 1px")]
        [InlineData("text-underline-offset", "from-font")]
        [InlineData("text-underline-offset", "1px 2px")]
        [InlineData("text-underline-position", "auto left")]
        [InlineData("text-underline-position", "under from-font")]
        [InlineData("text-underline-position", "left right")]
        [InlineData("text-underline-position", "over")]
        [InlineData("text-emphasis-style", "filled open")]
        [InlineData("text-emphasis-style", "dot circle")]
        [InlineData("text-emphasis-style", "none dot")]
        [InlineData("text-emphasis-style", "\"\"")]
        [InlineData("text-emphasis-style", "red")]
        [InlineData("text-emphasis-color", "1px")]
        [InlineData("text-emphasis-position", "left")]
        [InlineData("text-emphasis-position", "over under")]
        [InlineData("text-emphasis-position", "over left right")]
        public void LonghandIllegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("text-decoration-thickness", false, false)]
        [InlineData("text-decoration-skip-ink", true, false)]
        [InlineData("text-decoration-skip", false, false)]
        [InlineData("text-decoration-inset", false, false)]
        [InlineData("text-underline-offset", true, false)]
        [InlineData("text-underline-position", true, false)]
        [InlineData("text-emphasis-style", true, false)]
        [InlineData("text-emphasis-color", true, true)]
        [InlineData("text-emphasis-position", true, false)]
        [InlineData("text-indent", true, true)]
        public void InheritedAndAnimatableFlags(string name, bool inherited, bool animatable)
        {
            var property = ParseDeclaration($"{name}: initial");

            Assert.Equal(inherited, property.CanBeInherited);
            Assert.Equal(animatable, property.IsAnimatable);
        }

        [Theory]
        [InlineData("text-decoration-skip-ink")]
        [InlineData("text-underline-position")]
        [InlineData("text-emphasis-style")]
        public void InheritedLonghandsAcceptGlobalKeywords(string name)
        {
            Assert.True(ParseDeclaration($"{name}: inherit").HasValue);
            Assert.True(ParseDeclaration($"{name}: unset").HasValue);
        }

        [Fact]
        public void TextDecorationShorthandExpandsThickness()
        {
            var declaration = ParseDeclarations("text-decoration: underline wavy red 3px");

            Assert.Equal("underline", declaration.GetPropertyValue("text-decoration-line"));
            Assert.Equal("wavy", declaration.GetPropertyValue("text-decoration-style"));
            Assert.Equal("rgb(255, 0, 0)", declaration.GetPropertyValue("text-decoration-color"));
            Assert.Equal("3px", declaration.GetPropertyValue("text-decoration-thickness"));
        }

        [Theory]
        [InlineData("text-decoration: underline from-font")]
        [InlineData("text-decoration: 2px")]
        [InlineData("text-decoration: overline 10% dotted")]
        public void TextDecorationShorthandAcceptsThickness(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.True(property.HasValue);
        }

        [Fact]
        public void TextDecorationShorthandDoesNotIncludeSkipInk()
        {
            Assert.False(ParseDeclaration("text-decoration: underline none-ink").HasValue);
            Assert.False(ParseDeclaration("text-decoration: underline all").HasValue);

            var declaration = ParseDeclarations("text-decoration: underline");
            Assert.True(string.IsNullOrEmpty(declaration.GetPropertyValue("text-decoration-skip-ink")));
        }

        [Fact]
        public void TextDecorationShorthandSerializesThickness()
        {
            var declaration = ParseDeclarations("text-decoration-line: underline; text-decoration-style: solid; text-decoration-color: red; text-decoration-thickness: 2px");

            Assert.Contains("2px", declaration.GetPropertyValue("text-decoration"));
        }

        [Fact]
        public void TextEmphasisShorthandExpandsToLonghands()
        {
            var declaration = ParseDeclarations("text-emphasis: filled circle red");

            Assert.Equal("filled circle", declaration.GetPropertyValue("text-emphasis-style"));
            Assert.Equal("rgb(255, 0, 0)", declaration.GetPropertyValue("text-emphasis-color"));
        }

        [Fact]
        public void TextEmphasisShorthandColorFirst()
        {
            var declaration = ParseDeclarations("text-emphasis: #00f open sesame");

            Assert.Equal("open sesame", declaration.GetPropertyValue("text-emphasis-style"));
            Assert.Equal("rgb(0, 0, 255)", declaration.GetPropertyValue("text-emphasis-color"));
        }

        [Fact]
        public void TextEmphasisShorthandStyleOnlyAndColorOnly()
        {
            var styleOnly = ParseDeclarations("text-emphasis: triangle");
            Assert.Equal("triangle", styleOnly.GetPropertyValue("text-emphasis-style"));

            var colorOnly = ParseDeclarations("text-emphasis: blue");
            Assert.Equal("rgb(0, 0, 255)", colorOnly.GetPropertyValue("text-emphasis-color"));
        }

        [Fact]
        public void TextEmphasisShorthandSerializes()
        {
            var declaration = ParseDeclarations("text-emphasis-style: dot; text-emphasis-color: red");

            Assert.Equal("dot rgb(255, 0, 0)", declaration.GetPropertyValue("text-emphasis"));
        }

        [Theory]
        [InlineData("text-emphasis: red blue")]
        [InlineData("text-emphasis: dot sesame")]
        [InlineData("text-emphasis: 1px")]
        [InlineData("text-emphasis: filled red filled")]
        public void TextEmphasisShorthandIllegal(string snippet)
        {
            Assert.False(ParseDeclaration(snippet).HasValue);
        }

        [Fact]
        public void TextEmphasisShorthandIsInheritedAndAnimatable()
        {
            var property = ParseDeclaration("text-emphasis: dot");

            Assert.True(property.CanBeInherited);
            Assert.True(property.IsAnimatable);
        }

        [Theory]
        [InlineData("hanging 2em", "hanging 2em")]
        [InlineData("2em hanging", "2em hanging")]
        [InlineData("each-line 10%", "each-line 10%")]
        [InlineData("1em hanging each-line", "1em hanging each-line")]
        [InlineData("each-line hanging 1em", "each-line hanging 1em")]
        [InlineData("calc(1em + 2px)", "calc(1em + 2px)")]
        [InlineData("calc(10% - 1px) each-line", "calc(10% - 1px) each-line")]
        public void TextIndentKeywordsAndCalc(string value, string expected)
        {
            var property = ParseDeclaration($"text-indent: {value}");

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("hanging")]
        [InlineData("each-line")]
        [InlineData("hanging each-line")]
        [InlineData("1em hanging hanging")]
        [InlineData("1em 2em")]
        [InlineData("1em each-line each-line")]
        [InlineData("none hanging")]
        public void TextIndentIllegal(string value)
        {
            Assert.False(ParseDeclaration($"text-indent: {value}").HasValue);
        }
    }
}
