using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class SelectorParsingFixture
    {
        private readonly dynamic _stylesheets = new Stylesheets();
        private readonly Stylesheet _parsed;

        public SelectorParsingFixture()
        {
            _parsed = new StylesheetParser().Parse(_stylesheets.Css3);
        }

        [Test]
        public void Parser_Identifies_Global_Elements()
        {
            Assert.AreEqual("* {\r\n}", _parsed.RuleSets[0].ToString());
        }

        [Test]
        public void Parser_Identifies_Elements()
        {
            Assert.AreEqual("E {\r\n}", _parsed.RuleSets[1].ToString());
        }

        [Test]
        public void Parser_Identifies_Attributed_Elements()
        {
            Assert.AreEqual("E[foo] {\r\n}", _parsed.RuleSets[2].ToString());
        }
        
        [Test]
        public void Parser_Identifies_Filtered_Attribute_Elements()
        {
            Assert.AreEqual("E[foo=\"bar\"] {\r\n}", _parsed.RuleSets[3].ToString());
            Assert.AreEqual("E[foo~=\"bar\"] {\r\n}", _parsed.RuleSets[4].ToString());
            Assert.AreEqual("E[foo$=\"bar\"] {\r\n}", _parsed.RuleSets[5].ToString());
            Assert.AreEqual("E[foo^=\"bar\"] {\r\n}", _parsed.RuleSets[6].ToString());
            Assert.AreEqual("E[foo*=\"bar\"] {\r\n}", _parsed.RuleSets[7].ToString());
            Assert.AreEqual("E[foo|=\"en\"] {\r\n}", _parsed.RuleSets[8].ToString());
        }

        [Test]
        public void Parser_Identifies_Pseudo_Classes()
        {
            Assert.AreEqual("E:root {\r\n}", _parsed.RuleSets[9].ToString());
            Assert.AreEqual("E:nth-child(n) {\r\n}", _parsed.RuleSets[10].ToString());
            Assert.AreEqual("E:nth-last-child(n) {\r\n}", _parsed.RuleSets[11].ToString());
            Assert.AreEqual("E:nth-of-type(n) {\r\n}", _parsed.RuleSets[12].ToString());
            Assert.AreEqual("E:nth-last-of-type(n) {\r\n}", _parsed.RuleSets[13].ToString());
            Assert.AreEqual("E:first-child {\r\n}", _parsed.RuleSets[14].ToString());
            Assert.AreEqual("E:last-child {\r\n}", _parsed.RuleSets[15].ToString());
            Assert.AreEqual("E:first-of-type {\r\n}", _parsed.RuleSets[16].ToString());
            Assert.AreEqual("E:last-of-type {\r\n}", _parsed.RuleSets[17].ToString());
            Assert.AreEqual("E:only-child {\r\n}", _parsed.RuleSets[18].ToString());
            Assert.AreEqual("E:only-of-type {\r\n}", _parsed.RuleSets[19].ToString());
            Assert.AreEqual("E:empty {\r\n}",_parsed.RuleSets[20].ToString());
            Assert.AreEqual("E:link {\r\n}",_parsed.RuleSets[21].ToString());
            Assert.AreEqual("E:visited {\r\n}", _parsed.RuleSets[22].ToString());
            Assert.AreEqual("E:active {\r\n}",_parsed.RuleSets[23].ToString());
            Assert.AreEqual("E:hover {\r\n}", _parsed.RuleSets[24].ToString());
            Assert.AreEqual("E:focus {\r\n}", _parsed.RuleSets[25].ToString());
            Assert.AreEqual("E:target {\r\n}", _parsed.RuleSets[26].ToString());
            Assert.AreEqual("E:lang(fr) {\r\n}", _parsed.RuleSets[27].ToString());
            Assert.AreEqual("E:enabled {\r\n}", _parsed.RuleSets[28].ToString());
            Assert.AreEqual("E:disabled {\r\n}", _parsed.RuleSets[29].ToString());
            Assert.AreEqual("E:checked {\r\n}", _parsed.RuleSets[30].ToString());
            Assert.AreEqual("E::first-line {\r\n}", _parsed.RuleSets[31].ToString());
            Assert.AreEqual("E::first-letter {\r\n}",_parsed.RuleSets[32].ToString());
            Assert.AreEqual("E::before {\r\n}", _parsed.RuleSets[33].ToString());
            Assert.AreEqual("E::after {\r\n}", _parsed.RuleSets[34].ToString());
            Assert.AreEqual("E:not(s) {\r\n}", _parsed.RuleSets[37].ToString());
        }

        [Test]
        public void Parser_Identifies_Elements_And_Classes()
        {
            Assert.AreEqual("E.warning {\r\n}", _parsed.RuleSets[35].ToString());
            Assert.AreEqual("E#myid {\r\n}", _parsed.RuleSets[36].ToString());
        }

        [Test]
        public void Parser_Identifies_Relationships()
        {
            Assert.AreEqual("E F {\r\n}", _parsed.RuleSets[38].ToString());
            Assert.AreEqual("E > F {\r\n}", _parsed.RuleSets[39].ToString());
            Assert.AreEqual("E + F {\r\n}", _parsed.RuleSets[40].ToString());
            Assert.AreEqual("E ~ F {\r\n}", _parsed.RuleSets[41].ToString());
        }

        [Test]
        public void Parser_Identifies_Complex_Relationships()
        {
            Assert.AreEqual("E:focus:hover {\r\n}", _parsed.RuleSets[43].ToString());
            Assert.AreEqual("E.warning:target {\r\n}", _parsed.RuleSets[44].ToString());
            Assert.AreEqual("E * p {\r\n}", _parsed.RuleSets[45].ToString());
            Assert.AreEqual("E p *[href] {\r\n}", _parsed.RuleSets[46].ToString());
            Assert.AreEqual("E F > G H {\r\n}", _parsed.RuleSets[48].ToString());
            Assert.AreEqual("E.warning + h2 {\r\n}", _parsed.RuleSets[49].ToString());
            Assert.AreEqual("E F + G {\r\n}", _parsed.RuleSets[50].ToString());
            Assert.AreEqual("E + *[REL=up] {\r\n}", _parsed.RuleSets[51].ToString());
            Assert.AreEqual("E F G.warning {\r\n}", _parsed.RuleSets[52].ToString());
            Assert.AreEqual("E.warning.level {\r\n}", _parsed.RuleSets[53].ToString());
        }

        [Test]
        public void Parser_Identifies_Namespace_Pipes()
        {
            Assert.AreEqual("E|F {\r\n}", _parsed.RuleSets[55].ToString());
            Assert.AreEqual("E|* {\r\n}", _parsed.RuleSets[56].ToString());
            Assert.AreEqual("|F {\r\n}", _parsed.RuleSets[57].ToString());
            Assert.AreEqual("*|F {\r\n}", _parsed.RuleSets[58].ToString());
        }

        [Test]
        public void Parser_Global_Selectors()
        {
            Assert.AreEqual("*[hreflang|=en] {\r\n}", _parsed.RuleSets[59].ToString());
            Assert.AreEqual("*.warning {\r\n}", _parsed.RuleSets[60].ToString());
            Assert.AreEqual("*:warning {\r\n}", _parsed.RuleSets[61].ToString());
            Assert.AreEqual("*:warning::before {\r\n}", _parsed.RuleSets[62].ToString());
        }
    }
}
