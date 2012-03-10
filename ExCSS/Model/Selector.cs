using System.Collections.Generic;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a CSS selector
    /// </summary>
    public class Selector
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Selector"/> class.
        /// </summary>
        public Selector()
        {
            SimpleSelectors = new List<SimpleSelector>();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();
            var first = true;
            var previousHasNamespace = false;

            foreach (var selector in SimpleSelectors)
            {
                // Track namespace usage across selectors
                if (first) 
                {
                    first = false;
                } 
                else if(!previousHasNamespace && (!selector.Combinator.HasValue || selector.Combinator.Value != Combinator.Namespace))
                {
                    builder.Append(" ");
                }

                builder.Append(selector.ToString());

                // Track whether a namespace pipe is being built
                previousHasNamespace = selector.Combinator.HasValue && selector.Combinator.Value == Combinator.Namespace;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Gets or sets the simple selectors.
        /// </summary>
        /// <value>
        /// The simple selectors.
        /// </value>
        public List<SimpleSelector> SimpleSelectors { get; set; }
    }
}