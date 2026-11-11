using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class ScrollAndSnapPropertyTests : CssConstructionFunctions
    {
        private static StyleDeclaration Style(string declarations) =>
            ((StyleRule)ParseStyleSheet(".t { " + declarations + " }").StyleRules.First()).Style;

        private static void Illegal<TProp>(string name, string value) where TProp : Property
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.IsType<TProp>(property);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("10px")]
        [InlineData("2em")]
        [InlineData("inherit")]
        public void ScrollMarginLonghandsAccept(string value)
        {
            TestForLegalValue<ScrollMarginTopProperty>("scroll-margin-top", value);
            TestForLegalValue<ScrollMarginRightProperty>("scroll-margin-right", value);
            TestForLegalValue<ScrollMarginBottomProperty>("scroll-margin-bottom", value);
            TestForLegalValue<ScrollMarginLeftProperty>("scroll-margin-left", value);
        }

        [Theory]
        [InlineData("auto")]
        [InlineData("5%")]
        [InlineData("red")]
        public void ScrollMarginRejects(string value)
        {
            Illegal<ScrollMarginTopProperty>("scroll-margin-top", value);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("auto")]
        [InlineData("10px")]
        [InlineData("5%")]
        [InlineData("unset")]
        public void ScrollPaddingLonghandsAccept(string value)
        {
            TestForLegalValue<ScrollPaddingTopProperty>("scroll-padding-top", value);
            TestForLegalValue<ScrollPaddingRightProperty>("scroll-padding-right", value);
            TestForLegalValue<ScrollPaddingBottomProperty>("scroll-padding-bottom", value);
            TestForLegalValue<ScrollPaddingLeftProperty>("scroll-padding-left", value);
        }

        [Fact]
        public void ScrollPaddingRejectsKeyword()
        {
            Illegal<ScrollPaddingLeftProperty>("scroll-padding-left", "smooth");
        }

        [Theory]
        [InlineData("1px", "1px", "1px", "1px", "1px")]
        [InlineData("1px 2px", "1px", "2px", "1px", "2px")]
        [InlineData("1px 2px 3px", "1px", "2px", "3px", "2px")]
        [InlineData("1px 2px 3px 4px", "1px", "2px", "3px", "4px")]
        public void ScrollMarginShorthandExpands(string value, string top, string right, string bottom, string left)
        {
            var style = Style($"scroll-margin: {value}");
            Assert.Equal(value, style.ScrollMargin);
            Assert.Equal(top, style.ScrollMarginTop);
            Assert.Equal(right, style.ScrollMarginRight);
            Assert.Equal(bottom, style.ScrollMarginBottom);
            Assert.Equal(left, style.ScrollMarginLeft);
        }

        [Theory]
        [InlineData("auto", "auto", "auto", "auto", "auto")]
        [InlineData("1px auto", "1px", "auto", "1px", "auto")]
        [InlineData("5% 2px 3px 4px", "5%", "2px", "3px", "4px")]
        public void ScrollPaddingShorthandExpands(string value, string top, string right, string bottom, string left)
        {
            var style = Style($"scroll-padding: {value}");
            Assert.Equal(value, style.ScrollPadding);
            Assert.Equal(top, style.ScrollPaddingTop);
            Assert.Equal(right, style.ScrollPaddingRight);
            Assert.Equal(bottom, style.ScrollPaddingBottom);
            Assert.Equal(left, style.ScrollPaddingLeft);
        }

        [Theory]
        [InlineData("1px 2px 3px 4px 5px")]
        [InlineData("auto")]
        public void ScrollMarginShorthandRejects(string value)
        {
            Illegal<ScrollMarginProperty>("scroll-margin", value);
        }

        [Fact]
        public void ScrollMarginAndPaddingAreAnimatable()
        {
            Assert.True(ParseDeclaration("scroll-margin-top: 1px").IsAnimatable);
            Assert.True(ParseDeclaration("scroll-padding-left: 1px").IsAnimatable);
            Assert.False(ParseDeclaration("scroll-snap-type: x").IsAnimatable);
            Assert.False(ParseDeclaration("scroll-margin-top: 1px").IsInherited);
        }

        [Theory]
        [InlineData("auto")]
        [InlineData("contain")]
        [InlineData("none")]
        public void OverscrollBehaviorLonghandsAccept(string value)
        {
            TestForLegalValue<OverscrollBehaviorXProperty>("overscroll-behavior-x", value);
            TestForLegalValue<OverscrollBehaviorYProperty>("overscroll-behavior-y", value);
            TestForLegalValue<OverscrollBehaviorProperty>("overscroll-behavior", value);
        }

        [Theory]
        [InlineData("smooth")]
        [InlineData("contain none")]
        public void OverscrollBehaviorLonghandRejects(string value)
        {
            Illegal<OverscrollBehaviorXProperty>("overscroll-behavior-x", value);
        }

        [Theory]
        [InlineData("contain", "contain", "contain")]
        [InlineData("contain none", "contain", "none")]
        [InlineData("auto contain", "auto", "contain")]
        public void OverscrollBehaviorShorthandExpands(string value, string x, string y)
        {
            var style = Style($"overscroll-behavior: {value}");
            Assert.Equal(value, style.OverscrollBehavior);
            Assert.Equal(x, style.OverscrollBehaviorX);
            Assert.Equal(y, style.OverscrollBehaviorY);
        }

        [Theory]
        [InlineData("auto none contain")]
        [InlineData("scroll")]
        public void OverscrollBehaviorShorthandRejects(string value)
        {
            Illegal<OverscrollBehaviorProperty>("overscroll-behavior", value);
        }

        [Theory]
        [InlineData("none")]
        [InlineData("x")]
        [InlineData("y mandatory")]
        [InlineData("block proximity")]
        [InlineData("inline mandatory")]
        [InlineData("both mandatory")]
        public void ScrollSnapTypeAccepts(string value)
        {
            TestForLegalValue<ScrollSnapTypeProperty>("scroll-snap-type", value);
        }

        [Theory]
        [InlineData("mandatory")]
        [InlineData("x y")]
        [InlineData("none mandatory")]
        [InlineData("x mandatory proximity")]
        [InlineData("10px")]
        public void ScrollSnapTypeRejects(string value)
        {
            Illegal<ScrollSnapTypeProperty>("scroll-snap-type", value);
        }

        [Theory]
        [InlineData("none")]
        [InlineData("start")]
        [InlineData("center end")]
        [InlineData("start start")]
        public void ScrollSnapAlignAccepts(string value)
        {
            TestForLegalValue<ScrollSnapAlignProperty>("scroll-snap-align", value);
        }

        [Theory]
        [InlineData("none start")]
        [InlineData("start end center")]
        [InlineData("left")]
        public void ScrollSnapAlignRejects(string value)
        {
            Illegal<ScrollSnapAlignProperty>("scroll-snap-align", value);
        }

        [Theory]
        [InlineData("normal")]
        [InlineData("always")]
        public void ScrollSnapStopAccepts(string value)
        {
            TestForLegalValue<ScrollSnapStopProperty>("scroll-snap-stop", value);
        }

        [Fact]
        public void ScrollSnapStopRejects()
        {
            Illegal<ScrollSnapStopProperty>("scroll-snap-stop", "never");
        }

        [Theory]
        [InlineData("auto")]
        [InlineData("smooth")]
        public void ScrollBehaviorAccepts(string value)
        {
            TestForLegalValue<ScrollBehaviorProperty>("scroll-behavior", value);
        }

        [Fact]
        public void ScrollBehaviorRejects()
        {
            Illegal<ScrollBehaviorProperty>("scroll-behavior", "instant");
        }

        [Theory]
        [InlineData("auto")]
        [InlineData("thin")]
        [InlineData("none")]
        public void ScrollbarWidthAccepts(string value)
        {
            TestForLegalValue<ScrollbarWidthProperty>("scrollbar-width", value);
        }

        [Fact]
        public void ScrollbarWidthRejects()
        {
            Illegal<ScrollbarWidthProperty>("scrollbar-width", "wide");
        }

        [Theory]
        [InlineData("auto", "auto")]
        [InlineData("red blue", "rgb(255, 0, 0) rgb(0, 0, 255)")]
        [InlineData("#fff #000", "rgb(255, 255, 255) rgb(0, 0, 0)")]
        [InlineData("rgb(0, 0, 0) transparent", "rgb(0, 0, 0) rgba(0, 0, 0, 0)")]
        public void ScrollbarColorAccepts(string value, string expected)
        {
            var property = ParseDeclaration($"scrollbar-color: {value}");
            Assert.IsType<ScrollbarColorProperty>(property);
            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("red")]
        [InlineData("red blue green")]
        [InlineData("auto red")]
        public void ScrollbarColorRejects(string value)
        {
            Illegal<ScrollbarColorProperty>("scrollbar-color", value);
        }

        [Theory]
        [InlineData("auto")]
        [InlineData("stable")]
        [InlineData("stable both-edges")]
        [InlineData("both-edges stable")]
        public void ScrollbarGutterAccepts(string value)
        {
            TestForLegalValue<ScrollbarGutterProperty>("scrollbar-gutter", value);
        }

        [Theory]
        [InlineData("both-edges")]
        [InlineData("auto stable")]
        [InlineData("stable stable")]
        public void ScrollbarGutterRejects(string value)
        {
            Illegal<ScrollbarGutterProperty>("scrollbar-gutter", value);
        }

        [Fact]
        public void ScrollbarColorIsAnimatable()
        {
            Assert.True(ParseDeclaration("scrollbar-color: red blue").IsAnimatable);
        }
    }
}
