namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;
    using System.Linq;

    public class CssContainerTests : CssConstructionFunctions
    {
        [Fact]
        public void SimpleContainer()
        {
            const string source = "@container tall (min-width: 500px) and (min-height: 300px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            Assert.Equal(source, result.StylesheetText.Text);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("@container tall (min-width: 500px) and (min-height: 300px) { h2 { line-height: 1.6 } }", rule.Text);
            Assert.Equal("tall", rule.Name);
            Assert.Equal("(min-width: 500px) and (min-height: 300px)", rule.ConditionText);
            var childRule = rule.Children.OfType<StyleRule>().First();
            Assert.Equal("h2 { line-height: 1.6 }", childRule.ToCss());
        }

        [Fact]
        public void ContainerWithoutName()
        {
            const string source = "@container (min-width: 500px) and (min-height: 300px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            Assert.Equal(source, result.StylesheetText.Text);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("@container (min-width: 500px) and (min-height: 300px) { h2 { line-height: 1.6 } }", rule.Text);
            Assert.Equal(string.Empty, rule.Name);
            Assert.Equal("(min-width: 500px) and (min-height: 300px)", rule.ConditionText);
            var childRule = rule.Children.OfType<StyleRule>().First();
            Assert.Equal("h2 { line-height: 1.6 }", childRule.ToCss());
        }

        [Fact]
        public void ContainerWithoutCondition()
        {
            const string source = "@container tall {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            Assert.Equal(source, result.StylesheetText.Text);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("@container tall { h2 { line-height: 1.6 } }", rule.Text);
            Assert.Equal("tall", rule.Name);
            Assert.Equal(string.Empty, rule.ConditionText);
            var childRule = rule.Children.OfType<StyleRule>().First();
            Assert.Equal("h2 { line-height: 1.6 }", childRule.ToCss());
        }

        [Fact]
        public void ContainerWithComparisonOperators()
        {
            const string source = "@container tall (width < 500px) and (height >= 300px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            Assert.Equal(source, result.StylesheetText.Text);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("@container tall (width < 500px) and (height >= 300px) { h2 { line-height: 1.6 } }", rule.Text);
            Assert.Equal("tall", rule.Name);
            Assert.Equal("(width < 500px) and (height >= 300px)", rule.ConditionText);
            var childRule = rule.Children.OfType<StyleRule>().First();
            Assert.Equal("h2 { line-height: 1.6 }", childRule.ToCss());
        }

        [Fact]
        public void CSSWithTwoContainers()
        {
            const string source = @"li {
  container-type: inline-size;
}

@container (min-width: 45ch) {
  li span {
    color: rgb(255, 0, 0);
    font-size: 2rem !important;
  }
}

@container (min-width: 70ch) {
  li span {
    color: rgb(0, 0, 255);
    font-size: 3rem !important;
  }
}";
            var result = ParseStyleSheet(source);
            Assert.Equal(source, result.StylesheetText.Text);
            Assert.Equal(3, result.Rules.Length);
            var rule1 = result.Rules[0] as StyleRule;
            var rule2 = result.Rules[1] as ContainerRule;
            var rule3 = result.Rules[2] as ContainerRule;
            Assert.NotNull(rule1);
            Assert.NotNull(rule2);
            Assert.NotNull(rule3);
            Assert.Equal("li { container-type: inline-size }", rule1.ToCss());
            Assert.Equal("@container (min-width: 45ch) { li span { color: rgb(255, 0, 0); font-size: 2rem !important } }", rule2.ToCss());
            Assert.Equal("@container (min-width: 70ch) { li span { color: rgb(0, 0, 255); font-size: 3rem !important } }", rule3.ToCss());
        }

        [Fact]
        public void ContainerNotConditionIsNotName()
        {
            const string source = "@container not (min-width: 100px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("", rule.Name);
            Assert.Equal("not (min-width: 100px)", rule.ConditionText);
            Assert.Equal("h2 { line-height: 1.6 }", rule.Children.OfType<StyleRule>().First().ToCss());
        }

        [Fact]
        public void ContainerNamedWithNotCondition()
        {
            const string source = "@container sidebar not (min-width: 100px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("sidebar", rule.Name);
            Assert.Equal("not (min-width: 100px)", rule.ConditionText);
            Assert.Equal("h2 { line-height: 1.6 }", rule.Children.OfType<StyleRule>().First().ToCss());
        }

        [Fact]
        public void ContainerAndIsNotName()
        {
            const string source = "@container and (min-width: 100px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("", rule.Name);
            Assert.Equal("h2 { line-height: 1.6 }", rule.Children.OfType<StyleRule>().First().ToCss());
        }

        [Fact]
        public void ContainerOrIsNotName()
        {
            const string source = "@container or (min-width: 100px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("", rule.Name);
            Assert.Equal("h2 { line-height: 1.6 }", rule.Children.OfType<StyleRule>().First().ToCss());
        }

        [Fact]
        public void ContainerNamedWithAndCondition()
        {
            const string source = "@container card (min-width: 100px) and (max-width: 500px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("card", rule.Name);
            Assert.Equal("(min-width: 100px) and (max-width: 500px)", rule.ConditionText);
            Assert.Equal("h2 { line-height: 1.6 }", rule.Children.OfType<StyleRule>().First().ToCss());
        }

        [Fact]
        public void ContainerNamedWithOrCondition()
        {
            const string source = "@container card (min-width: 100px) or (max-width: 50px) {h2 { line-height: 1.6; } }";
            var result = ParseStyleSheet(source);
            var rule = result.Rules[0] as ContainerRule;
            Assert.NotNull(rule);
            Assert.Equal("card", rule.Name);
            Assert.Equal("h2 { line-height: 1.6 }", rule.Children.OfType<StyleRule>().First().ToCss());
        }
    }
}
