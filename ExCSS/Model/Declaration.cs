using System.Text;

namespace ExCSS.Model 
{
	public class Declaration 
    {  
		public override string ToString() 
        {
			var builder = new StringBuilder();
			builder.AppendFormat("{0}: {1}{2}", Name, Expression, Important ? " !important" : "");

			return builder.ToString();
		}

        public string Name { get; set; }
        public bool Important { get; set; }
        public Expression Expression { get; set; }

	}
}