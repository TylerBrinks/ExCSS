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
    }
}
