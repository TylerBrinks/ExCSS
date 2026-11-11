using System.Collections.Generic;
using System.Linq;

namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    /// <summary>
    /// CSS Color 4/5 function forms that the strict Layer A color grammar doesn't compute — oklch(),
    /// oklab(), lab(), lch(), and color-mix(), plus the lenient CSS Color 4 space-separated / slash-alpha
    /// forms of rgb()/hsl()/hwb() — now resolve to real sRGB colors via
    /// <see cref="ColorFunctionExtensions.ToResolvedColor"/>. (These are what a Tailwind v4 default
    /// palette and its opacity modifiers are authored in.)
    /// </summary>
    public class ModernColorTests : CssConstructionFunctions
    {
        // Tokenize a bare CSS color value and resolve it to a concrete sRGB Color, mirroring how the
        // object-model resolver consumes an already-parsed token stream (nested functions arrive as a
        // single FunctionToken).
        private static Color Resolve(string value)
        {
            var color = Tokenize(value).ToResolvedColor();
            Assert.True(color.HasValue, $"expected '{value}' to resolve to a color");
            return color.Value;
        }

        private static List<Token> Tokenize(string value)
        {
            var lexer = new Lexer(new TextSource(value));
            var tokens = new List<Token>();
            Token token;
            do
            {
                token = lexer.Get();
                if (token.Type != TokenType.EndOfFile && token.Type != TokenType.Whitespace)
                {
                    tokens.Add(token);
                }
            } while (token.Type != TokenType.EndOfFile);

            return tokens;
        }

        // ── hsl()/hwb() as solid colors (previously would stack-overflow at resolve time) ──

        [Theory]
        [InlineData("hsl(120, 100%, 50%)")]
        [InlineData("hsl(120 100% 50%)")]
        public void Hsl_ResolvesToGreen(string value)
        {
            var c = Resolve(value);
            Assert.Equal(0, c.R);
            Assert.Equal(255, c.G);
            Assert.Equal(0, c.B);
            Assert.Equal(255, c.A);
        }

        [Fact]
        public void Hsla_SlashAndCommaAlpha_AreApplied()
        {
            Assert.InRange(Resolve("hsla(0, 100%, 50%, 0.5)").A, 127, 128);
            Assert.InRange(Resolve("hsl(0 100% 50% / 50%)").A, 127, 128);
        }

        [Fact]
        public void Hwb_ResolvesToPrimary()
        {
            var c = Resolve("hwb(240 0% 0%)");
            Assert.Equal(0, c.R);
            Assert.Equal(0, c.G);
            Assert.Equal(255, c.B);
        }

        [Fact]
        public void Gray_Resolves()
        {
            var c = Resolve("gray(128)");
            Assert.Equal(128, c.R);
            Assert.Equal(128, c.G);
            Assert.Equal(128, c.B);
        }

        // ── Space-separated & slash-alpha rgb() (lenient CSS Color 4 forms) ────────

        [Theory]
        [InlineData("rgb(10 20 30)")]
        [InlineData("rgb(10, 20, 30)")]
        public void Rgb_SpaceAndCommaSeparated_Resolve(string value)
        {
            var c = Resolve(value);
            Assert.Equal(10, c.R);
            Assert.Equal(20, c.G);
            Assert.Equal(30, c.B);
            Assert.Equal(255, c.A);
        }

        [Fact]
        public void Rgb_SlashAlpha_IsApplied()
        {
            Assert.InRange(Resolve("rgb(10 20 30 / 0.5)").A, 127, 128);
            Assert.InRange(Resolve("rgba(10 20 30 / 50%)").A, 127, 128);
        }

        // ── oklch hue-angle units ─────────────────────────────────────────────────

        [Theory]
        [InlineData("oklch(0.7 0.15 0.25turn)")] // 0.25turn == 90deg
        [InlineData("oklch(0.7 0.15 100grad)")]  // 100grad == 90deg
        [InlineData("oklch(0.7 0.15 1.5707963rad)")] // ~90deg
        public void Oklch_HueAngleUnits_MatchDegrees(string value)
        {
            var expected = Resolve("oklch(0.7 0.15 90deg)");
            var c = Resolve(value);
            Assert.InRange(c.R, expected.R - 1, expected.R + 1);
            Assert.InRange(c.G, expected.G - 1, expected.G + 1);
            Assert.InRange(c.B, expected.B - 1, expected.B + 1);
        }

        [Fact]
        public void NoneComponents_ResolveToZero()
        {
            // `none` resolves to 0: oklab(1 none none) == oklab(1 0 0) == white.
            var c = Resolve("oklab(1 none none)");
            Assert.InRange(c.R, 252, 255);
            Assert.InRange(c.G, 252, 255);
            Assert.InRange(c.B, 252, 255);
        }

        [Fact]
        public void ColorMix_InPolarSpace_WithHueMethod_Resolves()
        {
            // A polar interpolation space plus an explicit hue-interpolation method.
            var c = Resolve("color-mix(in oklch longer hue, oklch(0.7 0.2 20), oklch(0.7 0.2 200))");
            Assert.False(c.R == 0 && c.G == 0 && c.B == 0, "expected a real mixed color, not black");
        }

        // ── Lightness extremes (exact, space-independent) ─────────────────────────

        [Theory]
        [InlineData("oklab(0 0 0)")]
        [InlineData("lab(0 0 0)")]
        [InlineData("oklch(0 0 0)")]
        [InlineData("lch(0 0 0)")]
        public void Lightness0_IsBlack(string value)
        {
            var c = Resolve(value);
            Assert.Equal(0, c.R);
            Assert.Equal(0, c.G);
            Assert.Equal(0, c.B);
            Assert.Equal(255, c.A);
        }

        [Theory]
        [InlineData("oklab(1 0 0)")]
        [InlineData("lab(100 0 0)")]
        [InlineData("oklch(1 0 0)")]
        [InlineData("lch(100 0 0)")]
        public void MaxLightness_NoChroma_IsWhite(string value)
        {
            var c = Resolve(value);
            Assert.InRange(c.R, 252, 255);
            Assert.InRange(c.G, 252, 255);
            Assert.InRange(c.B, 252, 255);
        }

        // ── Chroma / hue actually take effect (not a no-op) ───────────────────────

        [Fact]
        public void Oklch_RedHue_IsReddish()
        {
            // ~sRGB red sits near oklch(0.63 0.26 29deg); assert the channel ordering, not exact bytes.
            var c = Resolve("oklch(0.63 0.26 29)");
            Assert.True(c.R > c.G && c.R > c.B, $"expected reddish, got {c.R},{c.G},{c.B}");
            Assert.True(c.R > 200, $"expected a strong red channel, got R={c.R}");
        }

        [Fact]
        public void Oklch_PercentAndAngleUnits_AreHonored()
        {
            // 70% lightness == 0.7; 240deg is a blue-ish hue.
            var c = Resolve("oklch(70% 0.15 240deg)");
            Assert.True(c.B > c.R, $"expected blue-dominant, got {c.R},{c.G},{c.B}");
        }

        [Fact]
        public void Oklch_SlashAlpha_IsApplied()
        {
            var c = Resolve("oklch(0 0 0 / 0.5)");
            Assert.InRange(c.A, 127, 128);
        }

        [Fact]
        public void Lch_ResolvesToNonBlackColor()
        {
            var c = Resolve("lch(50% 40 30)");
            Assert.False(c.R == 0 && c.G == 0 && c.B == 0, "expected a real color, not black");
            Assert.True(c.R > c.B, $"expected a warm hue, got {c.R},{c.G},{c.B}");
        }

        [Fact]
        public void Lab_ResolvesToNonBlackColor()
        {
            var c = Resolve("lab(50% 40 30)");
            Assert.False(c.R == 0 && c.G == 0 && c.B == 0, "expected a real color, not black");
        }

        // ── color-mix() ───────────────────────────────────────────────────────────

        [Fact]
        public void ColorMix_Srgb_WhiteBlack_IsMidGray()
        {
            var c = Resolve("color-mix(in srgb, white, black)");
            Assert.InRange(c.R, 127, 128);
            Assert.InRange(c.G, 127, 128);
            Assert.InRange(c.B, 127, 128);
            Assert.Equal(255, c.A);
        }

        [Fact]
        public void ColorMix_Srgb_RedBlue_IsPurple()
        {
            var c = Resolve("color-mix(in srgb, red, blue)");
            Assert.InRange(c.R, 127, 128);
            Assert.Equal(0, c.G);
            Assert.InRange(c.B, 127, 128);
        }

        [Fact]
        public void ColorMix_Srgb_WeightedPercentages_ShiftTowardHeavierColor()
        {
            // 25% white + (implicit) 75% black -> 0.25 gray.
            var c = Resolve("color-mix(in srgb, white 25%, black)");
            Assert.InRange(c.R, 63, 64);
        }

        [Fact]
        public void ColorMix_WithTransparent_IsOpacityModifier()
        {
            // The Tailwind v4 opacity-modifier shape: color at 50%, mixed with transparent -> same
            // color at half alpha.
            var c = Resolve("color-mix(in oklab, blue 50%, transparent)");
            Assert.True(c.B > 250, $"expected blue preserved, got B={c.B}");
            Assert.InRange(c.R, 0, 4);
            Assert.InRange(c.G, 0, 4);
            Assert.InRange(c.A, 126, 129);
        }

        [Fact]
        public void ColorMix_PercentageBeforeColor_IsAccepted()
        {
            // Per CSS Color 5 §3.1 the percentage may precede the color; `30% white` == `white 30%`.
            var before = Resolve("color-mix(in srgb, 30% white, black)");
            var after = Resolve("color-mix(in srgb, white 30%, black)");
            Assert.Equal(after.R, before.R);
            Assert.Equal(after.G, before.G);
            Assert.Equal(after.B, before.B);
            Assert.InRange(before.R, 76, 77); // 0.30 gray
        }

        [Fact]
        public void ColorMix_PercentagesBelow100_ScaleResultAlpha()
        {
            // 30% red + 30% blue: components mix 50/50, but the 60% total scales alpha to 0.6.
            var c = Resolve("color-mix(in srgb, red 30%, blue 30%)");
            Assert.InRange(c.A, 152, 154); // 0.6 * 255 ≈ 153
        }

        [Fact]
        public void ColorMix_WithNestedOklchOperand_Resolves()
        {
            // The Tailwind v4 opacity-modifier shape with an oklch operand (rather than a plain named
            // color): the nested function must resolve through the full resolver.
            var c = Resolve("color-mix(in oklab, oklch(0.62 0.2 265) 50%, transparent)");
            Assert.True(c.B > c.R, $"expected blue-dominant, got {c.R},{c.G},{c.B}");
            Assert.InRange(c.A, 126, 129);
        }

        // ── Parse level: the value survives the Layer A cascade ───────────────────

        [Theory]
        [InlineData("oklch(0.63 0.26 29)")]
        [InlineData("oklab(0.7 0.1 0.1)")]
        [InlineData("lab(50% 40 30)")]
        [InlineData("lch(50% 40 30)")]
        [InlineData("color-mix(in srgb, red, blue)")]
        [InlineData("hsl(280 70% 55%)")]
        [InlineData("hwb(240 0% 0%)")]
        [InlineData("rgb(1 2 3 / 0.5)")]
        public void ModernColorFunction_IsAcceptedAsBackgroundColor(string value)
        {
            var sheet = ParseStyleSheet($"div {{ background-color: {value}; }}");
            var rule = sheet.StyleRules.OfType<StyleRule>().Single();
            var declared = rule.Style.GetPropertyValue("background-color");
            Assert.False(string.IsNullOrEmpty(declared));
        }

        [Fact]
        public void Oklch_ViaBackgroundShorthand_ExpandsToBackgroundColor()
        {
            // Regression: the `background` shorthand must accept a CSS Color 4/5 function and carry it to
            // the background-color longhand (a strict-only Layer A color grammar would drop it).
            var sheet = ParseStyleSheet("div { background: oklch(0.63 0.26 29); }");
            var rule = sheet.StyleRules.OfType<StyleRule>().Single();
            var bgColor = rule.Style.GetPropertyValue("background-color");
            Assert.False(string.IsNullOrEmpty(bgColor));

            // And the carried value resolves to a real (reddish) color, not black.
            var resolved = Resolve(bgColor);
            Assert.True(resolved.R > resolved.G && resolved.R > resolved.B,
                $"expected reddish, got {resolved.R},{resolved.G},{resolved.B}");
        }

        [Fact]
        public void HslAndHwb_AsSolidColors_DoNotThrow()
        {
            // hsl()/hwb() as solid colors must parse and resolve without recursion/overflow.
            var green = Resolve("hsl(120 100% 50%)");
            Assert.Equal(255, green.G);
            var blue = Resolve("hwb(240 0% 0%)");
            Assert.Equal(255, blue.B);
        }
    }
}
