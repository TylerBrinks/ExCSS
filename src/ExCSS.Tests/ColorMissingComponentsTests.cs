using System;
using System.Threading.Tasks;

namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    /// <summary>
    /// <c>none</c> (missing) components, powerless hues and gamut-preserving mixing (CSS Color 4 sections 12 and 13): a color is
    /// resolved to the sRGB color a browser draws, but inside a <c>color-mix()</c> it keeps the coordinates it was written with.
    /// The mixed values are the ones Chrome draws.
    /// </summary>
    public class ColorMissingComponentsTests
    {
        private static Color Resolve(string value) => ColorTestHelpers.Resolve(value);

        private static bool TryResolve(string value) => ColorTestHelpers.TryResolve(value).HasValue;

        [Theory]
        [InlineData("rgb(none 0 0)", 0, 0, 0, 255)]
        [InlineData("rgb(none 128 255 / 50%)", 0, 128, 255, 128)]
        [InlineData("rgb(255 none none / none)", 255, 0, 0, 0)]
        [InlineData("lab(50 40 20 / none)", 185, 88, 88, 0)]
        [InlineData("hsl(none 100% 50%)", 255, 0, 0, 255)]
        [InlineData("hwb(none 0% 0% / none)", 255, 0, 0, 0)]
        [InlineData("color(srgb none 0 0)", 0, 0, 0, 255)]
        public void NoneIsZero_AndAMissingAlphaIsTransparent(string text, int r, int g, int b, int a)
        {
            var color = Resolve(text);

            Assert.InRange((int)color.R, r - 3, r + 3);
            Assert.InRange((int)color.G, g - 3, g + 3);
            Assert.InRange((int)color.B, b - 3, b + 3);
            Assert.InRange((int)color.A, a - 1, a + 1);
        }

        [Theory]
        [InlineData("rgb(none, 0, 0)")]
        [InlineData("hsl(none, 100%, 50%)")]
        [InlineData("rgba(0, 0, 0, none)")]
        public void NoneIsNotPartOfTheLegacyCommaSyntax(string text) => Assert.False(TryResolve(text));

        [Fact]
        public void ColorMix_AMissingComponentTakesTheOtherOperandsValue()
        {
            var missing = Resolve("color-mix(in oklab, oklab(none 0.1 0.05), oklab(0.8 -0.1 0.02))");
            var filled = Resolve("color-mix(in oklab, oklab(0.8 0.1 0.05), oklab(0.8 -0.1 0.02))");
            var zero = Resolve("color-mix(in oklab, oklab(0 0.1 0.05), oklab(0.8 -0.1 0.02))");

            Assert.Equal(filled, missing);
            Assert.NotEqual(zero, missing);
        }

        [Fact]
        public void ColorMix_InLab_AMissingLightnessTakesTheOtherOperandsValue()
        {
            var missing = Resolve("color-mix(in lab, lab(none 40 20), lab(80 -20 10))");
            var filled = Resolve("color-mix(in lab, lab(80 40 20), lab(80 -20 10))");

            Assert.Equal(filled, missing);
        }

        [Fact]
        public void ColorMix_AMissingHslComponentLandsOnTheAnalogousComponent()
        {
            // hsl lightness is analogous to oklab lightness, so a missing one is missing in an oklab mix.
            var missing = Resolve("color-mix(in oklab, hsl(120 100% none), lab(80 -20 10))");
            var zero = Resolve("color-mix(in oklab, hsl(120 100% 0%), lab(80 -20 10))");

            Assert.NotEqual(zero, missing);
        }

        [Fact]
        public void ColorMix_AMissingAlphaTakesTheOtherOperandsAlpha()
        {
            var mixed = Resolve("color-mix(in srgb, rgb(255 0 0 / none), rgb(0 0 255 / 0.5))");

            Assert.InRange((int)mixed.A, 126, 129);
            Assert.InRange((int)mixed.R, 126, 129);
            Assert.InRange((int)mixed.B, 126, 129);
        }

        [Fact]
        public void ColorMix_BothMissing_MixesAsZero()
        {
            var mixed = Resolve("color-mix(in srgb, rgb(255 none 0), rgb(0 none 255))");

            Assert.Equal(0, mixed.G);
        }

        [Fact]
        public void ColorMix_InTheWrittenSpace_UsesTheWrittenValues()
        {
            var mixed = Resolve("color-mix(in srgb, rgb(none 0 0), rgb(200 0 0))");

            Assert.InRange((int)mixed.R, 198, 202);
        }

        [Fact]
        public void ColorMix_HwbAndHslMissingComponents()
        {
            var hsl = Resolve("color-mix(in hsl, hsl(none 100% 50%), hsl(120 100% 50%))");
            var hwb = Resolve("color-mix(in hwb, hwb(none 0% 0%), hwb(120 0% 0%))");

            Assert.True(hsl.G > 200 && hsl.R < 60);
            Assert.True(hwb.G > 200 && hwb.R < 60);
        }

        [Fact]
        public void ColorMix_ColorFunctionOperandKeepsItsOutOfGamutCoordinates()
        {
            var mixed = Resolve("color-mix(in oklab, color(display-p3 0 1 0), color(display-p3 0 1 0))");

            Assert.Equal(0, mixed.R);
            Assert.True(mixed.G >= 254);
        }

        [Fact]
        public void ColorMix_ColorFunction_AMissingComponentTakesTheOtherOperandsValue()
        {
            var missing = Resolve("color-mix(in srgb, color(srgb none 0 0), color(srgb 0.8 0 0))");

            Assert.InRange((int)missing.R, 202, 206);
        }

        // -- The values a color-mix() with a wide-gamut or gray operand has, mixed before the clip --

        [Theory]
        [InlineData("color-mix(in srgb, oklch(0.7 0.3 150) 90%, red)", 0, 183, 0)]
        [InlineData("color-mix(in lch, oklch(0.7 0.3 150) 80%, red)", 32, 186, 0)]
        [InlineData("color-mix(in oklab, oklch(0.7 0.3 150) 80%, red)", 69, 183, 0)]
        [InlineData("color-mix(in oklch, red, lab(60 90 60) 90%)", 255, 0, 43)]
        [InlineData("color-mix(in srgb, red, blue)", 128, 0, 128)]
        [InlineData("color-mix(in lab, lab(50 40 20), lab(70 -30 30))", 165, 141, 101)]
        [InlineData("color-mix(in display-p3, oklch(0.7 0.3 150), white)", 0, 231, 146)]
        [InlineData("color-mix(in oklab, color-mix(in xyz, oklch(0.7 0.3 150), blue), white)", 130, 203, 222)]
        // A lightness out of its range is clamped, and a negative chroma is 0, when the color is read, before any mix (CSS Color 4 section 9).
        [InlineData("color-mix(in oklab, lab(150 0 0), black)", 99, 99, 99)]
        [InlineData("color-mix(in srgb, lab(150 0 0), black)", 127, 128, 128)]
        [InlineData("color-mix(in oklab, lab(-30 0 0), white)", 99, 99, 99)]
        [InlineData("color-mix(in oklab, oklab(1.4 0 0), black)", 99, 99, 99)]
        [InlineData("color-mix(in oklab, oklch(0.5 -0.2 100), white)", 174, 174, 174)]
        [InlineData("color-mix(in lab, lab(120 20 20), lab(20 -20 -20))", 145, 145, 145)]
        [InlineData("color-mix(in oklab, oklch(1.5 0.3 150), red)", 219, 194, 75)]
        // A gray has no hue of its own, so the other operand's stands in for it in lch.
        [InlineData("color-mix(in lch, oklch(0.5 0 120), oklch(0.6 0.2 240))", 68, 117, 164)]
        [InlineData("color-mix(in lch, lab(50 0 0), lch(60 40 240))", 97, 138, 161)]
        // Chrome calls a hue missing at a chroma of 0.02 or less, so a near-gray (the grays of a Bootstrap palette) takes the other operand's hue too.
        [InlineData("color-mix(in oklch, #f8f9fa, red)", 255, 158, 142)]
        [InlineData("color-mix(in oklch, #6c757d, red)", 194, 89, 76)]
        [InlineData("color-mix(in oklch, #212529, red)", 143, 46, 36)]
        [InlineData("color-mix(in oklch, #808081, red)", 198, 99, 85)]
        [InlineData("color-mix(in oklch, lch(55 2 300), #f00)", 201, 100, 86)]
        [InlineData("color-mix(in lch, #f8f9fa, #0a7)", 136, 211, 218)]
        [InlineData("color-mix(in lch, oklab(0.6 0 0), red)", 200, 97, 71)]
        // Two grays: both hues are missing, and Chrome mixes them at hue 0.
        [InlineData("color-mix(in oklch, #6c757d, #6c757d)", 125, 112, 115)]
        [InlineData("color-mix(in oklch, #6c757d, #adb5bd)", 156, 143, 147)]
        [InlineData("color-mix(in oklch, #6c757d, #f8f9fa)", 185, 178, 180)]
        // `longer hue` between two equal hues goes half way round (CSS Color 4: -180 < difference <= 0 adds a turn), which a gray's substituted hue always is.
        [InlineData("color-mix(in oklch longer hue, #888, red)", 0, 156, 178)]
        [InlineData("color-mix(in oklch longer hue, red, red)", 0, 169, 219)]
        [InlineData("color-mix(in lch longer hue, #888, #00f)", 57, 115, 0)]
        public void AColorMix_WithAnOperandOutsideTheGamut_IsMixedBeforeTheClip(string text, int r, int g, int b)
        {
            var color = Resolve(text);

            Assert.InRange((int)color.R, r - 2, r + 2);
            Assert.InRange((int)color.G, g - 2, g + 2);
            Assert.InRange((int)color.B, b - 2, b + 2);
        }

        // -- The Oklab coordinates a color is read with --

        private static double[] Oklab(string text)
        {
            Assert.True(ColorTestHelpers.Tokenize(text).TryResolveOklab(out var oklab), text);
            return oklab;
        }

        [Fact]
        public void Oklab_IsReadAsWritten_EvenOutsideTheGamut()
        {
            var v = Oklab("oklab(0.8 0.2 0.1)");

            Assert.Equal(0.8, v[0], 6);
            Assert.Equal(0.2, v[1], 6);
            Assert.Equal(0.1, v[2], 6);
        }

        [Fact]
        public void Oklch_IsReadAsRectangularCoordinates_WithItsFullChroma()
        {
            // oklch(0.7 0.3 150) is far outside sRGB: the clipped color has a smaller chroma than the 0.3 it was written with.
            var v = Oklab("oklch(0.7 0.3 150)");

            Assert.Equal(0.7, v[0], 6);
            Assert.Equal(0.3 * Math.Cos(150 * Math.PI / 180), v[1], 6);
            Assert.Equal(0.3 * Math.Sin(150 * Math.PI / 180), v[2], 6);
        }

        [Fact]
        public void Lab_OutsideTheGamut_KeepsMoreChromaThanItsClippedColor()
        {
            var wide = Oklab("lab(60 90 60)");
            var clipped = Resolve("lab(60 90 60)");

            var clippedOklab = ColorSpaceMath.ToSpace(clipped.R / 255d, clipped.G / 255d, clipped.B / 255d, ColorSpaceMath.Space.Oklab);
            Assert.True(Math.Sqrt(wide[1] * wide[1] + wide[2] * wide[2]) > Math.Sqrt(clippedOklab[1] * clippedOklab[1] + clippedOklab[2] * clippedOklab[2]) + 0.02);
        }

        [Fact]
        public void DisplayP3Color_IsReadOutsideTheSrgbGamut()
        {
            var wide = Oklab("color(display-p3 0 1 0)");
            var clipped = Resolve("color(display-p3 0 1 0)");

            var clippedOklab = ColorSpaceMath.ToSpace(clipped.R / 255d, clipped.G / 255d, clipped.B / 255d, ColorSpaceMath.Space.Oklab);
            Assert.True(Math.Sqrt(wide[1] * wide[1] + wide[2] * wide[2]) > Math.Sqrt(clippedOklab[1] * clippedOklab[1] + clippedOklab[2] * clippedOklab[2]) + 0.02);
        }

        [Fact]
        public void ColorMix_InOklch_MixesChromaAndLightness()
        {
            // red oklch(0.628 0.2577 29.2), blue oklch(0.452 0.3132 264.05): the midpoint has the mean lightness and chroma.
            var v = Oklab("color-mix(in oklch, red, blue)");

            Assert.Equal(0.540, v[0], 2);
            Assert.Equal(0.2855, Math.Sqrt(v[1] * v[1] + v[2] * v[2]), 2);
        }

        [Theory]
        [InlineData("")]
        [InlineData("red")]
        [InlineData("#ff0000")]
        [InlineData("rgb(255 0 0)")]
        [InlineData("hsl(120 100% 50%)")]
        [InlineData("lab(")]
        [InlineData("lab(50 40)")]
        [InlineData("oklch(0.5 0.2)")]
        [InlineData("lch(50 60 purple)")]
        [InlineData("color-mix(in srgb, red)")]
        public void AnythingElse_IsNotAWideColor(string text)
        {
            Assert.False(ColorTestHelpers.Tokenize(text).TryResolveOklab(out var v));
            Assert.Empty(v);
        }

        [Theory]
        [InlineData("lab(150 0 0)", 1, 0, 0)]
        [InlineData("lab(-30 0 0)", 0, 0, 0)]
        [InlineData("oklab(1.4 0 0)", 1, 0, 0)]
        [InlineData("oklch(0.5 -0.2 100)", 0.5, 0, 0)]
        [InlineData("oklch(1.5 0.3 150)", 1, 0.3 * -0.8660254, 0.3 * 0.5)]
        public void AnOutOfRangeLightnessOrChroma_IsClampedWhenTheColorIsRead(string text, double l, double a, double b)
        {
            var v = Oklab(text);

            Assert.Equal(l, v[0], 3);
            Assert.Equal(a, v[1], 3);
            Assert.Equal(b, v[2], 3);
        }

        [Fact]
        public void TryResolveMixOperand_ReportsTheMissingComponents()
        {
            Assert.True(ColorTestHelpers.Tokenize("oklab(none 0.1 0.05 / none)").TryResolveMixOperand(out _, out var missing));
            Assert.Equal(1 | ColorSpaceMath.AlphaMissing, missing);

            // A missing chroma has no analogue in oklab, a missing lightness does.
            Assert.True(ColorTestHelpers.Tokenize("oklch(none none 120)").TryResolveMixOperand(out _, out missing));
            Assert.Equal(1, missing);

            Assert.True(ColorTestHelpers.Tokenize("red").TryResolveMixOperand(out _, out missing));
            Assert.Equal(0, missing);
        }

        [Fact]
        public async Task ANestedColorMix_IsReadOnceAtEachLevel()
        {
            // Every level used to be read twice (once for the color, once for its Oklab), so the work doubled with each level: 26 levels is a
            // few milliseconds read once and minutes read twice per level, so the bound separates them on any machine.
            var text = "blue";
            for (var i = 0; i < 26; i++) text = $"color-mix(in oklab, {text}, red)";

            var read = Task.Run(() =>
            {
                Assert.True(TryResolve(text));
                Assert.True(ColorTestHelpers.Tokenize(text).TryResolveOklab(out _));
            });
            var finished = await Task.WhenAny(read, Task.Delay(TimeSpan.FromSeconds(20)));
            Assert.Same(read, finished);
            await read;
        }

        [Fact]
        public void ColorMix_NestingBeyondTheDepthLimit_IsInvalidRatherThanOverflowingTheStack()
        {
            var text = "blue";
            for (var i = 0; i < 200; i++) text = $"color-mix(in oklab, {text}, red)";

            Assert.False(TryResolve(text));
        }

        // -- Powerless hues --

        [Fact]
        public void OnlyThePolarSpaces_HaveAHueThatCanBePowerless()
        {
            Assert.False(ColorSpaceMath.IsHuePowerless(new[] { 0.5, 0, 0 }, ColorSpaceMath.Space.Oklab));
            Assert.False(ColorSpaceMath.IsHuePowerless(new double[] { 0, 100, 50 }, ColorSpaceMath.Space.Hsl));
            Assert.True(ColorSpaceMath.IsHuePowerless(new double[] { 0, 0, 50 }, ColorSpaceMath.Space.Hsl));
            Assert.True(ColorSpaceMath.IsHuePowerless(new[] { 0, 0.001, 50 }, ColorSpaceMath.Space.Hsl));
            Assert.False(ColorSpaceMath.IsHuePowerless(new[] { 0, 0.01, 50 }, ColorSpaceMath.Space.Hsl));
            Assert.True(ColorSpaceMath.IsHuePowerless(new double[] { 0, 60, 40 }, ColorSpaceMath.Space.Hwb));
            Assert.False(ColorSpaceMath.IsHuePowerless(new double[] { 0, 20, 40 }, ColorSpaceMath.Space.Hwb));

            // Chrome's own limit, found by bisection: 0.02 is missing, 0.021 is not.
            Assert.True(ColorSpaceMath.IsHuePowerless(new[] { 0.5, 0, 0 }, ColorSpaceMath.Space.Oklch));
            Assert.True(ColorSpaceMath.IsHuePowerless(new[] { 0.5, 0.02, 0 }, ColorSpaceMath.Space.Oklch));
            Assert.False(ColorSpaceMath.IsHuePowerless(new[] { 0.5, 0.021, 0 }, ColorSpaceMath.Space.Oklch));
            Assert.True(ColorSpaceMath.IsHuePowerless(new[] { 50, 0.02, 0 }, ColorSpaceMath.Space.Lch));
            Assert.False(ColorSpaceMath.IsHuePowerless(new[] { 50, 0.021, 0 }, ColorSpaceMath.Space.Lch));
        }

        [Fact]
        public void ResolveMissingHues_TheOtherColorsHueStandsInForAGray()
        {
            var gray = new[] { 0.5, 0, 123 };
            var colored = new[] { 0.6, 0.2, 240 };
            ColorSpaceMath.ResolveMissingHues(gray, colored, ColorSpaceMath.Space.Oklch);
            Assert.Equal(240, gray[2]);
            Assert.Equal(240, colored[2]);

            var gray1 = new[] { 0.5, 0, 123 };
            var gray2 = new[] { 0.6, 0.01, 45 };
            ColorSpaceMath.ResolveMissingHues(gray1, gray2, ColorSpaceMath.Space.Oklch);
            Assert.Equal(0, gray1[2]);
            Assert.Equal(0, gray2[2]);

            // A hue written in the mix's own space is never powerless.
            var written = new[] { 0.5, 0, 123 };
            var other = new[] { 0.6, 0.2, 240 };
            ColorSpaceMath.ResolveMissingHues(written, other, ColorSpaceMath.Space.Oklch, hue1IsWritten: true);
            Assert.Equal(123, written[2]);
        }

        // -- Converting to and from Oklab --

        [Theory]
        [InlineData("Srgb")]
        [InlineData("SrgbLinear")]
        [InlineData("DisplayP3")]
        [InlineData("A98Rgb")]
        [InlineData("ProphotoRgb")]
        [InlineData("Rec2020")]
        [InlineData("Lab")]
        [InlineData("Oklab")]
        [InlineData("XyzD65")]
        [InlineData("XyzD50")]
        [InlineData("Lch")]
        [InlineData("Oklch")]
        [InlineData("Hsl")]
        [InlineData("Hwb")]
        public void ToSpaceFromOklab_IsTheInverseOfToSpace_ForAnInGamutColor(string spaceName)
        {
            var space = (ColorSpaceMath.Space) Enum.Parse(typeof(ColorSpaceMath.Space), spaceName);
            var oklab = ColorSpaceMath.ToSpace(0.8, 0.4, 0.2, ColorSpaceMath.Space.Oklab);
            var expected = ColorSpaceMath.ToSpace(0.8, 0.4, 0.2, space);
            var actual = ColorSpaceMath.ToSpaceFromOklab(oklab, space);
            for (var i = 0; i < 3; i++) Assert.Equal(expected[i], actual[i], 3);

            // and ToOklab undoes it
            var back = ColorSpaceMath.ToOklab(expected, space);
            for (var i = 0; i < 3; i++) Assert.Equal(oklab[i], back[i], 3);
        }

        [Fact]
        public void ToSpaceFromOklab_KeepsAnOutOfRangeColorOutOfRange()
        {
            // oklch(0.7 0.3 150) is far outside sRGB: the red channel is negative, not clipped to 0.
            var wide = ColorSpaceMath.ToOklab(new[] { 0.7, 0.3, 150 }, ColorSpaceMath.Space.Oklch);

            Assert.True(ColorSpaceMath.ToSpaceFromOklab(wide, ColorSpaceMath.Space.Srgb)[0] < -0.1);
            Assert.True(ColorSpaceMath.ToSpaceFromOklab(wide, ColorSpaceMath.Space.SrgbLinear)[0] < -0.01);
        }
    }
}
