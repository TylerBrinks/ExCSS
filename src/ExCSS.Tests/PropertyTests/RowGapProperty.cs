using Xunit;

namespace ExCSS.Tests.PropertyTests
{
    public class RowGapPropertyTests : CssConstructionFunctions
    {
        [Theory]
        [MemberData(nameof(LengthOrPercentOrGlobalTestValues))]
        public void RowGapLegalValues(string value)
            => TestForLegalValue<RowGapProperty>(PropertyNames.RowGap, value);
    }
}
