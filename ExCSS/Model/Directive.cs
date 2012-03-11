using System.Collections.Generic;
using System.Text;

namespace ExCSS.Model
{
    /// <summary>
    /// Defines a CSS directive hierarchy
    /// </summary>
    public class Directive : IDeclarationContainer, IRuleSetContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Directive"/> class.
        /// </summary>
        public Directive()
        {
            Mediums = new List<Medium>();
            Directives = new List<Directive>();
            RuleSets = new List<RuleSet>();
            Declarations = new List<Declaration>();
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
            switch (Type)
            {
                case DirectiveType.Charset:
                    builder.Append(ToCharSetString());
                    return;
                    //return ToCharSetString();

                case DirectiveType.Page:
                    builder.Append(ToPageString());
                    return;
                    //return ToPageString();

                case DirectiveType.Media:
                    builder.Append(ToMediaString());
                    return;
                    //return ToMediaString();

                case DirectiveType.Import:
                    builder.Append(ToImportString());
                    return;
                    //return ToImportString();

                case DirectiveType.FontFace:
                    builder.Append(ToFontFaceString());
                    return;
                    //return ToFontFaceString();
            }

            builder.AppendFormat("{0} ", Name);

            if (Expression != null)
            {
                builder.AppendFormat("{0} ", Expression);
            }

            var first = true;
            foreach (var m in Mediums)
            {
                if (first)
                {
                    first = false;
                    builder.Append(" ");
                }
                else
                {
                    builder.Append(", ");
                }

                builder.Append(m.ToString());
            }

            var hasBlock = (Declarations.Count > 0 || Declarations.Count > 0 || RuleSets.Count > 0);

            if (!hasBlock)
            {
                builder.Append(";");
                return;// builder.ToString();
            }

            builder.Append(" {\r\n");

            foreach (var dir in Directives)
            {
                builder.AppendFormat("{0}\r\n", dir.ToCharSetString());
            }

            foreach (var rules in RuleSets)
            {
                builder.AppendFormat("{0}\r\n", rules);
            }

            first = true;
            foreach (var dec in Declarations)
            {
                if (first) { first = false; } else { builder.Append(";"); }
                builder.Append("\r\n\t");
                builder.Append(dec.ToString());
            }

            builder.Append("\r\n}");
            // return builder.ToString();
        }

        /// <summary>
        /// Converts a string to a CSS font face representation.
        /// </summary>
        /// <returns>Font Face CSS</returns>
        private string ToFontFaceString()
        {
            var builder = new System.Text.StringBuilder();
            builder.Append("@font-face {");

            var first = true;

            foreach (var dec in Declarations)
            {
                if (first) { first = false; } else { builder.Append(";"); }
                builder.Append("\r\n\t");
                builder.Append(dec.ToString());
            }

            builder.Append("\r\n}");
            return builder.ToString();
        }
        /// <summary>
        /// TConverts a string to a CSS import representation.
        /// </summary>
        /// <returns>Import CSS</returns>
        private string ToImportString()
        {
            var builder = new System.Text.StringBuilder();
            builder.Append("@import ");
            if (Expression != null)
            {
                builder.AppendFormat("{0} ", Expression);
            }

            var first = true;

            foreach (var m in Mediums)
            {
                if (first)
                {
                    first = false;
                    builder.Append(" ");
                }
                else
                {
                    builder.Append(", ");
                }
                builder.Append(m.ToString());
            }

            builder.Append(";");
            return builder.ToString();
        }
        /// <summary>
        /// Converts a string to a CSS media representation.
        /// </summary>
        /// <returns>Media CSS</returns>
        private string ToMediaString()
        {
            var builder = new System.Text.StringBuilder();
            builder.Append("@media");

            var first = true;
            foreach (var m in Mediums)
            {
                if (first)
                {
                    first = false;
                    builder.Append(" ");
                }
                else
                {
                    builder.Append(", ");
                }
                builder.Append(m.ToString());
            }
            builder.Append(" {\r\n");

            foreach (var rules in RuleSets)
            {
                builder.AppendFormat("{0}\r\n", rules);
            }

            builder.Append("}");
            return builder.ToString();
        }
        /// <summary>
        /// Converts a string to a CSS Page representation.
        /// </summary>
        /// <returns>Page CSS</returns>
        private string ToPageString()
        {
            var txt = new System.Text.StringBuilder();
            txt.Append("@page ");
            if (Expression != null) { txt.AppendFormat("{0} ", Expression); }
            txt.Append("{\r\n");

            var first = true;
            foreach (var dec in Declarations)
            {

                if (first) { first = false; } else { txt.Append(";"); }
                txt.Append("\r\n\t");
                txt.Append(dec.ToString());
            }

            txt.Append("}");
            return txt.ToString();
        }
        /// <summary>
        /// Converts a string to a CSS character set expression representation.
        /// </summary>
        /// <returns>Character Set CSS</returns>
        private string ToCharSetString()
        {
            return string.Format("{0} {1}", Name, Expression);
        }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public DirectiveType Type { get; set; }
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
        /// <summary>
        /// Gets or sets the mediums.
        /// </summary>
        /// <value>
        /// The mediums.
        /// </value>
        public List<Medium> Mediums { get; set; }
        /// <summary>
        /// Gets or sets the directives.
        /// </summary>
        /// <value>
        /// The directives.
        /// </value>
        public List<Directive> Directives { get; set; }
        /// <summary>
        /// Gets or sets the rule sets.
        /// </summary>
        /// <value>
        /// The rule sets.
        /// </value>
        public List<RuleSet> RuleSets { get; set; }
        /// <summary>
        /// Gets or sets the declarations.
        /// </summary>
        /// <value>
        /// The declarations.
        /// </value>
        public List<Declaration> Declarations { get; set; }
    }
}