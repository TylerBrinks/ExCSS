
// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public abstract class RuleSet 
    {
        internal StyleSheet Context;

        internal RuleSet(StyleSheet context)
        {
            Context = context;
            RuleType = RuleType.Unknown;
        }

        public RuleType RuleType { get; set; }
    }
}
