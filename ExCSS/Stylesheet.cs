using System.Collections.Generic;
using System.Text;
using ExCSS.Model;

namespace ExCSS
{
    /// <summary>
    /// Represents a stylesheet with a list of rules and directives
    /// </summary>
    public class Stylesheet : IRuleSetContainer
    {
        public Stylesheet()
        {
            Directives = new List<Directive>();
            RuleSets = new List<RuleSet>();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return BuildElementString(new StringBuilder());
        }

        internal string BuildElementString(StringBuilder builder)
        {
            foreach (var directive in Directives)
            {
                directive.BuildElementString(builder);
                builder.Append("\r\n");
                //builder.AppendFormat("{0}\r\n", directive.BuildElementString(builder));
            }

            if (builder.Length > 0)
            {
                builder.Append("\r\n");
            }

            foreach (var rule in RuleSets)
            {
                rule.BuildElementString(builder);
                builder.Append("\r\n");
                //builder.AppendFormat("{0}\r\n", rule.BuildElementString(builder));
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets the directives.
        /// </summary>
        public List<Directive> Directives { get; private set; }
        /// <summary>
        /// Gets the rule sets.
        /// </summary>
        public List<RuleSet> RuleSets { get; private set; }
    }
}