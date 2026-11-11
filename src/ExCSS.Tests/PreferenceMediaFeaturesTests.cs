namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    // Registration-only coverage for the preference/interaction media features
    // (Media Queries 5 / 4). Evaluation/matching is out of scope; these tests
    // assert only that each feature is recognized as a typed media feature and
    // does not degrade to "not all" (which is what an UnknownMediaFeature does).
    public class PreferenceMediaFeaturesTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("prefers-color-scheme", "dark")]
        [InlineData("prefers-color-scheme", "light")]
        [InlineData("prefers-reduced-motion", "reduce")]
        [InlineData("prefers-reduced-motion", "no-preference")]
        [InlineData("prefers-contrast", "more")]
        [InlineData("prefers-reduced-transparency", "reduce")]
        [InlineData("update", "fast")]
        [InlineData("any-hover", "hover")]
        [InlineData("any-pointer", "fine")]
        public void RegisteredPreferenceFeatureParsesAndRoundTrips(string feature, string value)
        {
            var source = $"@media ({feature}: {value}) {{ h1 {{ color: green }} }}";
            var sheet = ParseStyleSheet(source);

            Assert.Equal(1, sheet.Rules.Length);
            Assert.IsType<MediaRule>(sheet.Rules[0]);
            var media = (MediaRule)sheet.Rules[0];

            // A registered feature round-trips; an unregistered one degrades to "not all".
            Assert.NotEqual("not all", media.ConditionText);
            Assert.Equal($"({feature}: {value})", media.Media.MediaText);
            Assert.Equal(1, media.Media.Length);
            Assert.Equal(1, media.Rules.Length);
        }

        [Fact]
        public void PreferenceFeatureCombinedWithMediaTypeParses()
        {
            var source = "@media screen and (prefers-color-scheme: dark) { h1 { color: green } }";
            var sheet = ParseStyleSheet(source);

            Assert.Equal(1, sheet.Rules.Length);
            var media = Assert.IsType<MediaRule>(sheet.Rules[0]);
            Assert.Equal("screen and (prefers-color-scheme: dark)", media.Media.MediaText);
            Assert.Equal(1, media.Rules.Length);
        }

        [Fact]
        public void UnregisteredFeatureStillDegradesToNotAll()
        {
            // Guards the assertion above: a genuinely unknown feature must degrade,
            // proving the round-trip checks are meaningful.
            var source = "@media (prefers-nonsense: banana) { h1 { color: green } }";
            var sheet = ParseStyleSheet(source);

            Assert.Equal(1, sheet.Rules.Length);
            var media = Assert.IsType<MediaRule>(sheet.Rules[0]);
            Assert.Equal("not all", media.ConditionText);
        }

        [Theory]
        [InlineData("forced-colors", "none")]
        [InlineData("forced-colors", "active")]
        [InlineData("prefers-reduced-data", "no-preference")]
        [InlineData("prefers-reduced-data", "reduce")]
        [InlineData("inverted-colors", "none")]
        [InlineData("inverted-colors", "inverted")]
        [InlineData("dynamic-range", "standard")]
        [InlineData("dynamic-range", "high")]
        [InlineData("video-dynamic-range", "standard")]
        [InlineData("video-dynamic-range", "high")]
        [InlineData("overflow-block", "none")]
        [InlineData("overflow-block", "scroll")]
        [InlineData("overflow-block", "paged")]
        [InlineData("overflow-inline", "none")]
        [InlineData("overflow-inline", "scroll")]
        public void KeywordFeatureAcceptsItsValues(string feature, string value)
        {
            var sheet = ParseStyleSheet($"@media ({feature}: {value}) {{ h1 {{ color: green }} }}");

            var media = Assert.IsType<MediaRule>(sheet.Rules[0]);
            Assert.Equal($"({feature}: {value})", media.Media.MediaText);
            Assert.Equal(1, media.Media.Length);
        }

        [Theory]
        [InlineData("forced-colors", "reduce")]
        [InlineData("forced-colors", "banana")]
        [InlineData("prefers-reduced-data", "active")]
        [InlineData("inverted-colors", "active")]
        [InlineData("dynamic-range", "paged")]
        [InlineData("video-dynamic-range", "none")]
        [InlineData("overflow-block", "inverted")]
        [InlineData("overflow-inline", "paged")]
        public void KeywordFeatureRejectsForeignValues(string feature, string value)
        {
            var sheet = ParseStyleSheet($"@media ({feature}: {value}) {{ h1 {{ color: green }} }}");

            var media = Assert.IsType<MediaRule>(sheet.Rules[0]);
            Assert.Equal("not all", media.ConditionText);
        }
    }
}
