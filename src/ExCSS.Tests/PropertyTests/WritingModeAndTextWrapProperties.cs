using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class WritingModeAndTextWrapProperties
    {
        // ---- new longhands: accept / reject / inherited ----

        [Theory]
        [InlineData("writing-mode", "horizontal-tb")]
        [InlineData("writing-mode", "vertical-rl")]
        [InlineData("writing-mode", "vertical-lr")]
        [InlineData("writing-mode", "sideways-rl")]
        [InlineData("writing-mode", "sideways-lr")]
        [InlineData("writing-mode", "inherit")]
        [InlineData("text-orientation", "mixed")]
        [InlineData("text-orientation", "upright")]
        [InlineData("text-orientation", "sideways")]
        [InlineData("text-combine-upright", "none")]
        [InlineData("text-combine-upright", "all")]
        [InlineData("text-combine-upright", "digits")]
        [InlineData("text-combine-upright", "digits 2")]
        [InlineData("text-combine-upright", "digits 3")]
        [InlineData("text-combine-upright", "digits 4")]
        [InlineData("line-break", "auto")]
        [InlineData("line-break", "loose")]
        [InlineData("line-break", "normal")]
        [InlineData("line-break", "strict")]
        [InlineData("line-break", "anywhere")]
        [InlineData("white-space-collapse", "collapse")]
        [InlineData("white-space-collapse", "preserve")]
        [InlineData("white-space-collapse", "preserve-breaks")]
        [InlineData("white-space-collapse", "preserve-spaces")]
        [InlineData("white-space-collapse", "break-spaces")]
        [InlineData("text-wrap-mode", "wrap")]
        [InlineData("text-wrap-mode", "nowrap")]
        [InlineData("text-wrap-style", "auto")]
        [InlineData("text-wrap-style", "balance")]
        [InlineData("text-wrap-style", "stable")]
        [InlineData("text-wrap-style", "pretty")]
        [InlineData("text-wrap-style", "avoid-orphans")]
        [InlineData("text-align-all", "start")]
        [InlineData("text-align-all", "end")]
        [InlineData("text-align-all", "left")]
        [InlineData("text-align-all", "right")]
        [InlineData("text-align-all", "center")]
        [InlineData("text-align-all", "justify")]
        [InlineData("text-align-all", "match-parent")]
        public void LonghandAcceptsValue(string name, string value)
        {
            var property = ParseDeclaration(name + ": " + value);

            Assert.NotNull(property);
            Assert.Equal(name, property.Name);
            Assert.True(property.HasValue);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("writing-mode", "vertical")]
        [InlineData("writing-mode", "lr-tb")]
        [InlineData("writing-mode", "horizontal-tb vertical-rl")]
        [InlineData("text-orientation", "vertical")]
        [InlineData("text-orientation", "mixed upright")]
        [InlineData("text-combine-upright", "digits 1")]
        [InlineData("text-combine-upright", "digits 5")]
        [InlineData("text-combine-upright", "digits 2.5")]
        [InlineData("text-combine-upright", "all 2")]
        [InlineData("text-combine-upright", "2")]
        [InlineData("line-break", "tight")]
        [InlineData("white-space-collapse", "pre")]
        [InlineData("white-space-collapse", "wrap")]
        [InlineData("text-wrap-mode", "balance")]
        [InlineData("text-wrap-mode", "wrap nowrap")]
        [InlineData("text-wrap-style", "wrap")]
        [InlineData("text-align-all", "justify-all")]
        [InlineData("text-align-all", "auto")]
        public void LonghandRejectsValue(string name, string value)
        {
            var property = ParseDeclaration(name + ": " + value);

            Assert.False(property != null && property.HasValue);
        }

        [Theory]
        [InlineData("writing-mode")]
        [InlineData("text-orientation")]
        [InlineData("text-combine-upright")]
        [InlineData("line-break")]
        [InlineData("white-space-collapse")]
        [InlineData("text-wrap-mode")]
        [InlineData("text-wrap-style")]
        [InlineData("text-align-all")]
        [InlineData("text-align-last")]
        [InlineData("white-space")]
        [InlineData("text-wrap")]
        [InlineData("text-align")]
        public void PropertyIsInherited(string name)
        {
            var property = ParseDeclaration(name + ": initial");

            Assert.NotNull(property);
            Assert.True(property.CanBeInherited);
        }

        [Theory]
        [InlineData("writing-mode")]
        [InlineData("text-orientation")]
        [InlineData("text-combine-upright")]
        [InlineData("line-break")]
        [InlineData("white-space-collapse")]
        [InlineData("text-wrap-mode")]
        [InlineData("text-wrap-style")]
        [InlineData("text-align-all")]
        [InlineData("white-space")]
        [InlineData("text-wrap")]
        [InlineData("text-align")]
        public void PropertyAcceptsGlobalKeywords(string name)
        {
            foreach (var keyword in new[] {"inherit", "initial", "unset", "revert", "revert-layer"})
            {
                var style = ParseDeclarations(name + ": " + keyword);
                Assert.Equal(keyword, style.GetPropertyValue(name));
            }
        }

        // ---- white-space shorthand ----

        [Theory]
        [InlineData("normal", "collapse", "wrap")]
        [InlineData("pre", "preserve", "nowrap")]
        [InlineData("nowrap", "collapse", "nowrap")]
        [InlineData("pre-wrap", "preserve", "wrap")]
        [InlineData("pre-line", "preserve-breaks", "wrap")]
        [InlineData("break-spaces", "break-spaces", "wrap")]
        [InlineData("PRE", "preserve", "nowrap")]
        public void WhiteSpaceExpandsLegacyKeywords(string value, string collapse, string mode)
        {
            var style = ParseDeclarations("white-space: " + value);

            Assert.Equal(collapse, style.GetPropertyValue("white-space-collapse"));
            Assert.Equal(mode, style.GetPropertyValue("text-wrap-mode"));
        }

        [Theory]
        [InlineData("collapse", "collapse", "wrap")]
        [InlineData("preserve", "preserve", "wrap")]
        [InlineData("nowrap", "collapse", "nowrap")]
        [InlineData("preserve nowrap", "preserve", "nowrap")]
        [InlineData("nowrap preserve", "preserve", "nowrap")]
        [InlineData("preserve-spaces nowrap", "preserve-spaces", "nowrap")]
        [InlineData("wrap break-spaces", "break-spaces", "wrap")]
        [InlineData("preserve-breaks wrap", "preserve-breaks", "wrap")]
        public void WhiteSpaceExpandsCombinedForm(string value, string collapse, string mode)
        {
            var style = ParseDeclarations("white-space: " + value);

            Assert.Equal(collapse, style.GetPropertyValue("white-space-collapse"));
            Assert.Equal(mode, style.GetPropertyValue("text-wrap-mode"));
        }

        [Theory]
        [InlineData("bogus")]
        [InlineData("preserve preserve-breaks")]
        [InlineData("wrap nowrap")]
        [InlineData("preserve wrap nowrap")]
        [InlineData("normal nowrap")]
        [InlineData("pre pre")]
        [InlineData("preserve bogus")]
        public void WhiteSpaceRejectsInvalid(string value)
        {
            var property = ParseDeclaration("white-space: " + value);

            Assert.False(property != null && property.HasValue);
        }

        [Fact]
        public void WhiteSpaceDeclarationReportsAuthoredValue()
        {
            var property = ParseDeclaration("white-space: pre-wrap");

            Assert.IsType<WhiteSpaceProperty>(property);
            Assert.True(property.HasValue);
            Assert.Equal("pre-wrap", property.Value);
        }

        [Theory]
        [InlineData("collapse", "wrap", "normal")]
        [InlineData("preserve", "nowrap", "pre")]
        [InlineData("collapse", "nowrap", "nowrap")]
        [InlineData("preserve", "wrap", "pre-wrap")]
        [InlineData("preserve-breaks", "wrap", "pre-line")]
        [InlineData("break-spaces", "wrap", "break-spaces")]
        [InlineData("preserve-spaces", "nowrap", "preserve-spaces nowrap")]
        [InlineData("break-spaces", "nowrap", "break-spaces nowrap")]
        public void WhiteSpaceLonghandsSerializeBackToShorthand(string collapse, string mode, string expected)
        {
            var style = ParseDeclarations("white-space-collapse: " + collapse + "; text-wrap-mode: " + mode);

            Assert.Equal(expected, style.WhiteSpace);
            Assert.Equal(expected, style.GetPropertyValue("white-space"));
            Assert.Contains("white-space:" + Compact(expected), Compact(style.CssText));
        }

        [Fact]
        public void WhiteSpaceLonghandsAloneDoNotSerializeShorthand()
        {
            var style = ParseDeclarations("white-space-collapse: preserve");

            Assert.Equal(string.Empty, style.WhiteSpace);
            Assert.Contains("white-space-collapse", style.CssText);
            Assert.DoesNotContain("white-space:", Compact(style.CssText));
        }

        [Fact]
        public void WhiteSpaceCssomSetterUpdatesLonghands()
        {
            var style = ParseDeclarations("");
            style.WhiteSpace = "pre-line";

            Assert.Equal("preserve-breaks", style.GetPropertyValue("white-space-collapse"));
            Assert.Equal("wrap", style.GetPropertyValue("text-wrap-mode"));
            Assert.Equal("pre-line", style.WhiteSpace);
        }

        [Fact]
        public void WhiteSpaceLaterLonghandOverridesShorthand()
        {
            var style = ParseDeclarations("white-space: pre; text-wrap-mode: wrap");

            Assert.Equal("pre-wrap", style.WhiteSpace);
        }

        [Fact]
        public void WhiteSpaceImportantPropagatesToLonghands()
        {
            var style = ParseDeclarations("white-space: nowrap !important");

            Assert.Equal("important", style.GetPropertyPriority("white-space-collapse"));
            Assert.Equal("important", style.GetPropertyPriority("text-wrap-mode"));
        }

        // ---- text-wrap shorthand ----

        [Theory]
        [InlineData("wrap", "wrap", "auto")]
        [InlineData("nowrap", "nowrap", "auto")]
        [InlineData("balance", "wrap", "balance")]
        [InlineData("pretty", "wrap", "pretty")]
        [InlineData("stable", "wrap", "stable")]
        [InlineData("nowrap balance", "nowrap", "balance")]
        [InlineData("balance nowrap", "nowrap", "balance")]
        [InlineData("wrap pretty", "wrap", "pretty")]
        [InlineData("auto", "wrap", "auto")]
        public void TextWrapExpandsToLonghands(string value, string mode, string style)
        {
            var declarations = ParseDeclarations("text-wrap: " + value);

            Assert.Equal(mode, declarations.GetPropertyValue("text-wrap-mode"));
            Assert.Equal(style, declarations.GetPropertyValue("text-wrap-style"));
        }

        [Theory]
        [InlineData("bogus")]
        [InlineData("wrap nowrap")]
        [InlineData("balance pretty")]
        [InlineData("wrap balance pretty")]
        [InlineData("wrap bogus")]
        [InlineData("pre")]
        public void TextWrapRejectsInvalid(string value)
        {
            var property = ParseDeclaration("text-wrap: " + value);

            Assert.False(property != null && property.HasValue);
        }

        [Theory]
        [InlineData("wrap", "auto", "wrap")]
        [InlineData("nowrap", "auto", "nowrap")]
        [InlineData("wrap", "balance", "balance")]
        [InlineData("nowrap", "pretty", "nowrap pretty")]
        public void TextWrapLonghandsSerializeBackToShorthand(string mode, string style, string expected)
        {
            var declarations = ParseDeclarations("text-wrap-mode: " + mode + "; text-wrap-style: " + style);

            Assert.Equal(expected, declarations.TextWrap);
            Assert.Contains("text-wrap:" + Compact(expected), Compact(declarations.CssText));
        }

        // ---- text-align shorthand ----

        [Theory]
        [InlineData("start", "start", "auto")]
        [InlineData("end", "end", "auto")]
        [InlineData("left", "left", "auto")]
        [InlineData("right", "right", "auto")]
        [InlineData("center", "center", "auto")]
        [InlineData("justify", "justify", "auto")]
        [InlineData("match-parent", "match-parent", "auto")]
        [InlineData("justify-all", "justify", "justify")]
        [InlineData("CENTER", "center", "auto")]
        public void TextAlignExpandsToLonghands(string value, string all, string last)
        {
            var style = ParseDeclarations("text-align: " + value);

            Assert.Equal(all, style.GetPropertyValue("text-align-all"));
            Assert.Equal(last, style.GetPropertyValue("text-align-last"));
        }

        [Fact]
        public void TextAlignResetsPreviousTextAlignLast()
        {
            var style = ParseDeclarations("text-align-last: right; text-align: center");

            Assert.Equal("auto", style.GetPropertyValue("text-align-last"));
        }

        [Theory]
        [InlineData("bogus")]
        [InlineData("left right")]
        [InlineData("auto")]
        [InlineData("justify-all center")]
        public void TextAlignRejectsInvalid(string value)
        {
            var property = ParseDeclaration("text-align: " + value);

            Assert.False(property != null && property.HasValue);
        }

        [Theory]
        [InlineData("start", "auto", "start")]
        [InlineData("center", "auto", "center")]
        [InlineData("justify", "auto", "justify")]
        [InlineData("justify", "justify", "justify-all")]
        public void TextAlignLonghandsSerializeBackToShorthand(string all, string last, string expected)
        {
            var style = ParseDeclarations("text-align-all: " + all + "; text-align-last: " + last);

            Assert.Equal(expected, style.TextAlign);
            Assert.Contains("text-align:" + Compact(expected), Compact(style.CssText));
        }

        [Fact]
        public void TextAlignLonghandsWithDistinctLastStaySeparate()
        {
            var style = ParseDeclarations("text-align-all: left; text-align-last: center");

            Assert.True(string.IsNullOrEmpty(style.TextAlign));
            Assert.Equal("left", style.TextAlignAll);
            Assert.Equal("center", style.TextAlignLast);
        }

        [Fact]
        public void TextAlignLastAcceptsGlobalKeywordsAndStartEnd()
        {
            Assert.Equal("inherit", ParseDeclarations("text-align-last: inherit").TextAlignLast);
            Assert.Equal("end", ParseDeclarations("text-align-last: end").TextAlignLast);
        }

        [Fact]
        public void TextAlignKeepsWorkingAsDeclaration()
        {
            var property = ParseDeclaration("text-align:justify");

            Assert.Equal("text-align", property.Name);
            Assert.True(property.HasValue);
            Assert.Equal("justify", property.Value);
        }

        // ---- CSSOM accessors ----

        [Fact]
        public void CssomAccessorsRoundTrip()
        {
            var style = ParseDeclarations(
                "writing-mode: vertical-rl; text-orientation: upright; text-combine-upright: digits 2; line-break: strict");

            Assert.Equal("vertical-rl", style.WritingMode);
            Assert.Equal("upright", style.TextOrientation);
            Assert.Equal("digits 2", style.TextCombineUpright);
            Assert.Equal("strict", style.LineBreak);

            style = ParseDeclarations("white-space-collapse: break-spaces; text-wrap-mode: nowrap; text-wrap-style: balance");

            Assert.Equal("break-spaces", style.WhiteSpaceCollapse);
            Assert.Equal("nowrap", style.TextWrapMode);
            Assert.Equal("balance", style.TextWrapStyle);
        }

        private static string Compact(string css)
        {
            return css.Replace(" ", string.Empty).Replace("\r", string.Empty).Replace("\n", string.Empty);
        }

        private static StyleDeclaration ParseDeclarations(string declarations)
        {
            var parser = new StylesheetParser();
            var style = new StyleDeclaration(parser);
            style.Update(declarations);
            return style;
        }

        private static Property ParseDeclaration(string source)
        {
            return new StylesheetParser().ParseDeclaration(source);
        }
    }
}
