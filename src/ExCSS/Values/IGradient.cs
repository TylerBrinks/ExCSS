using System.Collections.Generic;

namespace ExCSS
{
    public interface IGradient : IImageSource
    {
        IEnumerable<GradientStop> Stops { get; }
        bool IsRepeating { get; }
    }
}