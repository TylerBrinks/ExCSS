
namespace ExCSS
{
    public /*abstract*/ class Function : Term
    {
        private Function()
        {
        }

        internal static Function Create(string name, TermList arguments)
        {
            var f = new Function();
            f.Text = name + "(" + arguments.ToString() + ")";
            return f;
        }
    }
}
