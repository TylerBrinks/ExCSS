using System;

namespace ExCSS
{
    [Flags]
    public enum Sandboxes : ushort
    {
        None = 0,
        Navigation = 0x1,
        AuxiliaryNavigation = 0x2,
        TopLevelNavigation = 0x4,
        Plugins = 0x8,
        Origin = 0x10,
        Forms = 0x20,
        PointerLock = 0x40,
        Scripts = 0x80,
        AutomaticFeatures = 0x100,
        Fullscreen = 0x200,
        DocumentDomain = 0x400
    }
}