using System;
using System.Runtime.InteropServices;

namespace ExCSS
{
    [StructLayout(LayoutKind.Explicit, Pack = 1, CharSet = CharSet.Unicode)]
    public struct Priority : IEquatable<Priority>, IComparable<Priority>
    {
        [FieldOffset(0)] private readonly byte _tags;
        [FieldOffset(1)] private readonly byte _classes;
        [FieldOffset(2)] private readonly byte _ids;
        [FieldOffset(3)] private readonly byte _inlines;
        [FieldOffset(0)] private readonly uint _priority;

        public static readonly Priority Zero = new (0u);
        public static readonly Priority OneTag = new (0, 0, 0, 1);
        public static readonly Priority OneClass = new (0, 0, 1, 0);
        public static readonly Priority OneId = new (0, 1, 0, 0);
        public static readonly Priority Inline = new (1, 0, 0, 0);

        public Priority(uint priority)
        {
            _inlines = _ids = _classes = _tags = 0;
            _priority = priority;
        }

        public Priority(byte inlines, byte ids, byte classes, byte tags)
        {
            _priority = 0;
            _inlines = inlines;
            _ids = ids;
            _classes = classes;
            _tags = tags;
        }

        public byte Ids => _ids;
        public byte Tags => _tags;
        public byte Classes => _classes;
        public byte Inlines => _inlines;

        public static Priority operator +(Priority a, Priority b)
        {
            return new(a._priority + b._priority);
        }

        public static bool operator ==(Priority a, Priority b)
        {
            return a._priority == b._priority;
        }
        public static bool operator >(Priority a, Priority b)
        {
            return a._priority > b._priority;
        }

        public static bool operator >=(Priority a, Priority b)
        {
            return a._priority >= b._priority;
        }

        public static bool operator <(Priority a, Priority b)
        {
            return a._priority < b._priority;
        }

        public static bool operator <=(Priority a, Priority b)
        {
            return a._priority <= b._priority;
        }

        public static bool operator !=(Priority a, Priority b)
        {
            return a._priority != b._priority;
        }

        public bool Equals(Priority other)
        {
            return _priority == other._priority;
        }

        public override bool Equals(object obj)
        {
            return obj is Priority && Equals((Priority) obj);
        }

        public override int GetHashCode()
        {
            return (int) _priority;
        }

        public int CompareTo(Priority other)
        {
            return this == other ? 0 : (this > other ? 1 : -1);
        }

        public override string ToString()
        {
            return $"({_inlines}, {_ids}, {_classes}, {_tags})";
        }
    }
}