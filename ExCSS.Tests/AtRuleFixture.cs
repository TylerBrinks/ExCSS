using System;
using System.Diagnostics;
using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class AtRuleFixture
    {
        #region Charset
        [Test]
        public void Parser_Reads_Character_Sets_Symbols()
        {
            var parser = new Parser();
            var css = parser.Parse("@charset 'utf-8';");

            var charset = css.CharsetDirectives;

            Assert.AreEqual("@charset 'utf-8';", charset[0].ToString());
        }

        [Test]
        public void Parser_Reads_Character_Sets_Strings()
        {
            var parser = new Parser();
            var css = parser.Parse("@charset 'utf-8';");
            var charset = css.CharsetDirectives;

            Assert.AreEqual("@charset 'utf-8';", charset[0].ToString());

            css = parser.Parse("@charset \"utf-8\";");
            charset = css.CharsetDirectives;

            Assert.AreEqual("@charset 'utf-8';", charset[0].ToString());
        }
        #endregion

        #region Imports
        [Test]
        public void Parser_Reads_Imports_Double_Quoted()
        {
            var parser = new Parser();
            var css = parser.Parse("@import \"style.css\";");

            var imports = css.ImportDirectives;

            Assert.AreEqual("@import url(style.css);", imports[0].ToString());
        }

        [Test]
        public void Parser_Reads_Imports_URL_Double_Quoted()
        {
            var parser = new Parser();
            var css = parser.Parse("@import url(\"style.css\");");

            var imports = css.ImportDirectives;


            Assert.AreEqual("@import url(style.css);", imports[0].ToString());
        }

        [Test]
        public void Parser_Reads_Imports_With_Single_Media()
        {
            var parser = new Parser();
            var css = parser.Parse("@import url(\"style.css\") print;");

            var imports = css.ImportDirectives;

            Assert.AreEqual("@import url(style.css) print;", imports[0].ToString());
        }

        [Test]
        public void Parser_Reads_Imports_With_Multiple_Media()
        {
            var parser = new Parser();
            var css = parser.Parse("@import url(\"style.css\") projection, tv;");

            var imports = css.ImportDirectives;

            Assert.AreEqual("@import url(style.css) projection, tv;", imports[0].ToString());
        }

        [Test]
        public void Parser_Reads_Imports_With_Constraints()
        {
            var parser = new Parser();
            var css = parser.Parse("@import url('style.css') handheld and (max-width: 400px);");

            var imports = css.ImportDirectives;

            Assert.AreEqual("@import url(style.css) handheld and (max-width: 400px);", imports[0].ToString());
        }

        [Test]
        public void Parser_Reads_Imports_With_Plain_And_Quoted_Meida()
        {
            var parser = new Parser();
            var css = parser.Parse("@import url(style.css) screen \"Plain style\";");

            var imports = css.ImportDirectives;

            Assert.AreEqual("@import url(style.css) screen 'Plain style';", imports[0].ToString());
        }

        [Test]
        public void Parser_Reads_Imports_With_Quoted_Media_And_Delimiters()
        {
            var parser = new Parser();
            var css = parser.Parse("@import url(style.css) \"Four-columns and dark\";");

            var imports = css.ImportDirectives;

            Assert.AreEqual("@import url(style.css) 'Four-columns and dark';", imports[0].ToString());
        }

        [Test]
        public void Parser_Reads_Imports_With_Quoted_URL_And_Media()
        {
            var parser = new Parser();
            var css = parser.Parse("@import \"style.css\" \"Style Sheet\";");

            var imports = css.ImportDirectives;

            Assert.AreEqual("@import url(style.css) 'Style Sheet';", imports[0].ToString());
        }
        #endregion

        #region Fontface
        [Test]
        public void Parser_Reads_Fontface()
        {
            var parser = new Parser();
            var css = parser.Parse(
@"@font-face
{
    font-family: testFont;
    src: url(SomeFont.ttf),
         url(SomeFont_Italic.tff); 
}");

            var fontfaces = css.FontFaceDirectives;

            Assert.AreEqual("@font-face{font-family:testFont;src:url(SomeFont.ttf),url(SomeFont_Italic.tff);}", fontfaces[0].ToString());
        }

        #endregion

        #region Keyframes
        [Test]
        public void Parser_Reads_Keyframes()
        {
            var parser = new Parser();
            var css = parser.Parse("@keyframes test-keyframes{from {top:0px;} to {top:200px;}}");

            var keyframes = css.KeyframeDirectives;

            Assert.AreEqual(@"@keyframes test-keyframes{from{top:0px;}to{top:200px;}}", keyframes[0].ToString());
        }

        [Test]
        public void KeyFrames_ToString_Test()
        {
            var parser = new Parser();
            Assert.AreEqual("@keyframes ixp-bounce{0%,100%{transform:translateY(0);}}",
                parser.Parse("@keyframes ixp-bounce { 0%, 100% { transform: translateY(0);}}").ToString());
        }

        [Test]
        public void KeyFrames_ToString_With_Vendor_Prefix_Test()
        {
            var parser = new Parser();
            Assert.AreEqual("@-moz-keyframes ixp-bounce{0%,100%{transform:translateY(0);}}",
                parser.Parse("@-moz-keyframes ixp-bounce { 0%, 100% { transform: translateY(0);}}").ToString());
        }

        [Test]
        public void KeyFrames_Multi_Test()
        {
            var parser = new Parser();
            var css = @"
            @-moz-keyframes ixp-tada {
                0% {transform: scale(111);}
                10%,20% {transform: scale(222) rotate(-3deg);}   
                100% {transform: scale(333) rotate(0);}
            }
            @keyframes ixp-tada {
                0% {transform: scale(666);}
                100% {transform: scale(1) rotate(777);}
            }
            ";

            var result = parser.Parse(css).ToString();
            Console.WriteLine(result);
            Assert.That(result.Contains("@keyframes ixp-tada"));
            Assert.AreEqual(@"@-moz-keyframes ixp-tada{0%{transform:scale(111);}10%,20%{transform:scale(222) rotate(-3deg);}100%{transform:scale(333) rotate(0);}}@keyframes ixp-tada{0%{transform:scale(666);}100%{transform:scale(1) rotate(777);}}", result);
        }

        [Test]
        public void KeyFrames_Browser_Prefix_Works_Test()
        {
            var parser = new Parser();
            StyleSheet result = null;
            Assert.DoesNotThrow(() =>
            {
                result = parser.Parse(@"@-webkit-keyframes ixp-bounce { 100% { -webkit-transform: translateY(0); } }");
            });
            Assert.That(result.ToString(), Is.StringContaining("transform"));

        }

        #endregion

        #region Media
        [Test]
        public void Parser_Reads_Media_Queries()
        {
            var parser = new Parser();
            var css = parser.Parse("@media print {body { font-size: 12pt; } h1 { font-size: 24pt; }}");

            var media = css.MediaDirectives;

            Assert.AreEqual("@media print {body{font-size:12pt;}h1{font-size:24pt;}}", media[0].ToString());
        }

        #endregion

        #region Page
        [Test]
        public void Parser_Reads_Page_Directives()
        {
            var parser = new Parser();
            var css = parser.Parse("@page {size: auto;margin: 10%;}");

            var pages = css.PageDirectives;

            Assert.AreEqual("@page {size:auto;margin:10%;}", pages[0].ToString());
        }

        [Test]
        public void Parser_Reads_Page_Directives_With_Pseudo()
        {
            var parser = new Parser();
            var css = parser.Parse("@page :left {size: auto;margin: 10%;}");

            var pages = css.PageDirectives;

            Assert.AreEqual("@page :left{size:auto;margin:10%;}", pages[0].ToString());
        }

        #endregion

        #region Supports
        [Test]
        public void Parser_Reads_Supports_Directives()
        {
            var parser = new Parser();
            var css = parser.Parse("@supports (animation-name: test){h2 {top: 0px;float: none;}}");

            var supports = css.SupportsDirectives;

            Assert.AreEqual("@supports (animation-name: test){h2{top:0px;float:none;}}", supports[0].ToString());
        }
        #endregion

        #region Namespace
        [Test]
        public void Parser_Reads_Namespace_Directives_With_Prefix()
        {
            var parser = new Parser();
            var css = parser.Parse("@namespace toto \"http://toto.example.org\";");

            var namespaces = css.NamespaceDirectives;

            Assert.AreEqual("@namespace toto 'http://toto.example.org';", namespaces[0].ToString());
        }

        [Test]
        public void Parser_Reads_Namespace_Directives()
        {
            var parser = new Parser();
            var css = parser.Parse("@namespace \"http://toto.example.org\";");

            var namespaces = css.NamespaceDirectives;

            Assert.AreEqual("@namespace 'http://toto.example.org';", namespaces[0].ToString());
        }
        #endregion


        #region Unknown directive
        [Test]
        public void Parser_Does_Not_Stop_On_Unknown_Directive()
        {
            var input = "@custom url(img.jpg);.other{color:red;}";
            var parser = new Parser();
            var css = parser.Parse(input);

            Assert.AreEqual(input, css.ToString(false));
            Assert.AreEqual(2, css.Rules.Count);
            Assert.AreEqual(1, css.StyleRules.Count);
        }

        [Test]
        public void Parser_Does_Not_Stop_On_Unknown_Directive_With_Body()
        {
            var input = "@-ms-viewport{width:device-width;}.other{color:red;}";
            var parser = new Parser();
            var css = parser.Parse(input);

            Assert.AreEqual(input, css.ToString(false));
            Assert.AreEqual(2, css.Rules.Count);
            Assert.AreEqual(1, css.StyleRules.Count);
        }
        #endregion
    }
}
