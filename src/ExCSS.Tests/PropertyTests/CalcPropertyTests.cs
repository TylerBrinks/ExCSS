using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class CalcPropertyTests : CssConstructionFunctions
    {
        [Theory]
        // calc()/min()/max()/clamp() (CSS Values 4 10) are accepted wherever a compatible numeric value is,
        // and the authored text is preserved.
        [InlineData("width: calc(100% - 20px)", "width")]
        [InlineData("width: calc(50% + 2em)", "width")]
        [InlineData("width: calc((100% - 20px) / 3)", "width")]
        [InlineData("margin-top: calc(10px * 2)", "margin-top")]
        [InlineData("width: min(100%, 500px)", "width")]
        [InlineData("width: max(50%, 200px)", "width")]
        [InlineData("width: clamp(200px, 50%, 500px)", "width")]
        [InlineData("width: calc(min(10px, 5%) + 2px)", "width")]
        public void CalcAcceptedInLengthContext(string declaration, string name)
        {
            var property = ParseDeclaration(declaration);

            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
        }

        [Fact]
        public void CalcPreservesAuthoredText()
        {
            var property = ParseDeclaration("width: calc(100% - 20px)");

            Assert.Equal("calc(100% - 20px)", property.Value);
        }

        [Theory]
        // A calc() also works in the angle context.
        [InlineData("transform: rotate(calc(45deg * 2))")]
        [InlineData("transform: rotate(calc(1turn / 4))")]
        public void CalcAcceptedInAngleContext(string declaration)
        {
            var property = ParseDeclaration(declaration);

            Assert.True(property.HasValue);
        }

        [Theory]
        // Type-checking (CSS Values 4 10.10): dimensionally-inconsistent or malformed expressions are
        // rejected.
        [InlineData("width: calc(1px + 1s)")]      // length + time
        [InlineData("width: calc(10px * 20px)")]   // length * length
        [InlineData("width: calc(100% / 0)")]      // divide by a constant zero
        [InlineData("width: calc(1px +1px)")]      // "+" needs whitespace on both sides
        [InlineData("width: calc(5)")]             // bare number is not a length
        [InlineData("width: calc()")]              // empty
        [InlineData("width: clamp(1px, 2px)")]     // clamp() takes exactly three arguments
        [InlineData("width: calc(1px + )")]        // missing operand
        public void CalcRejectsInvalidExpressions(string declaration)
        {
            var property = ParseDeclaration(declaration);

            Assert.False(property.HasValue);
        }
    }
}
