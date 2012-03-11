using System.Text;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a CSS function
    /// </summary>
    public class Function
    {
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
            builder.AppendFormat("{0}(", Name);
            if (Expression != null)
            {
                var first = true;
                foreach (var t in Expression.Terms)
                {
                    if (first)
                    {
                        first = false;
                    }
                    else if (!t.Value.EndsWith("="))
                    {
                        builder.Append(", ");
                    }

                    var quoteMe = t.Type == TermType.String && !t.Value.EndsWith("=");
                    if (quoteMe)
                    {
                        builder.Append("'");
                    }

                    builder.Append(t.ToString());

                    if (quoteMe)
                    {
                        builder.Append("'");
                    }
                }
            }

            builder.Append(")");
            //return builder.ToString();
        }


        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
        /// <summary>
        /// Gets or sets the expression.
        /// </summary>
        /// <value>
        /// The expression.
        /// </value>
        public Expression Expression { get; set; }
    }
}