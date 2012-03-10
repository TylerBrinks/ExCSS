
namespace ExCSS.Model
{
    /// <summary>
    /// Defines a stylesheet attribute.
    /// </summary>
    public class Attribute
    {
        private string _value;

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();
            builder.AppendFormat("[{0}", Operand);

           // if (_op.HasValue)
            //{
                switch (Operator)
                {
                    //default: //AttributeOperator.None
                    //    break;
                    case AttributeOperator.Equals:
                        builder.Append("=");
                        break;
                    case AttributeOperator.InList:
                        builder.Append("~=");
                        break;
                    case AttributeOperator.Hyphenated:
                        builder.Append("|=");
                        break;
                    case AttributeOperator.BeginsWith:
                        builder.Append("$=");
                        break;
                    case AttributeOperator.EndsWith:
                        builder.Append("^=");
                        break;
                    case AttributeOperator.Contains:
                        builder.Append("*=");
                        break;
                }

                builder.Append(Value);
            //}

            builder.Append("]");

            return builder.ToString();
        }

        /// <summary>
        /// Gets or sets the operand.
        /// </summary>
        /// <value>
        /// The operand.
        /// </value>
        public string Operand { get; set; }
        /// <summary>
        /// Gets or sets the operator.
        /// </summary>
        /// <value>
        /// The operator.
        /// </value>
        public AttributeOperator Operator { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }

        /// <summary>
        /// Gets or sets the operator string.
        /// </summary>
        /// <value>
        /// The operator string.
        /// </value>
        //public string OperatorString
        //{
        //    get
        //    {
        //        return _op.HasValue
        //            ? _op.Value.ToString() 
        //            : null;
        //    }
        //    set
        //    {
        //        _op = (AttributeOperator)Enum.Parse(typeof(AttributeOperator), value);
        //    }
        //}
    }
}