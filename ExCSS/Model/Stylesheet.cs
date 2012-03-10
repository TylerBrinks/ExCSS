using System.Collections.Generic;
using System.Text;

namespace ExCSS.Model
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
            var txt = new StringBuilder();

            foreach (var dr in Directives)
            {
                txt.AppendFormat("{0}\r\n", dr);
            }

            if (txt.Length > 0) { txt.Append("\r\n"); }

            foreach (var rules in RuleSets)
            {
                txt.AppendFormat("{0}\r\n", rules);
            }

            return txt.ToString();
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