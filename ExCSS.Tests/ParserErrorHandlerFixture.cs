using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class ParserErrorHandlerFixture
    {
        [Test]
        public void Lexer_Handles_Double_Quote_New_Line()
        {
            var stylesheet = new Parser().Parse("@import \"\r\n");

            Assert.AreEqual(1, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.UnexpectedLineBreak, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(10, stylesheet.Errors[0].Column);
            Assert.AreEqual("Expected double quoted string to terminate before form feed or line feed.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_Single_Quote_New_Line()
        {
            var stylesheet = new Parser().Parse("@import '\r\n");

            Assert.AreEqual(1, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.UnexpectedLineBreak, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(10, stylesheet.Errors[0].Column);
            Assert.AreEqual("Expected single quoted string to terminate before form feed or line feed.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_Double_Quote_Backslash()
        {
            var stylesheet = new Parser().Parse("@import \\");

            Assert.AreEqual(2, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(10, stylesheet.Errors[0].Column);
            Assert.AreEqual("Unexpected line break or EOF.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_Single_Quote_Backslash()
        {
            var stylesheet = new Parser().Parse("@import '\\");

            Assert.AreEqual(1, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(11, stylesheet.Errors[0].Column);
            Assert.AreEqual("Expected single quoted string to terminate before end of file.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_Backslash_Newline()
        {
            var stylesheet = new Parser().Parse("@import \\\r\n");

            Assert.AreEqual(2, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.UnexpectedLineBreak, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(10, stylesheet.Errors[0].Column);
            Assert.AreEqual("Unexpected line break or EOF.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_URL_EoF()
        {
            var stylesheet = new Parser().Parse(".class{ prop: url(");

            Assert.AreEqual(1, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(19, stylesheet.Errors[0].Column);
            Assert.AreEqual("Expected URL to terminate before line break or end of file.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_URL_New_Line()
        {
            var stylesheet = new Parser().Parse(".class{ prop: url(\"\r\n}");

            Assert.AreEqual(2, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.UnexpectedLineBreak, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(20, stylesheet.Errors[0].Column);
            Assert.AreEqual("Expected URL to terminate before line break or end of file.", stylesheet.Errors[0].Message);

            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[1].ParserError);
            Assert.AreEqual(2, stylesheet.Errors[1].Line);
            Assert.AreEqual(2, stylesheet.Errors[1].Column);
            Assert.AreEqual("Expected URL to terminate before line break or end of file.", stylesheet.Errors[1].Message);
        }

        [Test]
        public void Lexer_Handles_URL_Backslash_EoF()
        {
            var stylesheet = new Parser().Parse(".class{ prop: url(\"\\");

            Assert.AreEqual(3, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(19, stylesheet.Errors[0].Column);
            Assert.AreEqual("Expected URL to terminate before line break or end of file.", stylesheet.Errors[0].Message);

            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[1].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[1].Line);
            Assert.AreEqual(21, stylesheet.Errors[1].Column);
            Assert.AreEqual("Unexpected line break or EOF.", stylesheet.Errors[1].Message);
        }

        [Test]
        public void Lexer_Handles_URL_Single_Quote_New_Line()
        {
            var stylesheet = new Parser().Parse(".class{ prop: url('\r\n");

            Assert.AreEqual(2, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.UnexpectedLineBreak, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(20, stylesheet.Errors[0].Column);

            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[1].ParserError);
            Assert.AreEqual(2, stylesheet.Errors[1].Line);
            Assert.AreEqual(1, stylesheet.Errors[1].Column);
        }

        [Test]
        public void Lexer_Handles_URL_Single_Quote_Backslash_EOF()
        {
            var stylesheet = new Parser().Parse(".class{ prop: url('\\");

            Assert.AreEqual(3, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(19, stylesheet.Errors[0].Column);

            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[1].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[1].Line);
            Assert.AreEqual(21, stylesheet.Errors[1].Column);
        }

        [Test]
        public void Lexer_Handles_Url_Unquoted()
        {
            var stylesheet = new Parser().Parse(".class{url(test')}");

            Assert.AreEqual(2, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.InvalidCharacter, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(16, stylesheet.Errors[0].Column);
            Assert.AreEqual("Expected quotation or open paren in URL.", stylesheet.Errors[0].Message);

            Assert.AreEqual(ParserError.UnexpectedLineBreak, stylesheet.Errors[1].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[1].Line);
            Assert.AreEqual(18, stylesheet.Errors[1].Column);
            Assert.AreEqual("An unexpected error occurred.", stylesheet.Errors[1].Message);
        }

        [Test]
        public void Lexer_Handles_Post_URL_Errant_Character()
        {
            var stylesheet = new Parser().Parse(".class{ prop: url('a'-");

            Assert.AreEqual(2, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.InvalidCharacter, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(22, stylesheet.Errors[0].Column);
            Assert.AreEqual("Invalid character in URL.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_Hash_Backslash()
        {
            var stylesheet = new Parser().Parse("n#\\");

            Assert.AreEqual(1, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.InvalidCharacter, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(3, stylesheet.Errors[0].Column);
            Assert.AreEqual("Invalid character after #.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_Numeric_Backslash()
        {
            var stylesheet = new Parser().Parse("#a\\");

            Assert.AreEqual(2, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.InvalidCharacter, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(3, stylesheet.Errors[0].Column);
            Assert.AreEqual("Invalid character after #.", stylesheet.Errors[0].Message);

            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[1].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[1].Line);
            Assert.AreEqual(4, stylesheet.Errors[1].Column);
            Assert.AreEqual("Unexpected line break or EOF.", stylesheet.Errors[1].Message);
        }

        [Test]
        public void Lexer_Handles_Double_Quotes_Backslash()
        {
            var stylesheet = new Parser().Parse(".calss{a: \"\\");

            Assert.AreEqual(1, stylesheet.Errors.Count);
            Assert.AreEqual(ParserError.EndOfFile, stylesheet.Errors[0].ParserError);
            Assert.AreEqual(1, stylesheet.Errors[0].Line);
            Assert.AreEqual(13, stylesheet.Errors[0].Column);
            Assert.AreEqual("Expected double quoted string to terminate before end of file.", stylesheet.Errors[0].Message);
        }

        [Test]
        public void Lexer_Handles_Empty_Value()
        {
            Assert.DoesNotThrow(() =>
            {
                var stylesheet = new Parser().Parse(".foo{clear:;}");
                Assert.AreEqual(1, stylesheet.Errors.Count);
                Assert.AreEqual(".foo{clear:;}", stylesheet.ToString());
            });
        }

        [Test]
        public void Lexer_Handles_Invalid_Important_Usage()
        {
            Assert.DoesNotThrow(() =>
            {
                var stylesheet = new Parser().Parse(@".accordion-a {background-color: #c6c6c6; !important;}");
                Assert.IsTrue(stylesheet.Errors.Count > 0);
            });
        }
    }
}
