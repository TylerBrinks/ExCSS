using System;

namespace ExCSS
{
    [Flags]
    internal enum PropertyFlags : byte
    {
        None = 0x0,
        Inherited = 0x1,
        Hashless = 0x2,
        Unitless = 0x4,
        Animatable = 0x8,
        Shorthand = 0x10
    }
}