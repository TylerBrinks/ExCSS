using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents an expression in CSS
    /// </summary>
    public class Expression
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Expression"/> class.
        /// </summary>
        public Expression()
        {
            Terms = new List<Term>();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new StringBuilder();
            BuildElementString(builder);

            return builder.ToString();
        }

        internal void BuildElementString(StringBuilder builder)
        {
            var first = true;

            foreach (var term in Terms)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.AppendFormat("{0} ", term.Seperator.HasValue ? term.Seperator.Value.ToString(CultureInfo.InvariantCulture) : "");
                }

                term.BuildElementString(builder);
            }
        }

        /// <summary>
        /// Gets or sets the terms.
        /// </summary>
        /// <value>
        /// The terms.
        /// </value>
        public List<Term> Terms { get; set; }
    }
}