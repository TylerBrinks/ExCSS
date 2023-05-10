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
        {
            var snippet = $"flex-direction: {value}";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-direction", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexDirectionProperty>(property);
            var concrete = (FlexDirectionProperty)property;
            Assert.True(concrete.HasValue);
            Assert.Equal(value, concrete.Value);
        }

        [Theory]
        [MemberData(nameof(FlexWrapTestDataValues))]
        public void FlexWrapLegalValues(string value)
        {
            var snippet = $"flex-wrap: {value}";
            var property = ParseDeclaration(snippet);
            Assert.Equal("flex-wrap", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<FlexWrapProperty>(property);
            var concrete = (FlexWrapProperty)property;
            Assert.True(concrete.HasValue);
            Assert.Equal(value, concrete.Value);
        }

        [Theory]
        [MemberData(nameof(OrderTestDataValues))]
        public void OrderLegalValues(string value)
        {
            var snippet = $"order: {value}";
            var property = ParseDeclaration(snippet);
            Assert.Equal("order", property.Name);
            Assert.False(property.IsImportant);
            Assert.IsType<OrderProperty>(property);
            var concrete = (OrderProperty)property;
            Assert.True(concrete.HasValue);
            Assert.Equal(value, concrete.Value);
        }

        public static IEnumerable<object[]> FlexDirectionTestDataValues
            => FlexDirectionProperty.KeywordValues.ToObjectArray();

        public static IEnumerable<object[]> FlexWrapTestDataValues
            => FlexWrapProperty.KeywordValues.ToObjectArray();

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
    }
}
