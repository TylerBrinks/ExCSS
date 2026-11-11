using System.Linq;
using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class ScrollTimelinePropertyTests : CssConstructionFunctions
    {
        private static string Longhand(string declaration, string longhand)
        {
            var sheet = ParseStyleSheet($".test {{ {declaration} }}");
            var rule = (StyleRule)sheet.StyleRules.First();
            return rule.Style.GetPropertyValue(longhand);
        }

        private static void AssertIllegal(string snippet)
        {
            var property = ParseDeclaration(snippet);

            Assert.False(property.HasValue, snippet);
        }

        [Theory]
        [InlineData("normal")]
        [InlineData("allow-discrete")]
        [InlineData("normal, allow-discrete")]
        [InlineData("allow-discrete, normal, allow-discrete")]
        public void TransitionBehaviorLegal(string value)
            => TestForLegalValue<TransitionBehaviorProperty>("transition-behavior", value);

        [Theory]
        [InlineData("transition-behavior: discrete")]
        [InlineData("transition-behavior: none")]
        [InlineData("transition-behavior: normal allow-discrete")]
        [InlineData("transition-behavior: normal,")]
        [InlineData("transition-behavior: 1s")]
        public void TransitionBehaviorIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("replace")]
        [InlineData("add")]
        [InlineData("accumulate")]
        [InlineData("replace, add, accumulate")]
        public void AnimationCompositionLegal(string value)
            => TestForLegalValue<AnimationCompositionProperty>("animation-composition", value);

        [Theory]
        [InlineData("animation-composition: multiply")]
        [InlineData("animation-composition: add accumulate")]
        [InlineData("animation-composition: auto")]
        public void AnimationCompositionIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("auto")]
        [InlineData("none")]
        [InlineData("--my-timeline")]
        [InlineData("--a, --b, auto")]
        [InlineData("scroll()")]
        [InlineData("scroll(root)")]
        [InlineData("scroll(self inline)")]
        [InlineData("scroll(y nearest)")]
        [InlineData("view()")]
        [InlineData("view(inline)")]
        [InlineData("view(10px)")]
        [InlineData("view(auto 20%)")]
        [InlineData("view(block 10px 20px)")]
        [InlineData("view(10px 20px y)")]
        [InlineData("scroll(root block), view(), --x")]
        public void AnimationTimelineLegal(string value)
            => TestForLegalValue<AnimationTimelineProperty>("animation-timeline", value);

        [Theory]
        [InlineData("animation-timeline: my-timeline")]
        [InlineData("animation-timeline: scroll(root self)")]
        [InlineData("animation-timeline: scroll(block inline)")]
        [InlineData("animation-timeline: scroll(root root)")]
        [InlineData("animation-timeline: scroll(10px)")]
        [InlineData("animation-timeline: view(block inline)")]
        [InlineData("animation-timeline: view(10px block 20px)")]
        [InlineData("animation-timeline: view(10px 20px 30px)")]
        [InlineData("animation-timeline: view(root)")]
        [InlineData("animation-timeline: auto none")]
        [InlineData("animation-timeline: --a --b")]
        [InlineData("animation-timeline: scroll(),")]
        public void AnimationTimelineIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("animation-range-start", "normal")]
        [InlineData("animation-range-start", "10%")]
        [InlineData("animation-range-start", "20px")]
        [InlineData("animation-range-start", "calc(10% + 5px)")]
        [InlineData("animation-range-start", "cover")]
        [InlineData("animation-range-start", "entry 25%")]
        [InlineData("animation-range-start", "exit-crossing 10px")]
        [InlineData("animation-range-start", "contain, entry 25%, normal")]
        [InlineData("animation-range-end", "normal")]
        [InlineData("animation-range-end", "exit 75%")]
        [InlineData("animation-range-end", "entry-crossing")]
        public void AnimationRangeEdgeLegal(string name, string value)
        {
            var property = ParseDeclaration($"{name}: {value}");

            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("animation-range-start: cover entry")]
        [InlineData("animation-range-start: 10% 20%")]
        [InlineData("animation-range-start: 10% cover")]
        [InlineData("animation-range-start: bogus")]
        [InlineData("animation-range-start: entry 10% 20%")]
        [InlineData("animation-range-start: 10")]
        [InlineData("animation-range-end: normal 10%")]
        [InlineData("animation-range-end: auto")]
        public void AnimationRangeEdgeIllegal(string snippet) => AssertIllegal(snippet);

        [Fact]
        public void AnimationRangeStartAndEndAreDistinctProperties()
        {
            Assert.IsType<AnimationRangeStartProperty>(ParseDeclaration("animation-range-start: normal"));
            Assert.IsType<AnimationRangeEndProperty>(ParseDeclaration("animation-range-end: normal"));
        }

        [Theory]
        [InlineData("cover", "cover", "cover")]
        [InlineData("normal", "normal", "normal")]
        [InlineData("10% 90%", "10%", "90%")]
        [InlineData("entry 10% exit 90%", "entry 10%", "exit 90%")]
        [InlineData("entry 10% 80%", "entry 10%", "80%")]
        [InlineData("20% exit", "20%", "exit")]
        [InlineData("entry 20%", "entry 20%", "entry")]
        [InlineData("cover entry", "cover", "entry")]
        [InlineData("10%", "10%", "normal")]
        [InlineData("cover 20%, 10% 30%", "cover 20%, 10%", "cover, 30%")]
        public void AnimationRangeShorthandExpands(string value, string start, string end)
        {
            Assert.Equal(start, Longhand($"animation-range: {value};", "animation-range-start"));
            Assert.Equal(end, Longhand($"animation-range: {value};", "animation-range-end"));
        }

        [Theory]
        [InlineData("animation-range: 10% 20% 30%")]
        [InlineData("animation-range: bogus")]
        [InlineData("animation-range: normal,")]
        public void AnimationRangeShorthandIllegal(string snippet) => AssertIllegal(snippet);

        [Fact]
        public void AnimationRangeShorthandIsAShorthand()
        {
            var property = ParseDeclaration("animation-range: entry 10% exit 90%");

            Assert.IsType<AnimationRangeProperty>(property);
            Assert.Equal("entry 10% exit 90%", property.Value);
        }

        [Theory]
        [InlineData("none")]
        [InlineData("--tl")]
        [InlineData("--a, none, --b")]
        public void ScrollTimelineNameLegal(string value)
            => TestForLegalValue<ScrollTimelineNameProperty>("scroll-timeline-name", value);

        [Theory]
        [InlineData("scroll-timeline-name: tl")]
        [InlineData("scroll-timeline-name: auto")]
        [InlineData("scroll-timeline-name: --")]
        [InlineData("scroll-timeline-name: --a --b")]
        [InlineData("view-timeline-name: tl")]
        public void TimelineNameIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("block")]
        [InlineData("inline")]
        [InlineData("x")]
        [InlineData("y")]
        [InlineData("x, y, block")]
        public void ScrollTimelineAxisLegal(string value)
        {
            TestForLegalValue<ScrollTimelineAxisProperty>("scroll-timeline-axis", value);
            TestForLegalValue<ViewTimelineAxisProperty>("view-timeline-axis", value);
        }

        [Theory]
        [InlineData("scroll-timeline-axis: z")]
        [InlineData("scroll-timeline-axis: auto")]
        [InlineData("scroll-timeline-axis: block inline")]
        [InlineData("view-timeline-axis: both")]
        public void TimelineAxisIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("--tl")]
        [InlineData("none")]
        [InlineData("--a, --b")]
        public void ViewTimelineNameLegal(string value)
            => TestForLegalValue<ViewTimelineNameProperty>("view-timeline-name", value);

        [Theory]
        [InlineData("auto")]
        [InlineData("10px")]
        [InlineData("10px 20%")]
        [InlineData("auto 5px")]
        [InlineData("10px, 5% auto")]
        public void ViewTimelineInsetLegal(string value)
            => TestForLegalValue<ViewTimelineInsetProperty>("view-timeline-inset", value);

        [Theory]
        [InlineData("view-timeline-inset: 10px 20px 30px")]
        [InlineData("view-timeline-inset: 10")]
        [InlineData("view-timeline-inset: none")]
        [InlineData("view-timeline-inset: 10px,")]
        public void ViewTimelineInsetIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("none")]
        [InlineData("all")]
        [InlineData("--a")]
        [InlineData("--a, --b, --c")]
        public void TimelineScopeLegal(string value)
            => TestForLegalValue<TimelineScopeProperty>("timeline-scope", value);

        [Theory]
        [InlineData("timeline-scope: a")]
        [InlineData("timeline-scope: none, --a")]
        [InlineData("timeline-scope: all, --a")]
        [InlineData("timeline-scope: --a all")]
        [InlineData("timeline-scope: auto")]
        public void TimelineScopeIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("--tl", "--tl", "initial")]
        [InlineData("--tl inline", "--tl", "inline")]
        [InlineData("none y", "none", "y")]
        [InlineData("--a block, --b x", "--a, --b", "block, x")]
        public void ScrollTimelineShorthandExpands(string value, string name, string axis)
        {
            Assert.IsType<ScrollTimelineProperty>(ParseDeclaration($"scroll-timeline: {value}"));
            Assert.Equal(name, Longhand($"scroll-timeline: {value};", "scroll-timeline-name"));
            Assert.Equal(axis, Longhand($"scroll-timeline: {value};", "scroll-timeline-axis"));
        }

        [Theory]
        [InlineData("scroll-timeline: inline")]
        [InlineData("scroll-timeline: --a --b")]
        [InlineData("scroll-timeline: block --a")]
        [InlineData("scroll-timeline: --a block block")]
        [InlineData("scroll-timeline: --a,")]
        public void ScrollTimelineShorthandIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("--tl", "--tl", "initial", "initial")]
        [InlineData("--tl inline", "--tl", "inline", "initial")]
        [InlineData("--tl 10px", "--tl", "initial", "10px")]
        [InlineData("--tl x 10px 20px", "--tl", "x", "10px 20px")]
        [InlineData("--tl 10px 20px x", "--tl", "x", "10px 20px")]
        [InlineData("--tl auto y", "--tl", "y", "auto")]
        [InlineData("--a block 5%, --b y auto", "--a, --b", "block, y", "5%, auto")]
        public void ViewTimelineShorthandExpands(string value, string name, string axis, string inset)
        {
            Assert.IsType<ViewTimelineProperty>(ParseDeclaration($"view-timeline: {value}"));
            Assert.Equal(name, Longhand($"view-timeline: {value};", "view-timeline-name"));
            Assert.Equal(axis, Longhand($"view-timeline: {value};", "view-timeline-axis"));
            Assert.Equal(inset, Longhand($"view-timeline: {value};", "view-timeline-inset"));
        }

        [Theory]
        [InlineData("view-timeline: block")]
        [InlineData("view-timeline: 10px")]
        [InlineData("view-timeline: --a block inline")]
        [InlineData("view-timeline: --a 10px 20px 30px")]
        public void ViewTimelineShorthandIllegal(string snippet) => AssertIllegal(snippet);

        [Theory]
        [InlineData("transition-behavior: allow-discrete")]
        [InlineData("animation-timeline: scroll(root)")]
        [InlineData("animation-range: cover")]
        [InlineData("scroll-timeline: --a")]
        [InlineData("view-timeline: --a")]
        [InlineData("timeline-scope: --a")]
        public void GlobalKeywordsAreAccepted(string snippet)
        {
            var name = snippet.Substring(0, snippet.IndexOf(':'));

            foreach (var keyword in new[] {"inherit", "initial", "unset", "revert", "revert-layer"})
            {
                var property = ParseDeclaration($"{name}: {keyword}");
                Assert.True(property.HasValue, $"{name}: {keyword}");
                Assert.Equal(keyword, property.Value);
            }
        }

        [Theory]
        [InlineData("transition-behavior", true)]
        [InlineData("animation-timeline", true)]
        [InlineData("animation-range-start", true)]
        public void PropertiesAreNotInherited(string name, bool known)
        {
            var property = ParseDeclaration($"{name}: initial");

            Assert.Equal(known, property.HasValue);
            Assert.False(property.CanBeInherited);
        }

        [Fact]
        public void TransitionShorthandTakesBehaviorInAnyPosition()
        {
            Assert.Equal("allow-discrete", Longhand("transition: opacity 1s allow-discrete;", "transition-behavior"));
            Assert.Equal("allow-discrete", Longhand("transition: allow-discrete opacity 1s;", "transition-behavior"));
            Assert.Equal("normal", Longhand("transition: opacity 1s 2s normal;", "transition-behavior"));
            Assert.Equal("opacity", Longhand("transition: opacity 1s allow-discrete;", "transition-property"));
            Assert.Equal("1s", Longhand("transition: opacity 1s allow-discrete;", "transition-duration"));
            Assert.Equal("1s", Longhand("transition: allow-discrete 2s 1s;", "transition-delay"));
        }

        [Fact]
        public void TransitionShorthandBehaviorListAligns()
        {
            var css = "transition: opacity 1s allow-discrete, width 2s normal;";

            Assert.Equal("allow-discrete, normal", Longhand(css, "transition-behavior"));
            Assert.Equal("opacity, width", Longhand(css, "transition-property"));
        }

        [Fact]
        public void TransitionShorthandWithoutBehaviorResetsIt()
        {
            Assert.Equal("initial", Longhand("transition-behavior: allow-discrete; transition: opacity 1s;", "transition-behavior"));
        }

        [Fact]
        public void TransitionShorthandRejectsTwoBehaviors()
        {
            AssertIllegal("transition: opacity 1s normal allow-discrete");
        }

        [Fact]
        public void AnimationShorthandTakesTimeline()
        {
            Assert.Equal("scroll(root)", Longhand("animation: spin 1s linear scroll(root);", "animation-timeline"));
            Assert.Equal("spin", Longhand("animation: spin 1s linear scroll(root);", "animation-name"));
            Assert.Equal("view(block 10px)", Longhand("animation: view(block 10px) 1s spin;", "animation-timeline"));
            Assert.Equal("auto", Longhand("animation: auto spin 1s;", "animation-timeline"));
            Assert.Equal("spin", Longhand("animation: auto spin 1s;", "animation-name"));
            Assert.Equal("--tl", Longhand("animation: spin 1s --tl;", "animation-timeline"));
            Assert.Equal("spin", Longhand("animation: spin 1s --tl;", "animation-name"));
        }

        [Fact]
        public void AnimationShorthandKeepsDashedIdentAsNameWhenAlone()
        {
            Assert.Equal("--spin", Longhand("animation: 1s --spin;", "animation-name"));
            Assert.Equal("initial", Longhand("animation: 1s --spin;", "animation-timeline"));
        }

        [Fact]
        public void AnimationShorthandNoneIsTheFillMode()
        {
            Assert.Equal("none", Longhand("animation: none 1s;", "animation-fill-mode"));
            Assert.Equal("initial", Longhand("animation: none 1s;", "animation-timeline"));
        }

        [Fact]
        public void AnimationShorthandResetsTimelineRangeAndComposition()
        {
            var css = "animation-timeline: scroll(); animation-range: cover; animation-composition: add; animation: spin 1s;";

            Assert.Equal("initial", Longhand(css, "animation-timeline"));
            Assert.Equal("initial", Longhand(css, "animation-range-start"));
            Assert.Equal("initial", Longhand(css, "animation-range-end"));
            Assert.Equal("initial", Longhand(css, "animation-composition"));
        }

        [Fact]
        public void AnimationShorthandRejectsTwoTimelines()
        {
            AssertIllegal("animation: spin 1s scroll() view()");
        }

        [Fact]
        public void AnimationLonghandsStillSerializeIndividually()
        {
            var sheet = ParseStyleSheet(".a { animation-name: spin; animation-timeline: scroll(); }");

            Assert.Contains("animation-timeline: scroll()", sheet.ToCss());
        }
    }
}
