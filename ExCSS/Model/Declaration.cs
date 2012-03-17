using System.Text;

namespace ExCSS.Model 
{
	public class Declaration 
    {
        public override string ToString()
        {
            var builder = new StringBuilder();
            BuildElementString(builder);

            return builder.ToString();
        }

        internal void BuildElementString(StringBuilder builder)
        {
            //builder.AppendFormat("{0}: {1}{2}", Name, Expression, Important ? " !important" : "");
            builder.AppendFormat("{0}: ", Name);// Expression, Important ? " !important" : "");

            Expression.BuildElementString(builder);

            builder.Append(Important ? " !important" : "");
        }

	    public string Name { get; set; }
        public bool Important { get; set; }
	    public Expression Expression { get; set; }
    }
}