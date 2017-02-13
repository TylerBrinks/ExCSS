using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExCSS
{
    public sealed class Medium : StylesheetNode
    {

        //private static readonly string[] KnownTypes = {Keywords.Screen, Keywords.Speech, Keywords.Print, Keywords.All};
        public IEnumerable<MediaFeature> Features => Children.OfType<MediaFeature>();
        public string Type { get; internal set; }
        public bool IsExclusive { get; internal set; }
        public bool IsInverse { get; internal set; }

        public string Constraints
        {
            get
            {
                var constraints = Features.Select(m => m.ToCss());
                return string.Join(" and ", constraints);
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as Medium;
            if ((other != null) &&
                (other.IsExclusive == IsExclusive) &&
                (other.IsInverse == IsInverse) &&
                other.Type.Is(Type) &&
                (other.Features.Count() == Features.Count()))
            {
                foreach (var feature in other.Features)
                {
                    var isShared = Features.Any(m => m.Name.Is(feature.Name) && m.Value.Is(feature.Value));
                    if (!isShared)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
        public override void ToCss(TextWriter writer, IStyleFormatter formatter)
        {
            writer.Write(formatter.Medium(IsExclusive, IsInverse, Type, Features));
        }
    }
}