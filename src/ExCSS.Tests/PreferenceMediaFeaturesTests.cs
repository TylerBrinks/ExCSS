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
    }
}
