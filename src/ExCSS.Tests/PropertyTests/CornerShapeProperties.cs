namespace ExCSS.Tests.PropertyTests
{
    using Xunit;

    /// <summary>CSS Borders 4 corner-shape family: what is accepted and how a value is normalized.</summary>
    public class CornerShapePropertyTests : CssConstructionFunctions
    {
        public static readonly TheoryData<string> ShapeLonghands = new TheoryData<string>
        {
            "corner-top-left-shape", "corner-top-right-shape", "corner-bottom-right-shape",
            "corner-bottom-left-shape", "corner-start-start-shape", "corner-start-end-shape",
            "corner-end-start-shape", "corner-end-end-shape",
        };

        [Theory]
        [InlineData("round", "round")]
        [InlineData("scoop", "scoop")]
        [InlineData("BEVEL", "bevel")]
        [InlineData("notch", "notch")]
        [InlineData("square", "square")]
        [InlineData("squircle", "squircle")]
        [InlineData("superellipse(2)", "superellipse(2)")]
        [InlineData("superellipse( -1.5 )", "superellipse(-1.5)")]
        [InlineData("superellipse(infinity)", "superellipse(infinity)")]
        [InlineData("superellipse(-infinity)", "superellipse(-infinity)")]
        public void ALonghandAcceptsEveryShapeValue(string value, string expected)
        {
            var style = ParseDeclarations("corner-top-left-shape: " + value);
            Assert.Equal(expected, style.GetPropertyValue("corner-top-left-shape"));
        }

        [Theory]
        [MemberData(nameof(ShapeLonghands))]
        public void EveryShapeLonghandIsRegistered(string name)
        {
            var property = ParseDeclaration($"{name}: squircle");
            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.False(property.IsInherited);
            Assert.Equal("squircle", property.Value);

            Assert.Equal("inherit", ParseDeclarations($"{name}: inherit").GetPropertyValue(name));
        }

        [Theory]
        [InlineData("wobbly")]
        [InlineData("round bevel")]
        [InlineData("superellipse()")]
        [InlineData("superellipse(1 2)")]
        [InlineData("superellipse(bevel)")]
        [InlineData("10px")]
        public void ALonghandRejectsAnythingElse(string value)
        {
            var style = ParseDeclarations("corner-top-left-shape: " + value);
            Assert.Equal("", style.GetPropertyValue("corner-top-left-shape"));
        }

        [Theory]
        [InlineData("corner-shape: bevel scoop", "corner-top-left-shape", "bevel", "corner-top-right-shape", "scoop")]
        [InlineData("corner-shape: bevel scoop squircle notch", "corner-bottom-right-shape", "squircle", "corner-bottom-left-shape", "notch")]
        [InlineData("corner-top-shape: notch", "corner-top-left-shape", "notch", "corner-top-right-shape", "notch")]
        [InlineData("corner-bottom-shape: notch square", "corner-bottom-left-shape", "notch", "corner-bottom-right-shape", "square")]
        [InlineData("corner-block-end-shape: squircle", "corner-end-start-shape", "squircle", "corner-end-end-shape", "squircle")]
        [InlineData("corner-inline-start-shape: bevel scoop", "corner-start-start-shape", "bevel", "corner-end-start-shape", "scoop")]
        [InlineData("border-left-radius: 4px 8px", "border-top-left-radius", "4px 4px", "border-bottom-left-radius", "8px 8px")]
        [InlineData("border-inline-end-radius: 4px", "border-start-end-radius", "4px 4px", "border-end-end-radius", "4px 4px")]
        [InlineData("corner-top: scoop 4px", "corner-top-left-shape", "scoop", "border-top-right-radius", "4px 4px")]
        [InlineData("corner-inline-end: 4px bevel", "corner-start-end-shape", "bevel", "border-end-end-radius", "4px 4px")]
        [InlineData("corner-start-end: bevel 3px", "corner-start-end-shape", "bevel", "border-start-end-radius", "3px")]
        [InlineData("corner-top-left: 3px bevel", "corner-top-left-shape", "bevel", "border-top-left-radius", "3px")]
        [InlineData("corner: notch 5px / 7px", "corner-bottom-right-shape", "notch", "border-bottom-left-radius", "5px 7px")]
        public void AShorthandExpandsToTheLonghandsItNames(string declaration, string first, string firstValue, string second, string secondValue)
        {
            var style = ParseDeclarations(declaration);

            Assert.Equal(firstValue, style.GetPropertyValue(first));
            Assert.Equal(secondValue, style.GetPropertyValue(second));
        }

        [Theory]
        [InlineData("corner-shape: bevel scoop squircle notch square")]
        [InlineData("corner-top-shape: bevel scoop notch")]
        [InlineData("border-top-radius: 1px 2px 3px")]
        [InlineData("corner-top: bevel bevel bevel")]
        [InlineData("corner-top-left: bevel bevel")]
        [InlineData("corner: wobbly")]
        public void AShorthandWithTooManyOrUnknownValuesIsInvalid(string declaration)
        {
            var style = ParseDeclarations(declaration);
            Assert.Equal(0, style.Length);
        }

        [Fact]
        public void PhysicalRadiusLonghandsDoNotSerializeIntoCornerShorthands()
        {
            var css = ParseStyleSheet(".x { border-top-left-radius: 4px; border-top-right-radius: 4px; }").ToCss();
            Assert.DoesNotContain("border-top-radius", css);
            Assert.DoesNotContain("corner", css);
        }

        [Fact]
        public void BorderRadiusStillReconstructs()
        {
            var css = ParseStyleSheet(
                ".x { border-top-left-radius: 4px; border-top-right-radius: 4px; border-bottom-right-radius: 4px; border-bottom-left-radius: 4px; }").ToCss();
            Assert.Contains("border-radius: 4px", css);
        }

        [Fact]
        public void StyleDeclaration_ExposesCornerAccessors()
        {
            var style = new StyleDeclaration(new StylesheetParser());
            style.CornerTopLeftShape = "bevel";
            style.CornerEndEndShape = "scoop";
            Assert.Equal("bevel", style.CornerTopLeftShape);
            Assert.Equal("scoop", style.GetPropertyValue("corner-end-end-shape"));
            style.CornerShape = "notch";
            Assert.Equal("notch", style.GetPropertyValue("corner-bottom-left-shape"));
        }
    }
}
