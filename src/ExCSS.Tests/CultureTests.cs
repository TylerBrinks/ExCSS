using System.Globalization;
using Xunit;

namespace ExCSS.Tests
{
    public class CultureTests : CssConstructionFunctions
    {
        [Fact]
        public void NegativeIntegerIsParsedWithTheInvariantCulture()
        {
            var previous = CultureInfo.CurrentCulture;
            try
            {
                var culture = (CultureInfo) new CultureInfo("en-US").Clone();
                culture.NumberFormat.NegativeSign = "~";
                CultureInfo.CurrentCulture = culture;

                var sheet = ParseStyleSheet(".style{ z-index: -1; }");

                var rule = Assert.IsType<StyleRule>(sheet.Rules[0]);
                Assert.Equal("-1", rule.Style.ZIndex);
            }
            finally
            {
                CultureInfo.CurrentCulture = previous;
            }
        }
    }
}
