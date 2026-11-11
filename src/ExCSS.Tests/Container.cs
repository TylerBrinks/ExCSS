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

        private static ContainerRule ParseContainer(string source)
        {
            return ParseStyleSheet(source).Rules[0] as ContainerRule;
        }

        [Fact]
        public void StyleQueryCustomProperty()
        {
            var rule = ParseContainer("@container style(--theme: dark) { h2 { color: white } }");
            Assert.NotNull(rule);
            Assert.Equal(string.Empty, rule.Name);
            Assert.Equal("style(--theme: dark)", rule.ConditionText);
            Assert.Equal("@container style(--theme: dark) { h2 { color: rgb(255, 255, 255) } }", rule.Text);
            Assert.Equal(ContainerConditionType.Style, rule.Condition.Type);
            var feature = Assert.Single(rule.Condition.Operands);
            Assert.Equal(ContainerConditionType.Declaration, feature.Type);
            Assert.Equal("--theme", feature.Property);
            Assert.Equal("dark", feature.Value);
            Assert.Equal(1, rule.Children.OfType<StyleRule>().Count());
        }

        [Fact]
        public void StyleQueryRegularPropertyAndRawValue()
        {
            var rule = ParseContainer("@container style(font-size:  calc(1rem + 2px)) { h2 { color: red } }");
            Assert.Equal("style(font-size: calc(1rem + 2px))", rule.ConditionText);
            var feature = rule.Condition.Operands[0];
            Assert.Equal("font-size", feature.Property);
            Assert.Equal("calc(1rem + 2px)", feature.Value);
        }

        [Fact]
        public void StyleQueryNotValidatedAgainstPropertyGrammar()
        {
            var rule = ParseContainer("@container style(color: not-a-color) { h2 { color: red } }");
            Assert.Equal("style(color: not-a-color)", rule.ConditionText);
        }

        [Fact]
        public void StyleQueryBooleanFeature()
        {
            var rule = ParseContainer("@container style(--flag) { h2 { color: red } }");
            Assert.Equal("style(--flag)", rule.ConditionText);
            var feature = rule.Condition.Operands[0];
            Assert.Equal("--flag", feature.Property);
            Assert.Null(feature.Value);
        }

        [Fact]
        public void StyleQueryNot()
        {
            var rule = ParseContainer("@container style(not (--a: b)) { h2 { color: red } }");
            Assert.Equal("style(not (--a: b))", rule.ConditionText);
            var not = rule.Condition.Operands[0];
            Assert.Equal(ContainerConditionType.Not, not.Type);
            Assert.Equal("--a", not.Operands[0].Property);
            Assert.Equal("b", not.Operands[0].Value);
        }

        [Fact]
        public void StyleQueryAndOrInsideFunction()
        {
            var and = ParseContainer("@container style((--a: 1) and (--b: 2)) { h2 { color: red } }");
            Assert.Equal("style((--a: 1) and (--b: 2))", and.ConditionText);
            var group = and.Condition.Operands[0];
            Assert.Equal(ContainerConditionType.And, group.Type);
            Assert.Equal(new[] { "--a", "--b" }, group.Operands.Select(o => o.Property));
            Assert.Equal(new[] { "1", "2" }, group.Operands.Select(o => o.Value));

            var or = ParseContainer("@container style((--a: 1) or (--b: 2) or (--c: 3)) { h2 { color: red } }");
            Assert.Equal("style((--a: 1) or (--b: 2) or (--c: 3))", or.ConditionText);
            Assert.Equal(ContainerConditionType.Or, or.Condition.Operands[0].Type);
            Assert.Equal(3, or.Condition.Operands[0].Operands.Count);
        }

        [Fact]
        public void StyleQueriesJoinedAtTopLevel()
        {
            var rule = ParseContainer("@container style(--a: 1) or style(--b: 2) { h2 { color: red } }");
            Assert.Equal("style(--a: 1) or style(--b: 2)", rule.ConditionText);
            Assert.Equal(ContainerConditionType.Or, rule.Condition.Type);
            Assert.All(rule.Condition.Operands, o => Assert.Equal(ContainerConditionType.Style, o.Type));
            Assert.Equal("@container style(--a: 1) or style(--b: 2) { h2 { color: rgb(255, 0, 0) } }", rule.Text);
        }

        [Fact]
        public void StyleQueryNotAtTopLevel()
        {
            var rule = ParseContainer("@container not style(--a: 1) { h2 { color: red } }");
            Assert.Equal(string.Empty, rule.Name);
            Assert.Equal("not style(--a: 1)", rule.ConditionText);
            Assert.Equal(ContainerConditionType.Not, rule.Condition.Type);
            Assert.Equal(ContainerConditionType.Style, rule.Condition.Operands[0].Type);
        }

        [Fact]
        public void NamedContainerWithStyleAndSizeQuery()
        {
            var rule = ParseContainer("@container card style(--x: 1) and (min-width: 400px) { h2 { color: red } }");
            Assert.Equal("card", rule.Name);
            Assert.Equal("style(--x: 1) and (min-width: 400px)", rule.ConditionText);
            Assert.Equal("@container card style(--x: 1) and (min-width: 400px) { h2 { color: rgb(255, 0, 0) } }", rule.Text);
            Assert.Equal(ContainerConditionType.And, rule.Condition.Type);
            Assert.Equal(ContainerConditionType.Style, rule.Condition.Operands[0].Type);
            Assert.Equal(ContainerConditionType.Size, rule.Condition.Operands[1].Type);
            Assert.Equal("(min-width: 400px)", rule.Condition.Operands[1].Value);
        }

        [Fact]
        public void NamedContainerWithStyleQuery()
        {
            var rule = ParseContainer("@container card style(--x: 1) { h2 { color: red } }");
            Assert.Equal("card", rule.Name);
            Assert.Equal("style(--x: 1)", rule.ConditionText);
        }

        [Fact]
        public void SizeQueryBeforeStyleQuery()
        {
            var rule = ParseContainer("@container (min-width: 400px) or style(--x: 1) { h2 { color: red } }");
            Assert.Equal("(min-width: 400px) or style(--x: 1)", rule.ConditionText);
            Assert.Equal(ContainerConditionType.Or, rule.Condition.Type);
        }

        [Fact]
        public void NestedMixedGroupsKeepParentheses()
        {
            var rule = ParseContainer("@container ((min-width: 400px) and style(--a: 1)) or (not style(--b: 2)) { h2 { color: red } }");
            Assert.Equal("((min-width: 400px) and style(--a: 1)) or (not style(--b: 2))", rule.ConditionText);
            Assert.Equal(ContainerConditionType.Or, rule.Condition.Type);
            Assert.Equal(ContainerConditionType.And, rule.Condition.Operands[0].Type);
            Assert.Equal(ContainerConditionType.Not, rule.Condition.Operands[1].Type);
        }

        [Fact]
        public void StyleConditionRoundTrips()
        {
            var sources = new[]
            {
                "@container style(--theme: dark) { h2 { color: red } }",
                "@container card style(--x: 1) and (min-width: 400px) { h2 { color: red } }",
                "@container style(not (--a: b)) { h2 { color: red } }",
                "@container style(--a: 1) or style(--b: 2) { h2 { color: red } }",
                "@container ((min-width: 400px) and style(--a: 1)) or (not style(--b: 2)) { h2 { color: red } }"
            };

            foreach (var source in sources)
            {
                var first = ParseContainer(source);
                var second = ParseContainer(first.ToCss());
                Assert.Equal(first.ToCss(), second.ToCss());
                Assert.Equal(first.ConditionText, second.ConditionText);
            }
        }

        [Fact]
        public void PlainSizeQueryHasNoCondition()
        {
            var rule = ParseContainer("@container (min-width: 400px) { h2 { color: red } }");
            Assert.Null(rule.Condition);
        }

        [Theory]
        [InlineData("@container style() { h2 { color: red } }")]
        [InlineData("@container style(--a:) { h2 { color: red } }")]
        [InlineData("@container style(--a: 1) and foo { h2 { color: red } }")]
        [InlineData("@container style(--a: 1) and style(--b: 2) or style(--c: 3) { h2 { color: red } }")]
        [InlineData("@container style(--a: 1) and not style(--b: 2) { h2 { color: red } }")]
        [InlineData("@container style((--a: 1) and (--b: 2) or (--c: 3)) { h2 { color: red } }")]
        [InlineData("@container style(not --a: 1) { h2 { color: red } }")]
        [InlineData("@container style(--a: 1) and { h2 { color: red } }")]
        [InlineData("@container style(--a: 1) style(--b: 2) { h2 { color: red } }")]
        public void InvalidStyleConditionMatchesNothing(string source)
        {
            var rule = ParseContainer(source);
            Assert.NotNull(rule);
            Assert.Null(rule.Condition);
            Assert.Equal("not all", rule.ConditionText);
        }
    }
}
