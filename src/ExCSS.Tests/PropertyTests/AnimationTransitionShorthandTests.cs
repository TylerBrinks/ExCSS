using System.Linq;
using Xunit;

namespace ExCSS.Tests
{
    public class AnimationTransitionShorthandTests
    {
        private static string[] Expand(string declaration, params string[] longhands)
        {
            var sheet = new StylesheetParser().Parse("#a { " + declaration + "; }");
            var style = sheet.StyleRules.Single().Style;

            return longhands.Select(name => style.First(p => p.Name == name).Value).ToArray();
        }

        private static readonly string[] AnimationLonghands =
        {
            "animation-name", "animation-duration", "animation-timing-function", "animation-delay",
            "animation-iteration-count", "animation-direction", "animation-fill-mode", "animation-play-state"
        };

        private static readonly string[] TransitionLonghands =
        {
            "transition-property", "transition-duration", "transition-timing-function", "transition-delay"
        };

        [Fact]
        public void AnimationEveryComponentLandsInItsOwnLonghand() =>
            Assert.Equal(
                new[] { "fadeIn", "10s", "ease-in-out", "2s", "infinite", "alternate", "forwards", "paused" },
                Expand("animation: fadeIn 10s ease-in-out 2s infinite alternate forwards paused", AnimationLonghands));

        [Theory]
        [InlineData("animation: fadeIn 10s", "fadeIn", "10s", "initial")]
        [InlineData("animation: 10s fadeIn", "fadeIn", "10s", "initial")]
        [InlineData("animation: fadeIn 10s 2s", "fadeIn", "10s", "2s")]
        [InlineData("animation: 2s fadeIn", "fadeIn", "2s", "initial")]
        [InlineData("animation: 1s 2s slide", "slide", "1s", "2s")]
        [InlineData("animation: slide 2s ease 1s", "slide", "2s", "1s")]
        [InlineData("animation: slide ease 2s 1s", "slide", "2s", "1s")]
        public void AnimationNameDurationAndDelayAreToldApart(string declaration, string name, string duration, string delay) =>
            Assert.Equal(new[] { name, duration, delay }, Expand(declaration, "animation-name", "animation-duration", "animation-delay"));

        [Fact]
        public void AnimationWithoutATimeStillExpands() =>
            Assert.Equal("fadeIn", Expand("animation: fadeIn", "animation-name")[0]);

        [Fact]
        public void AnimationListKeepsOneEntryPerAnimation() =>
            Assert.Equal(new[] { "a, b", "1s, 2s" }, Expand("animation: a 1s, b 2s", "animation-name", "animation-duration"));

        [Fact]
        public void AnimationKeywordsAreNotNames() =>
            Assert.Equal(new[] { "slide", "infinite", "reverse", "forwards" },
                Expand("animation: infinite reverse forwards slide", "animation-name", "animation-iteration-count", "animation-direction", "animation-fill-mode"));

        [Fact]
        public void AnimationNoneDoesNotDisturbTheOtherComponents() =>
            // 'none' is both an animation-name and an animation-fill-mode keyword; either reading is the
            // initial value of the other longhand, so only the remaining components are observable.
            Assert.Equal(new[] { "1s", "none" }, Expand("animation: none 1s", "animation-duration", "animation-fill-mode"));

        [Fact]
        public void AnimationGlobalKeywordIsNotAName() =>
            Assert.DoesNotContain(
                new StylesheetParser().Parse("#a { animation: 1s inherit; }").StyleRules.Single().Style,
                p => p.Name == "animation-name" && p.Value == "inherit");

        [Fact]
        public void AnimationQuotedNameRoundTrips() =>
            Assert.Equal("\"my anim\"", Expand("animation: 1s \"my anim\"", "animation-name")[0]);

        [Fact]
        public void TransitionEveryComponentLandsInItsOwnLonghand() =>
            Assert.Equal(new[] { "opacity", "300ms", "ease-in", "1s" },
                Expand("transition: opacity 300ms ease-in 1s", TransitionLonghands));

        [Theory]
        [InlineData("transition: opacity 1s 2s ease", "opacity", "1s", "ease", "2s")]
        [InlineData("transition: 1s 2s", "all", "1s", "initial", "2s")]
        [InlineData("transition: 1s ease 2s opacity", "opacity", "1s", "ease", "2s")]
        public void TransitionFirstTimeIsDurationSecondIsDelay(string declaration, string property, string duration, string timing, string delay)
        {
            var values = Expand(declaration, TransitionLonghands);
            Assert.Equal(duration, values[1]);
            Assert.Equal(delay, values[3]);
            Assert.Equal(timing, values[2]);
            if (property != "all") Assert.Equal(property, values[0]);
        }

        [Fact]
        public void TransitionPropertyAndDurationAreNotLostToTheTime() =>
            Assert.Equal(new[] { "opacity", "300ms" }, Expand("transition: opacity 300ms", "transition-property", "transition-duration"));

        [Fact]
        public void TransitionListKeepsPerLayerTimes() =>
            Assert.Equal(new[] { "opacity, width", "1s, 3s", "2s, 4s" },
                Expand("transition: opacity 1s 2s, width 3s 4s", "transition-property", "transition-duration", "transition-delay"));
    }
}
