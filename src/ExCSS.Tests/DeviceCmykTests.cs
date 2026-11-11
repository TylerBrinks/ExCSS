using System;
using System.Globalization;
using System.Linq;

namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    /// <summary>
    /// CSS Color 5 <c>device-cmyk()</c> (https://www.w3.org/TR/css-color-5/#device-cmyk): the color is carried in its own C/M/Y/K
    /// components, never collapsed to a guessed sRGB value.
    /// </summary>
    public class DeviceCmykTests : CssConstructionFunctions
    {
        private static Color Resolve(string value) => ColorTestHelpers.Resolve(value);

        [Fact]
        public void FourNumbers_ResolveToCmykComponents()
        {
            var color = Resolve("device-cmyk(0 0.5 1 0.25)");

            Assert.True(color.IsDeviceCmyk);
            Assert.Equal(0f, color.C);
            Assert.Equal(0.5f, color.M, 3);
            Assert.Equal(1f, color.Y, 3);
            Assert.Equal(0.25f, color.K, 3);
            Assert.Equal(255, color.A);
        }

        [Fact]
        public void NoSrgbApproximationIsComputed()
        {
            var color = Resolve("device-cmyk(1 0 0 0)");

            Assert.True(color.IsDeviceCmyk);
            Assert.Equal(0, color.R);
            Assert.Equal(0, color.G);
            Assert.Equal(0, color.B);
        }

        [Fact]
        public void PercentageComponents_NormalizeTo01()
        {
            var color = Resolve("device-cmyk(0% 50% 100% 25%)");

            Assert.Equal(0f, color.C);
            Assert.Equal(0.5f, color.M, 3);
            Assert.Equal(1f, color.Y, 3);
            Assert.Equal(0.25f, color.K, 3);
        }

        [Theory]
        [InlineData("device-cmyk(0 0 0 0 / 0.5)")]
        [InlineData("device-cmyk(0 0 0 0 / 50%)")]
        public void SlashAlpha_IsApplied(string text)
        {
            var color = Resolve(text);

            Assert.True(color.IsDeviceCmyk);
            Assert.InRange((int)color.A, 127, 128);
        }

        [Fact]
        public void NoAlpha_DefaultsToOpaque() => Assert.Equal(255, Resolve("device-cmyk(0.1 0.2 0.3 0.4)").A);

        [Fact]
        public void NoneComponents_ResolveToZero()
        {
            var color = Resolve("device-cmyk(none none none none)");

            Assert.True(color.IsDeviceCmyk);
            Assert.Equal(0f, color.C);
            Assert.Equal(0f, color.M);
            Assert.Equal(0f, color.Y);
            Assert.Equal(0f, color.K);
        }

        [Fact]
        public void OutOfRangeComponents_AreClamped()
        {
            var color = Resolve("device-cmyk(2 -1 1.5 0.5)");

            Assert.Equal(1f, color.C);
            Assert.Equal(0f, color.M);
            Assert.Equal(1f, color.Y);
            Assert.Equal(0.5f, color.K, 3);
        }

        [Theory]
        [InlineData("device-cmyk(0 0 0)")]
        [InlineData("device-cmyk(0 0 0 0 0)")]
        [InlineData("device-cmyk(0 0 0 0 0.5)")]
        [InlineData("device-cmyk()")]
        [InlineData("device-cmyk(red 0 0 0)")]
        [InlineData("device-cmyk(from red c m y k)")]
        public void MalformedForms_AreNotAColor(string text) =>
            Assert.False(ColorTestHelpers.TryResolve(text).HasValue);

        // -- Value semantics of the struct --

        [Fact]
        public void ToString_RoundTripsAsDeviceCmykSyntax()
        {
            var color = Color.FromDeviceCmyk(0f, 0.5f, 1f, 0.25f);

            var text = color.ToString();

            Assert.Equal("device-cmyk(0 0.5 1 0.25)", text);
            Assert.Equal(color, Resolve(text));
        }

        [Fact]
        public void ToString_WithAlpha_UsesTheSlash()
        {
            Assert.Equal("device-cmyk(0 0.5 1 0.25 / 0.5)", Color.FromDeviceCmyk(0f, 0.5f, 1f, 0.25f, 0.5f).ToString());
        }

        [Fact]
        public void ToString_IsCultureInvariant()
        {
            var previous = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = new CultureInfo("de-DE");
                Assert.Equal("device-cmyk(0 0.5 1 0.25)", Color.FromDeviceCmyk(0f, 0.5f, 1f, 0.25f).ToString());
            }
            finally
            {
                CultureInfo.CurrentCulture = previous;
            }
        }

        [Fact]
        public void Equality_ComparesCmykComponents()
        {
            var a = Color.FromDeviceCmyk(0f, 0.5f, 1f, 0.25f);
            var same = Color.FromDeviceCmyk(0f, 0.5f, 1f, 0.25f);
            var different = Color.FromDeviceCmyk(0.1f, 0.5f, 1f, 0.25f);

            Assert.Equal(a, same);
            Assert.NotEqual(a, different);
            Assert.True(a == same);
            Assert.False(a == different);
            Assert.True(a != different);
            Assert.False(a != same);
            Assert.Equal(a.GetHashCode(), same.GetHashCode());
            Assert.NotEqual(a.GetHashCode(), different.GetHashCode());
        }

        [Fact]
        public void ACmykColor_NeverEqualsAnSrgbOne_EvenWithTheSameBytes()
        {
            // An all-zero CMYK color and opaque black share every sRGB byte, but are different colors.
            var cmyk = Color.FromDeviceCmyk(0f, 0f, 0f, 0f);

            Assert.NotEqual(Color.Black, cmyk);
            Assert.True(cmyk != Color.Black);
            Assert.False(Color.Black.IsDeviceCmyk);
        }

        [Fact]
        public void Alpha_IsPartOfEquality()
        {
            Assert.NotEqual(Color.FromDeviceCmyk(0f, 0f, 0f, 1f, 0.5f), Color.FromDeviceCmyk(0f, 0f, 0f, 1f));
        }

        [Fact]
        public void CompareTo_DistinguishesCmykColors()
        {
            var a = (IComparable<Color>)Color.FromDeviceCmyk(0f, 0.5f, 1f, 0.25f);
            var same = Color.FromDeviceCmyk(0f, 0.5f, 1f, 0.25f);
            var different = Color.FromDeviceCmyk(1f, 0.5f, 1f, 0.25f);
            var rgb = Color.FromRgb(255, 0, 0);

            Assert.Equal(0, a.CompareTo(same));
            Assert.NotEqual(0, a.CompareTo(different));
            Assert.True(a.CompareTo(rgb) > 0);
            Assert.True(((IComparable<Color>)rgb).CompareTo(Color.FromDeviceCmyk(0f, 0f, 0f, 0f)) < 0);
        }

        [Fact]
        public void SrgbColors_AreUnaffected()
        {
            var color = new Color(1, 2, 3, 4);

            Assert.False(color.IsDeviceCmyk);
            Assert.Equal(0f, color.C);
            Assert.Equal(new Color(1, 2, 3, 4), color);
            Assert.NotEqual(new Color(1, 2, 3, 5), color);
            Assert.Equal(color.Value, color.GetHashCode());
        }

        [Fact]
        public void StaticMix_OfTwoCmykColors_BlendsComponents_AndWithAnSrgbColorIsNotSupported()
        {
            var cyan = Color.FromDeviceCmyk(1f, 0f, 0f, 0f);
            var magenta = Color.FromDeviceCmyk(0f, 1f, 0f, 0f);

            var mixed = Color.Mix(0.25, cyan, magenta);

            Assert.True(mixed.IsDeviceCmyk);
            Assert.Equal(0.25f, mixed.C, 3);
            Assert.Equal(0.75f, mixed.M, 3);
            Assert.Throws<NotSupportedException>(() => Color.Mix(0.5, cyan, Color.Red));
            Assert.Throws<NotSupportedException>(() => Color.Mix(0.5, Color.Red, cyan));
        }

        // -- color-mix() between two device-cmyk() operands --
        // There is no "cmyk" interpolation space and no profile-free conversion, so two CMYK operands mix in C/M/Y/K themselves;
        // a CMYK operand with an sRGB one stays invalid.

        [Fact]
        public void ColorMix_TwoDeviceCmykOperands_MixesInCmykSpace()
        {
            var color = Resolve("color-mix(in srgb, device-cmyk(0 1 1 0), device-cmyk(1 0 0 0))");

            Assert.True(color.IsDeviceCmyk);
            Assert.Equal(0.5f, color.C, 2);
            Assert.Equal(0.5f, color.M, 2);
            Assert.Equal(0.5f, color.Y, 2);
            Assert.Equal(0f, color.K, 2);
            Assert.Equal(255, color.A);
        }

        [Fact]
        public void ColorMix_TwoDeviceCmykOperands_WeightedPercentage_ShiftsTowardHeavierOperand()
        {
            var color = Resolve("color-mix(in srgb, device-cmyk(0 1 1 0) 25%, device-cmyk(1 0 0 0))");

            Assert.Equal(0.75f, color.C, 2);
            Assert.Equal(0.25f, color.M, 2);
            Assert.Equal(0.25f, color.Y, 2);
        }

        [Fact]
        public void ColorMix_TwoDeviceCmykOperands_SubTotalPercentages_ScaleResultAlpha()
        {
            var color = Resolve("color-mix(in srgb, device-cmyk(0 1 1 0) 30%, device-cmyk(1 0 0 0) 30%)");

            Assert.True(color.IsDeviceCmyk);
            Assert.InRange((int)color.A, 152, 154);
        }

        [Fact]
        public void ColorMix_TwoDeviceCmykOperands_AlphaIsPremultiplied()
        {
            var color = Resolve("color-mix(in srgb, device-cmyk(1 0 0 0 / 0), device-cmyk(0 1 0 0))");

            // The transparent operand contributes no color: the result is the opaque-side magenta at half alpha.
            Assert.Equal(0f, color.C, 3);
            Assert.Equal(1f, color.M, 3);
            Assert.InRange((int)color.A, 127, 128);
        }

        [Fact]
        public void ColorMix_DeclaredSpaceIsIgnored_NotAnError()
        {
            var inOklab = Resolve("color-mix(in oklab, device-cmyk(0 1 1 0), device-cmyk(1 0 0 0))");
            var inSrgb = Resolve("color-mix(in srgb, device-cmyk(0 1 1 0), device-cmyk(1 0 0 0))");

            Assert.Equal(inSrgb, inOklab);
        }

        [Theory]
        [InlineData("color-mix(in srgb, device-cmyk(0 1 1 0), red)")]
        [InlineData("color-mix(in srgb, red, device-cmyk(0 1 1 0))")]
        public void ColorMix_OneDeviceCmykOneSrgbOperand_IsInvalid(string text) =>
            Assert.False(ColorTestHelpers.TryResolve(text).HasValue);

        [Fact]
        public void ACmykColor_IsNotAnOklabOperand()
        {
            Assert.False(ColorTestHelpers.Tokenize("color-mix(in srgb, device-cmyk(0 1 1 0), device-cmyk(1 0 0 0))").TryResolveOklab(out _));
            Assert.False(ColorTestHelpers.Tokenize("device-cmyk(0 1 1 0)").TryResolveMixOperand(out _, out _));
        }

        // -- Parse level --

        [Theory]
        [InlineData("device-cmyk(0 0.5 1 0.25)")]
        [InlineData("device-cmyk(0% 50% 100% 25% / 0.5)")]
        public void DeviceCmyk_IsAcceptedAsAColor_AndRoundTrips(string value)
        {
            var sheet = ParseStyleSheet($"div {{ color: {value}; background-color: {value}; border-color: {value}; }}");
            var style = sheet.StyleRules.OfType<StyleRule>().Single().Style;

            Assert.Equal(value, style.GetPropertyValue("color"));
            Assert.Equal(value, style.GetPropertyValue("background-color"));
            Assert.Equal(value, style.GetPropertyValue("border-top-color"));
        }

        [Fact]
        public void DeviceCmyk_IsAcceptedInShorthands_AndAsLightDarkAndColorMixArms()
        {
            var sheet = ParseStyleSheet(
                "div { background: device-cmyk(0 1 1 0); color: light-dark(device-cmyk(0 0 0 1), device-cmyk(0 0 0 0)); outline: 1px solid color-mix(in srgb, device-cmyk(0 1 1 0), device-cmyk(1 0 0 0)); }");
            var style = sheet.StyleRules.OfType<StyleRule>().Single().Style;

            Assert.Equal("device-cmyk(0 1 1 0)", style.GetPropertyValue("background-color"));
            Assert.Equal("light-dark(device-cmyk(0 0 0 1), device-cmyk(0 0 0 0))", style.GetPropertyValue("color"));
            Assert.False(string.IsNullOrEmpty(style.GetPropertyValue("outline-color")));
        }
    }
}
