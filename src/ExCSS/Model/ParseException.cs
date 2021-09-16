using System;

namespace ExCSS
{
    public class ParseException : Exception
    {
        public ParseException(string message) : base(message)
        {
        }
    }
}