namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    public class LengthTests
    {
        [Theory]
        [InlineData("10px", 10f, Length.Unit.Px)]
        [InlineData("1.5em", 1.5f, Length.Unit.Em)]
        [InlineData("-3pt", -3f, Length.Unit.Pt)]
        [InlineData("50%", 50f, Length.Unit.Percent)]
        [InlineData("2.5rem", 2.5f, Length.Unit.Rem)]
        [InlineData("0px", 0f, Length.Unit.Px)]
        public void LengthTryParseAcceptsValueWithUnit(string source, float value, Length.Unit unit)
        {
            Assert.True(Length.TryParse(source, out var result));
            Assert.Equal(value, result.Value);
            Assert.Equal(unit, result.Type);
        }

        [Theory]
        // A zero length may omit its unit (CSS Values 3 5.2).
        [InlineData("0")]
        [InlineData("0.0")]
        [InlineData("-0")]
        public void LengthTryParseAcceptsUnitlessZero(string source)
        {
            Assert.True(Length.TryParse(source, out var result));
            Assert.Equal(0f, result.Value);
            Assert.Equal(Length.Unit.Px, result.Type);
        }

        [Theory]
        // Input that isn't a number at all must be rejected rather than silently becoming zero.
        [InlineData("")]
        [InlineData("auto")]
        [InlineData("inherit")]
        [InlineData("red")]
        [InlineData("px")]
        [InlineData("%")]
        [InlineData("garbage")]
        // A non-zero number still requires a unit.
        [InlineData("5")]
        [InlineData("-2.5")]
        // An unrecognised unit is not a length.
        [InlineData("10foo")]
        public void LengthTryParseRejectsNonLength(string source)
        {
            Assert.False(Length.TryParse(source, out var result));
            Assert.Equal(default, result);
        }

        [Theory]
        [InlineData("10q", Length.Unit.Q)]
        [InlineData("2cap", Length.Unit.Cap)]
        [InlineData("2ic", Length.Unit.Ic)]
        [InlineData("2lh", Length.Unit.Lh)]
        [InlineData("2rlh", Length.Unit.Rlh)]
        [InlineData("2rex", Length.Unit.Rex)]
        [InlineData("2rch", Length.Unit.Rch)]
        [InlineData("2rcap", Length.Unit.Rcap)]
        [InlineData("2ric", Length.Unit.Ric)]
        [InlineData("2vi", Length.Unit.Vi)]
        [InlineData("2vb", Length.Unit.Vb)]
        [InlineData("2svw", Length.Unit.Svw)]
        [InlineData("2svh", Length.Unit.Svh)]
        [InlineData("2svi", Length.Unit.Svi)]
        [InlineData("2svb", Length.Unit.Svb)]
        [InlineData("2svmin", Length.Unit.Svmin)]
        [InlineData("2svmax", Length.Unit.Svmax)]
        [InlineData("2lvw", Length.Unit.Lvw)]
        [InlineData("2lvh", Length.Unit.Lvh)]
        [InlineData("2lvi", Length.Unit.Lvi)]
        [InlineData("2lvb", Length.Unit.Lvb)]
        [InlineData("2lvmin", Length.Unit.Lvmin)]
        [InlineData("2lvmax", Length.Unit.Lvmax)]
        [InlineData("2dvw", Length.Unit.Dvw)]
        [InlineData("2dvh", Length.Unit.Dvh)]
        [InlineData("2dvi", Length.Unit.Dvi)]
        [InlineData("2dvb", Length.Unit.Dvb)]
        [InlineData("2dvmin", Length.Unit.Dvmin)]
        [InlineData("2dvmax", Length.Unit.Dvmax)]
        [InlineData("2cqw", Length.Unit.Cqw)]
        [InlineData("2cqh", Length.Unit.Cqh)]
        [InlineData("2cqi", Length.Unit.Cqi)]
        [InlineData("2cqb", Length.Unit.Cqb)]
        [InlineData("2cqmin", Length.Unit.Cqmin)]
        [InlineData("2cqmax", Length.Unit.Cqmax)]
        public void LengthTryParseAcceptsCssValues4Units(string source, Length.Unit unit)
        {
            Assert.True(Length.TryParse(source, out var result));
            Assert.Equal(unit, result.Type);
            Assert.Equal(source.Substring(source.StartsWith("10") ? 2 : 1), result.UnitString);
        }

        [Theory]
        [InlineData("10PX", 10f, Length.Unit.Px)]
        [InlineData("10Q", 10f, Length.Unit.Q)]
        [InlineData("2CqMin", 2f, Length.Unit.Cqmin)]
        [InlineData("2REM", 2f, Length.Unit.Rem)]
        public void LengthUnitsAreCaseInsensitive(string source, float value, Length.Unit unit)
        {
            Assert.True(Length.TryParse(source, out var result));
            Assert.Equal(value, result.Value);
            Assert.Equal(unit, result.Type);
        }

        [Fact]
        public void AngleUnitsAreCaseInsensitive()
        {
            Assert.True(Angle.TryParse("90DEG", out var result));
            Assert.Equal(Angle.Unit.Deg, result.Type);
            Assert.Equal(Angle.Unit.Turn, Angle.GetUnit("Turn"));
        }

        [Theory]
        [InlineData("10qq")]
        [InlineData("10svx")]
        [InlineData("10cq")]
        [InlineData("10dv")]
        [InlineData("10lvm")]
        [InlineData("10rlhx")]
        public void LengthTryParseRejectsUnknownUnits(string source)
        {
            Assert.False(Length.TryParse(source, out _));
        }

        [Fact]
        public void QuarterMillimetreIsAbsolute()
        {
            var length = new Length(40f, Length.Unit.Q);

            Assert.True(length.IsAbsolute);
            Assert.Equal(new Length(1f, Length.Unit.Cm).ToPixel(), length.ToPixel(), 3);
            Assert.Equal(10f, length.To(Length.Unit.Mm), 3);
        }

        [Theory]
        [InlineData(Length.Unit.Cap)]
        [InlineData(Length.Unit.Lh)]
        [InlineData(Length.Unit.Rlh)]
        [InlineData(Length.Unit.Svw)]
        [InlineData(Length.Unit.Dvmax)]
        [InlineData(Length.Unit.Cqw)]
        [InlineData(Length.Unit.Cqmin)]
        public void RelativeCssValues4UnitsCannotBeConvertedToPixels(Length.Unit unit)
        {
            var length = new Length(1f, unit);

            Assert.True(length.IsRelative);
            Assert.Throws<System.InvalidOperationException>(() => length.ToPixel());
        }

        [Fact]
        public void ExistingUnitNumericValuesAreStable()
        {
            Assert.Equal(1, (int) Length.Unit.Px);
            Assert.Equal(15, (int) Length.Unit.Percent);
            Assert.Equal(16, (int) Length.Unit.Q);
        }
    }
}
