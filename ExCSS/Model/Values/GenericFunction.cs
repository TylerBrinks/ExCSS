using System.Collections.Generic;
using System.Text;
using ExCSS.Model;

// ReSharper disable once CheckNamespace
namespace ExCSS
{
    public class GenericFunction : Function
    {
        internal List<Term> Arguments { get; private set; }

        public GenericFunction(string name, List<Term> arguments)
        {
            Text = name;
            Arguments = arguments;
        }

        public override string ToString()
        {
            var builder = new StringBuilder().Append(Text);
            builder.Append(Specification.ParenOpen);

            for (var i = 0; i < Arguments.Count; i++)
            {
                builder.Append(Arguments[i]);

                if (i != Arguments.Count - 1)
                {
                    builder.Append(Specification.Comma);
                }
            }

            builder.Append(Specification.ParenClose);
            return builder.ToString();
        }
    }
}