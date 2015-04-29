using System.Linq;
using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class SelectorFixture
    {
        [Test]
        public void Parser_Reads_Hex_Color()
        {
            var parser = new Parser();
            var css = parser.Parse("html{color:#000000;}");

            Assert.AreEqual("html{color:#000;}", css.ToString(false));
        }

        [Test]
        public void Parser_Reads_Important_Flag()
        {
            var parser = new Parser();
            var css = parser.Parse("table.fullWidth {width: 100% !important;}");

            var rules = css.Rules;

            Assert.AreEqual("table.fullWidth{width:100% !important;}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Global_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("*{}");

            var rules = css.Rules;

            Assert.AreEqual("*{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Mixed_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("button,.button,input[type=button]{}");

            var rules = css.Rules;

            Assert.AreEqual("button,.button,input[type=\"button\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Class_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse(".one, .two{}");

            var rules = css.Rules;

            Assert.AreEqual(".one,.two{}", rules[0].ToString());
            var selector = (rules[0] as StyleRule).Selector as AggregateSelectorList;
            Assert.AreEqual(2, selector.Length);
            Assert.AreEqual(".one", selector[0].ToString());
            Assert.AreEqual(".two", selector[1].ToString());
        }

        [Test]
        public void Parser_Reads_Element_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("E{}");

            var rules = css.Rules;

            Assert.AreEqual("E{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Empty_Attribute_Element_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo]{}");

            var rules = css.Rules;

            Assert.AreEqual("E[foo]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Quoted_Attribute_Element_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo=\"bar\"]{}");

            var rules = css.Rules;

            Assert.AreEqual("E[foo=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Space_Separated_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo~=\"bar\"]{}");

            var rules = css.Rules;

            Assert.AreEqual("E[foo~=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Starts_With_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo^=\"bar\"]{}");

            var rules = css.Rules;

            Assert.AreEqual("E[foo^=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Ends_With_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo$=\"bar\"]{}");

            var rules = css.Rules;

            Assert.AreEqual("E[foo$=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Contains_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo*=\"bar\"]{}");

            var rules = css.Rules;

            Assert.AreEqual("E[foo*=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Dash_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo|=\"bar\"]{}");

            var rules = css.Rules;

            Assert.AreEqual("E[foo|=\"bar\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Multiple_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E[foo=\"bar\"][rel=\"important\"]{}");

            var rules = css.Rules;

            Assert.AreEqual("E[foo=\"bar\"][rel=\"important\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Pseudo_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("E:pseudo{}");

            var rules = css.Rules;

            Assert.AreEqual("E:pseudo{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Pseudo_Functions()
        {
            var parser = new Parser();
            var css = parser.Parse("E:nth-child(n){}");

            var rules = css.Rules;

            Assert.AreEqual("E:nth-child(1n+0){}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Pseudo_Functions_With_Negative_Rules()
        {
            var parser = new Parser();
            var css = parser.Parse("E:nth-last-of-type(-n+2){}");

            var rules = css.Rules;

            Assert.AreEqual("E:nth-last-of-type(-1n+2){}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Pseudo_Element()
        {
            var parser = new Parser();
            var css = parser.Parse("E::first-line{}");

            var rules = css.Rules;

            Assert.AreEqual("E::first-line{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Class_Attributed_Elements()
        {
            var parser = new Parser();
            var css = parser.Parse("E.warning{}");

            var rules = css.Rules;

            Assert.AreEqual("E.warning{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Id_Elements()
        {
            var parser = new Parser();
            var css = parser.Parse("E#id{}");

            var rules = css.Rules;

            Assert.AreEqual("E#id{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Descendant_Elements()
        {
            var parser = new Parser();
            var css = parser.Parse("E F{}");

            var rules = css.Rules;

            Assert.AreEqual("E F{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Child_Elements()
        {
            var parser = new Parser();
            var css = parser.Parse("E > F{}");

            var rules = css.Rules;

            Assert.AreEqual("E>F{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Adjacent_Sibling_Elements()
        {
            var parser = new Parser();
            var css = parser.Parse("E + F{}");

            var rules = css.Rules;

            Assert.AreEqual("E+F{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_General_Sibling_Elements()
        {
            var parser = new Parser();
            var css = parser.Parse("E + F{}");

            var rules = css.Rules;

            Assert.AreEqual("E+F{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Multiple_Pseudo_Classes()
        {
            var parser = new Parser();
            var css = parser.Parse("E:focus:hover{}");

            var rules = css.Rules;

            Assert.AreEqual("E:focus:hover{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Element_Class_Pseudo_Classes()
        {
            var parser = new Parser();
            var css = parser.Parse("E.class:hover{}");

            var rules = css.Rules;

            Assert.AreEqual("E.class:hover{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Global_Combinator()
        {
            var parser = new Parser();
            var css = parser.Parse("E * p{}");

            var rules = css.Rules;

            Assert.AreEqual("E * p{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Global_Attribute()
        {
            var parser = new Parser();
            var css = parser.Parse("E p *[href]{}");

            var rules = css.Rules;

            Assert.AreEqual("E p *[href]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Descendand_And_Child_Combinators()
        {
            var parser = new Parser();
            var css = parser.Parse("E F>G H{}");

            var rules = css.Rules;

            Assert.AreEqual("E F>G H{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Classed_Element_Combinators()
        {
            var parser = new Parser();
            var css = parser.Parse("E.warning + h2{}");

            var rules = css.Rules;

            Assert.AreEqual("E.warning+h2{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Descendand_And_Sibling_Combinators()
        {
            var parser = new Parser();
            var css = parser.Parse("E F+G{}");

            var rules = css.Rules;

            Assert.AreEqual("E F+G{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Attributed_Descendants()
        {
            var parser = new Parser();
            var css = parser.Parse("E + *[REL=up]{}");

            var rules = css.Rules;

            Assert.AreEqual("E+*[REL=\"up\"]{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Chained_Classes()
        {
            var parser = new Parser();
            var css = parser.Parse("E.first.second{}");

            var rules = css.Rules;

            Assert.AreEqual("E.first.second{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Namespace_Selectors()
        {
            var parser = new Parser();
            var css = parser.Parse("ns|F{}");

            var rules = css.Rules;

            Assert.AreEqual("ns|F{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Namespace_Global()
        {
            var parser = new Parser();
            var css = parser.Parse("ns|*{}");

            var rules = css.Rules;

            Assert.AreEqual("ns|*{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Element_With_No_Namespace_Global()
        {
            var parser = new Parser();
            var css = parser.Parse("|E{}");

            var rules = css.Rules;

            Assert.AreEqual("|E{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Element_With_Any_Namespace_Global()
        {
            var parser = new Parser();
            var css = parser.Parse("*|E{}");

            var rules = css.Rules;

            Assert.AreEqual("*|E{}", rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Any_Comments_Without_Errors()
        {
            var validComments = new[] {"/*/", "/**/", "/***/", "/****/", "/* anything */", "/* // */", "/*// */"};
            var parser = new Parser();

            foreach (var comment in validComments)
            {
                var stylesheet = parser.Parse(comment);
                Assert.AreEqual(0, stylesheet.Errors.Count, string.Format("{0} is not valid", comment));
            }
        }

        [Test]
        public void Parser_Reads_Multiline_Comments()
        {
            string comments = "#sidebar { }\r\n" +
                "/*********************************************************************************/\r\n" +
                "/* Footer                                                                        */\r\n" +
                "/*********************************************************************************/\r\n" +
                "\r\n" +
                "#footer {}\r\n" +
                "\r\n" +
                "/*********************************************************************************/\r\n" +
                "/* Copyright                                                                     */\r\n" +
                "/*********************************************************************************/\r\n" +
                "\r\n" +
                "#copyright {}\r\n";

            var stylesheet = new Parser().Parse(comments);
            Assert.AreEqual(0, stylesheet.Errors.Count);
            Assert.AreEqual(3, stylesheet.Rules.Count);
            Assert.AreEqual("#sidebar{}", stylesheet.Rules[0].ToString());
            Assert.AreEqual("#footer{}", stylesheet.Rules[1].ToString());
            Assert.AreEqual("#copyright{}", stylesheet.Rules[2].ToString());
        }

        [Test]
        public void Parser_Reads_Background_Gradients()
        {
            var parser = new Parser();
            var css = parser.Parse(@".bg{background:-moz-linear-gradient(top, #FFF 0, #FFF 100%) !important;}");

            Assert.That(css.Errors.Count == 0);
            Assert.AreEqual(1, css.Rules.Count());
            Assert.AreEqual(@".bg{background:-moz-linear-gradient(top,#FFF 0,#FFF 100%) !important;}", css.Rules[0].ToString());
        }

        [Test]
        public void Parser_Reads_Background_Gradients_With_Nested_RGBA()
        {
            var parser = new Parser();
            var css = parser.Parse(@".bg{background: -moz-linear-gradient(left,     rgba(0, 119, 152,     1) 0%,        rgba(1,160, 155,1) 100%);}");
            var cssResult = css.Rules[0].ToString();

            Assert.That(css.Errors.Count == 0);
            Assert.AreEqual(@".bg{background:-moz-linear-gradient(left,#007798 0%,#01A09B 100%);}", cssResult);
        }

        [Test]
        public void Parser_Reads_Background_Gradients_Many()
        {
            var parser = new Parser();
            var css = parser.Parse(@".group-sticker .no-override:hover:before,
                    .group-sticker .no-override.on:before {
                      z-index: 2;
                      border-radius: 100%;
                      background: #9ACE48 !important;
                      background: -webkit-gradient(linear, left top, left bottom, color-stop(0, #9ACE48), color-stop(100%, #84B23C)) !important;
                      background: -moz-linear-gradient(left, rgba(0,119,152,1) 0%, rgba(1,160,155,1) 100%);
                      background: -webkit-gradient-linear(left top, left bottom, color-stop(0, #9ACE48), color-stop(100%, #84B23C)) !important;
                      background: -webkit-linear-gradient(top, #9ACE48 0, #84B23C 100%) !important;
                      background: -o-linear-gradient(top, #9ACE48 0, #84B23C 100%) !important;
                      background: -ms-linear-gradient(top, #9ACE48 0, #84B23C 100%) !important;
                      background: linear-gradient(to bottom, #9ACE48 0, #84B23C 100%) !important;
                    }");
            var cssResult = css.Rules[0].ToString();

            Assert.That(css.Errors.Count == 0);
            Assert.AreEqual(@".group-sticker .no-override:hover:before,.group-sticker .no-override.on:before{z-index:2;border-radius:100%;background:#9ACE48 !important;background:-webkit-gradient(linear,left top,left bottom,color-stop(0,#9ACE48),color-stop(100%,#84B23C)) !important;background:-moz-linear-gradient(left,#007798 0%,#01A09B 100%);background:-webkit-gradient-linear(left top,left bottom,color-stop(0,#9ACE48),color-stop(100%,#84B23C)) !important;background:-webkit-linear-gradient(top,#9ACE48 0,#84B23C 100%) !important;background:-o-linear-gradient(top,#9ACE48 0,#84B23C 100%) !important;background:-ms-linear-gradient(top,#9ACE48 0,#84B23C 100%) !important;background:linear-gradient(to bottom,#9ACE48 0,#84B23C 100%) !important;}", cssResult);
        }

        [Test]
        public void Parser_Reads_Background_Gradients_With_Legacy_Syntax()
        {
            var parser = new Parser();
            var css = parser.Parse(@".c{background: -webkit-gradient(linear, left top, left bottom, color-stop(0, #9ACE48), color-stop(100%, #84B23C)) !important;}");
            var cssResult = css.Rules[0].ToString();

            Assert.That(css.Errors.Count == 0);
            Assert.AreEqual(@".c{background:-webkit-gradient(linear,left top,left bottom,color-stop(0,#9ACE48),color-stop(100%,#84B23C)) !important;}", cssResult);
        }


        [Test]
        public void Parser_Reads_Background_Gradients_With_Nested_Fn()
        {
            var parser = new Parser();
            var css = parser.Parse(@".c{background:-webkit-gradient(linear, left top, left bottom, color-stop(0, #9ACE48), color-stop(100%,#84B23C)) !important;}");
            var cssResult = css.Rules[0].ToString();

            Assert.That(css.Errors.Count == 0);
            Assert.AreEqual(@".c{background:-webkit-gradient(linear,left top,left bottom,color-stop(0,#9ACE48),color-stop(100%,#84B23C)) !important;}", cssResult);
        }

        [Test]
        public void Parser_Reads_Font_Shorthand()
        {
            var parser = new Parser();
            var css = parser.Parse(@".body {font: 300 italic 1.3em/1.7em 'FB Armada', sans-serif;}");

            Assert.That(css.Errors.Count == 0);
            Assert.AreEqual(@".body{font:300 italic 1.3em/1.7em 'FB Armada',sans-serif;}", css.ToString());
        }
        
        [Test]
        public void Parser_Reads_Multi_Microsoft_Filter_Fns()
        {
            var css = @".test {
background: rgb(30,87,153); /* Old browsers */
/* IE9 SVG, needs conditional override of 'filter' to 'none' */
background: url(data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiA/Pgo8c3ZnIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgd2lkdGg9IjEwMCUiIGhlaWdodD0iMTAwJSIgdmlld0JveD0iMCAwIDEgMSIgcHJlc2VydmVBc3BlY3RSYXRpbz0ibm9uZSI+CiAgPGxpbmVhckdyYWRpZW50IGlkPSJncmFkLXVjZ2ctZ2VuZXJhdGVkIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSIgeDE9IjAlIiB5MT0iMCUiIHgyPSIwJSIgeTI9IjEwMCUiPgogICAgPHN0b3Agb2Zmc2V0PSIwJSIgc3RvcC1jb2xvcj0iIzFlNTc5OSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjUwJSIgc3RvcC1jb2xvcj0iIzI5ODlkOCIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjUxJSIgc3RvcC1jb2xvcj0iIzIwN2NjYSIgc3RvcC1vcGFjaXR5PSIxIi8+CiAgICA8c3RvcCBvZmZzZXQ9IjEwMCUiIHN0b3AtY29sb3I9IiM3ZGI5ZTgiIHN0b3Atb3BhY2l0eT0iMSIvPgogIDwvbGluZWFyR3JhZGllbnQ+CiAgPHJlY3QgeD0iMCIgeT0iMCIgd2lkdGg9IjEiIGhlaWdodD0iMSIgZmlsbD0idXJsKCNncmFkLXVjZ2ctZ2VuZXJhdGVkKSIgLz4KPC9zdmc+);
background: -moz-linear-gradient(top,  rgba(30,87,153,1) 0%, rgba(41,137,216,1) 50%, rgba(32,124,202,1) 51%, rgba(125,185,232,1) 100%); /* FF3.6+ */
background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,rgba(30,87,153,1)), color-stop(50%,rgba(41,137,216,1)), color-stop(51%,rgba(32,124,202,1)), color-stop(100%,rgba(125,185,232,1))); /* Chrome,Safari4+ */
background: -webkit-linear-gradient(top,  rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 51%,rgba(125,185,232,1) 100%); /* Chrome10+,Safari5.1+ */
background: -o-linear-gradient(top,  rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 51%,rgba(125,185,232,1) 100%); /* Opera 11.10+ */
background: -ms-linear-gradient(top,  rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 51%,rgba(125,185,232,1) 100%); /* IE10+ */
background: linear-gradient(to bottom,  rgba(30,87,153,1) 0%,rgba(41,137,216,1) 50%,rgba(32,124,202,1) 51%,rgba(125,185,232,1) 100%); /* W3C */
filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#1e5799', endColorstr='#7db9e8',GradientType=0 ); /* IE6-8 */
filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#1e5799',endColorstr='#7db9e8',GradientType=0); /* IE6-8 */
}";
            var results = new Parser().Parse(css);
            Assert.That(results.Errors.Count == 0);
        }

        [Test]
        public void Parser_Reads_Microsoft_Star_Hack()
        {
            var css = @"body{*display:inline;}";
            var results = new Parser().Parse(css);
            Assert.That(results.Errors.Count == 0);
            Assert.AreEqual(css, results.ToString());
            Assert.AreEqual("*display", results.StyleRules.Single().Declarations.Single().Name);
        }
    }
}
