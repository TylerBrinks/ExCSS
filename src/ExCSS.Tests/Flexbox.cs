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

        [Theory]
        [MemberData(nameof(AlignContentTestDataValues))]
        public void AlignContentLegalValues(string value)
            => TestForLegalValue<AlignContentProperty>(PropertyNames.AlignContent, value);

        [Theory]
        [MemberData(nameof(AlignContentInvalidPrefixTestDataValues))]
        public void AlignContentIllegalPrefixValues(string value)
        {
            var snippet = $"align-content: {value}";
            var property = ParseDeclaration(snippet);
            Assert.Equal("align-content", property.Name);
            Assert.False(property.HasValue);
        }

        [Theory]
        [MemberData(nameof(AlignItemsTestDataValues))]
        public void AlignItemsLegalValues(string value)
            => TestForLegalValue<AlignItemsProperty>(PropertyNames.AlignItems, value);

        [Theory]
        [MemberData(nameof(AlignSelfTestDataValues))]
        public void AlignSelfLegalValues(string value)
            => TestForLegalValue<AlignSelfProperty>(PropertyNames.AlignSelf, value);

        [Theory]
        [MemberData(nameof(AlignItemsInvalidPrefixTestDataValues))]
        public void AlignItemsIllegalPrefixValues(string value)
        {
            var snippet = $"align-items: {value}";
            var property = ParseDeclaration(snippet);
            Assert.Equal("align-items", property.Name);
            Assert.False(property.HasValue);
        }

        [Theory]
        [MemberData(nameof(JustifyContentTestDataValues))]
        public void JustifyContentLegalValues(string value)
            => TestForLegalValue<JustifyContentProperty>(PropertyNames.JustifyContent, value);

        [Theory]
        [MemberData(nameof(JustifyContentInvalidPrefixTestDataValues))]
        public void JustifyContentIllegalPrefixValues(string value)
        {
            var snippet = $"align-items: {value}";
            var property = ParseDeclaration(snippet);
            Assert.Equal("align-items", property.Name);
            Assert.False(property.HasValue);
        }

        [Theory]
        [MemberData(nameof(FlexFlowTestDataValues))]
        public void FlexFlowLegalValues(string value)
            => TestForLegalValue<FlexFlowProperty>(PropertyNames.FlexFlow, value);

        [Theory]
        [MemberData(nameof(FlexFlowExpandedTestValues))]
        public void FlexFlowShorthandValueExpanded(string propertyValue, string expectedDirection, string expectedWrap)
        {
            var source = $".test {{ flex-flow: {propertyValue}; }}";
            var styleSheet = ParseStyleSheet(source);
            var rule = styleSheet.StyleRules.First() as StyleRule;

            Assert.Equal(rule.Style.FlexDirection, expectedDirection);
            Assert.Equal(rule.Style.FlexWrap, expectedWrap);
        }

        public static IEnumerable<object[]> FlexDirectionTestDataValues
            => FlexDirectionProperty.KeywordValues.ToObjectArray();

        public static IEnumerable<object[]> FlexWrapTestDataValues
            => FlexWrapProperty.KeywordValues.ToObjectArray();

        public static IEnumerable<object[]> FlexFlowTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { "row" },
                    new object[] { "row-reverse" },
                    new object[] { "column" },
                    new object[] { "column-reverse" },
                    new object[] { "nowrap" },
                    new object[] { "wrap" },
                    new object[] { "wrap-reverse" },
                    new object[] { "row nowrap" },
                    new object[] { "column wrap" },
                    new object[] { "column-reverse wrap-reverse" },
                }.Union(Property.GlobalKeywordValues.ToObjectArray());
            }
        }

        public static IEnumerable<object[]> FlexFlowExpandedTestValues
        {
            get
            {
                return new[]
                {
                    new object[] { "row nowrap", "row", "nowrap" },
                    new object[] { "column wrap", "column", "wrap" },
                    new object[] { "column-reverse wrap-reverse", "column-reverse", "wrap-reverse" },
                };
            }
        }
        
        public static IEnumerable<object[]> AlignContentTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { Keywords.Center },
                    new object[] { Keywords.Start },
                    new object[] { Keywords.End },
                    new object[] { Keywords.FlexStart },
                    new object[] { Keywords.FlexEnd },
                    new object[] { Keywords.Normal },
                    new object[] { Keywords.Baseline },
                    new object[] { $"{Keywords.First} {Keywords.Baseline}" },
                    new object[] { $"{Keywords.Last} {Keywords.Baseline}" },
                    new object[] { Keywords.SpaceBetween },
                    new object[] { Keywords.SpaceAround },
                    new object[] { Keywords.SpaceEvenly },
                    new object[] { Keywords.Stretch },
                    new object[] { $"{Keywords.Safe} {Keywords.Center}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.Center}" },
                }.Union(Property.GlobalKeywordValues.ToObjectArray());
            }
        }

        public static IEnumerable<object[]> AlignSelfTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { Keywords.Auto },
                    new object[] { Keywords.Stretch },
                    new object[] { Keywords.Center },
                    new object[] { Keywords.Start },
                    new object[] { Keywords.End },
                    new object[] { Keywords.FlexStart },
                    new object[] { Keywords.FlexEnd },
                    new object[] { Keywords.Normal },
                    new object[] { Keywords.Baseline },
                    new object[] { $"{Keywords.First} {Keywords.Baseline}" },
                    new object[] { $"{Keywords.Last} {Keywords.Baseline}" },
                    new object[] { $"{Keywords.Safe} {Keywords.Center}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.Center}" },
                }.Union(Property.GlobalKeywordValues.ToObjectArray());
            }
        }

        public static IEnumerable<object[]> JustifyContentTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { Keywords.Center },
                    new object[] { Keywords.Start },
                    new object[] { Keywords.End },
                    new object[] { Keywords.FlexStart },
                    new object[] { Keywords.FlexEnd },
                    new object[] { Keywords.Left },
                    new object[] { Keywords.Right },
                    new object[] { Keywords.Normal },
                    new object[] { Keywords.SpaceBetween },
                    new object[] { Keywords.SpaceAround },
                    new object[] { Keywords.SpaceEvenly },
                    new object[] { Keywords.Stretch },
                    new object[] { $"{Keywords.Safe} {Keywords.Center}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.Center}" },
                }.Union(Property.GlobalKeywordValues.ToObjectArray());
            }
        }

        public static IEnumerable<object[]> AlignItemsTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { Keywords.Normal },
                    new object[] { Keywords.Stretch },
                    new object[] { Keywords.Center },
                    new object[] { Keywords.Start },
                    new object[] { Keywords.End },
                    new object[] { Keywords.FlexStart },
                    new object[] { Keywords.FlexEnd },
                    new object[] { Keywords.SelfStart },
                    new object[] { Keywords.SelfEnd },
                    new object[] { Keywords.Baseline },
                    new object[] { $"{Keywords.First} {Keywords.Baseline}" },
                    new object[] { $"{Keywords.Last} {Keywords.Baseline}" },
                    new object[] { $"{Keywords.Safe} {Keywords.Center}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.Center}" },
                }.Union(Property.GlobalKeywordValues.ToObjectArray());
            }
        }


        public static IEnumerable<object[]> AlignContentInvalidPrefixTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { $"{Keywords.Safe} {Keywords.Start}" },
                    new object[] { $"{Keywords.Safe} {Keywords.End}" },
                    new object[] { $"{Keywords.Safe} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.Safe} {Keywords.FlexEnd}" },

                    new object[] { $"{Keywords.Unsafe} {Keywords.Start}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.End}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.FlexEnd}" },

                    new object[] { $"{Keywords.First} {Keywords.Start}" },
                    new object[] { $"{Keywords.First} {Keywords.End}" },
                    new object[] { $"{Keywords.First} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.First} {Keywords.FlexEnd}" },
                
                    new object[] { $"{Keywords.Last} {Keywords.Start}" },
                    new object[] { $"{Keywords.Last} {Keywords.End}" },
                    new object[] { $"{Keywords.Last} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.Last} {Keywords.FlexEnd}" },
                };
            }
        }

        public static IEnumerable<object[]> AlignItemsInvalidPrefixTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { $"{Keywords.Safe} {Keywords.Start}" },
                    new object[] { $"{Keywords.Safe} {Keywords.End}" },
                    new object[] { $"{Keywords.Safe} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.Safe} {Keywords.FlexEnd}" },
                    new object[] { $"{Keywords.Safe} {Keywords.SelfStart}" },
                    new object[] { $"{Keywords.Safe} {Keywords.SelfEnd}" },

                    new object[] { $"{Keywords.Unsafe} {Keywords.Start}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.End}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.FlexEnd}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.SelfStart}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.SelfEnd}" },

                    new object[] { $"{Keywords.First} {Keywords.Start}" },
                    new object[] { $"{Keywords.First} {Keywords.End}" },
                    new object[] { $"{Keywords.First} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.First} {Keywords.FlexEnd}" },
                    new object[] { $"{Keywords.First} {Keywords.SelfStart}" },
                    new object[] { $"{Keywords.First} {Keywords.SelfEnd}" },

                    new object[] { $"{Keywords.Last} {Keywords.Start}" },
                    new object[] { $"{Keywords.Last} {Keywords.End}" },
                    new object[] { $"{Keywords.Last} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.Last} {Keywords.FlexEnd}" },
                    new object[] { $"{Keywords.Last} {Keywords.SelfStart}" },
                    new object[] { $"{Keywords.Last} {Keywords.SelfEnd}" }
                };
            }
        }

        public static IEnumerable<object[]> JustifyContentInvalidPrefixTestDataValues
        {
            get
            {
                return new[]
                {
                    new object[] { $"{Keywords.Safe} {Keywords.Start}" },
                    new object[] { $"{Keywords.Safe} {Keywords.End}" },
                    new object[] { $"{Keywords.Safe} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.Safe} {Keywords.FlexEnd}" },
                    new object[] { $"{Keywords.Safe} {Keywords.SelfStart}" },
                    new object[] { $"{Keywords.Safe} {Keywords.SelfEnd}" },

                    new object[] { $"{Keywords.Unsafe} {Keywords.Start}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.End}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.FlexStart}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.FlexEnd}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.SelfStart}" },
                    new object[] { $"{Keywords.Unsafe} {Keywords.SelfEnd}" },
                };
            }
        }

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
    }
}
