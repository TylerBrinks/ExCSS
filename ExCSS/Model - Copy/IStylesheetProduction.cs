
namespace ExCSS.Model
{
    public interface IStylesheetProduction
    {
        IStylesheetProduction CreateChildProduction(TokenType tokenType);
        void SetProductionValue(string value);
    }
}
