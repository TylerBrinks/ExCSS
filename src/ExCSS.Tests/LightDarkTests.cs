using System.Linq;

namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    /// <summary><c>light-dark(&lt;color&gt;, &lt;color&gt;)</c> (CSS Color 5 section 4): validated and kept as written; there is no color scheme to resolve it with.</summary>
    public class LightDarkTests : CssConstructionFunctions
    {
        private static string Declared(string property, string value)
        {
            var sheet = new StylesheetParser().Parse($"div {{ {property}: {value}; }}");
            var rule = sheet.StyleRules.OfType<StyleRule>().Single();
            return rule.Style.GetPropertyValue(property);
        }

        [Theory]
        [InlineData("color")]
        [InlineData("background-color")]
        [InlineData("border-top-color")]
        [InlineData("outline-color")]
        [InlineData("text-decoration-color")]
        [InlineData("column-rule-color")]
        [InlineData("fill")]
        [InlineData("stroke")]
        public void LightDark_IsAcceptedWhereverAColorIs_AndRoundTrips(string property)
        {
            Assert.Equal("light-dark(white, black)", Declared(property, "light-dark(white, black)"));
        }

        [Theory]
        [InlineData("light-dark(#fff, #000)")]
        [InlineData("light-dark(rgb(255 255 255), hsl(0 0% 0% / 0.5))")]
        [InlineData("light-dark(oklch(0.9 0.05 250), color(display-p3 0 0 0))")]
        [InlineData("light-dark(color-mix(in srgb, red, blue), transparent)")]
        [InlineData("light-dark(currentcolor, red)")]
        [InlineData("light-dark(light-dark(red, blue), green)")]
        [InlineData("light-dark(red, light-dark(blue, green))")]
        public void LightDark_ArmsAreAnyColor(string value)
        {
            Assert.Equal(value, Declared("color", value));
        }

        [Fact]
        public void LightDark_IsAcceptedInShorthandsAndLists()
        {
            var sheet = ParseStyleSheet(
                "div { background: light-dark(#eee, #111); border: 1px solid light-dark(#ccc, #333); box-shadow: 0 0 2px light-dark(rgba(0,0,0,.2), rgba(255,255,255,.2)); " +
                "background-image: linear-gradient(light-dark(red, blue), light-dark(white, black)); }");
            var style = sheet.StyleRules.OfType<StyleRule>().Single().Style;

            Assert.Equal("light-dark(#eee, #111)", style.GetPropertyValue("background-color"));
            Assert.Equal("light-dark(#ccc, #333)", style.GetPropertyValue("border-top-color"));
            Assert.False(string.IsNullOrEmpty(style.GetPropertyValue("box-shadow")));
            Assert.False(string.IsNullOrEmpty(style.GetPropertyValue("background-image")));
        }

        [Fact]
        public void LightDark_IsCaseInsensitive()
        {
            Assert.False(string.IsNullOrEmpty(Declared("color", "LIGHT-DARK(red, blue)")));
        }

        [Theory]
        [InlineData("light-dark(red)")]
        [InlineData("light-dark(red, blue, green)")]
        [InlineData("light-dark()")]
        [InlineData("light-dark(red,)")]
        [InlineData("light-dark(, red)")]
        [InlineData("light-dark(red, 12px)")]
        [InlineData("light-dark(notacolor, red)")]
        [InlineData("light-dark(red blue, green)")]
        public void LightDark_WithoutExactlyTwoColors_IsInvalid(string value)
        {
            Assert.True(string.IsNullOrEmpty(Declared("color", value)));
        }

        [Fact]
        public void LightDark_IsNotAColorForPropertiesThatTakeNone()
        {
            // display takes keywords only.
            Assert.True(string.IsNullOrEmpty(Declared("display", "light-dark(red, blue)")));
        }

        [Fact]
        public void TryGetLightDark_ExposesBothArms()
        {
            var tokens = ColorTestHelpers.Tokenize("light-dark(rgb(255 0 0), #00f)");

            Assert.True(tokens.TryGetLightDark(out var light, out var dark));
            Assert.Single(light);
            Assert.Equal("rgb", ((FunctionToken)light[0]).Data);
            Assert.Single(dark);
            Assert.Equal(TokenType.Color, dark[0].Type);
        }

        [Fact]
        public void TryResolveLightDark_ResolvesEachArmToSrgb()
        {
            var tokens = ColorTestHelpers.Tokenize("light-dark(rgb(255 0 0), color-mix(in srgb, blue, blue))");

            Assert.True(tokens.TryResolveLightDark(out var light, out var dark));
            Assert.Equal(new Color(255, 0, 0), light);
            Assert.Equal(new Color(0, 0, 255), dark);
        }

        [Fact]
        public void TryResolveLightDark_AnArmThatIsNotASolidColor_IsNull()
        {
            var tokens = ColorTestHelpers.Tokenize("light-dark(currentcolor, red)");

            Assert.True(tokens.TryResolveLightDark(out var light, out var dark));
            Assert.Null(light);
            Assert.Equal(Color.Red, dark);
        }

        [Fact]
        public void LightDark_IsNotASingleColorWithoutAScheme()
        {
            Assert.Null(ColorTestHelpers.TryResolve("light-dark(red, blue)"));
            Assert.False(ColorTestHelpers.Tokenize("red").TryGetLightDark(out _, out _));
            Assert.False(ColorTestHelpers.Tokenize("light-dark(red)").TryGetLightDark(out _, out _));
        }
    }
}
