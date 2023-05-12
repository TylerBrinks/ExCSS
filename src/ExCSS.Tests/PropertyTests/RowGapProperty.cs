using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class RowGapPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [MemberData(nameof(RowGapTestDataValues))]
        public void RowGapLegalValues(string value)
            => TestForLegalValue<RowGapProperty>(PropertyNames.RowGap, value);

        public static IEnumerable<object[]> RowGapTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { "20px" },
                    new object[] { "1em" },
                    new object[] { "3vmin" },
                    new object[] { "0.5cm" },
                    new object[] { "10%" },
                }.Union(Property.GlobalKeywordValues.ToObjectArray());
            }
        }
    }
}
