namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;

    //[TestFixture]
    public class GradientTests : CssConstructionFunctions
    {
        [Fact]
        public void InLinearGradient()
        {
            var source = "linear-gradient(135deg, red, blue)";
            var value = ParseValue(source);
            Assert.Equal(1, value.Count);
            Assert.Equal("linear-gradient", value[0].Data);
        }

        [Fact]
        public void InRadialGradient()
        {
            var source = "radial-gradient(ellipse farthest-corner at 45px 45px , #00FFFF, rgba(0, 0, 255, 0) 50%, #0000FF 95%)";
            var value = ParseValue(source);
            Assert.Equal("radial-gradient", value[0].Data);
        }

        [Fact]
        public void BackgroundImageLinearGradientWithAngle()
        {
            var source = "background-image: linear-gradient(135deg, red, blue)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<LinearGradient>(image);
            //var gradient = image as LinearGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Angle.TripleHalfQuarter.Value, gradient.Angle);
            //Assert.Equal(2, gradient.Stops.Count());
            //Assert.Equal(Color.Red, gradient.Stops.First().Color);
            //Assert.Equal(Color.Blue, gradient.Stops.Last().Color);
        }

        [Fact]
        public void BackgroundImageLinearGradientWithSide()
        {
            var source = "background-image: linear-gradient(to right, red, orange, yellow, green, blue, indigo, violet)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<LinearGradient>(image);
            //var gradient = image as LinearGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Angle.Quarter.Value, gradient.Angle);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(7, stops.Length);
            //Assert.Equal(Colors.GetColor("red").Value, stops[0].Color);
            //Assert.Equal(Colors.GetColor("orange").Value, stops[1].Color);
            //Assert.Equal(Colors.GetColor("yellow").Value, stops[2].Color);
            //Assert.Equal(Colors.GetColor("green").Value, stops[3].Color);
            //Assert.Equal(Colors.GetColor("blue").Value, stops[4].Color);
            //Assert.Equal(Colors.GetColor("indigo").Value, stops[5].Color);
            //Assert.Equal(Colors.GetColor("violet").Value, stops[6].Color);
        }

        [Fact]
        public void BackgroundImageLinearGradientWithCornerAndRgba()
        {
            var source = "background-image: linear-gradient(to bottom right, red, rgba(255,0,0,0))";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<LinearGradient>(image);
            //var gradient = image as LinearGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Angle.TripleHalfQuarter.Value, gradient.Angle);
            //Assert.Equal(2, gradient.Stops.Count());
            //Assert.Equal(Color.Red, gradient.Stops.First().Color);
            //Assert.Equal(Color.FromRgba(255, 0, 0, 0), gradient.Stops.Last().Color);
        }

        [Fact]
        public void BackgroundImageLinearGradientWithSideAndHsl()
        {
            var source = "background-image: linear-gradient(to bottom, hsl(0, 80%, 70%), #bada55)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<LinearGradient>(image);
            //var gradient = image as LinearGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Angle.Half.Value, gradient.Angle);
            //Assert.Equal(2, gradient.Stops.Count());
            //Assert.Equal(Color.FromHsl(0f, 0.8f, 0.7f), gradient.Stops.First().Color);
            //Assert.Equal(Color.FromHex("bada55"), gradient.Stops.Last().Color);
        }

        [Fact]
        public void BackgroundImageLinearGradientNoAngle()
        {
            var source = "background-image: linear-gradient(yellow, blue 20%, #0f0)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<LinearGradient>(image);
            //var gradient = image as LinearGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Angle.Half.Value, gradient.Angle);
            //Assert.Equal(3, gradient.Stops.Count());
            //Assert.Equal(Colors.GetColor("yellow").Value, gradient.Stops.First().Color);
            //Assert.Equal(Colors.GetColor("blue").Value, gradient.Stops.Skip(1).First().Color);
            //Assert.Equal(Color.FromRgb(0, 255, 0), gradient.Stops.Skip(2).First().Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientCircleFarthestCorner()
        {
            var source = "background-image: radial-gradient(circle farthest-corner at 45px 45px , #00FFFF 0%, rgba(0, 0, 255, 0) 50%, #0000FF 95%)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(new Length(45f, Length.Unit.Px), gradient.X);
            //Assert.Equal(new Length(45f, Length.Unit.Px), gradient.Y);
            //Assert.Equal(true, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestCorner, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(3, stops.Length);
            //Assert.Equal(Color.FromRgb(0, 255, 255), stops[0].Color);
            //Assert.Equal(Color.FromRgba(0, 0, 255, 0), stops[1].Color);
            //Assert.Equal(Color.FromRgb(0, 0, 255), stops[2].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientEllipseFarthestCorner()
        {
            var source = "background-image: radial-gradient(ellipse farthest-corner at 470px 47px , #FFFF80 20%, rgba(204, 153, 153, 0.4) 30%, #E6E6FF 60%)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(new Length(470f, Length.Unit.Px), gradient.X);
            //Assert.Equal(new Length(47f, Length.Unit.Px), gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestCorner, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(3, stops.Length);
            //Assert.Equal(Color.FromRgb(0xFF, 0xFF, 0x80), stops[0].Color);
            //Assert.Equal(Color.FromRgba(204, 153, 153, 0.4f), stops[1].Color);
            //Assert.Equal(Color.FromRgb(0xE6, 0xE6, 0xFF), stops[2].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientFarthestCornerWithPoint()
        {
            var source = "background-image: radial-gradient(farthest-corner at 45px 45px , #FF0000 0%, #0000FF 100%)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(new Length(45f, Length.Unit.Px), gradient.X);
            //Assert.Equal(new Length(45f, Length.Unit.Px), gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestCorner, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(2, stops.Length);
            //Assert.Equal(Color.FromRgb(255, 0, 0), stops[0].Color);
            //Assert.Equal(Color.FromRgb(0, 0, 255), stops[1].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientSingleSize()
        {
            var source = "background-image: radial-gradient(16px at 60px 50% , #000000 0%, #000000 14px, rgba(0, 0, 0, 0.3) 18px, rgba(0, 0, 0, 0) 19px)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(new Length(60f, Length.Unit.Px), gradient.X);
            //Assert.Equal(Length.Half, gradient.Y);
            //Assert.Equal(true, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.None, gradient.Mode);
            //Assert.Equal(new Length(16f, Length.Unit.Px), gradient.Width);
            //Assert.Equal(new Length(16f, Length.Unit.Px), gradient.Height);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(4, stops.Length);
            //Assert.Equal(Color.FromRgb(0, 0, 0), stops[0].Color);
            //Assert.Equal(Color.FromRgb(0, 0, 0), stops[1].Color);
            //Assert.Equal(Color.FromRgba(0, 0, 0, 0.3), stops[2].Color);
            //Assert.Equal(Color.Transparent, stops[3].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientCircle()
        {
            var source = "background-image: radial-gradient(circle, yellow, green)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Length.Half, gradient.X);
            //Assert.Equal(Length.Half, gradient.Y);
            //Assert.Equal(true, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestCorner, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(2, stops.Length);
            //Assert.Equal(Color.FromName("yellow").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[1].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientOnlyGradientStops()
        {
            var source = "background-image: radial-gradient(yellow, green)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Length.Half, gradient.X);
            //Assert.Equal(Length.Half, gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestCorner, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(2, stops.Length);
            //Assert.Equal(Color.FromName("yellow").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[1].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientEllipseAtCenter()
        {
            var source = "background-image: radial-gradient(ellipse at center, yellow 0%, green 100%)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Length.Half, gradient.X);
            //Assert.Equal(Length.Half, gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestCorner, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(2, stops.Length);
            //Assert.Equal(Color.FromName("yellow").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[1].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientFarthestCornerWithoutPoint()
        {
            var source = "background-image: radial-gradient(farthest-corner, yellow, green)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Length.Half, gradient.X);
            //Assert.Equal(Length.Half, gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestCorner, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(2, stops.Length);
            //Assert.Equal(Color.FromName("yellow").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[1].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientClosestSideWithPoint()
        {
            var source = "background-image: radial-gradient(closest-side at 20px 30px, red, yellow, green)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(new Length(20f, Length.Unit.Px), gradient.X);
            //Assert.Equal(new Length(30f, Length.Unit.Px), gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.ClosestSide, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(3, stops.Length);
            //Assert.Equal(Color.FromName("red").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("yellow").Value, stops[1].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[2].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientSizeAndPoint()
        {
            var source = "background-image: radial-gradient(20px 30px at 20px 30px, red, yellow, green)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(new Length(20f, Length.Unit.Px), gradient.X);
            //Assert.Equal(new Length(30f, Length.Unit.Px), gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.None, gradient.Mode);
            //Assert.Equal(new Length(20f, Length.Unit.Px), gradient.Width);
            //Assert.Equal(new Length(30f, Length.Unit.Px), gradient.Height);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(3, stops.Length);
            //Assert.Equal(Color.FromName("red").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("yellow").Value, stops[1].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[2].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientClosestSideCircleShuffledWithPoint()
        {
            var source = "background-image: radial-gradient(closest-side circle at 20px 30px, red, yellow, green)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(new Length(20f, Length.Unit.Px), gradient.X);
            //Assert.Equal(new Length(30f, Length.Unit.Px), gradient.Y);
            //Assert.Equal(true, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.ClosestSide, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(3, stops.Length);
            //Assert.Equal(Color.FromName("red").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("yellow").Value, stops[1].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[2].Color);
        }

        [Fact]
        public void BackgroundImageRadialGradientFarthestSideLeftBottom()
        {
            var source = "background-image: radial-gradient(farthest-side at left bottom, red, yellow 50px, green);";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.False(gradient.IsRepeating);
            //Assert.Equal(Length.Zero, gradient.X);
            //Assert.Equal(Length.Full, gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestSide, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(3, stops.Length);
            //Assert.Equal(Color.FromName("red").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("yellow").Value, stops[1].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[2].Color);
        }

        [Fact]
        public void BackgroundImageRepeatingLinearGradientRedBlue()
        {
            var source = "background-image: repeating-linear-gradient(red, blue 20px, red 40px)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<LinearGradient>(image);
            //var gradient = image as LinearGradient;
            //Assert.True(gradient.IsRepeating);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(3, stops.Length);
            //Assert.Equal(Color.FromName("red").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("blue").Value, stops[1].Color);
            //Assert.Equal(Color.FromName("red").Value, stops[2].Color);
        }

        [Fact]
        public void BackgroundImageRepeatingRadialGradientRedBlue()
        {
            var source = "background-image: repeating-radial-gradient(red, blue 20px, red 40px)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.True(gradient.IsRepeating);
            //Assert.Equal(Length.Half, gradient.X);
            //Assert.Equal(Length.Half, gradient.Y);
            //Assert.Equal(false, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.FarthestCorner, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(3, stops.Length);
            //Assert.Equal(Color.FromName("red").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("blue").Value, stops[1].Color);
            //Assert.Equal(Color.FromName("red").Value, stops[2].Color);
        }

        [Fact]
        public void BackgroundImageRepeatingRadialGradientFunky()
        {
            var source = "background-image: repeating-radial-gradient(circle closest-side at 20px 30px, red, yellow, green 100%, yellow 150%, red 200%)";
            var property = ParseDeclaration(source);
            Assert.IsType<BackgroundImageProperty>(property);
            var backgroundImage = property as BackgroundImageProperty;
            Assert.True(property.HasValue);
            Assert.False(property.IsInitial);
            //Assert.Equal(1, backgroundImage.Images.Count());
            //var image = backgroundImage.Images.First();
            //Assert.IsType<RadialGradient>(image);
            //var gradient = image as RadialGradient;
            //Assert.True(gradient.IsRepeating);
            //Assert.Equal(new Length(20f, Length.Unit.Px), gradient.X);
            //Assert.Equal(new Length(30f, Length.Unit.Px), gradient.Y);
            //Assert.Equal(true, gradient.IsCircle);
            //Assert.Equal(RadialGradient.SizeMode.ClosestSide, gradient.Mode);
            //var stops = gradient.Stops.ToArray();
            //Assert.Equal(5, stops.Length);
            //Assert.Equal(Color.FromName("red").Value, stops[0].Color);
            //Assert.Equal(Color.FromName("yellow").Value, stops[1].Color);
            //Assert.Equal(Color.FromName("green").Value, stops[2].Color);
            //Assert.Equal(Color.FromName("yellow").Value, stops[3].Color);
            //Assert.Equal(Color.FromName("red").Value, stops[4].Color);
        }
    }
}
