using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class CustomPropertyTests : CssConstructionFunctions
    {
        private static StyleDeclaration Style(string declarations)
        {
            var sheet = ParseStyleSheet(".a { " + declarations + " }");
            return ((StyleRule)sheet.Rules[0]).Style;
        }

        [Theory]
        // A custom property (CSS Variables 1 2) is a first-class declaration - no unknown-declaration flag
        // needed - and its value is preserved verbatim.
        [InlineData("--my-color: red", "--my-color", "red")]
        [InlineData("--gap: 10px", "--gap", "10px")]
        [InlineData("--ratio: 16 / 9", "--ratio", "16/9")]
        [InlineData("--x: anything you like !", "--x", "anything you like !")]
        public void CustomPropertyIsStored(string declaration, string name, string value)
        {
            var style = Style(declaration);

            Assert.Equal(1, style.Length);
            Assert.Equal(name, style[0]);
            Assert.Equal(value, style[name]);
        }

        [Fact]
        public void CustomPropertyProducesCustomPropertyType()
        {
            var property = ParseDeclaration("--main: blue");

            Assert.IsType<CustomProperty>(property);
            Assert.Equal("--main", property.Name);
            Assert.True(property.HasValue);
        }

        [Theory]
        // "--" alone (fewer than one code point after the dashes) is reserved, not a custom property.
        [InlineData("-")]
        [InlineData("--")]
        public void NotACustomPropertyName(string name)
        {
            Assert.False(PropertyFactory.IsCustomPropertyName(name));
        }

        [Fact]
        public void CustomPropertyNameCheck()
        {
            Assert.True(PropertyFactory.IsCustomPropertyName("--x"));
            Assert.True(PropertyFactory.IsCustomPropertyName("--main-color"));
        }

        [Theory]
        // A var() reference is accepted on any property and kept verbatim; the property's own grammar is
        // not applied, since the referenced value is only known at cascade time (CSS Variables 1 3).
        [InlineData("color: var(--c)", "color", "var(--c)")]
        [InlineData("color: var(--c, blue)", "color", "var(--c, blue)")]
        [InlineData("width: calc(var(--w) + 1px)", "width", "calc(var(--w) + 1px)")]
        public void VarReferenceIsAcceptedOnLonghand(string declaration, string name, string value)
        {
            var style = Style(declaration);

            Assert.Equal(value, style[name]);
        }

        [Theory]
        // A shorthand whose value contains var() is kept whole - not sliced into longhands - because the
        // reference can only be split after substitution (CSS Variables 1 3.2).
        [InlineData("margin: var(--gap) 5px", "margin", "var(--gap) 5px")]
        [InlineData("margin: var(--all)", "margin", "var(--all)")]
        [InlineData("border: 1px solid var(--c)", "border", "1px solid var(--c)")]
        public void VarBearingShorthandIsKeptWhole(string declaration, string name, string value)
        {
            var style = Style(declaration);

            Assert.Equal(1, style.Length);
            Assert.Equal(name, style[0]);
            Assert.Equal(value, style[name]);
            // The longhands are not populated at parse time.
            Assert.Equal(string.Empty, style["margin-left"]);
        }

        [Fact]
        public void CustomPropertyDashDashLexesAsOneIdent()
        {
            // Regression for the lexer: "--foo" must be a single ident, not '-' delimiter + "-foo".
            var lexer = new Lexer(new TextSource("--foo")) { IsInValue = false };
            var token = lexer.Get();

            Assert.Equal(TokenType.Ident, token.Type);
            Assert.Equal("--foo", token.Data);
            Assert.Equal(TokenType.EndOfFile, lexer.Get().Type);
        }
    }
}
