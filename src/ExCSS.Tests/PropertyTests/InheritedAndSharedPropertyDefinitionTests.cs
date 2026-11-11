using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class InheritedAndSharedPropertyDefinitionTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("caption-side: bottom")]
        [InlineData("fill: red")]
        [InlineData("fill-opacity: 0.5")]
        [InlineData("fill-rule: evenodd")]
        [InlineData("stroke: red")]
        [InlineData("stroke-dasharray: 4 2")]
        [InlineData("stroke-dashoffset: 3px")]
        [InlineData("stroke-linecap: round")]
        [InlineData("stroke-linejoin: round")]
        [InlineData("stroke-miterlimit: 4")]
        [InlineData("stroke-opacity: 0.5")]
        [InlineData("stroke-width: 2px")]
        [InlineData("overflow-wrap: break-word")]
        [InlineData("word-wrap: break-word")]
        [InlineData("word-break: break-all")]
        [InlineData("text-align-last: center")]
        [InlineData("text-anchor: middle")]
        [InlineData("text-justify: inter-word")]
        public void PropertyIsInherited(string declaration)
        {
            var property = ParseDeclaration(declaration);
            Assert.NotNull(property);
            Assert.True(property.CanBeInherited);
        }

        [Theory]
        [InlineData("text-align-last: inherit")]
        [InlineData("text-align-last: initial")]
        [InlineData("text-align-last: unset")]
        public void TextAlignLastAcceptsCssWideKeywords(string declaration)
        {
            var property = ParseDeclaration(declaration);
            Assert.NotNull(property);
            Assert.True(property.HasValue);
            Assert.True(property.CanBeInherited);
        }

        [Theory]
        [InlineData("outline-offset: 2px")]
        [InlineData("perspective: 100px")]
        [InlineData("column-gap: normal")]
        [InlineData("row-gap: normal")]
        public void NonInheritedPropertiesStayNonInherited(string declaration)
        {
            var property = ParseDeclaration(declaration);
            Assert.NotNull(property);
            Assert.False(property.CanBeInherited);
        }

        [Theory]
        [InlineData("normal")]
        [InlineData("10px")]
        [InlineData("5%")]
        [InlineData("1em")]
        [InlineData("inherit")]
        public void ColumnGapLegalValues(string value)
            => TestForLegalValue<ColumnGapProperty>(PropertyNames.ColumnGap, value);

        [Theory]
        [InlineData("normal")]
        [InlineData("10px")]
        [InlineData("5%")]
        [InlineData("initial")]
        public void RowGapLegalValues(string value)
            => TestForLegalValue<RowGapProperty>(PropertyNames.RowGap, value);

        [Theory]
        [InlineData("column-gap: none")]
        [InlineData("column-gap: 10")]
        [InlineData("row-gap: auto")]
        [InlineData("row-gap: normal normal")]
        [InlineData("gap: 1px 2px 3px")]
        public void GapIllegalValues(string declaration)
        {
            var property = ParseDeclaration(declaration);
            Assert.True(property == null || !property.HasValue);
        }

        [Theory]
        [InlineData("normal")]
        [InlineData("normal 10px")]
        [InlineData("10px normal")]
        public void GapShorthandAcceptsNormal(string value)
            => TestForLegalValue<GapProperty>(PropertyNames.Gap, value);

        [Fact]
        public void GapShorthandNormalExpands()
        {
            var style = ParseDeclarations("gap: normal 10px");
            Assert.Equal("normal", style.RowGap);
            Assert.Equal("10px", style.ColumnGap);
        }

        [Theory]
        [InlineData("0")]
        [InlineData("2px")]
        [InlineData("-3px")]
        [InlineData("1em")]
        [InlineData("inherit")]
        public void OutlineOffsetLegalValues(string value)
            => TestForLegalValue<OutlineOffsetProperty>(PropertyNames.OutlineOffset, value);

        [Theory]
        [InlineData("outline-offset: 50%")]
        [InlineData("outline-offset: auto")]
        [InlineData("outline-offset: 1")]
        public void OutlineOffsetIllegalValues(string declaration)
        {
            var property = ParseDeclaration(declaration);
            Assert.True(property == null || !property.HasValue);
        }

        [Fact]
        public void OutlineOffsetStyleDeclarationAccessor()
        {
            var style = ParseDeclarations("outline-offset: 4px");
            Assert.Equal("4px", style.OutlineOffset);
        }

        [Theory]
        [InlineData("none")]
        [InlineData("100px")]
        [InlineData("2em")]
        [InlineData("initial")]
        public void PerspectiveLegalValues(string value)
            => TestForLegalValue<PerspectiveProperty>(PropertyNames.Perspective, value);

        [Theory]
        [InlineData("perspective: 50%")]
        [InlineData("perspective: auto")]
        public void PerspectiveIllegalValues(string declaration)
        {
            var property = ParseDeclaration(declaration);
            Assert.True(property == null || !property.HasValue);
        }

        [Theory]
        [InlineData("2")]
        [InlineData("1.5")]
        [InlineData("2px")]
        [InlineData("10%")]
        public void StrokeWidthLegalValues(string value)
            => TestForLegalValue<StrokeWidthProperty>(PropertyNames.StrokeWidth, value);

        [Theory]
        [InlineData("3")]
        [InlineData("-1.5")]
        [InlineData("3px")]
        [InlineData("10%")]
        public void StrokeDashoffsetLegalValues(string value)
            => TestForLegalValue<StrokeDashoffsetProperty>(PropertyNames.StrokeDashoffset, value);

        [Theory]
        [InlineData("stroke-width: red")]
        [InlineData("stroke-width: 1 2")]
        [InlineData("stroke-dashoffset: auto")]
        public void StrokeNumberIllegalValues(string declaration)
        {
            var property = ParseDeclaration(declaration);
            Assert.True(property == null || !property.HasValue);
        }

        [Theory]
        [InlineData("0.5")]
        [InlineData("1")]
        [InlineData("50%")]
        public void FillOpacityLegalValues(string value)
            => TestForLegalValue<FillOpacityProperty>(PropertyNames.FillOpacity, value);

        [Theory]
        [InlineData("0.5")]
        [InlineData("1")]
        [InlineData("50%")]
        public void StrokeOpacityLegalValues(string value)
            => TestForLegalValue<StrokeOpacityProperty>(PropertyNames.StrokeOpacity, value);

        [Theory]
        [InlineData("fill-opacity: 5px")]
        [InlineData("stroke-opacity: red")]
        public void OpacityIllegalValues(string declaration)
        {
            var property = ParseDeclaration(declaration);
            Assert.True(property == null || !property.HasValue);
        }
    }
}
