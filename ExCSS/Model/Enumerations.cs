
namespace ExCSS.Model
{
    internal static class RuleTypes
    {
        internal const string CharacterSet = "charset";
        internal const string Keyframes = "keyframes";
        internal const string Media = "media";
        internal const string Page = "page";
        internal const string Import = "import";
        internal const string FontFace = "font-face";
        internal const string Namespace = "namespace";
        internal const string Supports = "supports";
    }
    
    public enum Combinator
    {
        Child,
        Descendent,
        AdjacentSibling,
        Sibling
    }

    enum GrammarSegment
    {
        String,
        Url,
        Hash,           //#
        AtRule,         //@
        Ident,
        Function,
        Number,
        Percentage,
        Dimension,
        Range,
        CommentOpen,
        CommentClose,
        Column,
        Delimiter,
        IncludeMatch,   //~=
        DashMatch,      // |=
        PrefixMatch,    // ^=
        SuffixMatch,    // $=
        SubstringMatch, // *=
        NegationMatch,  // !=
        ParenOpen,
        ParenClose,
        CurlyBraceOpen,
        CurlyBracketClose,
        SquareBraceOpen,
        SquareBracketClose,
        Colon,
        Comma,
        Semicolon,
        Whitespace
    }

    public enum RuleType //: ushort
    {

        Unknown = 0,
        Style = 1,
        Charset = 2,
        Import = 3,
        Media = 4,
        FontFace = 5,
        Page = 6,
        Keyframes = 7,
        Keyframe = 8,
        Namespace = 10,
        CounterStyle = 11,
        Supports = 12,
        Document = 13,
        FontFeatureValues = 14,
        Viewport = 15,
        RegionStyle = 16
    }

    public enum RuleValueType// : ushort
    {
        Inherit = 0,
        PrimitiveValue = 1,
        ValueList = 2,
        Custom = 3
    }

    public enum UnitType //: ushort
    {
        Unknown = 0,
        Number = 1,
        Percentage = 2,
        Ems = 3,
        Exs = 4,
        Pixel = 5,
        Centimeter = 6,
        Millimeter = 7,
        Inch = 8,
        Point = 9,
        Percent = 10,
        Degree = 11,
        Radian = 12,
        Grad = 13,
        Millisecond = 14,
        Second = 15,
        Hertz = 16,
        KiloHertz = 17,
        Dimension = 18,
        String = 19,
        Uri = 20,
        Ident = 21,
        Attribute = 22,
        Counter = 23,
        Rect = 24,
        RGB = 25,
        ViewportWidth = 26,
        ViewportHeight = 28,
        ViewportMin = 29,
        ViewportMax = 30,
        Turn = 31,
    }

    public enum DirectionMode
    {
        LeftToRight,
        RightToLeft
    }
}
