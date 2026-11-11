using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class SvgPresentationPropertyTests : CssConstructionFunctions
    {
        // name, expected type, inherited, animatable
        public static IEnumerable<object[]> Registrations => new[]
        {
            new object[] { "stop-color", typeof(StopColorProperty), false, true },
            new object[] { "stop-opacity", typeof(StopOpacityProperty), false, true },
            new object[] { "flood-color", typeof(FloodColorProperty), false, true },
            new object[] { "flood-opacity", typeof(FloodOpacityProperty), false, true },
            new object[] { "lighting-color", typeof(LightingColorProperty), false, true },
            new object[] { "marker-start", typeof(MarkerStartProperty), true, false },
            new object[] { "marker-mid", typeof(MarkerMidProperty), true, false },
            new object[] { "marker-end", typeof(MarkerEndProperty), true, false },
            new object[] { "paint-order", typeof(PaintOrderProperty), true, false },
            new object[] { "clip-rule", typeof(ClipRuleProperty), true, false },
            new object[] { "color-interpolation", typeof(ColorInterpolationProperty), true, false },
            new object[] { "color-interpolation-filters", typeof(ColorInterpolationFiltersProperty), true, false },
            new object[] { "dominant-baseline", typeof(DominantBaselineProperty), true, false },
            new object[] { "alignment-baseline", typeof(AlignmentBaselineProperty), false, false },
            new object[] { "baseline-shift", typeof(BaselineShiftProperty), false, true },
            new object[] { "transform-box", typeof(TransformBoxProperty), false, false },
            new object[] { "inline-size", typeof(InlineSizeProperty), false, true },
            new object[] { "x", typeof(XProperty), false, true },
            new object[] { "y", typeof(YProperty), false, true },
            new object[] { "cx", typeof(CxProperty), false, true },
            new object[] { "cy", typeof(CyProperty), false, true },
            new object[] { "r", typeof(RProperty), false, true },
            new object[] { "rx", typeof(RxProperty), false, true },
            new object[] { "ry", typeof(RyProperty), false, true },
            new object[] { "d", typeof(DProperty), false, true },
        };

        [Theory]
        [MemberData(nameof(Registrations))]
        public void PropertyIsRegisteredWithFlags(string name, System.Type type, bool inherited, bool animatable)
        {
            var property = PropertyFactory.Instance.Create(name);

            Assert.NotNull(property);
            Assert.IsType(type, property);
            Assert.Equal(inherited, property.CanBeInherited);
            Assert.Equal(animatable, property.IsAnimatable);
            Assert.Equal(animatable, PropertyFactory.Instance.IsAnimatable(name));
        }

        [Fact]
        public void MarkerShorthandIsRegisteredAsInheritedShorthand()
        {
            var property = PropertyFactory.Instance.Create("marker");

            Assert.IsType<MarkerProperty>(property);
            Assert.True(property.CanBeInherited);
            Assert.Equal(new[] { "marker-start", "marker-mid", "marker-end" },
                PropertyFactory.Instance.GetLonghands("marker"));
        }

        private static void Legal(string name, string value, string expected = null)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.Equal(name, property.Name);
            Assert.NotNull(property);
            Assert.True(property.HasValue, $"{name}: {value} should be accepted");
            Assert.Equal(expected ?? value, property.Value);
        }

        private static void Illegal(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");
            Assert.NotNull(property);
            Assert.False(property.HasValue, $"{name}: {value} should be rejected");
        }

        [Theory]
        [InlineData("stop-color", "red", "rgb(255, 0, 0)")]
        [InlineData("stop-color", "#0F0", "rgb(0, 255, 0)")]
        [InlineData("stop-color", "currentColor", "currentColor")]
        [InlineData("stop-opacity", "0.5", "0.5")]
        [InlineData("stop-opacity", "50%", "50%")]
        [InlineData("stop-opacity", "1", "1")]
        [InlineData("flood-color", "blue", "rgb(0, 0, 255)")]
        [InlineData("flood-color", "currentColor", "currentColor")]
        [InlineData("flood-opacity", "0.25", "0.25")]
        [InlineData("flood-opacity", "25%", "25%")]
        [InlineData("lighting-color", "white", "rgb(255, 255, 255)")]
        [InlineData("lighting-color", "currentColor", "currentColor")]
        public void ColorAndOpacityPropertiesAcceptLegalValues(string name, string value, string expected)
            => Legal(name, value, expected);

        [Theory]
        [InlineData("stop-color", "red blue")]
        [InlineData("stop-color", "10px")]
        [InlineData("stop-opacity", "0.5 0.5")]
        [InlineData("stop-opacity", "red")]
        [InlineData("flood-color", "none")]
        [InlineData("flood-opacity", "5px")]
        [InlineData("lighting-color", "0.5")]
        public void ColorAndOpacityPropertiesRejectIllegalValues(string name, string value)
            => Illegal(name, value);

        [Theory]
        [InlineData("marker-start", "none")]
        [InlineData("marker-mid", "url(#arrow)")]
        [InlineData("marker-end", "url(#arrow)")]
        [InlineData("marker", "none")]
        [InlineData("marker", "url(#arrow)")]
        public void MarkerPropertiesAcceptLegalValues(string name, string value)
            => Legal(name, value, value.StartsWith("url") ? "url(\"#arrow\")" : value);

        [Theory]
        [InlineData("marker-start", "red")]
        [InlineData("marker-mid", "url(#a) url(#b)")]
        [InlineData("marker-end", "10px")]
        [InlineData("marker", "url(#a) url(#b) url(#c)")]
        [InlineData("marker", "auto")]
        public void MarkerPropertiesRejectIllegalValues(string name, string value)
            => Illegal(name, value);

        [Fact]
        public void MarkerShorthandExpandsToAllLonghands()
        {
            var style = ParseDeclarations("marker: url(#m)");

            Assert.Equal("url(\"#m\")", style.MarkerStart);
            Assert.Equal("url(\"#m\")", style.MarkerMid);
            Assert.Equal("url(\"#m\")", style.MarkerEnd);
        }

        [Fact]
        public void MarkerLonghandsOverrideShorthand()
        {
            var style = ParseDeclarations("marker: none; marker-end: url(#e)");

            Assert.Equal("none", style.MarkerStart);
            Assert.Equal("none", style.MarkerMid);
            Assert.Equal("url(\"#e\")", style.MarkerEnd);
        }

        [Theory]
        [InlineData("normal")]
        [InlineData("fill")]
        [InlineData("stroke")]
        [InlineData("markers")]
        [InlineData("stroke fill")]
        [InlineData("markers stroke")]
        [InlineData("fill stroke markers")]
        [InlineData("markers fill stroke")]
        public void PaintOrderLegal(string value) => Legal("paint-order", value);

        [Theory]
        [InlineData("fill fill")]
        [InlineData("normal fill")]
        [InlineData("fill stroke markers stroke")]
        [InlineData("fill, stroke")]
        [InlineData("auto")]
        [InlineData("10px")]
        public void PaintOrderIllegal(string value) => Illegal("paint-order", value);

        [Theory]
        [InlineData("clip-rule", "nonzero")]
        [InlineData("clip-rule", "evenodd")]
        [InlineData("color-interpolation", "auto")]
        [InlineData("color-interpolation-filters", "auto")]
        [InlineData("dominant-baseline", "auto")]
        [InlineData("dominant-baseline", "middle")]
        [InlineData("dominant-baseline", "hanging")]
        [InlineData("dominant-baseline", "text-top")]
        [InlineData("alignment-baseline", "baseline")]
        [InlineData("alignment-baseline", "central")]
        [InlineData("alignment-baseline", "text-bottom")]
        [InlineData("baseline-shift", "baseline")]
        [InlineData("baseline-shift", "sub")]
        [InlineData("baseline-shift", "super")]
        [InlineData("baseline-shift", "-2px")]
        [InlineData("baseline-shift", "25%")]
        [InlineData("transform-box", "content-box")]
        [InlineData("transform-box", "border-box")]
        [InlineData("transform-box", "fill-box")]
        [InlineData("transform-box", "stroke-box")]
        [InlineData("transform-box", "view-box")]
        [InlineData("inline-size", "auto")]
        [InlineData("inline-size", "100px")]
        [InlineData("inline-size", "50%")]
        public void KeywordAndLengthPropertiesAcceptLegalValues(string name, string value)
            => Legal(name, value);

        [Theory]
        [InlineData("clip-rule", "nonsense")]
        [InlineData("clip-rule", "nonzero evenodd")]
        [InlineData("color-interpolation", "rgb")]
        [InlineData("color-interpolation-filters", "none")]
        [InlineData("dominant-baseline", "10px")]
        [InlineData("dominant-baseline", "super")]
        [InlineData("alignment-baseline", "sub")]
        [InlineData("baseline-shift", "auto")]
        [InlineData("baseline-shift", "1px 2px")]
        [InlineData("transform-box", "padding-box")]
        [InlineData("transform-box", "fill-box stroke-box")]
        [InlineData("inline-size", "red")]
        [InlineData("inline-size", "1px 2px")]
        public void KeywordAndLengthPropertiesRejectIllegalValues(string name, string value)
            => Illegal(name, value);

        [Theory]
        [InlineData("x", "0")]
        [InlineData("x", "10px")]
        [InlineData("x", "-5px")]
        [InlineData("x", "50%")]
        [InlineData("y", "1.5em")]
        [InlineData("cx", "10px")]
        [InlineData("cy", "-10%")]
        [InlineData("r", "5px")]
        [InlineData("r", "50%")]
        [InlineData("rx", "5px")]
        [InlineData("rx", "auto")]
        [InlineData("ry", "10%")]
        [InlineData("ry", "auto")]
        public void GeometryPropertiesAcceptLegalValues(string name, string value)
            => Legal(name, value);

        [Theory]
        [InlineData("x", "auto")]
        [InlineData("x", "red")]
        [InlineData("y", "1px 2px")]
        [InlineData("cx", "none")]
        [InlineData("cy", "#fff")]
        [InlineData("r", "-5px")]
        [InlineData("r", "auto")]
        [InlineData("rx", "-1px")]
        [InlineData("ry", "-1%")]
        [InlineData("ry", "none")]
        public void GeometryPropertiesRejectIllegalValues(string name, string value)
            => Illegal(name, value);

        [Theory]
        [InlineData("none")]
        [InlineData("path(\"M 0 0 L 10 10 Z\")")]
        [InlineData("path(\"M10,20 C 1 2 3 4 5 6 z\")")]
        [InlineData("path(evenodd, \"M 0 0 h 10 v 10 z\")")]
        [InlineData("path(nonzero, \"m 0 0 l 1 1\")")]
        public void PathLegal(string value) => Legal("d", value);

        [Theory]
        [InlineData("path()")]
        [InlineData("path(\"\")")]
        [InlineData("path(\"L 10 10\")")]
        [InlineData("path(\"M 0 0 x\")")]
        [InlineData("path(M 0 0)")]
        [InlineData("path(\"M 0 0\", \"M 1 1\")")]
        [InlineData("path(evenodd \"M 0 0\")")]
        [InlineData("path(\"M 0 0\n h 1\")")]
        [InlineData("path(\"M 0 0\n\")")]
        [InlineData("path(bogus, \"M 0 0\")")]
        [InlineData("polygon(0 0, 1px 1px)")]
        [InlineData("url(#p)")]
        [InlineData("none none")]
        public void PathIllegal(string value) => Illegal("d", value);

        [Fact]
        public void GlobalKeywordsAreAccepted()
        {
            foreach (var name in new[] { "stop-color", "paint-order", "marker", "x", "d", "transform-box" })
            foreach (var keyword in GlobalKeywordTestValues)
            {
                var property = ParseDeclaration($"{name}: {keyword}");
                Assert.True(property.HasValue, $"{name}: {keyword}");
            }
        }

        [Fact]
        public void StyleDeclarationAccessorsRoundTrip()
        {
            var style = ParseDeclarations(
                "stop-color: red; stop-opacity: 0.5; flood-color: blue; flood-opacity: 0.25; lighting-color: white;" +
                "paint-order: stroke fill; clip-rule: evenodd; color-interpolation: sRGB;" +
                "color-interpolation-filters: linearRGB; dominant-baseline: middle; alignment-baseline: central;" +
                "baseline-shift: sub; transform-box: fill-box; inline-size: 20px;" +
                "x: 1px; y: 2px; cx: 3px; cy: 4px; r: 5px; rx: 6px; ry: auto; d: path(\"M 0 0 L 1 1\")");

            Assert.Equal("rgb(255, 0, 0)", style.StopColor);
            Assert.Equal("0.5", style.StopOpacity);
            Assert.Equal("rgb(0, 0, 255)", style.FloodColor);
            Assert.Equal("0.25", style.FloodOpacity);
            Assert.Equal("rgb(255, 255, 255)", style.LightingColor);
            Assert.Equal("stroke fill", style.PaintOrder);
            Assert.Equal("evenodd", style.ClipRule);
            Assert.Equal("srgb", style.ColorInterpolation);
            Assert.Equal("linearrgb", style.ColorInterpolationFilters);
            Assert.Equal("middle", style.DominantBaseline);
            Assert.Equal("central", style.AlignmentBaseline);
            Assert.Equal("sub", style.BaselineShift);
            Assert.Equal("fill-box", style.TransformBox);
            Assert.Equal("20px", style.InlineSize);
            Assert.Equal("1px", style.X);
            Assert.Equal("2px", style.Y);
            Assert.Equal("3px", style.Cx);
            Assert.Equal("4px", style.Cy);
            Assert.Equal("5px", style.R);
            Assert.Equal("6px", style.Rx);
            Assert.Equal("auto", style.Ry);
            Assert.Equal("path(\"M 0 0 L 1 1\")", style.D);
        }

        [Fact]
        public void AccessorSettersWriteTheProperty()
        {
            var style = ParseDeclarations(string.Empty);
            style.PaintOrder = "markers";
            style.Rx = "3px";
            style.D = "none";

            Assert.Equal("markers", style.GetPropertyValue("paint-order"));
            Assert.Equal("3px", style.GetPropertyValue("rx"));
            Assert.Equal("none", style.GetPropertyValue("d"));
            Assert.Equal(3, style.Count());
        }
    }
}
