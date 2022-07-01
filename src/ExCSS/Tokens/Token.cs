﻿namespace ExCSS
{
    public class Token
    {
        public static readonly Token Whitespace = new(TokenType.Whitespace, " ", TextPosition.Empty);
        public static readonly Token Comma = new(TokenType.Comma, ",", TextPosition.Empty);

        public Token(TokenType type, string data, TextPosition position)
        {
            Type = type;
            Data = data;
            Position = position;
        }

        public virtual string ToValue()
        {
            return Data;
        }

        public TextPosition Position { get; }
        public TokenType Type { get; }
        public string Data { get; }
    }
}