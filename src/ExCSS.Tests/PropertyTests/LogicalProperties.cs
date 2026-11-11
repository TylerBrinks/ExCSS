namespace ExCSS.Tests.PropertyTests
{
    using System.Linq;
    using Xunit;

    public class LogicalPropertyTests : CssConstructionFunctions
    {
        private static StyleDeclaration Declared(string css)
        {
            var sheet = ParseStyleSheet($".x {{ {css} }}");
            return sheet.Rules.OfType<StyleRule>().Single().Style;
        }

        private static string DeclaredValue(string property, string value)
        {
            return Declared($"{property}: {value};").GetPropertyValue(property);
        }

        public static readonly TheoryData<string> LengthAutoLonghands = new TheoryData<string>
        {
            "margin-block-start", "margin-block-end", "margin-inline-start", "margin-inline-end",
            "inset-block-start", "inset-block-end", "inset-inline-start", "inset-inline-end",
        };

        public static readonly TheoryData<string> LengthLonghands = new TheoryData<string>
        {
            "padding-block-start", "padding-block-end", "padding-inline-start", "padding-inline-end",
        };

        public static readonly TheoryData<string> LengthAutoShorthands = new TheoryData<string>
        {
            "margin-block", "margin-inline", "inset-block", "inset-inline",
        };

        public static readonly TheoryData<string> LengthShorthands = new TheoryData<string>
        {
            "padding-block", "padding-inline",
        };

        [Theory]
        [MemberData(nameof(LengthAutoLonghands))]
        public void AutoLengthLonghand_Accepts(string property)
        {
            Assert.Equal("auto", DeclaredValue(property, "auto"));
            Assert.Equal("10%", DeclaredValue(property, "10%"));
            Assert.Equal("-10px", DeclaredValue(property, "-10px"));
            Assert.Equal("1rem", DeclaredValue(property, "1rem"));
            Assert.Equal("0", DeclaredValue(property, "0"));
        }

        [Theory]
        [MemberData(nameof(LengthAutoLonghands))]
        [MemberData(nameof(LengthLonghands))]
        public void LengthLonghand_Rejects(string property)
        {
            Assert.Equal(string.Empty, DeclaredValue(property, "0deg"));
            Assert.Equal(string.Empty, DeclaredValue(property, "10"));
        }

        [Theory]
        [MemberData(nameof(LengthLonghands))]
        public void PaddingLonghand_AcceptsLengthAndPercentButNotAuto(string property)
        {
            Assert.Equal("5px", DeclaredValue(property, "5px"));
            Assert.Equal("5%", DeclaredValue(property, "5%"));
            Assert.Equal(string.Empty, DeclaredValue(property, "auto"));
        }

        [Theory]
        [MemberData(nameof(LengthAutoLonghands))]
        [MemberData(nameof(LengthLonghands))]
        public void Longhand_IsItsOwnProperty(string property)
        {
            var declared = ParseDeclaration($"{property}: 1px");
            Assert.Equal(property, declared.Name);
            Assert.True(declared.HasValue);
            Assert.False(declared.IsInherited);
            Assert.Equal("1px", declared.Value);
        }

        [Theory]
        [MemberData(nameof(LengthAutoShorthands))]
        [MemberData(nameof(LengthShorthands))]
        public void TwoValueShorthand_CollapsesEqualValues(string property)
        {
            Assert.Equal("100px", DeclaredValue(property, "100px 100px"));
            Assert.Equal("10% 5px", DeclaredValue(property, "10% 5px"));
        }

        [Theory]
        [MemberData(nameof(LengthAutoShorthands))]
        [MemberData(nameof(LengthShorthands))]
        public void TwoValueShorthand_RejectsInvalid(string property)
        {
            Assert.Equal(string.Empty, DeclaredValue(property, "0deg"));
            Assert.Equal(string.Empty, DeclaredValue(property, "1px 2px 3px"));
            Assert.Equal(string.Empty, DeclaredValue(property, "inherit 1px"));
        }

        [Theory]
        [InlineData("margin-block", "margin-block-start", "margin-block-end")]
        [InlineData("margin-inline", "margin-inline-start", "margin-inline-end")]
        [InlineData("padding-block", "padding-block-start", "padding-block-end")]
        [InlineData("padding-inline", "padding-inline-start", "padding-inline-end")]
        [InlineData("inset-block", "inset-block-start", "inset-block-end")]
        [InlineData("inset-inline", "inset-inline-start", "inset-inline-end")]
        [InlineData("border-block-width", "border-block-start-width", "border-block-end-width")]
        [InlineData("border-inline-width", "border-inline-start-width", "border-inline-end-width")]
        public void Shorthand_ExpandsToLogicalLonghands(string shorthand, string start, string end)
        {
            var one = Declared($"{shorthand}: 3px;");
            Assert.Equal("3px", one.GetPropertyValue(start));
            Assert.Equal("3px", one.GetPropertyValue(end));

            var two = Declared($"{shorthand}: 3px 4px;");
            Assert.Equal("3px", two.GetPropertyValue(start));
            Assert.Equal("4px", two.GetPropertyValue(end));
        }

        [Fact]
        public void Inset_ExpandsToPhysicalEdges()
        {
            var style = Declared("inset: 1px 2px 3px 4px;");
            Assert.Equal("1px", style.GetPropertyValue("top"));
            Assert.Equal("2px", style.GetPropertyValue("right"));
            Assert.Equal("3px", style.GetPropertyValue("bottom"));
            Assert.Equal("4px", style.GetPropertyValue("left"));

            style = Declared("inset: auto 5%;");
            Assert.Equal("auto", style.GetPropertyValue("top"));
            Assert.Equal("5%", style.GetPropertyValue("right"));
            Assert.Equal("auto", style.GetPropertyValue("bottom"));
            Assert.Equal("5%", style.GetPropertyValue("left"));
        }

        [Fact]
        public void Inset_RejectsInvalid()
        {
            Assert.Equal(string.Empty, DeclaredValue("inset", "1 2"));
            Assert.Equal(string.Empty, DeclaredValue("inset", "1px 2px 3px 4px 5px"));
        }

        [Theory]
        [InlineData("border-block-start")]
        [InlineData("border-block-end")]
        [InlineData("border-inline-start")]
        [InlineData("border-inline-end")]
        public void BorderEdgeShorthand_ExpandsToWidthStyleColor(string edge)
        {
            var style = Declared($"{edge}: 2px dashed red;");
            Assert.Equal("2px", style.GetPropertyValue($"{edge}-width"));
            Assert.Equal("dashed", style.GetPropertyValue($"{edge}-style"));
            Assert.Equal("rgb(255, 0, 0)", style.GetPropertyValue($"{edge}-color"));
        }

        [Theory]
        [InlineData("border-block", "block")]
        [InlineData("border-inline", "inline")]
        public void BorderAxisShorthand_ExpandsToBothEdges(string shorthand, string axis)
        {
            var style = Declared($"{shorthand}: 2px solid blue;");
            foreach (var edge in new[] { "start", "end" })
            {
                Assert.Equal("2px", style.GetPropertyValue($"border-{axis}-{edge}-width"));
                Assert.Equal("solid", style.GetPropertyValue($"border-{axis}-{edge}-style"));
                Assert.Equal("rgb(0, 0, 255)", style.GetPropertyValue($"border-{axis}-{edge}-color"));
            }
        }

        [Theory]
        [InlineData("border-block-start-width", "thin", "1px")]
        [InlineData("border-inline-end-width", "3px", "3px")]
        [InlineData("border-block-end-style", "double", "double")]
        [InlineData("border-inline-start-style", "none", "none")]
        [InlineData("border-block-start-color", "currentcolor", "currentColor")]
        public void BorderLonghands_Accept(string property, string value, string expected)
        {
            Assert.Equal(expected, DeclaredValue(property, value));
        }

        [Theory]
        [InlineData("border-block-start-width", "red")]
        [InlineData("border-inline-end-style", "5px")]
        [InlineData("border-block-end-color", "solid")]
        [InlineData("border-block-style", "1px")]
        [InlineData("border-inline-color", "dotted")]
        public void BorderLonghands_Reject(string property, string value)
        {
            Assert.Equal(string.Empty, DeclaredValue(property, value));
        }

        [Fact]
        public void BorderTwoValueShorthands_Expand()
        {
            var style = Declared("border-block-style: solid dashed; border-inline-color: red blue;");
            Assert.Equal("solid", style.GetPropertyValue("border-block-start-style"));
            Assert.Equal("dashed", style.GetPropertyValue("border-block-end-style"));
            Assert.Equal("rgb(255, 0, 0)", style.GetPropertyValue("border-inline-start-color"));
            Assert.Equal("rgb(0, 0, 255)", style.GetPropertyValue("border-inline-end-color"));
        }

        [Theory]
        [InlineData("border-start-start-radius")]
        [InlineData("border-start-end-radius")]
        [InlineData("border-end-start-radius")]
        [InlineData("border-end-end-radius")]
        public void LogicalRadiusLonghand(string property)
        {
            Assert.Equal("10px", DeclaredValue(property, "10px"));
            Assert.Equal("10px 5%", DeclaredValue(property, "10px 5%"));
            Assert.Equal(string.Empty, DeclaredValue(property, "10px 5% 3px"));
            Assert.Equal(string.Empty, DeclaredValue(property, "red"));
            Assert.Equal(property, ParseDeclaration($"{property}: 1px").Name);
        }

        [Fact]
        public void LogicalShorthand_DoesNotSerializeAsLogicalShorthand()
        {
            var css = ParseStyleSheet(".x { margin-block: 3px 4px; }").ToCss();
            Assert.Contains("margin-block-start: 3px", css);
            Assert.Contains("margin-block-end: 4px", css);
            Assert.DoesNotContain("margin-block:", css);
        }

        [Fact]
        public void PhysicalLonghands_DoNotSerializeAsLogicalShorthand()
        {
            var css = ParseStyleSheet(".x { top: 1px; right: 1px; bottom: 1px; left: 1px; }").ToCss();
            Assert.DoesNotContain("inset", css);

            css = ParseStyleSheet(".x { margin-top: 5pt; margin-bottom: 5pt; }").ToCss();
            Assert.DoesNotContain("margin-block", css);
        }

        [Fact]
        public void PhysicalShorthands_StillReconstruct()
        {
            var css = ParseStyleSheet(
                ".x { border-top: 1px solid red; border-right: 1px solid red; border-bottom: 1px solid red; border-left: 1px solid red; margin-top: 1px; margin-right: 1px; margin-bottom: 1px; margin-left: 1px; }").ToCss();
            Assert.Contains("border:", css);
            Assert.Contains("margin:", css);
        }

        [Fact]
        public void LogicalLonghandsCoexistWithPhysical()
        {
            var style = Declared("margin-left: 1px; margin-inline-start: 2px;");
            Assert.Equal("1px", style.GetPropertyValue("margin-left"));
            Assert.Equal("2px", style.GetPropertyValue("margin-inline-start"));
        }

        [Fact]
        public void StyleDeclaration_ExposesLogicalAccessors()
        {
            var style = new StyleDeclaration(new StylesheetParser());
            style.MarginBlockStart = "1px";
            style.PaddingInlineEnd = "2px";
            style.InsetBlockEnd = "3px";
            style.BorderInlineStartWidth = "4px";
            style.BorderEndEndRadius = "5px";
            Assert.Equal("1px", style.GetPropertyValue("margin-block-start"));
            Assert.Equal("2px", style.PaddingInlineEnd);
            Assert.Equal("3px", style.InsetBlockEnd);
            Assert.Equal("4px", style.BorderInlineStartWidth);
            Assert.Equal("5px", style.BorderEndEndRadius);
        }

        [Fact]
        public void GlobalKeywordsAreAccepted()
        {
            Assert.Equal("inherit", DeclaredValue("margin-block-start", "inherit"));
            Assert.Equal("initial", DeclaredValue("border-inline-end-style", "initial"));
            Assert.Equal("inherit", DeclaredValue("margin-block", "inherit"));
        }
    }
}
