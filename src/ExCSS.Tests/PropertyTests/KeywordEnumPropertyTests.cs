using Xunit;

namespace ExCSS.Tests.PropertyTests;

public class KeywordEnumPropertyTests : CssConstructionFunctions
{
    // name, inherited, animatable
    [Theory]
    [InlineData("mix-blend-mode", false, true)]
    [InlineData("background-blend-mode", false, false)]
    [InlineData("isolation", false, false)]
    [InlineData("image-rendering", true, false)]
    [InlineData("text-rendering", true, false)]
    [InlineData("shape-rendering", true, false)]
    [InlineData("vector-effect", false, false)]
    [InlineData("buffered-rendering", false, false)]
    [InlineData("print-color-adjust", true, false)]
    [InlineData("forced-color-adjust", true, false)]
    [InlineData("pointer-events", true, false)]
    [InlineData("user-select", false, false)]
    [InlineData("touch-action", false, false)]
    [InlineData("interactivity", true, false)]
    [InlineData("caret-color", true, true)]
    [InlineData("accent-color", true, true)]
    [InlineData("math-style", true, false)]
    [InlineData("math-shift", true, false)]
    [InlineData("math-depth", true, false)]
    [InlineData("float-reference", false, false)]
    [InlineData("footnote-display", false, false)]
    [InlineData("footnote-policy", false, false)]
    public void FlagsMatchSpecification(string name, bool inherited, bool animatable)
    {
        var property = ParseDeclaration(name + ": initial");
        Assert.Equal(name, property.Name);
        Assert.Equal(inherited, property.CanBeInherited);
        Assert.Equal(animatable, property.IsAnimatable);
    }

    [Theory]
    [InlineData("mix-blend-mode: normal")]
    [InlineData("mix-blend-mode: multiply")]
    [InlineData("mix-blend-mode: color-dodge")]
    [InlineData("mix-blend-mode: luminosity")]
    [InlineData("background-blend-mode: normal")]
    [InlineData("background-blend-mode: multiply, screen")]
    [InlineData("isolation: auto")]
    [InlineData("isolation: isolate")]
    [InlineData("image-rendering: auto")]
    [InlineData("image-rendering: crisp-edges")]
    [InlineData("image-rendering: pixelated")]
    [InlineData("text-rendering: optimizeLegibility")]
    [InlineData("text-rendering: geometricPrecision")]
    [InlineData("shape-rendering: crispEdges")]
    [InlineData("vector-effect: none")]
    [InlineData("vector-effect: non-scaling-stroke")]
    [InlineData("buffered-rendering: dynamic")]
    [InlineData("print-color-adjust: economy")]
    [InlineData("print-color-adjust: exact")]
    [InlineData("forced-color-adjust: preserve-parent-color")]
    [InlineData("pointer-events: auto")]
    [InlineData("pointer-events: none")]
    [InlineData("pointer-events: visiblePainted")]
    [InlineData("pointer-events: all")]
    [InlineData("user-select: auto")]
    [InlineData("user-select: text")]
    [InlineData("user-select: none")]
    [InlineData("user-select: all")]
    [InlineData("touch-action: auto")]
    [InlineData("touch-action: manipulation")]
    [InlineData("touch-action: pan-x pan-y")]
    [InlineData("touch-action: pan-y pinch-zoom")]
    [InlineData("touch-action: pan-left pan-up pinch-zoom")]
    [InlineData("interactivity: auto")]
    [InlineData("interactivity: inert")]
    [InlineData("caret-color: auto")]
    [InlineData("caret-color: red")]
    [InlineData("caret-color: #ff0000")]
    [InlineData("caret-color: currentColor")]
    [InlineData("accent-color: auto")]
    [InlineData("accent-color: rgb(1, 2, 3)")]
    [InlineData("math-style: normal")]
    [InlineData("math-style: compact")]
    [InlineData("math-shift: compact")]
    [InlineData("math-depth: auto-add")]
    [InlineData("math-depth: add(1)")]
    [InlineData("math-depth: add(-2)")]
    [InlineData("math-depth: 3")]
    [InlineData("math-depth: 0")]
    [InlineData("float-reference: inline")]
    [InlineData("float-reference: column")]
    [InlineData("float-reference: region")]
    [InlineData("float-reference: page")]
    [InlineData("footnote-display: block")]
    [InlineData("footnote-display: compact")]
    [InlineData("footnote-policy: auto")]
    [InlineData("footnote-policy: line")]
    [InlineData("footnote-policy: block")]
    [InlineData("pointer-events: inherit")]
    [InlineData("isolation: unset")]
    public void ValidValuesAreAccepted(string declaration)
    {
        var property = ParseDeclaration(declaration);
        Assert.True(property.HasValue, declaration);
    }

    [Theory]
    [InlineData("mix-blend-mode: bogus")]
    [InlineData("mix-blend-mode: multiply, screen")]
    [InlineData("background-blend-mode: bogus")]
    [InlineData("isolation: none")]
    [InlineData("image-rendering: sharp")]
    [InlineData("text-rendering: normal")]
    [InlineData("shape-rendering: pixelated")]
    [InlineData("vector-effect: auto")]
    [InlineData("buffered-rendering: none")]
    [InlineData("print-color-adjust: auto")]
    [InlineData("forced-color-adjust: economy")]
    [InlineData("pointer-events: visible-painted")]
    [InlineData("pointer-events: auto none")]
    [InlineData("user-select: element")]
    [InlineData("touch-action: pan-x pan-left")]
    [InlineData("touch-action: auto pan-x")]
    [InlineData("touch-action: manipulation pinch-zoom")]
    [InlineData("touch-action: pan-x pan-y pinch-zoom pan-up")]
    [InlineData("touch-action: 1")]
    [InlineData("interactivity: none")]
    [InlineData("caret-color: 12px")]
    [InlineData("accent-color: bogus")]
    [InlineData("math-style: auto")]
    [InlineData("math-shift: none")]
    [InlineData("math-depth: 1.5")]
    [InlineData("math-depth: add(1.5)")]
    [InlineData("math-depth: add(1 2)")]
    [InlineData("math-depth: add()")]
    [InlineData("math-depth: normal")]
    [InlineData("math-depth: 1px")]
    [InlineData("float-reference: top")]
    [InlineData("footnote-display: none")]
    [InlineData("footnote-policy: page")]
    public void InvalidValuesAreRejected(string declaration)
    {
        var property = ParseDeclaration(declaration);
        Assert.False(property.HasValue, declaration);
    }

    [Fact]
    public void StyleDeclarationExposesAccessors()
    {
        var declaration = ParseDeclarations("pointer-events: none; user-select: all; accent-color: red; math-depth: add(1)");
        Assert.Equal("none", declaration.PointerEvents);
        Assert.Equal("all", declaration.UserSelect);
        Assert.Equal("rgb(255, 0, 0)", declaration.AccentColor);
        Assert.Equal("add(1)", declaration.MathDepth);
    }
}
