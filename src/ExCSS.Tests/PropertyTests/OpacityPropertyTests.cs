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
        var concrete = (OpacityProperty)property;
        Assert.False(concrete.IsInherited);
        Assert.True(concrete.HasValue);
        Assert.Equal("50%", concrete.Value);
    }
}
