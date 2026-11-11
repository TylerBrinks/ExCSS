using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    /// <summary>
    /// <c>@layer</c> cascade layers (<see href="https://www.w3.org/TR/css-cascade-5/#layering">CSS
    /// Cascade 5</see>): the block form (<c>@layer name { … }</c>, incl. anonymous) and the
    /// order-declaring statement form (<c>@layer a, b, c;</c>). These assert the parsed object model;
    /// cascade precedence between layers is left to the consumer.
    /// </summary>
    public class CascadeLayerTests : CssConstructionFunctions
    {
        [Fact]
        public void NamedBlockLayerCapturesNameAndRules()
        {
            var sheet = ParseStyleSheet("@layer utilities { .a { color: red } }");
            var rule = Assert.IsAssignableFrom<ILayerRule>(sheet.Rules.First());

            Assert.Equal(RuleType.Layer, rule.Type);
            Assert.Equal("utilities", rule.Name);
            var inner = Assert.Single(rule.Rules);
            Assert.Equal(".a", ((StyleRule)inner).SelectorText);
        }

        [Fact]
        public void AnonymousBlockLayerHasEmptyName()
        {
            var sheet = ParseStyleSheet("@layer { .a { color: red } }");
            var rule = Assert.IsAssignableFrom<ILayerRule>(sheet.Rules.First());

            Assert.Equal(string.Empty, rule.Name);
            Assert.Single(rule.Rules);
        }

        [Fact]
        public void DottedLayerNameIsPreserved()
        {
            var sheet = ParseStyleSheet("@layer framework.utilities { .a { color: red } }");
            var rule = Assert.IsAssignableFrom<ILayerRule>(sheet.Rules.First());

            Assert.Equal("framework.utilities", rule.Name);
        }

        [Fact]
        public void StatementLayerCapturesOrderedNames()
        {
            var sheet = ParseStyleSheet("@layer reset, base, utilities;");
            var rule = (LayerStatementRule)sheet.Rules.First();

            Assert.Equal(RuleType.LayerStatement, rule.Type);
            Assert.Equal(new[] { "reset", "base", "utilities" }, rule.Names.ToArray());
        }

        [Fact]
        public void StatementLayerWithDottedNames()
        {
            var sheet = ParseStyleSheet("@layer a.b, c;");
            var rule = (LayerStatementRule)sheet.Rules.First();

            Assert.Equal(new[] { "a.b", "c" }, rule.Names.ToArray());
        }

        [Fact]
        public void BlockLayerRoundTrips()
        {
            var sheet = ParseStyleSheet("@layer utilities { .a { color: red } }");
            Assert.Equal("@layer utilities { .a { color: rgb(255, 0, 0) } }", sheet.Rules.First().ToCss());
        }

        [Fact]
        public void AnonymousBlockLayerRoundTrips()
        {
            var sheet = ParseStyleSheet("@layer { .a { color: red } }");
            Assert.Equal("@layer { .a { color: rgb(255, 0, 0) } }", sheet.Rules.First().ToCss());
        }

        [Fact]
        public void StatementLayerRoundTrips()
        {
            var sheet = ParseStyleSheet("@layer reset, base, utilities;");
            Assert.Equal("@layer reset, base, utilities;", sheet.Rules.First().ToCss());
        }

        [Fact]
        public void LayerAmongOtherRules()
        {
            var sheet = ParseStyleSheet(
                ".a { color: red } @layer utils { .b { color: blue } } .c { color: green }");

            Assert.Equal(3, sheet.Rules.Length);
            Assert.IsAssignableFrom<ILayerRule>(sheet.Rules.Skip(1).First());
        }
    }
}
