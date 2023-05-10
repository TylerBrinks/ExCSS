using System.Collections.Generic;
using System.Linq;

using Xunit;

namespace ExCSS.Tests
{
    public class Flexbox : CssConstructionFunctions
    {
        [Theory]
        [MemberData(nameof(FlexDirectionTestDataValues))]
        public void FlexDirectionLegalValues(string value)
            => TestForLegalValue<FlexDirectionProperty>(PropertyNames.FlexDirection, value);

        [Theory]
        [MemberData(nameof(FlexWrapTestDataValues))]
        public void FlexWrapLegalValues(string value)
            => TestForLegalValue<FlexWrapProperty>(PropertyNames.FlexWrap, value);

        [Theory]
        [MemberData(nameof(OrderTestDataValues))]
        public void OrderLegalValues(string value)
            => TestForLegalValue<OrderProperty>(PropertyNames.Order, value);

        [Theory]
        [MemberData(nameof(FlexBasisTestDataValues))]
        public void FlexBasisLegalValues(string value)
            => TestForLegalValue<FlexBasisProperty>(PropertyNames.FlexBasis, value);

        [Theory]
        [MemberData(nameof(FlexGrowShrinkTestDataValues))]
        public void FlexGrowLegalValues(string value)
            => TestForLegalValue<FlexGrowProperty>(PropertyNames.FlexGrow, value);

        [Theory]
        [MemberData(nameof(FlexGrowShrinkTestDataValues))]
        public void FlexShrinkLegalValues(string value)
            => TestForLegalValue<FlexShrinkProperty>(PropertyNames.FlexShrink, value);

        public static IEnumerable<object[]> FlexDirectionTestDataValues
            => FlexDirectionProperty.KeywordValues.ToObjectArray();

        public static IEnumerable<object[]> FlexWrapTestDataValues
            => FlexWrapProperty.KeywordValues.ToObjectArray();

        public static IEnumerable<object[]> FlexGrowShrinkTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { "3" },
                    new object[] { "0.6" }
                }.Union(Property.GlobalKeywordValues.ToObjectArray());
            }
        }

        public static IEnumerable<object[]> FlexBasisTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { "10em" },
                    new object[] { "3px" },
                    new object[] { "50%" }
                }.Union(FlexBasisProperty.KeywordValues.Union(Property.GlobalKeywordValues).ToObjectArray());
            }
        }

        public static IEnumerable<object[]> OrderTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { "-1" },
                    new object[] { "1" },
                }.Union(Property.GlobalKeywordValues.ToObjectArray());
            }
        }

        private void TestForLegalValue<TProp>(string propertyName, string value) where TProp : Property
        {
            var snippet = $"{propertyName}: {value}";
            var property = ParseDeclaration(snippet);
            Assert.Equal(propertyName, property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<TProp>(property);
            var concrete = (TProp)property;
            Assert.True(concrete.HasValue);
            Assert.Equal(value, concrete.Value);
        }
    }
}
