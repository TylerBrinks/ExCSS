using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class AspectRatioPropertyTests : CssConstructionFunctions
    {
        [Theory]
        // aspect-ratio: [ auto || <ratio> ] (CSS Sizing 4 4.1). The serialized form drops whitespace
        // around the slash, matching how every other value round-trips through TokenValue.
        [InlineData("auto", "auto")]
        [InlineData("16 / 9", "16/9")]
        [InlineData("16/9", "16/9")]
        [InlineData("1.5", "1.5")]
        [InlineData("2", "2")]
        [InlineData("auto 16 / 9", "auto 16/9")]
        [InlineData("16 / 9 auto", "16/9 auto")]
        [InlineData("0.5 / 0.25", "0.5/0.25")]
        public void AspectRatioLegalValues(string value, string expected)
        {
            var property = ParseDeclaration($"aspect-ratio: {value}");

            Assert.Equal("aspect-ratio", property.Name);
            Assert.IsType<AspectRatioProperty>(property);
            var concrete = (AspectRatioProperty)property;
            Assert.True(concrete.HasValue);
            Assert.Equal(expected, concrete.Value);
        }

        [Theory]
        [InlineData("aspect-ratio: none")]
        [InlineData("aspect-ratio: 16 /")]
        [InlineData("aspect-ratio: / 9")]
        [InlineData("aspect-ratio: 16 / 9 / 3")]
        [InlineData("aspect-ratio: -16 / 9")]
        [InlineData("aspect-ratio: auto auto")]
        [InlineData("aspect-ratio: 16 9")]
        [InlineData("aspect-ratio: 16px / 9")]
        public void AspectRatioIllegalValues(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.IsType<AspectRatioProperty>(property);
            Assert.False(property.HasValue);
        }

        [Theory]
        [InlineData("aspect-ratio: inherit")]
        [InlineData("aspect-ratio: initial")]
        public void AspectRatioGlobalKeywords(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.True(property.HasValue);
        }
    }
}
