namespace ExCSS.Tests
{
    using ExCSS;
    using Xunit;
    
    public class CssTokenizationTests
    {
        [Fact]
        public void CssParserIdentifier()
        {
            var teststring = "h1 { background: blue; }";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal(TokenType.Ident, token.Type);
        }

        [Fact]
        public void CssParserAtRule()
        {
            var teststring = "@media { background: blue; }";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal(TokenType.AtKeyword, token.Type);
        }

        [Fact]
        public void CssParserUrlUnquoted()
        {
            var url = "http://someurl";
            var teststring = "url(" + url + ")";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal(url, token.Data);
        }

        [Fact]
        public void CssParserUrlDoubleQuoted()
        {
            var url = "http://someurl";
            var teststring = "url(\"" + url + "\")";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal(url, token.Data);
        }

        [Fact]
        public void CssParserUrlSingleQuoted()
        {
            var url = "http://someurl";
            var teststring = "url('" + url + "')";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal(url, token.Data);
        }

        [Theory]
        // A start of fewer than six hex digits may still be followed by a '-<hex>' range end.
        [InlineData("U+41-5A", "41", "5A")]
        [InlineData("U+0-7F", "0", "7F")]
        [InlineData("U+400-4FF", "400", "4FF")]
        [InlineData("U+000041-00005A", "000041", "00005A")]
        public void CssParserUnicodeRangeWithStartAndEnd(string source, string start, string end)
        {
            var tokenizer = new Lexer(new TextSource(source));
            var token = tokenizer.Get();

            var range = Assert.IsType<RangeToken>(token);
            Assert.Equal(TokenType.Range, range.Type);
            Assert.Equal(start, range.Start);
            Assert.Equal(end, range.End);
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Theory]
        // The wildcard form is unchanged: '?' pads the value out to six digits.
        [InlineData("U+4??", "400", "4FF")]
        [InlineData("U+??????", "000000", "FFFFFF")]
        [InlineData("U+41", "41", "41")]
        public void CssParserUnicodeRangeSingleValueOrWildcard(string source, string start, string end)
        {
            var tokenizer = new Lexer(new TextSource(source));
            var token = tokenizer.Get();

            var range = Assert.IsType<RangeToken>(token);
            Assert.Equal(start, range.Start);
            Assert.Equal(end, range.End);
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Fact]
        public void CssParserUnicodeRangeSelectedRangeIsExpandedOnDemand()
        {
            var tokenizer = new Lexer(new TextSource("U+41-43"));
            var range = Assert.IsType<RangeToken>(tokenizer.Get());

            Assert.Equal(new[] {"A", "B", "C"}, range.SelectedRange);
        }

        [Fact]
        public void LexerOnlyCarriageReturn()
        {
            var teststring = "\r";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal("\n", token.Data);
        }

        [Fact]
        public void LexerCarriageReturnLineFeed()
        {
            var teststring = "\r\n";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal("\n", token.Data);
        }

        [Fact]
        public void LexerOnlyLineFeed()
        {
            var teststring = "\n";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal("\n", token.Data);
        }
    }
}
