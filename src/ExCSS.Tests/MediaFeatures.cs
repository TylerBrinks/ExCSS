namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;
    
    public class CssMediaFeaturesTests
    {
        [Fact]
        public void CssMediaFeatureFactory()
        {
            var aspectRatio = MediaFeatureFactory.Instance.Create(FeatureNames.AspectRatio);
            Assert.NotNull(aspectRatio);
            Assert.IsType<AspectRatioMediaFeature>(aspectRatio);
            Assert.False(aspectRatio.IsMaximum);
            Assert.False(aspectRatio.IsMinimum);

            var colorIndex = MediaFeatureFactory.Instance.Create(FeatureNames.ColorIndex);
            Assert.NotNull(colorIndex);
            Assert.IsType<ColorIndexMediaFeature>(colorIndex);
            Assert.False(colorIndex.IsMaximum);
            Assert.False(colorIndex.IsMinimum);

            var deviceWidth = MediaFeatureFactory.Instance.Create(FeatureNames.DeviceWidth);
            Assert.NotNull(deviceWidth);
            Assert.IsType<DeviceWidthMediaFeature>(deviceWidth);
            Assert.False(deviceWidth.IsMaximum);
            Assert.False(deviceWidth.IsMinimum);

            var monochrome = MediaFeatureFactory.Instance.Create(FeatureNames.MaxMonochrome);
            Assert.NotNull(monochrome);
            Assert.IsType<MonochromeMediaFeature>(monochrome);
            Assert.True(monochrome.IsMaximum);
            Assert.False(monochrome.IsMinimum);

            var illegal = MediaFeatureFactory.Instance.Create("illegal");
            Assert.Null(illegal);
        }

        [Fact]
        public void CssMediaWidthValidation()
        {
            var width = new WidthMediaFeature(FeatureNames.Width);
            /*var check = width.TrySetValue(new Length(100, Length.Unit.Px));
            var valid = width.Validate(new RenderDevice(100, 0));
            var invalid = width.Validate(new RenderDevice(0, 0));
            Assert.True(check);
            Assert.True(valid);
            Assert.False(invalid);*/
        }

        [Fact]
        public void CssMediaMaxHeightValidation()
        {
            var height = new HeightMediaFeature(FeatureNames.MaxHeight);
            /*var check = height.TrySetValue(new Length(100, Length.Unit.Px));
            var valid = height.Validate(new RenderDevice(0, 99));
            var invalid = height.Validate(new RenderDevice(0, 101));
            Assert.True(check);
            Assert.True(valid);
            Assert.False(invalid);*/
        }

        [Fact]
        public void CssMediaMinDeviceWidthValidation()
        {
            var devwidth = new DeviceWidthMediaFeature(FeatureNames.MinDeviceWidth);
            /*var check = devwidth.TrySetValue(new Length(100, Length.Unit.Px));
            var valid = devwidth.Validate(new RenderDevice(101, 0));
            var invalid = devwidth.Validate(new RenderDevice(99, 0));
            Assert.True(check);
            Assert.True(valid);
            Assert.False(invalid);*/
        }

        [Fact]
        public void CssMediaAspectRatio()
        {
            var ratio = new AspectRatioMediaFeature(FeatureNames.AspectRatio);
            /*var check = ratio.TrySetValue(new CssValueList(new List<ICssValue>(new ICssValue[] {
                new Number(1f, Number.Unit.Integer),
                null, //CssValue.Delimiter,
                new Number(1f, Number.Unit.Integer)
            })));
            var valid = ratio.Validate(new RenderDevice(100, 100));
            var invalid = ratio.Validate(new RenderDevice(16, 9));
            Assert.True(check);
            Assert.True(valid);
            Assert.False(invalid);*/
        }

        [Theory]
        [InlineData("(min-width: 600px)", MediaFeatureComparison.GreaterThanOrEqual)]
        [InlineData("(max-width: 600px)", MediaFeatureComparison.LessThanOrEqual)]
        [InlineData("(width: 600px)", MediaFeatureComparison.Equal)]
        [InlineData("(width > 600px)", MediaFeatureComparison.GreaterThan)]
        [InlineData("(width >= 600px)", MediaFeatureComparison.GreaterThanOrEqual)]
        [InlineData("(width < 600px)", MediaFeatureComparison.LessThan)]
        [InlineData("(width <= 600px)", MediaFeatureComparison.LessThanOrEqual)]
        public void CssMediaFeatureComparison(string condition, MediaFeatureComparison expected)
        {
            var parser = new StylesheetParser();
            var sheet = parser.Parse($"@media {condition} {{ h1 {{ color: red }} }}");
            var media = Assert.IsType<MediaRule>(sheet.Rules[0]);

            var feature = Assert.Single(Assert.Single(media.Media.Media).Features);
            Assert.Equal(expected, feature.Comparison);
            Assert.Equal(condition, media.ConditionText);
        }
    }
}
