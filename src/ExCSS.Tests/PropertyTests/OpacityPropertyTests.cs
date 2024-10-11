using Xunit;

namespace ExCSS.Tests.PropertyTests;
public class OpacityPropertyTests : CssConstructionFunctions
{
    [Fact]
    public void OpacityPercentLegal()
    {
        var snippet = "opacity: 50%";
        var property = ParseDeclaration(snippet);
        Assert.Equal("opacity", property.Name);
        Assert.False(property.IsImportant);
        Assert.IsType<OpacityProperty>(property);
        var result = (OpacityProperty)property;
        Assert.False(result.IsInherited);
        Assert.True(result.HasValue);
        Assert.Equal("50%", result.Value);
    }

    
    [Fact]
    public void OpacityVarianceTests()
    {
        var property = ParseDeclaration("opacity: 50%");
        var result = (OpacityProperty)property;
        Assert.Equal("50%", result.Value);

        property = ParseDeclaration("opacity: .50");
        result = (OpacityProperty)property;
        Assert.Equal("50%", result.Value);

        property = ParseDeclaration("opacity: 1");
        result = (OpacityProperty)property;
        Assert.Equal("100%", result.Value);

        property = ParseDeclaration("opacity: 0");
        result = (OpacityProperty)property;
        Assert.Equal("0%", result.Value);
    }
}
