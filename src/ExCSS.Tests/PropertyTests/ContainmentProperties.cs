using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class ContainmentPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("none")]
        [InlineData("strict")]
        [InlineData("content")]
        [InlineData("size")]
        [InlineData("layout")]
        [InlineData("style")]
        [InlineData("paint")]
        [InlineData("size layout")]
        [InlineData("paint layout style size")]
        [InlineData("style paint")]
        [InlineData("inline-size")]
        [InlineData("inline-size layout paint")]
        [InlineData("style INLINE-SIZE")]
        [InlineData("LAYOUT Paint")]
        [InlineData("layout paint style")]
        public void ContainLegal(string value)
        {
            var property = ParseDeclaration($"contain: {value}");

            Assert.Equal("contain", property.Name);
            Assert.IsType<ContainProperty>(property);
            Assert.True(property.HasValue);
            Assert.False(property.IsInherited);
        }

        [Theory]
        [InlineData("layout layout")]
        [InlineData("none layout")]
        [InlineData("layout none")]
        [InlineData("strict layout")]
        [InlineData("layout content")]
        [InlineData("strict content")]
        [InlineData("size inline-size")]
        [InlineData("inline-size size")]
        [InlineData("inline-size inline-size")]
        [InlineData("strict inline-size")]
        [InlineData("none inline-size")]
        [InlineData("layout, paint")]
        [InlineData("nonsense")]
        [InlineData("10px")]
        public void ContainIllegal(string value)
        {
            var property = ParseDeclaration($"contain: {value}");

            Assert.IsType<ContainProperty>(property);
            Assert.False(property.HasValue);
        }

        [Fact]
        public void ContainIsEmptyIllegal()
        {
            Assert.False(ParseDeclaration("contain:").HasValue);
        }

        [Theory]
        [InlineData("visible")]
        [InlineData("auto")]
        [InlineData("hidden")]
        public void ContentVisibilityLegal(string value)
        {
            var property = ParseDeclaration($"content-visibility: {value}");

            Assert.IsType<ContentVisibilityProperty>(property);
            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("none")]
        [InlineData("auto hidden")]
        [InlineData("collapse")]
        public void ContentVisibilityIllegal(string value)
        {
            var property = ParseDeclaration($"content-visibility: {value}");

            Assert.IsType<ContentVisibilityProperty>(property);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("contain-intrinsic-width", "none")]
        [InlineData("contain-intrinsic-width", "100px")]
        [InlineData("contain-intrinsic-height", "0")]
        [InlineData("contain-intrinsic-height", "auto 3em")]
        [InlineData("contain-intrinsic-inline-size", "auto none")]
        [InlineData("contain-intrinsic-block-size", "20pt")]
        [InlineData("contain-intrinsic-width", "calc(10px + 2em)")]
        public void ContainIntrinsicLonghandLegal(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.False(property.IsInherited);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("contain-intrinsic-width", "-1px")]
        [InlineData("contain-intrinsic-width", "50%")]
        [InlineData("contain-intrinsic-width", "auto")]
        [InlineData("contain-intrinsic-height", "10px 20px")]
        [InlineData("contain-intrinsic-height", "auto auto")]
        [InlineData("contain-intrinsic-block-size", "1")]
        [InlineData("contain-intrinsic-inline-size", "wide")]
        public void ContainIntrinsicLonghandIllegal(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("10px")]
        [InlineData("none")]
        [InlineData("10px 20px")]
        [InlineData("auto 10px")]
        [InlineData("auto 10px auto 20px")]
        [InlineData("10px auto 20px")]
        [InlineData("auto none none")]
        public void ContainIntrinsicSizeShorthandLegal(string value)
        {
            var property = ParseDeclaration($"contain-intrinsic-size: {value}");

            Assert.IsType<ContainIntrinsicSizeProperty>(property);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("10px 20px 30px")]
        [InlineData("auto")]
        [InlineData("auto auto 10px")]
        [InlineData("-5px")]
        [InlineData("50%")]
        public void ContainIntrinsicSizeShorthandIllegal(string value)
        {
            var property = ParseDeclaration($"contain-intrinsic-size: {value}");

            Assert.IsType<ContainIntrinsicSizeProperty>(property);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("10px", "10px", "10px")]
        [InlineData("10px 20px", "10px", "20px")]
        [InlineData("auto 10px", "auto 10px", "auto 10px")]
        [InlineData("auto 10px 20px", "auto 10px", "20px")]
        [InlineData("10px auto 20px", "10px", "auto 20px")]
        [InlineData("auto 10px auto 20px", "auto 10px", "auto 20px")]
        [InlineData("none", "none", "none")]
        public void ContainIntrinsicSizeExpandsToWidthAndHeight(string value, string width, string height)
        {
            var style = ParseDeclarations($"contain-intrinsic-size: {value}");

            Assert.Equal(width, style.ContainIntrinsicWidth);
            Assert.Equal(height, style.ContainIntrinsicHeight);
        }

        [Fact]
        public void ContainIntrinsicSizeSerializesFromItsLonghands()
        {
            var style = ParseDeclarations("contain-intrinsic-width: auto 5px; contain-intrinsic-height: auto 5px");
            Assert.Contains("contain-intrinsic-size: auto 5px", style.CssText);

            style = ParseDeclarations("contain-intrinsic-width: 5px; contain-intrinsic-height: 7px");
            Assert.Contains("contain-intrinsic-size: 5px 7px", style.CssText);
        }

        [Theory]
        [InlineData("sidebar", "sidebar", "normal")]
        [InlineData("sidebar / size", "sidebar", "size")]
        [InlineData("sidebar/inline-size", "sidebar", "inline-size")]
        [InlineData("a b / normal", "a b", "normal")]
        public void ContainerShorthandExpandsToNameAndType(string value, string name, string type)
        {
            var property = ParseDeclaration($"container: {value}");

            Assert.IsType<ContainerProperty>(property);
            Assert.True(property.HasValue);

            var style = ParseDeclarations($"container: {value}");

            Assert.Equal(name, style.ContainerName);
            Assert.Equal(type, style.ContainerType);
        }

        [Theory]
        [InlineData("sidebar")]
        [InlineData("a b")]
        [InlineData("none")]
        public void ContainerNameAcceptsCustomIdents(string value)
        {
            var property = ParseDeclaration($"container-name: {value}");

            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Fact]
        public void ContainerShorthandResetsTypeWhenOmitted()
        {
            var style = ParseDeclarations("container-type: size; container: foo");

            Assert.Equal("foo", style.ContainerName);
            Assert.Equal("normal", style.ContainerType);
        }

        [Theory]
        [InlineData("")]
        [InlineData("/ size")]
        [InlineData("foo /")]
        [InlineData("foo / bogus")]
        [InlineData("foo / size / size")]
        [InlineData("foo bar baz qux 10px")]
        public void ContainerShorthandIllegal(string value)
        {
            var property = ParseDeclaration($"container: {value}");

            Assert.IsType<ContainerProperty>(property);
            Assert.False(property.HasValue);
        }

        [Fact]
        public void ContainerSerializesFromItsLonghands()
        {
            var style = ParseDeclarations("container-name: foo; container-type: size");
            Assert.Contains("container: foo / size", style.CssText);

            style = ParseDeclarations("container-name: foo; container-type: normal");
            Assert.Contains("container: foo", style.CssText);
            Assert.DoesNotContain("normal", style.CssText);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("10px")]
        [InlineData("padding-box")]
        [InlineData("border-box 4px")]
        [InlineData("2em content-box")]
        [InlineData("CONTENT-BOX")]
        [InlineData("calc(1px + 2px)")]
        public void OverflowClipMarginLegal(string value)
        {
            var property = ParseDeclaration($"overflow-clip-margin: {value}");

            Assert.Equal("overflow-clip-margin", property.Name);
            Assert.IsType<OverflowClipMarginProperty>(property);
            Assert.False(property.IsInherited);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("5")]
        [InlineData("50%")]
        [InlineData("-3px")]
        [InlineData("margin-box")]
        [InlineData("border-box padding-box")]
        [InlineData("1px 2px")]
        [InlineData("border-box 1px 2px")]
        public void OverflowClipMarginIllegal(string value)
        {
            var property = ParseDeclaration($"overflow-clip-margin: {value}");

            Assert.Equal("overflow-clip-margin", property.Name);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("overflow-clip-margin-top", "border-box 3px")]
        [InlineData("overflow-clip-margin-right", "2px")]
        [InlineData("overflow-clip-margin-bottom", "content-box")]
        [InlineData("overflow-clip-margin-left", "0")]
        [InlineData("overflow-clip-margin-block-start", "1px padding-box")]
        [InlineData("overflow-clip-margin-block-end", "4px")]
        [InlineData("overflow-clip-margin-inline-start", "border-box")]
        [InlineData("overflow-clip-margin-inline-end", "2em")]
        [InlineData("overflow-clip-margin-inline", "border-box 3px")]
        [InlineData("overflow-clip-margin-block", "5px")]
        public void OverflowClipMarginPerSidePropertiesLegal(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
        }

        [Theory]
        [InlineData("overflow-clip-margin-top", "5")]
        [InlineData("overflow-clip-margin-left", "50%")]
        [InlineData("overflow-clip-margin-right", "-1px")]
        [InlineData("overflow-clip-margin-block-end", "margin-box")]
        [InlineData("overflow-clip-margin-inline", "1px 2px")]
        public void OverflowClipMarginPerSidePropertiesIllegal(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.False(property.HasValue);
        }

        [Fact]
        public void OverflowClipMarginShorthandsExpand()
        {
            var style = ParseDeclarations("overflow-clip-margin: border-box 3px");

            Assert.Equal("border-box 3px", style.OverflowClipMarginTop);
            Assert.Equal("border-box 3px", style.OverflowClipMarginRight);
            Assert.Equal("border-box 3px", style.OverflowClipMarginBottom);
            Assert.Equal("border-box 3px", style.OverflowClipMarginLeft);

            style = ParseDeclarations("overflow-clip-margin-block: 2px");
            Assert.Equal("2px", style.OverflowClipMarginBlockStart);
            Assert.Equal("2px", style.OverflowClipMarginBlockEnd);

            style = ParseDeclarations("overflow-clip-margin-inline: content-box");
            Assert.Equal("content-box", style.OverflowClipMarginInlineStart);
            Assert.Equal("content-box", style.OverflowClipMarginInlineEnd);
        }

        [Fact]
        public void OverflowClipMarginSerializesFromItsLonghands()
        {
            var style = ParseDeclarations(
                "overflow-clip-margin-top: 4px; overflow-clip-margin-right: 4px; " +
                "overflow-clip-margin-bottom: 4px; overflow-clip-margin-left: 4px");

            Assert.Contains("overflow-clip-margin: 4px", style.CssText);

            style = ParseDeclarations("overflow-clip-margin-block-start: 1px; overflow-clip-margin-block-end: 2px");
            Assert.DoesNotContain("overflow-clip-margin-block:", style.CssText);
        }

        [Fact]
        public void LonghandsAreRegisteredAsShorthandMembers()
        {
            var longhands = PropertyFactory.Instance.GetLonghands(PropertyNames.OverflowClipMargin).ToList();

            Assert.Equal(4, longhands.Count);
        }

        [Theory]
        [InlineData("contain")]
        [InlineData("content-visibility")]
        [InlineData("contain-intrinsic-width")]
        [InlineData("overflow-clip-margin-top")]
        public void KeywordsAreAccepted(string name)
        {
            foreach (var keyword in new[] { "initial", "inherit", "unset" })
            {
                Assert.True(ParseDeclaration($"{name}: {keyword}").HasValue);
            }
        }
    }
}
