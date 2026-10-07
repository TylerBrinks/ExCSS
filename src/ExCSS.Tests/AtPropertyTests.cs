using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    public class AtPropertyTests : CssConstructionFunctions
    {
        private static PropertyRule Parse(string source)
        {
            var sheet = ParseStyleSheet(source);
            return (PropertyRule)sheet.Rules.First();
        }

        [Fact]
        public void AtPropertyCapturesNameAndDescriptors()
        {
            var rule = Parse("@property --my-color { syntax: '<color>'; initial-value: red; inherits: false; }");

            Assert.Equal(RuleType.Property, rule.Type);
            Assert.Equal("--my-color", rule.Name);
            Assert.Equal("\"<color>\"", rule.Syntax);
            Assert.Equal("red", rule.InitialValue);
            Assert.Equal("false", rule.Inherits);
        }

        [Fact]
        public void AtPropertyRoundTrips()
        {
            var rule = Parse("@property --gap { syntax: '<length>'; initial-value: 0px; inherits: true; }");

            Assert.Equal("@property --gap { syntax: \"<length>\"; initial-value: 0px; inherits: true }",
                rule.ToCss());
        }

        [Fact]
        public void AtPropertyWithUniversalSyntax()
        {
            var rule = Parse("@property --x { syntax: '*'; inherits: false; }");

            Assert.Equal("\"*\"", rule.Syntax);
            Assert.Equal("false", rule.Inherits);
            Assert.Equal(string.Empty, rule.InitialValue);
        }

        [Fact]
        public void AtPropertyIsExposedAsIPropertyRule()
        {
            var sheet = ParseStyleSheet("@property --c { syntax: '<color>'; inherits: false; }");
            var rule = Assert.IsAssignableFrom<IPropertyRule>(sheet.Rules.First());

            Assert.Equal("--c", rule.Name);
        }

        [Fact]
        public void AtPropertyAmongOtherRules()
        {
            var sheet = ParseStyleSheet(
                ".a { color: red } @property --c { syntax: '<color>'; inherits: false; } .b { color: blue }");

            Assert.Equal(3, sheet.Rules.Length);
            Assert.IsType<StyleRule>(sheet.Rules[0]);
            Assert.IsType<PropertyRule>(sheet.Rules[1]);
            Assert.IsType<StyleRule>(sheet.Rules[2]);
        }
    }
}
