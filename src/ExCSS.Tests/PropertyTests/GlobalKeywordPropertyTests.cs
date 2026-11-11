using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class GlobalKeywordPropertyTests : CssConstructionFunctions
    {
        [Theory]
        // The five CSS-wide keywords are valid on every property (CSS Cascade 4 7.3; revert-layer is
        // Cascade 5). Before this, only inherit/initial were accepted on properties using OrDefault.
        [InlineData("color", "inherit")]
        [InlineData("color", "initial")]
        [InlineData("color", "unset")]
        [InlineData("color", "revert")]
        [InlineData("color", "revert-layer")]
        [InlineData("display", "unset")]
        [InlineData("display", "revert")]
        [InlineData("text-align", "unset")]
        [InlineData("float", "revert")]
        [InlineData("white-space", "revert-layer")]
        [InlineData("font-weight", "unset")]
        [InlineData("line-height", "revert")]
        public void CssWideKeywordIsAcceptedOnEveryProperty(string property, string keyword)
        {
            var declaration = ParseDeclaration($"{property}: {keyword}");

            Assert.Equal(property, declaration.Name);
            Assert.True(declaration.HasValue);
            Assert.Equal(keyword, declaration.Value);
        }

        [Theory]
        // A misspelled global keyword is still rejected - the change widened the accepted set, not the
        // grammar around it.
        [InlineData("color", "reverts")]
        [InlineData("color", "un-set")]
        [InlineData("display", "revertlayer")]
        public void MisspelledGlobalKeywordIsRejected(string property, string keyword)
        {
            var declaration = ParseDeclaration($"{property}: {keyword}");

            Assert.False(declaration.HasValue);
        }
    }
}
