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
        [InlineData("width: calc(10cqw + 2lh)", "width")]
        [InlineData("margin-left: calc(5svh - 3Q)", "margin-left")]
        [InlineData("width: calc(100vw - 20px)", "width")]
        [InlineData("height: calc(100vh - 2rem)", "height")]
        [InlineData("width: calc(50vmin + 10px)", "width")]
        [InlineData("height: calc(100vmax / 2)", "height")]
        [InlineData("width: calc(40ch + 1em)", "width")]
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

        [Theory]
        // A pure-number, pure-angle or pure-absolute-length expression folds to a plain value.
        [InlineData("flex-grow: calc(1 / 4)", "0.25")]
        [InlineData("flex-grow: calc(0.1 + 0.2)", "0.3")]
        [InlineData("flex-grow: min(0.5, 2)", "0.5")]
        [InlineData("transform: rotate(calc(45deg * 2))", "rotate(90deg)")]
        [InlineData("transform: rotate(calc(1turn / 4))", "rotate(90deg)")]
        [InlineData("transform: rotate(calc(0.5turn + 90deg))", "rotate(270deg)")]
        [InlineData("transform: rotate(calc(200grad - 90deg))", "rotate(90deg)")]
        [InlineData("width: calc(12px + 4px)", "16px")]
        [InlineData("width: calc(10px * 2)", "20px")]
        [InlineData("width: calc(1in + 24px)", "120px")]
        [InlineData("width: calc(2 * 3px)", "6px")]
        [InlineData("width: max(10px, 1in)", "96px")]
        [InlineData("width: clamp(10px, 200px, 50px)", "50px")]
        [InlineData("width: calc(0px + 0px)", "0")]
        public void CalcFoldsPureValues(string declaration, string expected)
        {
            var property = ParseDeclaration(declaration);

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        // Anything needing layout context is re-serialized as normalized text.
        [InlineData("width: calc(100% - 20px)", "calc(100% - 20px)")]
        [InlineData("width: calc(  100%   -   20px  )", "calc(100% - 20px)")]
        [InlineData("width: calc(100%*2)", "calc(100% * 2)")]
        [InlineData("width: calc(100%/2 + 1em)", "calc(100% / 2 + 1em)")]
        [InlineData("width: calc((100% - 20px) / 3)", "calc((100% - 20px) / 3)")]
        [InlineData("width: calc(((100%)) - 20px)", "calc(100% - 20px)")]
        [InlineData("width: calc(100% - (20px - 2em * 3 / 2))", "calc(100% - (20px - 2em * 3 / 2))")]
        [InlineData("width: calc(100% - (1em + 2em))", "calc(100% - (1em + 2em))")]
        [InlineData("width: calc(2 * (1em + 2px))", "calc(2 * (1em + 2px))")]
        [InlineData("width: calc(0px + 1em)", "calc(0px + 1em)")]
        [InlineData("width: calc(-(1em + 2px))", "calc(-(1em + 2px))")]
        [InlineData("width: MIN(100%,  500PX)", "min(100%, 500px)")]
        [InlineData("width: clamp(200px, 50%, 500px)", "clamp(200px, 50%, 500px)")]
        [InlineData("width: calc(min(10px, 5%) + 2px)", "calc(min(10px, 5%) + 2px)")]
        [InlineData("width: calc(10cqw + 2lh)", "calc(10cqw + 2lh)")]
        public void CalcSerializesNormalizedText(string declaration, string expected)
        {
            var property = ParseDeclaration(declaration);

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        // Serialized text re-parses to the same value.
        [InlineData("calc(100%*2)")]
        [InlineData("calc(100% - (1em + 2em))")]
        [InlineData("calc(-(1em + 2px))")]
        [InlineData("calc(0px + 1em)")]
        public void CalcSerializationIsStable(string calc)
        {
            var first = ParseDeclaration("width: " + calc).Value;
            var second = ParseDeclaration("width: " + first).Value;

            Assert.Equal(first, second);
        }

        [Fact]
        public void CalcValueKeepsNodeAndCategory()
        {
            var tokens = new Lexer(new TextSource("calc(1em + 2px)"));
            var function = Assert.IsType<FunctionToken>(tokens.Get());
            var node = CalcParser.Parse(function);
            var value = new CalcValue(node, CalcCategory.Length, new Token[] { function });

            Assert.Same(node, value.Node);
            Assert.Equal(CalcCategory.Length, value.Category);
            Assert.Equal("calc(1em + 2px)", value.CssText);
        }

        [Theory]
        // calc() is accepted where an <integer> is, evaluated and rounded half away from zero
        // (CSS Values 4 10.9).
        [InlineData("z-index: calc(1 + 2)", "3")]
        [InlineData("z-index: calc(7 / 2)", "4")]
        [InlineData("z-index: calc(-7 / 2)", "-4")]
        [InlineData("z-index: calc(2.4)", "2")]
        [InlineData("z-index: calc(2.5)", "3")]
        [InlineData("z-index: calc(-2.5)", "-3")]
        [InlineData("z-index: calc(0.49)", "0")]
        [InlineData("z-index: min(3, 5.5)", "3")]
        [InlineData("z-index: clamp(1, 10, 4)", "4")]
        [InlineData("order: calc(2 * 3)", "6")]
        public void CalcAcceptedInIntegerContext(string declaration, string expected)
        {
            var property = ParseDeclaration(declaration);

            Assert.True(property.HasValue);
            Assert.Equal(expected, property.Value);
        }

        [Theory]
        [InlineData("z-index: calc(1px + 2px)")]    // not a number
        [InlineData("z-index: calc(10% + 2)")]      // mixed type
        [InlineData("z-index: calc(1 / 0)")]        // constant divide by zero
        [InlineData("z-index: calc(3000000000)")]         // outside the integer range
        [InlineData("z-index: calc(1 +)")]          // malformed
        public void CalcRejectedInIntegerContext(string declaration)
        {
            var property = ParseDeclaration(declaration);

            Assert.False(property.HasValue);
        }
    }
}
