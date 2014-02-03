// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class Function : Term
    {
        internal static Function Create(string name, TermList arguments)
        {
            var function = new Function { Text = name + "(" + arguments + ")" };
            return function;
        }
    }
}
