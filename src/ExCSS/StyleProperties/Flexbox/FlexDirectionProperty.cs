using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    internal sealed class FlexDirectionProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.FlexDirectionConverter;

        internal FlexDirectionProperty()
            : base(PropertyNames.FlexDirection)
        { }

        internal override IValueConverter Converter => StyleConverter;

        internal static IEnumerable<string> KeywordValues
        {
            get
            {
                return new[]
                {
                    Keywords.Row,
                    Keywords.RowReverse,
                    Keywords.Column,
                    Keywords.ColumnReverse,
                }.Union(GlobalKeywordValues);
            }
        }
    }
}