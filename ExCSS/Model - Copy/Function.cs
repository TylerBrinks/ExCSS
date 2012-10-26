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

                    var quoted = term.Type == TermType.String && 
                        !term.Value.EndsWith("=") && 
                        !Expression.ToString().Contains(",");// i.e. -moz-gradient(top, 5px, 10px) should not be quoted
                    if (quoted)
                    {
                        builder.Append("'");
                    }

                    term.BuildElementString(builder);

                    if (quoted)
                    {
                        builder.Append("'");
                    }
                }
            }

            builder.Append(")");
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