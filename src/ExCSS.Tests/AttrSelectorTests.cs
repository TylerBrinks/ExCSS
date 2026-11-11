using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ExCSS.Tests;

public class AttrSelectorTests
{
    [Fact]
    public async Task FindAllAttrMatchSelectorsThatMatchAttributeName()
    {
        // Arrange
        var sheet = await GetAttributeStylesheetAsync("");

        // Act
        var list = GetAttributeStyleRules<AttrMatchSelector>(sheet);

        // Assert
        Assert.Equal(2, list.Count());
    }

    [Fact]
    public async Task FindAllAttrInListSelectorsThatMatchAttributeName()
    {
        // Arrange
        var sheet = await GetAttributeStylesheetAsync("~");

        // Act
        var list = GetAttributeStyleRules<AttrListSelector>(sheet);

        // Assert
        Assert.Equal(2, list.Count());
    }
    
    [Fact]
    public async Task FindAllAttrHyphenSelectorsThatMatchAttributeName()
    {
        // Arrange
        var sheet = await GetAttributeStylesheetAsync("|");

        // Act
        var list = GetAttributeStyleRules<AttrHyphenSelector>(sheet);

        // Assert
        Assert.Equal(2, list.Count());
    }
    
    [Fact]
    public async Task FindAllAttrBeginsSelectorsThatMatchAttributeName()
    {
        // Arrange
        var sheet = await GetAttributeStylesheetAsync("^");

        // Act
        var list = GetAttributeStyleRules<AttrBeginsSelector>(sheet);

        // Assert
        Assert.Equal(2, list.Count());
    }
    
    [Fact]
    public async Task FindAllAttrEndsSelectorsThatMatchAttributeName()
    {
        // Arrange
        var sheet = await GetAttributeStylesheetAsync("$");

        // Act
        var list = GetAttributeStyleRules<AttrEndsSelector>(sheet);

        // Assert
        Assert.Equal(2, list.Count());
    }
    
    [Fact]
    public async Task FindAllAttrContainsSelectorsThatMatchAttributeName()
    {
        // Arrange
        var sheet = await GetAttributeStylesheetAsync("*");

        // Act
        var list = GetAttributeStyleRules<AttrContainsSelector>(sheet);

        // Assert
        Assert.Equal(2, list.Count());
    }
    
    [Fact]
    public async Task FindAllAttrNotMatchSelectorsThatMatchAttributeName()
    {
        // Arrange
        var sheet = await GetAttributeStylesheetAsync("!");

        // Act
        var list = GetAttributeStyleRules<AttrNotMatchSelector>(sheet);

        // Assert
        Assert.Equal(2, list.Count());
    }

    [Theory]
    [InlineData("[type=button i]", "Insensitive", "[type=\"button\" i]")]
    [InlineData("[type=button I]", "Insensitive", "[type=\"button\" i]")]
    [InlineData("[type=\"button\"i]", "Insensitive", "[type=\"button\" i]")]
    [InlineData("[type = button   s ]", "Sensitive", "[type=\"button\" s]")]
    [InlineData("[type=button S]", "Sensitive", "[type=\"button\" s]")]
    [InlineData("[type=button]", "Default", "[type=\"button\"]")]
    public async Task CaseSensitivityFlagIsParsedAndSerialized(string selector, string expected, string text)
    {
        var sheet = await new StylesheetParser().ParseAsync(selector + " { color: red }");

        var rule = Assert.Single(sheet.StyleRules);
        var attr = Assert.IsType<AttrMatchSelector>(rule.Selector);

        Assert.Equal(expected, attr.CaseSensitivity.ToString());
        Assert.Equal("button", attr.Value);
        Assert.Equal(text, attr.Text);
    }

    [Theory]
    [InlineData("~=", typeof(AttrListSelector))]
    [InlineData("|=", typeof(AttrHyphenSelector))]
    [InlineData("^=", typeof(AttrBeginsSelector))]
    [InlineData("$=", typeof(AttrEndsSelector))]
    [InlineData("*=", typeof(AttrContainsSelector))]
    [InlineData("!=", typeof(AttrNotMatchSelector))]
    public async Task CaseSensitivityFlagIsAcceptedOnEveryValueOperator(string op, System.Type type)
    {
        var sheet = await new StylesheetParser().ParseAsync("[type" + op + "button i] { color: red }");

        var rule = Assert.Single(sheet.StyleRules);

        Assert.IsType(type, rule.Selector);
        Assert.Equal(AttrCaseSensitivity.Insensitive, ((IAttrSelector)rule.Selector).CaseSensitivity);
    }

    [Theory]
    [InlineData("[type i]")]
    [InlineData("[type=button x]")]
    [InlineData("[type=button i i]")]
    [InlineData("[type=button i s]")]
    [InlineData("[type=button i extra]")]
    public async Task InvalidCaseSensitivityFlagDropsTheRule(string selector)
    {
        var sheet = await new StylesheetParser().ParseAsync(selector + " { color: red } p { color: blue }");

        var rule = Assert.Single(sheet.StyleRules);

        Assert.Equal("p", rule.SelectorText);
    }

    private async Task<Stylesheet> GetAttributeStylesheetAsync(string combinator)
    {
        var css = @"[type" + combinator + "='button'] { background-color: #101010 } .sample-class[type"
                  + combinator + "='input'] { background-color: #121212 }";

        return await new StylesheetParser().ParseAsync(css);
    }

    private IEnumerable<IStyleRule> GetAttributeStyleRules<T>(Stylesheet sheet) where T : IAttrSelector
    {
        return sheet.StyleRules
            .Where(x =>
                (x.Selector is CompoundSelector selector &&
                 selector.Any(y => y is T { Attribute: "type" }))
                || x.Selector is T { Attribute: "type" }
            );
    }
}