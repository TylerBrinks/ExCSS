using System;
using System.Text;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a  CSS Selector
    /// </summary>
    public class SimpleSelector
    {
        /// <summary>
        /// Gets or sets the combinator string.
        /// </summary>
        /// <value>
        /// The combinator string.
        /// </value>
        public string CombinatorString
        {
            get
            {
                return Combinator.HasValue ? Combinator.ToString() : null;
            }
            set { Combinator = (Combinator)Enum.Parse(typeof(Combinator), value); }
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
            if (Combinator.HasValue)
            {
                switch (Combinator.Value)
                {
                    case Model.Combinator.PrecededImmediatelyBy:
                        builder.Append("+ ");
                        break;

                    case Model.Combinator.ChildOf:
                        builder.Append("> ");
                        break;

                    case Model.Combinator.PrecededBy:
                        builder.Append("~ ");
                        break;

                    case Model.Combinator.Namespace:
                        builder.Append("|");
                        break;
                }
            }

            if (ElementName != null)
            {
                builder.Append(ElementName);
            }

            if (ID != null)
            {
                builder.AppendFormat("#{0}", ID);
            }

            if (Class != null)
            {
                builder.AppendFormat(".{0}", Class);
            }

            if (Pseudo != null)
            {
                builder.AppendFormat(":{0}", Pseudo);
            }

            if (Attribute != null)
            {
                builder.Append(Attribute.ToString());
            }

            if (Function != null)
            {
                builder.Append(Function.ToString());
            }

            if (Child != null)
            {
                if (Child.ElementName != null)
                {
                    builder.Append(" ");
                }

                builder.Append(Child.ToString());
            }

            //return builder.ToString();
        }

        /// <summary>
        /// Gets or sets the combinator.
        /// </summary>
        /// <value>
        /// The combinator.
        /// </value>
        public Combinator? Combinator { get; set; }

        /// <summary>
        /// Gets or sets the name of the element.
        /// </summary>
        /// <value>
        /// The name of the element.
        /// </value>
        public string ElementName { get; set; }
        /// <summary>
        /// Gets or sets the ID.
        /// </summary>
        /// <value>
        /// The ID.
        /// </value>
        public string ID { get; set; }
        /// <summary>
        /// Gets or sets the class.
        /// </summary>
        /// <value>
        /// The class.
        /// </value>
        public string Class { get; set; }
        /// <summary>
        /// Gets or sets the pseudo.
        /// </summary>
        /// <value>
        /// The pseudo.
        /// </value>
        public string Pseudo { get; set; }
        /// <summary>
        /// Gets or sets the attribute.
        /// </summary>
        /// <value>
        /// The attribute.
        /// </value>
        public Attribute Attribute { get; set; }
        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        public Function Function { get; set; }
        /// <summary>
        /// Gets or sets the child.
        /// </summary>
        /// <value>
        /// The child.
        /// </value>
        public SimpleSelector Child { get; set; }
    }
}