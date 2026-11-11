using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class MaskPropertiesTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("mask-image", "none")]
        [InlineData("mask-image", "url(mask.png)")]
        [InlineData("mask-image", "url(mask.svg#m)")]
        [InlineData("mask-image", "linear-gradient(black, transparent)")]
        [InlineData("mask-image", "url(a.png), none, radial-gradient(red, blue)")]
        [InlineData("mask-mode", "match-source")]
        [InlineData("mask-mode", "alpha")]
        [InlineData("mask-mode", "LUMINANCE")]
        [InlineData("mask-mode", "alpha, luminance, match-source")]
        [InlineData("mask-repeat", "repeat")]
        [InlineData("mask-repeat", "no-repeat")]
        [InlineData("mask-repeat", "repeat-x")]
        [InlineData("mask-repeat", "repeat-y")]
        [InlineData("mask-repeat", "space round")]
        [InlineData("mask-repeat", "repeat-x, no-repeat")]
        [InlineData("mask-position", "center")]
        [InlineData("mask-position", "left top")]
        [InlineData("mask-position", "10px 20%")]
        [InlineData("mask-position", "right 10px bottom 20px")]
        [InlineData("mask-position", "0 0, center")]
        [InlineData("mask-size", "auto")]
        [InlineData("mask-size", "cover")]
        [InlineData("mask-size", "contain")]
        [InlineData("mask-size", "50% auto")]
        [InlineData("mask-size", "10px 20px, cover")]
        [InlineData("mask-origin", "border-box")]
        [InlineData("mask-origin", "padding-box")]
        [InlineData("mask-origin", "content-box")]
        [InlineData("mask-origin", "fill-box")]
        [InlineData("mask-origin", "stroke-box")]
        [InlineData("mask-origin", "view-box")]
        [InlineData("mask-origin", "content-box, border-box")]
        [InlineData("mask-clip", "border-box")]
        [InlineData("mask-clip", "no-clip")]
        [InlineData("mask-clip", "view-box, no-clip")]
        [InlineData("mask-composite", "add")]
        [InlineData("mask-composite", "subtract")]
        [InlineData("mask-composite", "intersect")]
        [InlineData("mask-composite", "exclude")]
        [InlineData("mask-composite", "add, exclude")]
        public void MaskLonghandsAcceptLegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue, $"{name}: {value}");
            Assert.False(property.IsInherited);
        }

        [Theory]
        [InlineData("mask-image", "banana")]
        [InlineData("mask-image", "url(a.png) url(b.png)")]
        [InlineData("mask-image", "url(a.png),")]
        [InlineData("mask-mode", "none")]
        [InlineData("mask-mode", "banana")]
        [InlineData("mask-mode", "alpha luminance")]
        [InlineData("mask-repeat", "banana")]
        [InlineData("mask-repeat", "repeat repeat repeat")]
        [InlineData("mask-position", "banana")]
        [InlineData("mask-size", "banana")]
        [InlineData("mask-size", "cover cover")]
        [InlineData("mask-origin", "no-clip")]
        [InlineData("mask-origin", "margin-box")]
        [InlineData("mask-origin", "border-box padding-box")]
        [InlineData("mask-clip", "text")]
        [InlineData("mask-clip", "banana")]
        [InlineData("mask-composite", "xor")]
        [InlineData("mask-composite", "source-over")]
        [InlineData("mask-composite", "add exclude")]
        public void MaskLonghandsRejectIllegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.False(property.HasValue, $"{name}: {value}");
        }

        [Fact]
        public void MaskLonghandsAreRegistered()
        {
            Assert.IsType<MaskImageProperty>(ParseDeclaration("mask-image: none"));
            Assert.IsType<MaskModeProperty>(ParseDeclaration("mask-mode: alpha"));
            Assert.IsType<MaskRepeatProperty>(ParseDeclaration("mask-repeat: repeat"));
            Assert.IsType<MaskPositionProperty>(ParseDeclaration("mask-position: center"));
            Assert.IsType<MaskSizeProperty>(ParseDeclaration("mask-size: auto"));
            Assert.IsType<MaskOriginProperty>(ParseDeclaration("mask-origin: border-box"));
            Assert.IsType<MaskClipProperty>(ParseDeclaration("mask-clip: no-clip"));
            Assert.IsType<MaskCompositeProperty>(ParseDeclaration("mask-composite: add"));
        }

        [Fact]
        public void MaskPositionAndSizeAreAnimatable()
        {
            Assert.True(ParseDeclaration("mask-position: center").IsAnimatable);
            Assert.True(ParseDeclaration("mask-size: cover").IsAnimatable);
            Assert.False(ParseDeclaration("mask-image: none").IsAnimatable);
            Assert.False(ParseDeclaration("mask-mode: alpha").IsAnimatable);
        }

        [Theory]
        [InlineData("mask-image: inherit")]
        [InlineData("mask-mode: initial")]
        [InlineData("mask-clip: unset")]
        [InlineData("mask-composite: revert")]
        public void MaskLonghandsAcceptGlobalKeywords(string snippet)
        {
            Assert.True(ParseDeclaration(snippet).HasValue);
        }

        [Fact]
        public void MaskPreservesAuthoredText()
        {
            var property = ParseDeclaration("mask-mode: alpha, luminance");

            Assert.Equal("alpha, luminance", property.Value);
        }

        [Fact]
        public void StyleDeclarationExposesMaskAccessors()
        {
            var style = ((StyleRule)ParseStyleSheet(
                ".a { mask-image: url(m.png); mask-mode: alpha; mask-repeat: no-repeat; mask-position: center; " +
                "mask-size: cover; mask-origin: content-box; mask-clip: no-clip; mask-composite: add }").Rules[0]).Style;

            Assert.Equal("url(\"m.png\")", style.MaskImage);
            Assert.Equal("alpha", style.MaskMode);
            Assert.Equal("no-repeat", style.MaskRepeat);
            Assert.Equal("center", style.MaskPosition);
            Assert.Equal("cover", style.MaskSize);
            Assert.Equal("content-box", style.MaskOrigin);
            Assert.Equal("no-clip", style.MaskClip);
            Assert.Equal("add", style.MaskComposite);
        }
    }
}
