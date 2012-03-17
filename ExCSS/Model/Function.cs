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
                foreach (var term in Expression.Terms)
                {
                    if(term.Value == null)
                    {
                        continue;
                    }

                    if (first)
                    {
                        first = false;
                    }
                    else if (!term.Value.EndsWith("="))
                    {
                        builder.Append(", ");
                    }

                    var quoteMe = term.Type == TermType.String && !term.Value.EndsWith("=");
                    if (quoteMe)
                    {
                        builder.Append("'");
                    }

                    //builder.Append(term.ToString());
                    term.BuildElementString(builder);

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