namespace ExCSS.Tests
{
    using System;
    using System.Linq;
    using Xunit;

    public class StylesheetRelativeUrlTests
    {
        private static readonly Uri Sheet = new("https://example.org/wp-content/themes/t/style.css?2022");

        private static string BackgroundOf(string css, Uri urlBase)
        {
            var sheet = new StylesheetParser { UrlBase = urlBase }.Parse(css);
            return sheet.StyleRules.Single().Style.GetPropertyValue("background-image");
        }

        [Theory]
        [InlineData("a { background-image: url('images/x.svg') }", "https://example.org/wp-content/themes/t/images/x.svg")]
        [InlineData("a { background-image: url(images/x.svg) }", "https://example.org/wp-content/themes/t/images/x.svg")]
        [InlineData("a { background-image: url(../img/x.png) }", "https://example.org/wp-content/themes/img/x.png")]
        [InlineData("a { background-image: url(/root/x.png) }", "https://example.org/root/x.png")]
        public void RelativeUrlIsResolvedAgainstTheStylesheet(string css, string expected)
        {
            Assert.Contains(expected, BackgroundOf(css, Sheet));
        }

        [Theory]
        [InlineData("a { background-image: url(data:image/png;base64,AAAA) }", "data:image/png;base64,AAAA")]
        [InlineData("a { background-image: url(https://cdn.example/x.png) }", "https://cdn.example/x.png")]
        [InlineData("a { background-image: url('#frag') }", "#frag")]
        public void NonRelativeUrlIsLeftAlone(string css, string expected)
        {
            Assert.Contains(expected, BackgroundOf(css, Sheet));
        }

        [Fact]
        public void EmptyUrlIsLeftAlone()
        {
            Assert.Contains("url(\"\")", BackgroundOf("a { background-image: url('') }", Sheet));
        }

        [Fact]
        public void UrlIsUnchangedByDefault()
        {
            var value = BackgroundOf("a { background-image: url(images/x.svg) }", null);

            Assert.Contains("images/x.svg", value);
            Assert.DoesNotContain("://", value);
        }

        [Fact]
        public void StylesheetExposesBaseUri()
        {
            Assert.Equal(Sheet, new StylesheetParser { UrlBase = Sheet }.Parse("a{}").BaseUri);
            Assert.Null(new StylesheetParser().Parse("a{}").BaseUri);
        }
    }
}
