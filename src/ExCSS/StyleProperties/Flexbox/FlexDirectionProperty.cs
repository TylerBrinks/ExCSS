using System.Collections.Generic;
using System.Linq;

namespace ExCSS
{
    internal sealed class FlexDirectionProperty : Property
    {
        private static readonly IValueConverter StyleConverter = Converters.FlexDirectionConverter
                                                                           .OrGlobalValue()
                                                                           .OrDefault(FlexDirection.Row);

        internal FlexDirectionProperty()
            : base(PropertyNames.FlexDirection)
        { }

        internal override IValueConverter Converter => StyleConverter;

        internal static IEnumerable<string> AllowedValues
        {
            get
            {
                return new[]
                {
                    Keywords.Row,
                    Keywords.RowReverse,
                    Keywords.Column,
                    Keywords.ColumnReverse,
                }.Union(GlobalPropertyValues);
            }
        }
    }
}