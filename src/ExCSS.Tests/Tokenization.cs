namespace ExCSS.Tests
{
    using ExCSS;
    using System;
    using System.Linq;
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
        public void CssParserFunctionWithNestedParentheses()
        {
            // A bare parenthesized group inside a function's arguments must not terminate the function:
            // only the ')' matching the function's own '(' does. Previously "calc((1px + 2px) * 3)" ended
            // at the inner ')', leaving "* 3)" behind as stray top-level tokens.
            var teststring = "calc((1px + 2px) * 3)";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();

            Assert.Equal(TokenType.Function, token.Type);
            Assert.Equal("calc", token.Data);
            Assert.Equal(teststring, token.ToValue());
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Fact]
        public void CssParserFunctionWithDeeplyNestedParentheses()
        {
            var teststring = "calc(((1px + 2px) * (3 - 1)) / 2)";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();

            Assert.Equal(TokenType.Function, token.Type);
            Assert.Equal(teststring, token.ToValue());
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Fact]
        public void CssParserFunctionWithNestedFunctionAndParentheses()
        {
            // A nested function's own parens are consumed by its own recursive call, so they never surface
            // as bare bracket tokens here - this asserts the depth counter doesn't double-count them.
            var teststring = "calc(min(10px, (2px + 3px)) * 2)";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();

            Assert.Equal(TokenType.Function, token.Type);
            Assert.Equal(teststring, token.ToValue());
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Fact]
        public void CssParserFunctionWithUnbalancedParenthesesStopsAtEof()
        {
            var teststring = "calc((1px + 2px";
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();

            Assert.Equal(TokenType.Function, token.Type);
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Fact]
        public void ValueContextHash()
        {
            // In a value context '#' begins a <hash-token> (CSS Syntax 4.3.4): an all-hex name is a color
            // literal, any other name stays an id hash-token (e.g. the '#id' inside element()). Previously a
            // non-hex hash was truncated at the first non-hex char into an empty color plus a stray ident.
            static void Check(string input, TokenType expectedType, string expectedData)
            {
                var lexer = new Lexer(new TextSource(input)) { IsInValue = true };
                var token = lexer.Get();
                Assert.Equal(expectedType, token.Type);
                Assert.Equal(expectedData, token.Data);
                Assert.Equal(TokenType.EndOfFile, lexer.Get().Type);
            }

            Check("#f00", TokenType.Color, "f00");
            Check("#abc123", TokenType.Color, "abc123");
            Check("#deadbeef", TokenType.Color, "deadbeef");
            Check("#hero", TokenType.Hash, "hero");
            Check("#top", TokenType.Hash, "top");
            Check("#f00bar", TokenType.Hash, "f00bar");

            // '#' not followed by a name code point is a plain '#' delimiter, not a hash-token.
            var delim = new Lexer(new TextSource("# ")) { IsInValue = true };
            Assert.Equal(TokenType.Delim, delim.Get().Type);
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

        [Theory]
        [InlineData("1e2", "1e2")]
        [InlineData("1E2", "1E2")]
        [InlineData("1e+2", "1e+2")]
        [InlineData("1e-2", "1e-2")]
        [InlineData("1.5e2", "1.5e2")]
        public void LexerScientificNotationTokenizesAsSingleNumber(string teststring, string expected)
        {
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal(TokenType.Number, token.Type);
            Assert.Equal(expected, token.Data);
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Theory]
        [InlineData("1em", "em")]
        [InlineData("1ex", "ex")]
        [InlineData("1e", "e")]
        [InlineData("1.5em", "em")]
        [InlineData("1e-foo", "e")]
        public void LexerScientificNotationNoExponentDigitStaysADimension(string teststring, string expectedUnit)
        {
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal(TokenType.Dimension, token.Type);
            Assert.Equal(expectedUnit, ((UnitToken)token).Unit);
        }

        [Fact]
        public void LexerScientificNotationSignWithNoDigitStaysADimensionPlusLeftoverSign()
        {
            var tokenizer = new Lexer(new TextSource("1e+"));
            var first = tokenizer.Get();
            Assert.Equal(TokenType.Dimension, first.Type);
            Assert.Equal("e", ((UnitToken)first).Unit);
            var second = tokenizer.Get();
            Assert.Equal(TokenType.Delim, second.Type);
            Assert.Equal("+", second.Data);
        }

        [Theory]
        [InlineData("1e2px", "1e2", "px")]
        [InlineData("1e-1em", "1e-1", "em")]
        [InlineData("1e2-foo", "1e2", "-foo")]
        [InlineData("1e2\\70 x", "1e2", "px")]
        public void LexerScientificNotationFollowedByUnitTokenizesAsSingleDimension(string teststring, string number, string unit)
        {
            var tokenizer = new Lexer(new TextSource(teststring));
            var token = tokenizer.Get();
            Assert.Equal(TokenType.Dimension, token.Type);
            Assert.Equal(number, token.Data);
            Assert.Equal(unit, ((UnitToken)token).Unit);
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Fact]
        public void LexerScientificNotationFollowedByPercentTokenizesAsSinglePercentage()
        {
            var tokenizer = new Lexer(new TextSource("1e2%"));
            var token = tokenizer.Get();
            Assert.Equal(TokenType.Percentage, token.Type);
            Assert.Equal("1e2", token.Data);
            Assert.Equal(TokenType.EndOfFile, tokenizer.Get().Type);
        }

        [Theory]
        [InlineData("width: 1e2px", "100px")]
        [InlineData("width: 1e-1em", "0.1em")]
        [InlineData("width: 1e2%", "100%")]
        [InlineData("width: 1em", "1em")]
        [InlineData("width: 1ex", "1ex")]
        public void ScientificNotationInPropertyValue(string declaration, string expected)
        {
            var sheet = new StylesheetParser().Parse("a { " + declaration + " }");
            var rule = (StyleRule)sheet.Rules[0];
            Assert.Equal(expected, rule.Style.Width);
        }

        private static Token Lex(string input) => new Lexer(new TextSource(input)).Get();

        [Theory]
        [InlineData("\"abc", true)]
        [InlineData("\"abc\n", false)]
        [InlineData("'abc", true)]
        [InlineData("'abc\n", false)]
        public void StringUnterminatedOrLineBreakValidity(string input, bool expectedValid)
        {
            var token = Lex(input);
            Assert.Equal(TokenType.String, token.Type);
            Assert.Equal("abc", token.Data);
            Assert.Equal(expectedValid, ((StringToken)token).IsValid);
        }

        [Theory]
        [InlineData("\"abc\\")]
        [InlineData("'abc\\")]
        public void StringDanglingBackslashAtEofIsStillValid(string input)
        {
            var token = Lex(input);
            Assert.Equal(TokenType.String, token.Type);
            Assert.Equal("abc", token.Data);
            Assert.True(((StringToken)token).IsValid);
        }

        [Theory]
        [InlineData("\"a\\\nb\"")]
        [InlineData("'a\\\nb'")]
        public void StringEscapedLineContinuationAddsNoCharacters(string input)
        {
            var token = Lex(input);
            Assert.Equal(TokenType.String, token.Type);
            Assert.Equal("ab", token.Data);
            Assert.True(((StringToken)token).IsValid);
        }

        [Theory]
        [InlineData("url(\"abc\\")]
        [InlineData("url('abc\\")]
        [InlineData("url(abc\\")]
        public void UrlDanglingBackslashAtEofAppendsReplacementCharacter(string input)
        {
            var token = Lex(input);
            Assert.Equal(TokenType.Url, token.Type);
            Assert.Equal("abc�", token.Data);
            Assert.True(((UrlToken)token).IsValid);
        }

        [Theory]
        [InlineData("url(\"a\\\nb\")")]
        [InlineData("url('a\\\nb')")]
        public void UrlEscapedLineContinuationAddsNoCharacters(string input)
        {
            var token = Lex(input);
            Assert.Equal(TokenType.Url, token.Type);
            Assert.Equal("ab", token.Data);
            Assert.True(((UrlToken)token).IsValid);
        }

        [Fact]
        public void UrlUnquotedLineBreakAfterBackslashIsStillBad()
        {
            var token = Lex("url(abc\\\n");
            Assert.Equal(TokenType.Url, token.Type);
            Assert.False(((UrlToken)token).IsValid);
        }

        [Fact]
        public void UnterminatedCommentIsNotValid()
        {
            var token = Lex("/* abc");
            Assert.Equal(TokenType.Comment, token.Type);
            Assert.False(((CommentToken)token).IsValid);
            Assert.Equal("/* abc", token.ToValue());
        }

        [Fact]
        public void TerminatedCommentIsValid()
        {
            var token = Lex("/* abc */");
            Assert.Equal(TokenType.Comment, token.Type);
            Assert.True(((CommentToken)token).IsValid);
            Assert.Equal("/* abc */", token.ToValue());
        }

        [Fact]
        public void StylesheetStringLineContinuationContributesNothing()
        {
            var sheet = new StylesheetParser().Parse("a { content: \"x\\\ny\"; }");
            var rule = Assert.IsType<StyleRule>(Assert.Single(sheet.Rules));
            Assert.Equal("\"xy\"", rule.Style.GetPropertyValue("content"));
        }

        [Fact]
        public void LexerDeeplyNestedFunctionsDoNotOverflowStack()
        {
            // Run on a small stack so that the guard is exercised deterministically.
            Exception error = null;
            Token token = null;
            var thread = new System.Threading.Thread(() =>
            {
                try
                {
                    var tokenizer = new Lexer(new TextSource(Repeat("a(", 100000)));
                    token = tokenizer.Get();
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }, 256 * 1024);
            thread.Start();
            thread.Join();

            Assert.Null(error);
            Assert.Equal(TokenType.Function, token.Type);
        }

        [Fact]
        public void LexerModeratelyNestedFunctionsStillTokenize()
        {
            const int depth = 200;
            var tokenizer = new Lexer(new TextSource(Repeat("a(", depth) + "1" + Repeat(")", depth)));
            var token = tokenizer.Get();
            for (var i = 1; i < depth; i++)
            {
                Assert.Equal(TokenType.Function, token.Type);
                token = ((FunctionToken)token).ArgumentTokens.First(t => t.Type == TokenType.Function);
            }
            Assert.Contains(((FunctionToken)token).ArgumentTokens, t => t.Type == TokenType.Number);
        }

        [Theory]
        [InlineData("calc(")]
        [InlineData("var(")]
        public void ParserDeeplyNestedFunctionsDoNotOverflowStack(string function)
        {
            Exception error = null;
            var thread = new System.Threading.Thread(() =>
            {
                try
                {
                    var css = "a { width: " + Repeat(function, 100000) + "1" + Repeat(")", 100000) + "; color: red; }";
                    var sheet = new StylesheetParser().Parse(css);
                    Assert.NotNull(sheet.ToCss());
                }
                catch (Exception ex)
                {
                    error = ex;
                }
            }, 256 * 1024);
            thread.Start();
            thread.Join();

            Assert.Null(error);
        }

        [Fact]
        public void ParserModeratelyNestedCalcStillParses()
        {
            var css = "a { width: " + Repeat("calc(", 100) + "1px" + Repeat(" + 1px)", 100) + "; }";
            var sheet = new StylesheetParser().Parse(css);
            var rule = (StyleRule)sheet.StyleRules.Single();
            // 100 levels of nesting must still parse to a non-empty value; the exact text depends on whether
            // calc() is folded when serialized, so it is not asserted here.
            Assert.False(string.IsNullOrEmpty(rule.Style.Width));
            Assert.Contains("px", rule.Style.Width);
        }

        private static string Repeat(string value, int count)
        {
            return string.Concat(System.Linq.Enumerable.Repeat(value, count));
        }
    }
}
