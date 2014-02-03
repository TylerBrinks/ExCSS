
// ReSharper disable once CheckNamespace
namespace ExCSS
{
    internal abstract class NthChildSelector : SimpleSelector, IToString
    {
        public int Step;
        public int Offset;
        internal string FunctionText { get; set; }

        internal string FormatSelector(string functionName)
        {
            var format = Offset < 0
                ? ":{0}({1}n{2})"
                : ":{0}({1}n+{2})";

            return string.IsNullOrEmpty(FunctionText)
                ? string.Format(format, functionName, Step, Offset)
                : string.Format(":{0}({1})", functionName, FunctionText);
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public new abstract string ToString(bool friendlyFormat, int indentation = 0);
    }
}