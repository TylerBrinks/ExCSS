﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExCSS
{
    public sealed class TokenValue : StylesheetNode, IEnumerable<Token>
    {
        private readonly List<Token> _tokens;
        public static TokenValue Initial = FromString(Keywords.Initial);
        public static TokenValue Empty = new(Enumerable.Empty<Token>());

        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            writer.Write(_tokens.ToText());
        }

        private TokenValue(Token token)
        {
            _tokens = new List<Token> {token};
        }

        public TokenValue(IEnumerable<Token> tokens)
        {
            _tokens = new List<Token>(tokens);
        }

        public static TokenValue FromString(string text)
        {
            var token = new Token(TokenType.Ident, text, TextPosition.Empty);
            return new TokenValue(token);
        }

        public Token this[int index] => _tokens[index];

        public int Count => _tokens.Count;

        public string Text => this.ToCss();

        public IEnumerator<Token> GetEnumerator()
        {
            return _tokens.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}