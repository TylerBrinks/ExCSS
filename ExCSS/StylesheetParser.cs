using System.Collections.Generic;
using System.Text;
using System.IO;
using ExCSS.Model;

namespace ExCSS
{
    /// <summary>
    /// Stylesheet Parser reads CSS rules compatible up to CSS v3.
    /// </summary>
    public class ExCSS
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExCSS"/> class.
        /// </summary>
        public ExCSS()
        {
            Errors = new List<string>();
        }

        /// <summary>
        /// Parses the specified content.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public Stylesheet Parse(string content)
        {
            var mem = new MemoryStream();
            var bytes = Encoding.ASCII.GetBytes(content);

            mem.Write(bytes, 0, bytes.Length);

            return Parse(mem);
        }

        /// <summary>
        /// Parses the specified stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public Stylesheet Parse(Stream stream)
        {
            Errors.Clear();

            var builder = new StringBuilder();
            var errorStream = new StringWriter(builder);
            var scanner = new Scanner(stream);
            var parser = new Parser(scanner)
            {
                errors =
                {
                    errorStream = errorStream
                }
            };

            parser.Parse();
            Stylesheet = parser.Stylesheet;
            SpitErrors(builder);
            return Stylesheet;
        }

        //public List<Token> GetTokens(string file)
        //{
        //    var scanner = new Scanner(file);

        //    var ts = new List<Token>();
        //    var t = scanner.Scan();

        //    if (t.val != "\0")
        //    {
        //        ts.Add(t);
        //    }

        //    while (t.val != "\0")
        //    {
        //        t = scanner.Scan();
        //        ts.Add(t);
        //    }

        //    return ts;
        //}

        /// <summary>
        /// Creates a list of errors found while parsing the stylesheet.
        /// </summary>
        /// <param name="builder">The string builder.</param>
        private void SpitErrors(StringBuilder builder)
        {
            var text = builder.ToString().Replace("\r", "");
            if (text.Length == 0)
            {
                return;
            }

            var lines = text.Split('\n');

            foreach (var line in lines)
            {
                Errors.Add(line);
            }
        }

        /// <summary>
        /// Gets the stylesheet.
        /// </summary>
        public Stylesheet Stylesheet { get; private set; }

        /// <summary>
        /// Gets the parser errors.
        /// </summary>
        public List<string> Errors { get; private set; }
    }
}