namespace ExCSS
{
    public enum ParseError : byte
    {
        EOF = 0,
        InvalidCharacter = 0x10,
        InvalidBlockStart = 0x11,
        InvalidToken = 0x12,
        ColonMissing = 0x13,
        IdentExpected = 0x14,
        InputUnexpected = 0x15,
        LineBreakUnexpected = 0x16,
        UnknownAtRule = 0x20,
        InvalidSelector = 0x30,
        InvalidKeyframe = 0x31,
        ValueMissing = 0x40,
        InvalidValue = 0x41,
        UnknownDeclarationName = 0x50
    }
}