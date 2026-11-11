namespace ExCSS.Tests
{
    using Xunit;
    using System.Linq;
    
    public class CssKeyframeRuleTests : CssConstructionFunctions
    {
        [Fact]
        public void KeyframeRuleWithFromAndMarginLeft()
        {
            var rule = ParseKeyframeRule(@"  from {
    margin-left: 0px;
  }");
            Assert.NotNull(rule);
            Assert.Equal("0%", rule.KeyText);
            Assert.Single(rule.Key.Stops);
            Assert.Single(rule.Style.Declarations);
            Assert.Equal("margin-left", rule.Style.Declarations.First().Name);
        }

        [Fact]
        public void KeyframeRuleWith50PercentAndMarginLeftOpacity()
        {
            var rule = ParseKeyframeRule(@"  50% {
    margin-left: 110px;
    opacity: 1;
  }");
            Assert.NotNull(rule);
            Assert.Equal("50%", rule.KeyText);
            Assert.Single(rule.Key.Stops);
            Assert.Equal(2, rule.Style.Declarations.Count());
            Assert.Equal("margin-left", rule.Style.Declarations.Skip(0).First().Name);
            Assert.Equal("opacity", rule.Style.Declarations.Skip(1).First().Name);
        }

        [Fact]
        public void KeyframeRuleWithToAndMarginLeft()
        {
            var rule = ParseKeyframeRule(@"  to {
    margin-left: 200px;
  }");
            Assert.NotNull(rule);
            Assert.Equal("100%", rule.KeyText);
            Assert.Single(rule.Key.Stops);
            Assert.Single(rule.Style.Declarations);
            Assert.Equal("margin-left", rule.Style.Declarations.First().Name);
        }

        [Fact]
        public void KeyframeRuleWithFromTo255075PercentAndPaddingTopPaddingLeftColor()
        {
            var rule = ParseKeyframeRule(@"  from,to, 25%, 50%,75%{
    padding-top: 200px;
    padding-left: 2em;
    color: red
  }");
            Assert.NotNull(rule);
            Assert.Equal("0%, 100%, 25%, 50%, 75%", rule.KeyText);
            Assert.Equal(5, rule.Key.Stops.Count());
            Assert.Equal(3, rule.Style.Declarations.Count());
            Assert.Equal("padding-top", rule.Style.Declarations.Skip(0).First().Name);
            Assert.Equal("padding-left", rule.Style.Declarations.Skip(1).First().Name);
            Assert.Equal("color", rule.Style.Declarations.Skip(2).First().Name);
        }

        [Fact]
        public void KeyframeRuleWith0AndNoDeclarations()
        {
            var rule = ParseKeyframeRule(@"  0% { }");
            Assert.NotNull(rule);
            Assert.Equal("0%", rule.KeyText);
            Assert.Single(rule.Key.Stops);
            Assert.Empty(rule.Style.Declarations);
        }

        [Fact]
        public void KeyframesQuotedNameIsParsedAndRequoted()
        {
            var sheet = ParseStyleSheet(@"@keyframes ""my anim"" { from { opacity: 0 } to { opacity: 1 } }");
            var rule = Assert.IsAssignableFrom<IKeyframesRule>(Assert.Single(sheet.Rules));
            Assert.Equal("my anim", rule.Name);
            Assert.Equal(2, rule.Rules.Length);
            Assert.StartsWith(@"@keyframes ""my anim"" ", sheet.ToCss());
        }

        [Fact]
        public void KeyframesQuotedReservedWordIsAllowed()
        {
            var sheet = ParseStyleSheet(@"@keyframes 'none' { from { opacity: 0 } }");
            var rule = Assert.IsAssignableFrom<IKeyframesRule>(Assert.Single(sheet.Rules));
            Assert.Equal("none", rule.Name);
        }

        [Theory]
        [InlineData("none")]
        [InlineData("initial")]
        [InlineData("INHERIT")]
        [InlineData("unset")]
        [InlineData("default")]
        public void KeyframesReservedIdentNameIsRejected(string name)
        {
            var sheet = ParseStyleSheet("@keyframes " + name + " { from { opacity: 0 } }");
            Assert.Empty(sheet.Rules);
        }

        [Fact]
        public void KeyframesWithoutANameIsRejected()
        {
            var sheet = ParseStyleSheet("@keyframes { from { opacity: 0 } }");
            Assert.Empty(sheet.Rules);
        }

        [Fact]
        public void KeyframesUnquotedNameStaysUnquoted()
        {
            var sheet = ParseStyleSheet("@keyframes fade { from { opacity: 0 } }");
            Assert.StartsWith("@keyframes fade ", sheet.ToCss());
        }

        [Fact]
        public void WebkitKeyframesAliasIsParsedWhenEnabled()
        {
            var sheet = ParseStyleSheet(
                "@-webkit-keyframes fade { from { opacity: 0 } to { opacity: 1 } }",
                enableVendorPrefixAliases: true);
            var rule = Assert.IsAssignableFrom<IKeyframesRule>(Assert.Single(sheet.Rules));
            Assert.Equal("fade", rule.Name);
            Assert.Equal(2, rule.Rules.Length);
            Assert.StartsWith("@keyframes fade ", sheet.ToCss());
        }

        [Fact]
        public void WebkitKeyframesAliasIsIgnoredByDefault()
        {
            var sheet = ParseStyleSheet("@-webkit-keyframes fade { from { opacity: 0 } }");
            Assert.Empty(sheet.Rules);

            var withUnknown = ParseStyleSheet("@-webkit-keyframes fade { from { opacity: 0 } }", includeUnknownRules: true);
            Assert.IsNotAssignableFrom<IKeyframesRule>(Assert.Single(withUnknown.Rules));
        }
    }
}
