using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class OverflowAxesPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("overflow-x")]
        [InlineData("overflow-y")]
        [InlineData("overflow")]
        public void OverflowKeywordsLegal(string name)
        {
            foreach (var keyword in new[] { "visible", "hidden", "clip", "scroll", "auto" })
            {
                var property = ParseDeclaration($"{name}: {keyword}");

                Assert.Equal(name, property.Name);
                Assert.True(property.HasValue);
                Assert.False(property.IsInherited);
                Assert.Equal(keyword, property.Value);
            }
        }

        [Theory]
        [InlineData("overflow-x: banana")]
        [InlineData("overflow-y: 10px")]
        [InlineData("overflow-x: hidden scroll")]
        public void OverflowAxisIllegal(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.False(property.HasValue);
        }

        [Fact]
        public void OverflowAxisIsNotInheritedAndAcceptsGlobalKeywords()
        {
            var property = ParseDeclaration("overflow-x: revert-layer");

            Assert.True(property.HasValue);
            Assert.False(property.CanBeInherited);
            Assert.Equal("revert-layer", property.Value);
        }

        [Fact]
        public void OverflowAxesExposedOnStyleDeclaration()
        {
            var style = ParseDeclarations("overflow-x: hidden; overflow-y: clip");

            Assert.Equal("hidden", style.OverflowX);
            Assert.Equal("clip", style.OverflowY);
        }

        [Fact]
        public void TextOverflowKeywordsLegal()
        {
            foreach (var keyword in new[] { "clip", "ellipsis" })
            {
                var property = ParseDeclaration($"text-overflow: {keyword}");

                Assert.Equal("text-overflow", property.Name);
                Assert.True(property.HasValue);
                Assert.False(property.IsInherited);
                Assert.False(property.CanBeInherited);
                Assert.Equal(keyword, property.Value);
            }
        }

        [Fact]
        public void TextOverflowStringLegal()
        {
            var property = ParseDeclaration("text-overflow: '[..]'");

            Assert.True(property.HasValue);
            Assert.Equal("\"[..]\"", property.Value);
        }

        [Theory]
        [InlineData("text-overflow: banana")]
        [InlineData("text-overflow: clip ellipsis")]
        [InlineData("text-overflow: 12px")]
        public void TextOverflowIllegal(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("text-overflow", "inherit")]
        [InlineData("text-overflow", "unset")]
        public void TextOverflowGlobalKeywords(string name, string keyword)
        {
            var property = ParseDeclaration($"{name}: {keyword}");

            Assert.True(property.HasValue);
            Assert.Equal(keyword, property.Value);
        }
    }

    public class EnumValueGapTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("overflow-wrap", "anywhere")]
        [InlineData("text-align-last", "match-parent")]
        [InlineData("text-align", "start")]
        [InlineData("text-align", "end")]
        [InlineData("text-align", "match-parent")]
        [InlineData("display", "contents")]
        [InlineData("display", "flow-root")]
        [InlineData("clear", "inline-start")]
        [InlineData("clear", "inline-end")]
        [InlineData("float", "inline-start")]
        [InlineData("float", "inline-end")]
        [InlineData("break-before", "recto")]
        [InlineData("break-before", "verso")]
        [InlineData("break-after", "recto")]
        [InlineData("break-after", "verso")]
        public void NewKeywordLegal(string name, string keyword)
        {
            var property = ParseDeclaration($"{name}: {keyword}");

            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.Equal(keyword, property.Value);
        }

        [Theory]
        [InlineData("break-inside", "recto")]
        [InlineData("float", "inline")]
        [InlineData("clear", "start")]
        public void KeywordStillIllegal(string name, string keyword)
        {
            var property = ParseDeclaration($"{name}: {keyword}");

            Assert.False(property.HasValue);
        }
    }

    public class AllPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("initial")]
        [InlineData("inherit")]
        [InlineData("unset")]
        [InlineData("revert")]
        [InlineData("revert-layer")]
        public void AllAcceptsCssWideKeywords(string keyword)
        {
            var property = ParseDeclaration($"all: {keyword}");

            Assert.Equal("all", property.Name);
            Assert.True(property.HasValue);
            Assert.Equal(keyword, property.Value);
        }

        [Theory]
        [InlineData("all: banana")]
        [InlineData("all: none")]
        [InlineData("all: inherit initial")]
        [InlineData("all: 10px")]
        public void AllRejectsEverythingElse(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.False(property.HasValue);
        }

        [Fact]
        public void AllExpandsToLonghandsExceptDirectionAndUnicodeBidi()
        {
            var style = ParseDeclarations("all: inherit");

            Assert.Equal("inherit", style.Color);
            Assert.Equal("inherit", style.Display);
            Assert.Equal("inherit", style.OverflowX);
            Assert.Equal("inherit", style.TextOverflow);
            Assert.Equal("inherit", style.MarginTop);
            Assert.Equal("", style.Direction);
            Assert.Equal("", style.UnicodeBidirectional);
        }

        [Theory]
        [InlineData("initial")]
        [InlineData("inherit")]
        [InlineData("unset")]
        [InlineData("revert")]
        [InlineData("revert-layer")]
        public void AllGivesEveryExpandedLonghandTheKeyword(string keyword)
        {
            var style = ParseDeclarations($"all: {keyword}");

            Assert.True(style.Length > 100);
            foreach (var declaration in style)
                Assert.True(keyword == declaration.Value, $"{declaration.Name}: {declaration.Value}");
        }

        [Fact]
        public void AllDoesNotTouchCustomProperties()
        {
            var style = ParseDeclarations("--x: 1; all: initial");

            Assert.Equal("1", style.GetPropertyValue("--x"));
        }

        [Fact]
        public void AllExpansionContainsNoShorthandsOrAliases()
        {
            var style = ParseDeclarations("all: unset");

            foreach (var declaration in style)
            {
                var property = (Property)declaration;
                Assert.False(property.IsShorthand, property.Name);
                Assert.NotEqual("word-wrap", property.Name);
                Assert.NotEqual("direction", property.Name);
                Assert.NotEqual("unicode-bidi", property.Name);
            }
        }

        [Fact]
        public void LaterDeclarationOverridesAll()
        {
            var style = ParseDeclarations("all: initial; color: red");

            Assert.Equal("rgb(255, 0, 0)", style.Color);
            Assert.Equal("initial", style.Display);
        }

        [Fact]
        public void AllWithVarIsKeptWhole()
        {
            var style = ParseDeclarations("all: var(--reset)");

            Assert.Equal("var(--reset)", style.GetPropertyValue("all"));
        }

        [Fact]
        public void AllPropertyIsImportant()
        {
            var style = ParseDeclarations("all: initial !important");

            Assert.Equal("important", style.GetPropertyPriority("color"));
        }
    }
}
