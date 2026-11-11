using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class HyphenationAndLineLayoutPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("hyphenate-character", "auto")]
        [InlineData("hyphenate-character", "\"-\"")]
        [InlineData("hyphenate-limit-chars", "auto")]
        [InlineData("hyphenate-limit-chars", "5 2 2")]
        [InlineData("hyphenate-limit-chars", "auto 3")]
        [InlineData("hyphenate-limit-lines", "no-limit")]
        [InlineData("hyphenate-limit-lines", "0")]
        [InlineData("hyphenate-limit-lines", "3")]
        [InlineData("hyphenate-limit-last", "none")]
        [InlineData("hyphenate-limit-last", "always")]
        [InlineData("hyphenate-limit-last", "column")]
        [InlineData("hyphenate-limit-last", "page")]
        [InlineData("hyphenate-limit-last", "spread")]
        [InlineData("hyphenate-limit-zone", "0")]
        [InlineData("hyphenate-limit-zone", "10px")]
        [InlineData("hyphenate-limit-zone", "15%")]
        [InlineData("hanging-punctuation", "none")]
        [InlineData("hanging-punctuation", "first")]
        [InlineData("hanging-punctuation", "first force-end last")]
        [InlineData("hanging-punctuation", "last allow-end")]
        [InlineData("line-clamp", "none")]
        [InlineData("line-clamp", "3")]
        [InlineData("block-ellipsis", "none")]
        [InlineData("block-ellipsis", "auto")]
        [InlineData("block-ellipsis", "\"...\"")]
        [InlineData("tab-size", "8")]
        [InlineData("tab-size", "2.5")]
        [InlineData("tab-size", "0")]
        [InlineData("tab-size", "4px")]
        [InlineData("tab-size", "2em")]
        [InlineData("tab-size", "calc(1px + 2px)")]
        [InlineData("initial-letter", "normal")]
        [InlineData("initial-letter", "3")]
        [InlineData("initial-letter", "2.5")]
        [InlineData("initial-letter", "3 2")]
        [InlineData("initial-letter", "3 drop")]
        [InlineData("initial-letter", "3 raise")]
        [InlineData("line-height-step", "none")]
        [InlineData("line-height-step", "0")]
        [InlineData("line-height-step", "12px")]
        [InlineData("line-height-step", "1.5em")]
        [InlineData("block-step-size", "none")]
        [InlineData("block-step-size", "24px")]
        [InlineData("block-step-insert", "margin-box")]
        [InlineData("block-step-insert", "padding-box")]
        [InlineData("block-step-insert", "content-box")]
        [InlineData("block-step-align", "auto")]
        [InlineData("block-step-align", "center")]
        [InlineData("block-step-align", "start")]
        [InlineData("block-step-align", "end")]
        [InlineData("block-step-round", "up")]
        [InlineData("block-step-round", "down")]
        [InlineData("block-step-round", "nearest")]
        [InlineData("text-box-trim", "none")]
        [InlineData("text-box-trim", "trim-start")]
        [InlineData("text-box-trim", "trim-end")]
        [InlineData("text-box-trim", "trim-both")]
        [InlineData("text-box-edge", "auto")]
        [InlineData("text-box-edge", "text")]
        [InlineData("text-box-edge", "cap alphabetic")]
        [InlineData("text-box-edge", "ex text")]
        [InlineData("text-box-edge", "ideographic-ink ideographic")]
        public void LegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("hyphenate-character", "none")]
        [InlineData("hyphenate-character", "auto auto")]
        [InlineData("hyphenate-character", "10px")]
        [InlineData("hyphenate-limit-chars", "banana")]
        [InlineData("hyphenate-limit-chars", "1 2 3 4")]
        [InlineData("hyphenate-limit-chars", "-1")]
        [InlineData("hyphenate-limit-chars", "1.5")]
        [InlineData("hyphenate-limit-lines", "-1")]
        [InlineData("hyphenate-limit-lines", "1.5")]
        [InlineData("hyphenate-limit-lines", "auto")]
        [InlineData("hyphenate-limit-last", "auto")]
        [InlineData("hyphenate-limit-last", "always page")]
        [InlineData("hyphenate-limit-zone", "auto")]
        [InlineData("hyphenate-limit-zone", "10")]
        [InlineData("hanging-punctuation", "first none")]
        [InlineData("hanging-punctuation", "force-end allow-end")]
        [InlineData("hanging-punctuation", "first first")]
        [InlineData("hanging-punctuation", "auto")]
        [InlineData("line-clamp", "0")]
        [InlineData("line-clamp", "-2")]
        [InlineData("line-clamp", "1.5")]
        [InlineData("line-clamp", "auto")]
        [InlineData("block-ellipsis", "3")]
        [InlineData("block-ellipsis", "auto none")]
        [InlineData("tab-size", "-1")]
        [InlineData("tab-size", "-4px")]
        [InlineData("tab-size", "50%")]
        [InlineData("tab-size", "auto")]
        [InlineData("tab-size", "4 8")]
        [InlineData("initial-letter", "0.5")]
        [InlineData("initial-letter", "3 0")]
        [InlineData("initial-letter", "3 1.5")]
        [InlineData("initial-letter", "normal 2")]
        [InlineData("initial-letter", "drop")]
        [InlineData("line-height-step", "-1px")]
        [InlineData("line-height-step", "5")]
        [InlineData("line-height-step", "10%")]
        [InlineData("line-height-step", "auto")]
        [InlineData("block-step-size", "-1px")]
        [InlineData("block-step-size", "auto")]
        [InlineData("block-step-insert", "border-box")]
        [InlineData("block-step-align", "left")]
        [InlineData("block-step-round", "round")]
        [InlineData("text-box-trim", "trim")]
        [InlineData("text-box-trim", "trim-start trim-end")]
        [InlineData("text-box-edge", "alphabetic")]
        [InlineData("text-box-edge", "cap cap")]
        [InlineData("text-box-edge", "text cap")]
        [InlineData("text-box-edge", "auto text")]
        public void IllegalValues(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("hyphenate-character")]
        [InlineData("hyphenate-limit-chars")]
        [InlineData("hyphenate-limit-lines")]
        [InlineData("hyphenate-limit-last")]
        [InlineData("hyphenate-limit-zone")]
        [InlineData("hanging-punctuation")]
        [InlineData("block-ellipsis")]
        [InlineData("tab-size")]
        [InlineData("line-height-step")]
        [InlineData("text-box-edge")]
        [InlineData("text-box")]
        public void InheritedProperties(string name)
        {
            Assert.True(ParseDeclaration($"{name}: inherit").CanBeInherited);
        }

        [Theory]
        [InlineData("line-clamp")]
        [InlineData("initial-letter")]
        [InlineData("block-step")]
        [InlineData("block-step-size")]
        [InlineData("block-step-insert")]
        [InlineData("block-step-align")]
        [InlineData("block-step-round")]
        [InlineData("text-box-trim")]
        public void NonInheritedProperties(string name)
        {
            Assert.False(ParseDeclaration($"{name}: inherit").CanBeInherited);
        }

        [Theory]
        [InlineData("hyphenate-limit-chars")]
        [InlineData("tab-size")]
        [InlineData("line-clamp")]
        [InlineData("initial-letter")]
        [InlineData("block-step-size")]
        [InlineData("text-box-trim")]
        public void GlobalKeywords(string name)
        {
            Assert.True(ParseDeclaration($"{name}: initial").HasValue);
            Assert.True(ParseDeclaration($"{name}: unset").HasValue);
        }

        [Fact]
        public void BlockStepShorthand_ExpandsToLonghands()
        {
            var declaration = ParseDeclarations("block-step: 24px padding-box center down");
            Assert.Equal("24px", declaration.GetPropertyValue("block-step-size"));
            Assert.Equal("padding-box", declaration.GetPropertyValue("block-step-insert"));
            Assert.Equal("center", declaration.GetPropertyValue("block-step-align"));
            Assert.Equal("down", declaration.GetPropertyValue("block-step-round"));
        }

        [Fact]
        public void BlockStepShorthand_AcceptsAnyOrderAndOmittedParts()
        {
            var declaration = ParseDeclarations("block-step: nearest 1em");
            Assert.Equal("1em", declaration.GetPropertyValue("block-step-size"));
            Assert.Equal("nearest", declaration.GetPropertyValue("block-step-round"));
            Assert.Equal("initial", declaration.GetPropertyValue("block-step-insert"));
        }

        [Theory]
        [InlineData("block-step: 10px 20px")]
        [InlineData("block-step: up down")]
        [InlineData("block-step: bogus")]
        public void BlockStepShorthand_IllegalValues(string snippet)
        {
            Assert.False(ParseDeclaration(snippet).HasValue);
        }

        [Fact]
        public void TextBoxShorthand_ExpandsToLonghands()
        {
            var declaration = ParseDeclarations("text-box: trim-both cap alphabetic");
            Assert.Equal("trim-both", declaration.GetPropertyValue("text-box-trim"));
            Assert.Equal("cap alphabetic", declaration.GetPropertyValue("text-box-edge"));
        }

        [Fact]
        public void TextBoxShorthand_EdgeBeforeTrim()
        {
            var declaration = ParseDeclarations("text-box: ex trim-start");
            Assert.Equal("trim-start", declaration.GetPropertyValue("text-box-trim"));
            Assert.Equal("ex", declaration.GetPropertyValue("text-box-edge"));
        }

        [Fact]
        public void TextBoxShorthand_NormalIsNoneAuto()
        {
            var declaration = ParseDeclarations("text-box: normal");
            Assert.Equal("none", declaration.GetPropertyValue("text-box-trim"));
            Assert.Equal("auto", declaration.GetPropertyValue("text-box-edge"));
        }

        [Fact]
        public void TextBoxShorthand_SingleComponent()
        {
            var declaration = ParseDeclarations("text-box: trim-end");
            Assert.Equal("trim-end", declaration.GetPropertyValue("text-box-trim"));
        }

        [Theory]
        [InlineData("text-box: normal trim-both")]
        [InlineData("text-box: trim-start trim-end")]
        [InlineData("text-box: bogus")]
        public void TextBoxShorthand_IllegalValues(string snippet)
        {
            Assert.False(ParseDeclaration(snippet).HasValue);
        }
    }
}
