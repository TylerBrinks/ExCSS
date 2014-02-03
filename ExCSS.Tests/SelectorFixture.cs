//using System;
//using System.Collections.Generic;
//using AngleSharp.Css;
//using NUnit.Framework;

//namespace ExCSS.Tests
//{
//    [TestFixture]
//    public class SelectorFixture
//    {
//        [Test]
//        public void Parser_Reads_Important_Flag()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("table.fullWidth {width: 100% !important;}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("table.fullWidth{width:100% !important;}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Global_Selectors()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("*{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("*{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Class_Selectors()
//        {
//            var parser = new Parser();
//            var css = parser.Parse(".one, .two{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual(".one,.two{}", rules[0].ToString());
//            var selector = rules[0].Selector as MultipleSelectorList;
//            Assert.AreEqual(2, selector.Length);
//            Assert.AreEqual(".one", selector[0].ToString());
//            Assert.AreEqual(".two", selector[1].ToString());

//            // Sample enumeration of selectors
//            foreach (var r in rules[0].Selector as IEnumerable<SimpleSelector>)
//            {
//                Console.WriteLine(r);
//            }
//        }

//        [Test]
//        public void Parser_Reads_Element_Selectors()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Empty_Attribute_Element_Selectors()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E[foo]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E[foo]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Quoted_Attribute_Element_Selectors()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E[foo=\"bar\"]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E[foo=\"bar\"]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Space_Separated_Attribute()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E[foo~=\"bar\"]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E[foo~=\"bar\"]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Starts_With_Attribute()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E[foo^=\"bar\"]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E[foo^=\"bar\"]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Ends_With_Attribute()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E[foo$=\"bar\"]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E[foo$=\"bar\"]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Contains_Attribute()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E[foo*=\"bar\"]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E[foo*=\"bar\"]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Dash_Attribute()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E[foo|=\"bar\"]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E[foo|=\"bar\"]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Multiple_Attribute()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E[foo=\"bar\"][rel=\"important\"]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E[foo=\"bar\"][rel=\"important\"]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Pseudo_Selectors()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E:pseudo{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E:pseudo{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Pseudo_Functions()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E:nth-child(n){}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E:nth-child(n){}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Pseudo_Functions_With_Negative_Rules()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E:nth-last-of-type(-n+2){}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E:nth-last-of-type(-n+2){}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Pseudo_Element()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E::first-line{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E::first-line{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Class_Attributed_Elements()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E.warning{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E.warning{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Id_Elements()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E#id{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E#id{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Descendant_Elements()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E F{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E F{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Child_Elements()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E > F{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E>F{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Adjacent_Sibling_Elements()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E + F{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E+F{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_General_Sibling_Elements()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E + F{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E+F{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Multiple_Pseudo_Classes()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E:focus:hover{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E:focus:hover{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Element_Class_Pseudo_Classes()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E.class:hover{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E.class:hover{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Global_Combinator()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E * p{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E * p{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Global_Attribute()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E p *[href]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E p *[href]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Descendand_And_Child_Combinators()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E F>G H{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E F>G H{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Classed_Element_Combinators()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E.warning + h2{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E.warning+h2{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Descendand_And_Sibling_Combinators()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E F+G{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E F+G{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Attributed_Descendants()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E + *[REL=up]{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E+*[REL=\"up\"]{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Chained_Classes()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("E.first.second{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("E.first.second{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Namespace_Selectors()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("ns|F{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("ns|F{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Namespace_Global()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("ns|*{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("ns|*{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Element_With_No_Namespace_Global()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("|E{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("|E{}", rules[0].ToString());
//        }

//        [Test]
//        public void Parser_Reads_Element_With_Any_Namespace_Global()
//        {
//            var parser = new Parser();
//            var css = parser.Parse("*|E{}");

//            var rules = css.Rulesets;

//            Assert.AreEqual("*|E{}", rules[0].ToString());
//        }
//    }
//}
