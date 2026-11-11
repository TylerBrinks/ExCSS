using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class ImagePropertiesTests : CssConstructionFunctions
    {
        [Theory]
        [InlineData("from-image")]
        [InlineData("FROM-IMAGE")]
        [InlineData("none")]
        [InlineData("0deg")]
        [InlineData("90deg")]
        [InlineData("-90deg")]
        [InlineData("100grad")]
        [InlineData("0.25turn")]
        [InlineData("1.5708rad")]
        [InlineData("flip")]
        [InlineData("90deg flip")]
        [InlineData("flip 90deg")]
        [InlineData("calc(90deg)")]
        [InlineData("calc(45deg + 45deg)")]
        [InlineData("calc(1turn / 4) flip")]
        [InlineData("flip calc(0.5turn)")]
        public void ImageOrientationLegalValues(string value)
        {
            var property = ParseDeclaration($"image-orientation: {value}");

            Assert.IsType<ImageOrientationProperty>(property);
            Assert.True(property.HasValue, value);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("bogus")]
        [InlineData("90")]
        [InlineData("0")]
        [InlineData("90px")]
        [InlineData("flip flip")]
        [InlineData("90deg 90deg")]
        [InlineData("90deg flip none")]
        [InlineData("none flip")]
        [InlineData("from-image flip")]
        [InlineData("90deg, flip")]
        [InlineData("calc(1px + 2px)")]
        [InlineData("calc(1 + 2)")]
        [InlineData("calc(45deg + 45deg) 90deg")]
        [InlineData("calc(45deg + 1px)")]
        public void ImageOrientationIllegalValues(string value)
        {
            var property = ParseDeclaration($"image-orientation: {value}");

            Assert.IsType<ImageOrientationProperty>(property);
            Assert.False(property.HasValue, value);
        }

        [Theory]
        [InlineData("from-image")]
        [InlineData("FROM-IMAGE")]
        [InlineData("300dpi")]
        [InlineData("2dppx")]
        [InlineData("2x")]
        [InlineData("118.11dpcm")]
        [InlineData("from-image 300dpi")]
        [InlineData("300dpi from-image")]
        [InlineData("300dpi snap")]
        [InlineData("snap from-image")]
        [InlineData("snap from-image 150dpi")]
        public void ImageResolutionLegalValues(string value)
        {
            var property = ParseDeclaration($"image-resolution: {value}");

            Assert.IsType<ImageResolutionProperty>(property);
            Assert.True(property.HasValue, value);
            Assert.Equal(value, property.Value);
        }

        [Theory]
        [InlineData("bogus")]
        [InlineData("snap")]
        [InlineData("0dpi")]
        [InlineData("-96dpi")]
        [InlineData("96")]
        [InlineData("96px")]
        [InlineData("90deg")]
        [InlineData("300dpi 600dpi")]
        [InlineData("from-image from-image")]
        [InlineData("snap snap 300dpi")]
        [InlineData("from-image 300dpi snap extra")]
        [InlineData("300dpi, snap")]
        public void ImageResolutionIllegalValues(string value)
        {
            var property = ParseDeclaration($"image-resolution: {value}");

            Assert.IsType<ImageResolutionProperty>(property);
            Assert.False(property.HasValue, value);
        }

        [Fact]
        public void ImagePropertiesAreInherited()
        {
            Assert.True(PropertyFactory.Instance.Create("image-orientation").IsInherited);
            Assert.True(PropertyFactory.Instance.Create("image-resolution").IsInherited);
        }

        [Fact]
        public void ImagePropertiesAcceptGlobalKeywords()
        {
            Assert.True(ParseDeclaration("image-orientation: inherit").HasValue);
            Assert.True(ParseDeclaration("image-resolution: initial").HasValue);
        }

        [Fact]
        public void StyleDeclarationExposesImageAccessors()
        {
            var style = ((StyleRule)ParseStyleSheet(
                ".a { image-orientation: 90deg flip; image-resolution: from-image 2x }").Rules[0]).Style;

            Assert.Equal("90deg flip", style.ImageOrientation);
            Assert.Equal("from-image 2x", style.ImageResolution);
        }
    }
}
