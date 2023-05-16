using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    internal sealed class FlexWrapProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.FlexWrapConverter;

        internal FlexWrapProperty()
            : base(PropertyNames.FlexWrap)
        { }

        internal override IValueConverter Converter => StyleConverter;

        internal static IEnumerable<string> KeywordValues
        {
            get
            {
                return new[]
                {
                    Keywords.Nowrap,
                    Keywords.Wrap,
                    Keywords.WrapReverse,
                }.Union(GlobalKeywordValues);
            }
        }
    }
}
