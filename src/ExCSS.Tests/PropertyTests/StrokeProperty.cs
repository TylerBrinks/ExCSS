namespace ExCSS.Tests
{
	using ExCSS;
	using Xunit;

	//[TestFixture]
	public class StrokePropertyTests : CssConstructionFunctions
	{
		[Fact]//[Test]
		public void StrokeColorRedLegal()
		{
			var snippet = "stroke: red";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeProperty>(property);
			var concrete = (StrokeProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("rgb(255, 0, 0)", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeColorHexLegal()
		{
			var snippet = "stroke: #0F0";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeProperty>(property);
			var concrete = (StrokeProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("rgb(0, 255, 0)", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeColorRgbaLegal()
		{
			var snippet = "stroke: rgba(1, 1, 1, 0)";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeProperty>(property);
			var concrete = (StrokeProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("rgba(1, 1, 1, 0)", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeColorRgbLegal()
		{
			var snippet = "stroke: rgb(1, 255, 100)";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeProperty>(property);
			var concrete = (StrokeProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("rgb(1, 255, 100)", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeNoneLegal()
		{
			var snippet = "stroke: none";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeProperty>(property);
			var concrete = (StrokeProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("none", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeColorRedRedIllegal()
		{
			var snippet = "stroke: red red";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeProperty>(property);
			var concrete = (StrokeProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}

		[Fact]//[Test]
		public void StrokeUrlLegal()
		{
			var snippet = "stroke: url(#linear)";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeProperty>(property);
			var concrete = (StrokeProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("url(\"#linear\")", concrete.Value);
		}


		[Fact]//[Test]
		public void StrokeDasharrayNumberNumberLegal()
		{
			var snippet = "stroke-dasharray: 5 5";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-dasharray", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeDasharrayProperty>(property);
			var concrete = (StrokeDasharrayProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("5 5", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeDasharrayLengthLengthLegal()
		{
			var snippet = "stroke-dasharray: 5px 5em";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-dasharray", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeDasharrayProperty>(property);
			var concrete = (StrokeDasharrayProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("5px 5em", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeDasharrayManyLegal()
		{
			var snippet = "stroke-dasharray: 1px 2em 3vh 4vw 5 6";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-dasharray", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeDasharrayProperty>(property);
			var concrete = (StrokeDasharrayProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("1px 2em 3vh 4vw 5 6", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeDasharrayNoneLegal()
		{
			var snippet = "stroke-dasharray: none";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-dasharray", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeDasharrayProperty>(property);
			var concrete = (StrokeDasharrayProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("none", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeDashoffsetLengthLegal()
		{
			var snippet = "stroke-dashoffset: 5px";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-dashoffset", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeDashoffsetProperty>(property);
			var concrete = (StrokeDashoffsetProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("5px", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeDashoffsetLengthLengthIllegal()
		{
			var snippet = "stroke-dashoffset: 5px 5px";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-dashoffset", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeDashoffsetProperty>(property);
			var concrete = (StrokeDashoffsetProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}

		[Fact]//[Test]
		public void StrokeDashoffsetPercentLegal()
		{
			var snippet = "stroke-dashoffset: 50%";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-dashoffset", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeDashoffsetProperty>(property);
			var concrete = (StrokeDashoffsetProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("50%", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeDashoffsetPercentPercentIllegal()
		{
			var snippet = "stroke-dashoffset: 50% 25%";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-dashoffset", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeDashoffsetProperty>(property);
			var concrete = (StrokeDashoffsetProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}

		[Fact]//[Test]
		public void StrokeLinecapButtLegal()
		{
			var snippet = "stroke-linecap: butt";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-linecap", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeLinecapProperty>(property);
			var concrete = (StrokeLinecapProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("butt", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeLinecapRoundLegal()
		{
			var snippet = "stroke-linecap: round";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-linecap", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeLinecapProperty>(property);
			var concrete = (StrokeLinecapProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("round", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeLinecapSquareLegal()
		{
			var snippet = "stroke-linecap: square";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-linecap", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeLinecapProperty>(property);
			var concrete = (StrokeLinecapProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("square", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeLinecapNoneIllegal()
		{
			var snippet = "stroke-linecap: none";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-linecap", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeLinecapProperty>(property);
			var concrete = (StrokeLinecapProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}

		[Fact]//[Test]
		public void StrokeLinejoinMiterLegal()
		{
			var snippet = "stroke-linejoin: miter";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-linejoin", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeLinejoinProperty>(property);
			var concrete = (StrokeLinejoinProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("miter", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeLinejoinRoundLegal()
		{
			var snippet = "stroke-linejoin: round";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-linejoin", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeLinejoinProperty>(property);
			var concrete = (StrokeLinejoinProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("round", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeLinejoinBevelLegal()
		{
			var snippet = "stroke-linejoin: bevel";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-linejoin", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeLinejoinProperty>(property);
			var concrete = (StrokeLinejoinProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("bevel", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeLinejoinNoneIllegal() {
			var snippet = "stroke-linejoin: none";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-linejoin", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeLinejoinProperty>(property);
			var concrete = (StrokeLinejoinProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}

		[Fact]//[Test]
		public void StrokeMiterlimitNumberLegal()
		{
			var snippet = "stroke-miterlimit: 2";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-miterlimit", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeMiterlimitProperty>(property);
			var concrete = (StrokeMiterlimitProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("2", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeMiterlimitNumberIlegal()
		{
			var snippet = "stroke-miterlimit: 0.5";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-miterlimit", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeMiterlimitProperty>(property);
			var concrete = (StrokeMiterlimitProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}

		[Fact]//[Test]
		public void StrokeMiterlimitNumberNumberIlegal()
		{
			var snippet = "stroke-miterlimit: 2 0.5";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-miterlimit", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeMiterlimitProperty>(property);
			var concrete = (StrokeMiterlimitProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}

		[Fact]//[Test]
		public void StrokeOpacitytNumberLegal()
		{
			var snippet = "stroke-opacity: 0.5";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-opacity", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeOpacityProperty>(property);
			var concrete = (StrokeOpacityProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("0.5", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeOpacityNumberNumberIllegal()
		{
			var snippet = "stroke-opacity: 0.5 0.5";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-opacity", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeOpacityProperty>(property);
			var concrete = (StrokeOpacityProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}
		
		[Fact]//[Test]
		public void StrokeWidthLengthLegal()
		{
			var snippet = "stroke-width: 5px";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-width", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeWidthProperty>(property);
			var concrete = (StrokeWidthProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("5px", concrete.Value);
		}


		[Fact]//[Test]
		public void StrokeWidthPercentLegal()
		{
			var snippet = "stroke-width: 5%";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-width", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeWidthProperty>(property);
			var concrete = (StrokeWidthProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.True(concrete.HasValue);
			Assert.Equal("5%", concrete.Value);
		}

		[Fact]//[Test]
		public void StrokeWidthNoneIllegal()
		{
			var snippet = "stroke-width: none";
			var property = ParseDeclaration(snippet);
			Assert.Equal("stroke-width", property.Name);
			Assert.False(property.IsImportant);
			Assert.IsType<StrokeWidthProperty>(property);
			var concrete = (StrokeWidthProperty)property;
			Assert.False(concrete.IsInherited);
			Assert.False(concrete.HasValue);
		}
	}
}
