using NUnit.Framework;

namespace ExCSS.Tests
{
	[TestFixture]
    public partial class AttributeTestFixture
    { 
		[Test]
		public void accelerator_true()
		{
			var termParts = new[]{"true"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{accelerator: true }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("true", expression.Terms[0].ToString());
		}
		[Test]
		public void accelerator_false()
		{
			var termParts = new[]{"false"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{accelerator: false }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("false", expression.Terms[0].ToString());
		}
		[Test]
		public void azimuth_left_side()
		{
			var termParts = new[]{"left-side"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{azimuth: left-side }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("left-side", expression.Terms[0].ToString());
		}
		[Test]
		public void azimuth_left()
		{
			var termParts = new[]{"left"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{azimuth: left }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("left", expression.Terms[0].ToString());
		}
		[Test]
		public void background_transparent_0_0()
		{
			var termParts = new[]{"transparent","0","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background: transparent 0 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("transparent", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
			Assert.AreEqual("0", expression.Terms[2].ToString());
		}
		[Test]
		public void background_none_repeat_scroll_0_0_transparent()
		{
			var termParts = new[]{"none","repeat","scroll","0","0","transparent"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background: none repeat scroll 0 0 transparent }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
			Assert.AreEqual("repeat", expression.Terms[1].ToString());
			Assert.AreEqual("scroll", expression.Terms[2].ToString());
			Assert.AreEqual("0", expression.Terms[3].ToString());
			Assert.AreEqual("0", expression.Terms[4].ToString());
			Assert.AreEqual("transparent", expression.Terms[5].ToString());
		}
		[Test]
		public void background_FFF_urlimagesimggif()
		{
			var termParts = new[]{"#FFF","url(images/img.gif)"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background: #FFF url(images/img.gif) }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#FFF", expression.Terms[0].ToString());
			Assert.AreEqual("url(images/img.gif)", expression.Terms[1].ToString());
		}
		[Test]
		public void background_CCC_transparent_urlimagesimgpng_fixed_repeat_x_repeat_y_no_repeat_top_left_center_right_bottom_1px_2px()
		{
			var termParts = new[]{"#CCC","transparent","url(images/img.png)","fixed","repeat-x","repeat-y","no-repeat","top","left","center","right","bottom","1px","2px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background: #CCC transparent url(images/img.png) fixed repeat-x repeat-y no-repeat top left center right bottom 1px 2px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
			Assert.AreEqual("transparent", expression.Terms[1].ToString());
			Assert.AreEqual("url(images/img.png)", expression.Terms[2].ToString());
			Assert.AreEqual("fixed", expression.Terms[3].ToString());
			Assert.AreEqual("repeat-x", expression.Terms[4].ToString());
			Assert.AreEqual("repeat-y", expression.Terms[5].ToString());
			Assert.AreEqual("no-repeat", expression.Terms[6].ToString());
			Assert.AreEqual("top", expression.Terms[7].ToString());
			Assert.AreEqual("left", expression.Terms[8].ToString());
			Assert.AreEqual("center", expression.Terms[9].ToString());
			Assert.AreEqual("right", expression.Terms[10].ToString());
			Assert.AreEqual("bottom", expression.Terms[11].ToString());
			Assert.AreEqual("1px", expression.Terms[12].ToString());
			Assert.AreEqual("2px", expression.Terms[13].ToString());
		}
		[Test]
		public void background_attachment_fixed()
		{
			var termParts = new[]{"fixed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-attachment: fixed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("fixed", expression.Terms[0].ToString());
		}
		[Test]
		public void background_attachment_scroll()
		{
			var termParts = new[]{"scroll"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-attachment: scroll }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("scroll", expression.Terms[0].ToString());
		}
		[Test]
		public void background_color_CCC_transparent()
		{
			var termParts = new[]{"#CCC","transparent"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-color: #CCC transparent }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
			Assert.AreEqual("transparent", expression.Terms[1].ToString());
		}
		[Test]
		public void background_color_CCC()
		{
			var termParts = new[]{"#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-color: #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
		}
		[Test]
		public void background_color_Silver()
		{
			var termParts = new[]{"Silver"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-color: Silver }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("Silver", expression.Terms[0].ToString());
		}
		[Test]
		public void background_image_none()
		{
			var termParts = new[]{"none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-image: none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
		}
		[Test]
		public void background_image_transparent_scroll_0_0()
		{
			var termParts = new[]{"transparent","scroll","0","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-image: transparent scroll 0 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("transparent", expression.Terms[0].ToString());
			Assert.AreEqual("scroll", expression.Terms[1].ToString());
			Assert.AreEqual("0", expression.Terms[2].ToString());
			Assert.AreEqual("0", expression.Terms[3].ToString());
		}
		[Test]
		public void background_image_0_0_fixed()
		{
			var termParts = new[]{"0","0","fixed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-image: 0 0 fixed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
			Assert.AreEqual("fixed", expression.Terms[2].ToString());
		}
		[Test]
		public void background_position_top_left()
		{
			var termParts = new[]{"top","left"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: top left }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("top", expression.Terms[0].ToString());
			Assert.AreEqual("left", expression.Terms[1].ToString());
		}
		[Test]
		public void background_position_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void background_position_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void background_position_5px_10Percent()
		{
			var termParts = new[]{"5px","10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 5px 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10%", expression.Terms[1].ToString());
		}
		[Test]
		public void background_position_5Percent_10px()
		{
			var termParts = new[]{"5%","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 5% 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void background_position_0()
		{
			var termParts = new[]{"0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
		}
		[Test]
		public void background_position_0_0()
		{
			var termParts = new[]{"0","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 0 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
		}
		[Test]
		public void background_position_0_0_0()
		{
			var termParts = new[]{"0","0","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 0 0 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
			Assert.AreEqual("0", expression.Terms[2].ToString());
		}
		[Test]
		public void background_position_0_0_0_0()
		{
			var termParts = new[]{"0","0","0","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 0 0 0 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
			Assert.AreEqual("0", expression.Terms[2].ToString());
			Assert.AreEqual("0", expression.Terms[3].ToString());
		}
		[Test]
		public void background_position_0px_0_0_0()
		{
			var termParts = new[]{"0px","0","0","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 0px 0 0 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0px", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
			Assert.AreEqual("0", expression.Terms[2].ToString());
			Assert.AreEqual("0", expression.Terms[3].ToString());
		}
		[Test]
		public void background_position_0_0px_0_0()
		{
			var termParts = new[]{"0","0px","0","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 0 0px 0 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
			Assert.AreEqual("0px", expression.Terms[1].ToString());
			Assert.AreEqual("0", expression.Terms[2].ToString());
			Assert.AreEqual("0", expression.Terms[3].ToString());
		}
		[Test]
		public void background_position_0_0_0px()
		{
			var termParts = new[]{"0","0","0px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 0 0 0px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
			Assert.AreEqual("0px", expression.Terms[2].ToString());
		}
		[Test]
		public void background_position_0_0_0px_0()
		{
			var termParts = new[]{"0","0","0px","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 0 0 0px 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
			Assert.AreEqual("0px", expression.Terms[2].ToString());
			Assert.AreEqual("0", expression.Terms[3].ToString());
		}
		[Test]
		public void background_position_1px_2em_0_3px()
		{
			var termParts = new[]{"1px","2em","0","3px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: 1px 2em 0 3px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("1px", expression.Terms[0].ToString());
			Assert.AreEqual("2em", expression.Terms[1].ToString());
			Assert.AreEqual("0", expression.Terms[2].ToString());
			Assert.AreEqual("3px", expression.Terms[3].ToString());
		}
		[Test]
		public void background_position_inherit()
		{
			var termParts = new[]{"inherit"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position: inherit }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("inherit", expression.Terms[0].ToString());
		}
		[Test]
		public void background_position_x_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position-x: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void background_position_x_10px()
		{
			var termParts = new[]{"10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position-x: 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10px", expression.Terms[0].ToString());
		}
		[Test]
		public void background_position_x_inherit()
		{
			var termParts = new[]{"inherit"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-position-x: inherit }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("inherit", expression.Terms[0].ToString());
		}
		[Test]
		public void background_repeat_repeat()
		{
			var termParts = new[]{"repeat"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-repeat: repeat }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("repeat", expression.Terms[0].ToString());
		}
		[Test]
		public void background_repeat_repeat_x()
		{
			var termParts = new[]{"repeat-x"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-repeat: repeat-x }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("repeat-x", expression.Terms[0].ToString());
		}
		[Test]
		public void background_repeat_repeat_y()
		{
			var termParts = new[]{"repeat-y"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-repeat: repeat-y }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("repeat-y", expression.Terms[0].ToString());
		}
		[Test]
		public void background_repeat_no_repeat()
		{
			var termParts = new[]{"no-repeat"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{background-repeat: no-repeat }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("no-repeat", expression.Terms[0].ToString());
		}
		[Test]
		public void border_solid_1px_CCC()
		{
			var termParts = new[]{"solid","1px","#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: solid 1px #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("solid", expression.Terms[0].ToString());
			Assert.AreEqual("1px", expression.Terms[1].ToString());
			Assert.AreEqual("#CCC", expression.Terms[2].ToString());
		}
		[Test]
		public void border_1px_none()
		{
			var termParts = new[]{"1px","none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: 1px none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("1px", expression.Terms[0].ToString());
			Assert.AreEqual("none", expression.Terms[1].ToString());
		}
		[Test]
		public void border_solid()
		{
			var termParts = new[]{"solid"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: solid }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("solid", expression.Terms[0].ToString());
		}
		[Test]
		public void border_dashed()
		{
			var termParts = new[]{"dashed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: dashed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("dashed", expression.Terms[0].ToString());
		}
		[Test]
		public void border_double()
		{
			var termParts = new[]{"double"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: double }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("double", expression.Terms[0].ToString());
		}
		[Test]
		public void border_ridge()
		{
			var termParts = new[]{"ridge"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: ridge }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("ridge", expression.Terms[0].ToString());
		}
		[Test]
		public void border_groove()
		{
			var termParts = new[]{"groove"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: groove }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("groove", expression.Terms[0].ToString());
		}
		[Test]
		public void border_inset()
		{
			var termParts = new[]{"inset"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: inset }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("inset", expression.Terms[0].ToString());
		}
		[Test]
		public void border_outset()
		{
			var termParts = new[]{"outset"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border: outset }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("outset", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_solid_1px_CCC()
		{
			var termParts = new[]{"solid","1px","#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: solid 1px #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("solid", expression.Terms[0].ToString());
			Assert.AreEqual("1px", expression.Terms[1].ToString());
			Assert.AreEqual("#CCC", expression.Terms[2].ToString());
		}
		[Test]
		public void border_bottom_1px_none()
		{
			var termParts = new[]{"1px","none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: 1px none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("1px", expression.Terms[0].ToString());
			Assert.AreEqual("none", expression.Terms[1].ToString());
		}
		[Test]
		public void border_bottom_solid()
		{
			var termParts = new[]{"solid"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: solid }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("solid", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_dashed()
		{
			var termParts = new[]{"dashed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: dashed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("dashed", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_double()
		{
			var termParts = new[]{"double"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: double }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("double", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_ridge()
		{
			var termParts = new[]{"ridge"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: ridge }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("ridge", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_groove()
		{
			var termParts = new[]{"groove"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: groove }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("groove", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_inset()
		{
			var termParts = new[]{"inset"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: inset }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("inset", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_outset()
		{
			var termParts = new[]{"outset"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom: outset }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("outset", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_color_CCC()
		{
			var termParts = new[]{"#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom-color: #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_style_dashed()
		{
			var termParts = new[]{"dashed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom-style: dashed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("dashed", expression.Terms[0].ToString());
		}
		[Test]
		public void border_bottom_width_2px()
		{
			var termParts = new[]{"2px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-bottom-width: 2px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("2px", expression.Terms[0].ToString());
		}
		[Test]
		public void border_collapse_collapse()
		{
			var termParts = new[]{"collapse"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-collapse: collapse }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("collapse", expression.Terms[0].ToString());
		}
		[Test]
		public void border_collapse_separate()
		{
			var termParts = new[]{"separate"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-collapse: separate }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("separate", expression.Terms[0].ToString());
		}
		[Test]
		public void border_color_CCC()
		{
			var termParts = new[]{"#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-color: #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_solid_1px_CCC()
		{
			var termParts = new[]{"solid","1px","#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: solid 1px #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("solid", expression.Terms[0].ToString());
			Assert.AreEqual("1px", expression.Terms[1].ToString());
			Assert.AreEqual("#CCC", expression.Terms[2].ToString());
		}
		[Test]
		public void border_left_1px_none()
		{
			var termParts = new[]{"1px","none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: 1px none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("1px", expression.Terms[0].ToString());
			Assert.AreEqual("none", expression.Terms[1].ToString());
		}
		[Test]
		public void border_left_solid()
		{
			var termParts = new[]{"solid"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: solid }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("solid", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_dashed()
		{
			var termParts = new[]{"dashed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: dashed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("dashed", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_double()
		{
			var termParts = new[]{"double"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: double }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("double", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_ridge()
		{
			var termParts = new[]{"ridge"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: ridge }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("ridge", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_groove()
		{
			var termParts = new[]{"groove"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: groove }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("groove", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_inset()
		{
			var termParts = new[]{"inset"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: inset }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("inset", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_outset()
		{
			var termParts = new[]{"outset"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left: outset }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("outset", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_style_dashed()
		{
			var termParts = new[]{"dashed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left-style: dashed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("dashed", expression.Terms[0].ToString());
		}
		[Test]
		public void border_left_width_2px()
		{
			var termParts = new[]{"2px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-left-width: 2px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("2px", expression.Terms[0].ToString());
		}
		[Test]
		public void border_radius_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-radius: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void border_radius_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-radius: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void border_radius_5px_10Percent()
		{
			var termParts = new[]{"5px","10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-radius: 5px 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10%", expression.Terms[1].ToString());
		}
		[Test]
		public void border_radius_5Percent_10px()
		{
			var termParts = new[]{"5%","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-radius: 5% 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void border_radius_5px_10px()
		{
			var termParts = new[]{"5px","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-radius: 5px 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void border_spacing_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-spacing: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void border_spacing_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-spacing: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void border_spacing_5px_10Percent()
		{
			var termParts = new[]{"5px","10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-spacing: 5px 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10%", expression.Terms[1].ToString());
		}
		[Test]
		public void border_spacing_5Percent_10px()
		{
			var termParts = new[]{"5%","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-spacing: 5% 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void border_spacing_5px_10px()
		{
			var termParts = new[]{"5px","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-spacing: 5px 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void border_style_dashed()
		{
			var termParts = new[]{"dashed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{border-style: dashed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("dashed", expression.Terms[0].ToString());
		}
		[Test]
		public void bottom_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{bottom: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void bottom_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{bottom: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void caption_side_top()
		{
			var termParts = new[]{"top"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{caption-side: top }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("top", expression.Terms[0].ToString());
		}
		[Test]
		public void caption_side_bottom()
		{
			var termParts = new[]{"bottom"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{caption-side: bottom }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("bottom", expression.Terms[0].ToString());
		}
		[Test]
		public void caption_side_left()
		{
			var termParts = new[]{"left"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{caption-side: left }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("left", expression.Terms[0].ToString());
		}
		[Test]
		public void caption_side_right()
		{
			var termParts = new[]{"right"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{caption-side: right }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("right", expression.Terms[0].ToString());
		}
		[Test]
		public void clear_none()
		{
			var termParts = new[]{"none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{clear: none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
		}
		[Test]
		public void clear_left()
		{
			var termParts = new[]{"left"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{clear: left }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("left", expression.Terms[0].ToString());
		}
		[Test]
		public void clear_right()
		{
			var termParts = new[]{"right"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{clear: right }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("right", expression.Terms[0].ToString());
		}
		[Test]
		public void clear_both()
		{
			var termParts = new[]{"both"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{clear: both }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("both", expression.Terms[0].ToString());
		}
		[Test]
		public void clip_auto()
		{
			var termParts = new[]{"auto"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{clip: auto }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("auto", expression.Terms[0].ToString());
		}
		[Test]
		public void color_CCC()
		{
			var termParts = new[]{"#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{color: #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
		}
		[Test]
		public void color_Red()
		{
			var termParts = new[]{"Red"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{color: Red }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("Red", expression.Terms[0].ToString());
		}
		[Test]
		public void content_none()
		{
			var termParts = new[]{"none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{content: none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
		}
		[Test]
		public void content_open_quote()
		{
			var termParts = new[]{"open-quote"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{content: open-quote }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("open-quote", expression.Terms[0].ToString());
		}
		[Test]
		public void content_inherit()
		{
			var termParts = new[]{"inherit"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{content: inherit }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("inherit", expression.Terms[0].ToString());
		}
		[Test]
		public void content_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{content:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void counter_increment_none()
		{
			var termParts = new[]{"none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{counter-increment: none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
		}
		[Test]
		public void counter_increment_subsection()
		{
			var termParts = new[]{"subsection"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{counter-increment: subsection }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("subsection", expression.Terms[0].ToString());
		}
		[Test]
		public void counter_reset_none()
		{
			var termParts = new[]{"none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{counter-reset: none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
		}
		[Test]
		public void counter_reset_subsection()
		{
			var termParts = new[]{"subsection"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{counter-reset: subsection }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("subsection", expression.Terms[0].ToString());
		}
		[Test]
		public void cue_cue_before()
		{
			var termParts = new[]{"cue-before"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{cue: cue-before }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("cue-before", expression.Terms[0].ToString());
		}
		[Test]
		public void cue_cue_after()
		{
			var termParts = new[]{"cue-after"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{cue: cue-after }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("cue-after", expression.Terms[0].ToString());
		}
		[Test]
		public void cursor_auto()
		{
			var termParts = new[]{"auto"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{cursor: auto }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("auto", expression.Terms[0].ToString());
		}
		[Test]
		public void cursor_e_resize()
		{
			var termParts = new[]{"e-resize"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{cursor: e-resize }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("e-resize", expression.Terms[0].ToString());
		}
		[Test]
		public void cursor_se_resize()
		{
			var termParts = new[]{"se-resize"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{cursor: se-resize }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("se-resize", expression.Terms[0].ToString());
		}
		[Test]
		public void direction_ltr()
		{
			var termParts = new[]{"ltr"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{direction: ltr }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("ltr", expression.Terms[0].ToString());
		}
		[Test]
		public void direction_rtl()
		{
			var termParts = new[]{"rtl"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{direction: rtl }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("rtl", expression.Terms[0].ToString());
		}
		[Test]
		public void display_none()
		{
			var termParts = new[]{"none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{display: none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
		}
		[Test]
		public void display_inline_block()
		{
			var termParts = new[]{"inline-block"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{display: inline-block }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("inline-block", expression.Terms[0].ToString());
		}
		[Test]
		public void display_table_row_group()
		{
			var termParts = new[]{"table-row-group"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{display: table-row-group }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("table-row-group", expression.Terms[0].ToString());
		}
		[Test]
		public void empty_cells_none()
		{
			var termParts = new[]{"none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{empty-cells: none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
		}
		[Test]
		public void empty_cells_show()
		{
			var termParts = new[]{"show"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{empty-cells: show }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("show", expression.Terms[0].ToString());
		}
		[Test]
		public void float_left()
		{
			var termParts = new[]{"left"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{float: left }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("left", expression.Terms[0].ToString());
		}
		[Test]
		public void font_italic_bold_12px30px_Georgia_serif()
		{
			var termParts = new[]{"italic","bold","12px/30px","Georgia","serif"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font: italic bold 12px/30px Georgia,serif }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("italic", expression.Terms[0].ToString());
			Assert.AreEqual("bold", expression.Terms[1].ToString());
			Assert.AreEqual("12px/30px", expression.Terms[2].ToString());
			Assert.AreEqual("Georgia", expression.Terms[3].ToString());
			Assert.AreEqual("serif", expression.Terms[4].ToString());
		}
		[Test]
		public void font_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void font_family_verdana_helvetica_arial_sans_serif()
		{
			var termParts = new[]{"verdana","helvetica","arial","sans-serif"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-family: verdana,helvetica,arial,sans-serif }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("verdana", expression.Terms[0].ToString());
			Assert.AreEqual("helvetica", expression.Terms[1].ToString());
			Assert.AreEqual("arial", expression.Terms[2].ToString());
			Assert.AreEqual("sans-serif", expression.Terms[3].ToString());
		}
		[Test]
		public void font_family_icon()
		{
			var termParts = new[]{"icon"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-family: icon }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("icon", expression.Terms[0].ToString());
		}
		[Test]
		public void font_family_message_box()
		{
			var termParts = new[]{"message-box"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-family: message-box }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("message-box", expression.Terms[0].ToString());
		}
		[Test]
		public void font_size_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-size: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void font_size_250px()
		{
			var termParts = new[]{"250px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-size: 250px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("250px", expression.Terms[0].ToString());
		}
		[Test]
		public void font_style_bold()
		{
			var termParts = new[]{"bold"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-style: bold }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("bold", expression.Terms[0].ToString());
		}
		[Test]
		public void font_style_oblique()
		{
			var termParts = new[]{"oblique"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-style: oblique }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("oblique", expression.Terms[0].ToString());
		}
		[Test]
		public void font_variant_none()
		{
			var termParts = new[]{"none"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-variant: none }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("none", expression.Terms[0].ToString());
		}
		[Test]
		public void font_variant_small_caps()
		{
			var termParts = new[]{"small-caps"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-variant: small-caps }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("small-caps", expression.Terms[0].ToString());
		}
		[Test]
		public void font_weight_bold()
		{
			var termParts = new[]{"bold"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-weight: bold }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("bold", expression.Terms[0].ToString());
		}
		[Test]
		public void font_weight_900()
		{
			var termParts = new[]{"900"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{font-weight: 900 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("900", expression.Terms[0].ToString());
		}
		[Test]
		public void height_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{height: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void height_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{height: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void left_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{left: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void left_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{left: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void letter_spacing_normal()
		{
			var termParts = new[]{"normal"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{letter-spacing: normal }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("normal", expression.Terms[0].ToString());
		}
		[Test]
		public void letter_spacing_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{letter-spacing: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void letter_spacing_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{letter-spacing:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void line_height_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{line-height: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void line_height_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{line-height: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void list_style_square_urlimagesimgpng()
		{
			var termParts = new[]{"square","url(images/img.png)"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{list-style: square url(images/img.png) }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("square", expression.Terms[0].ToString());
			Assert.AreEqual("url(images/img.png)", expression.Terms[1].ToString());
		}
		[Test]
		public void list_style_circle_top()
		{
			var termParts = new[]{"circle","top"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{list-style: circle top }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("circle", expression.Terms[0].ToString());
			Assert.AreEqual("top", expression.Terms[1].ToString());
		}
		[Test]
		public void list_style_circke_urlimagesimgpng()
		{
			var termParts = new[]{"circke","url(images/img.png)"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{list-style: circke url(images/img.png) }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("circke", expression.Terms[0].ToString());
			Assert.AreEqual("url(images/img.png)", expression.Terms[1].ToString());
		}
		[Test]
		public void list_style_image_urlimageslistpng()
		{
			var termParts = new[]{"url(images/list.png)"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{list-style-image: url(images/list.png) }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("url(images/list.png)", expression.Terms[0].ToString());
		}
		[Test]
		public void list_style_position_top()
		{
			var termParts = new[]{"top"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{list-style-position: top }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("top", expression.Terms[0].ToString());
		}
		[Test]
		public void list_style_type_square()
		{
			var termParts = new[]{"square"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{list-style-type: square }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("square", expression.Terms[0].ToString());
		}
		[Test]
		public void list_style_type_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{list-style-type:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_5px_10Percent()
		{
			var termParts = new[]{"5px","10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin: 5px 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10%", expression.Terms[1].ToString());
		}
		[Test]
		public void margin_5Percent_10px()
		{
			var termParts = new[]{"5%","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin: 5% 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void margin_5px_10px()
		{
			var termParts = new[]{"5px","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin: 5px 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void margin_bottom_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-bottom: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_bottom_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-bottom: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_bottom_0()
		{
			var termParts = new[]{"0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-bottom: 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_left_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-left: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_left_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-left: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_left_0()
		{
			var termParts = new[]{"0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-left: 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_right_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-right: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_right_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-right: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_right_0()
		{
			var termParts = new[]{"0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-right: 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_top_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-top: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_top_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-top: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void margin_top_0()
		{
			var termParts = new[]{"0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{margin-top: 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
		}
		[Test]
		public void max_height_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{max-height: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void max_height_10Percent_0()
		{
			var termParts = new[]{"10%","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{max-height: 10% 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
		}
		[Test]
		public void max_width_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{max-width: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void max_width_10Percent_0()
		{
			var termParts = new[]{"10%","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{max-width: 10% 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
		}
		[Test]
		public void min_height_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{min-height: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void min_height_10Percent_0()
		{
			var termParts = new[]{"10%","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{min-height: 10% 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
		}
		[Test]
		public void min_width_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{min-width: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void min_width_10Percent_0()
		{
			var termParts = new[]{"10%","0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{min-width: 10% 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
			Assert.AreEqual("0", expression.Terms[1].ToString());
		}
		[Test]
		public void _moz_border_radius_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{-moz-border-radius: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void _moz_border_radius_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{-moz-border-radius: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void _moz_border_radius_5px_10Percent()
		{
			var termParts = new[]{"5px","10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{-moz-border-radius: 5px 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10%", expression.Terms[1].ToString());
		}
		[Test]
		public void _moz_border_radius_5Percent_10px()
		{
			var termParts = new[]{"5%","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{-moz-border-radius: 5% 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void _moz_border_radius_5px_10px()
		{
			var termParts = new[]{"5px","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{-moz-border-radius: 5px 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void outline_CCC_dotted_thick()
		{
			var termParts = new[]{"#CCC","dotted","thick"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline: #CCC dotted thick }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
			Assert.AreEqual("dotted", expression.Terms[1].ToString());
			Assert.AreEqual("thick", expression.Terms[2].ToString());
		}
		[Test]
		public void outline_CCC()
		{
			var termParts = new[]{"#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline: #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
		}
		[Test]
		public void outline_dotted_4Percent()
		{
			var termParts = new[]{"dotted","4%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline: dotted 4% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("dotted", expression.Terms[0].ToString());
			Assert.AreEqual("4%", expression.Terms[1].ToString());
		}
		[Test]
		public void outline_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void outline_color_CCC()
		{
			var termParts = new[]{"#CCC"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline-color: #CCC }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("#CCC", expression.Terms[0].ToString());
		}
		[Test]
		public void outline_color_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline-color:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void outline_style_dotted()
		{
			var termParts = new[]{"dotted"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline-style: dotted }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("dotted", expression.Terms[0].ToString());
		}
		[Test]
		public void outline_width_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline-width: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void outline_width_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{outline-width:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void overflow_scroll()
		{
			var termParts = new[]{"scroll"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{overflow: scroll }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("scroll", expression.Terms[0].ToString());
		}
		[Test]
		public void overflow_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{overflow:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void overflow_X_scroll()
		{
			var termParts = new[]{"scroll"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{overflow-X: scroll }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("scroll", expression.Terms[0].ToString());
		}
		[Test]
		public void overflow_X_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{overflow-X:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void overflow_Y_scroll()
		{
			var termParts = new[]{"scroll"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{overflow-Y: scroll }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("scroll", expression.Terms[0].ToString());
		}
		[Test]
		public void overflow_Y_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{overflow-Y:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void padding_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{padding: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void padding_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{padding: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void padding_5px_10Percent()
		{
			var termParts = new[]{"5px","10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{padding: 5px 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10%", expression.Terms[1].ToString());
		}
		[Test]
		public void padding_5Percent_10px()
		{
			var termParts = new[]{"5%","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{padding: 5% 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void padding_5px_10px()
		{
			var termParts = new[]{"5px","10px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{padding: 5px 10px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10px", expression.Terms[1].ToString());
		}
		[Test]
		public void position_absolute()
		{
			var termParts = new[]{"absolute"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{position: absolute }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("absolute", expression.Terms[0].ToString());
		}
		[Test]
		public void position_fixed()
		{
			var termParts = new[]{"fixed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{position: fixed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("fixed", expression.Terms[0].ToString());
		}
		[Test]
		public void table_layout_fixed()
		{
			var termParts = new[]{"fixed"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{table-layout: fixed }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("fixed", expression.Terms[0].ToString());
		}
		[Test]
		public void table_layout_inherit()
		{
			var termParts = new[]{"inherit"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{table-layout: inherit }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("inherit", expression.Terms[0].ToString());
		}
		[Test]
		public void text_align_right()
		{
			var termParts = new[]{"right"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-align: right }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("right", expression.Terms[0].ToString());
		}
		[Test]
		public void text_align_center()
		{
			var termParts = new[]{"center"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-align: center }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("center", expression.Terms[0].ToString());
		}
		[Test]
		public void text_align_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-align:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
		[Test]
		public void text_decoration_underline()
		{
			var termParts = new[]{"underline"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-decoration: underline }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("underline", expression.Terms[0].ToString());
		}
		[Test]
		public void text_decoration_line_through()
		{
			var termParts = new[]{"line-through"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-decoration: line-through }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("line-through", expression.Terms[0].ToString());
		}
		[Test]
		public void text_indent_5px()
		{
			var termParts = new[]{"5px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-indent: 5px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
		}
		[Test]
		public void text_indent_10Percent()
		{
			var termParts = new[]{"10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-indent: 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("10%", expression.Terms[0].ToString());
		}
		[Test]
		public void text_indent_0()
		{
			var termParts = new[]{"0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-indent: 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
		}
		[Test]
		public void text_transform_capitalize()
		{
			var termParts = new[]{"capitalize"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{text-transform: capitalize }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("capitalize", expression.Terms[0].ToString());
		}
		[Test]
		public void vertical_align_5Percent()
		{
			var termParts = new[]{"5%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{vertical-align: 5% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5%", expression.Terms[0].ToString());
		}
		[Test]
		public void vertical_align_baseline()
		{
			var termParts = new[]{"baseline"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{vertical-align: baseline }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("baseline", expression.Terms[0].ToString());
		}
		[Test]
		public void vertical_align_text_top()
		{
			var termParts = new[]{"text-top"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{vertical-align: text-top }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("text-top", expression.Terms[0].ToString());
		}
		[Test]
		public void visibility_hidden()
		{
			var termParts = new[]{"hidden"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{visibility: hidden }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("hidden", expression.Terms[0].ToString());
		}
		[Test]
		public void visibility_collapse()
		{
			var termParts = new[]{"collapse"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{visibility: collapse }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("collapse", expression.Terms[0].ToString());
		}
		[Test]
		public void white_space_pre_line()
		{
			var termParts = new[]{"pre-line"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{white-space: pre-line }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("pre-line", expression.Terms[0].ToString());
		}
		[Test]
		public void white_space_normal()
		{
			var termParts = new[]{"normal"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{white-space: normal }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("normal", expression.Terms[0].ToString());
		}
		[Test]
		public void word_spacing_5px_10Percent()
		{
			var termParts = new[]{"5px","10%"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{word-spacing: 5px 10% }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("5px", expression.Terms[0].ToString());
			Assert.AreEqual("10%", expression.Terms[1].ToString());
		}
		[Test]
		public void z_index__1()
		{
			var termParts = new[]{"-1"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{z-index: -1 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("-1", expression.Terms[0].ToString());
		}
		[Test]
		public void z_index_0()
		{
			var termParts = new[]{"0"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{z-index: 0 }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0", expression.Terms[0].ToString());
		}
		[Test]
		public void z_index_0px()
		{
			var termParts = new[]{"0px"}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{z-index: 0px }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("0px", expression.Terms[0].ToString());
		}
		[Test]
		public void z_index_()
		{
			var termParts = new[]{""}; 
			var parser = new StylesheetParser();
			var style = parser.Parse("*{z-index:  }");
			var expression = style.RuleSets[0].Declarations[0].Expression;

			Assert.AreEqual(termParts.Length, expression.Terms.Count);
			Assert.AreEqual("", expression.Terms[0].ToString());
		}
	}
}
