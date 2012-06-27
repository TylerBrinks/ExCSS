using System.IO;
using System.Reflection;

namespace ExCSS.Tests
{
    /// <summary>
    /// The .NET resource editor isn't usable for large files like the test CSS stylesheets.  This works just as well
    /// using the Embedded test CSS documents.
    /// </summary>
    public sealed class Stylesheets
    {

		private string _bootstrap;

		/// <summary>
		/// Gets the file contents of "Bootstrap.css"
		/// </summary>
		public string Bootstrap 
		{
			get 
			{
				return _bootstrap = (_bootstrap ?? ReadAssemblyResource("Bootstrap.css"));
			}
		}

		private string _browserhacks;

		/// <summary>
		/// Gets the file contents of "BrowserHacks.css"
		/// </summary>
		public string BrowserHacks 
		{
			get 
			{
				return _browserhacks = (_browserhacks ?? ReadAssemblyResource("BrowserHacks.css"));
			}
		}

		private string _css3;

		/// <summary>
		/// Gets the file contents of "Css3.css"
		/// </summary>
		public string Css3 
		{
			get 
			{
				return _css3 = (_css3 ?? ReadAssemblyResource("Css3.css"));
			}
		}

		private string _err;

		/// <summary>
		/// Gets the file contents of "Err.css"
		/// </summary>
		public string Err 
		{
			get 
			{
				return _err = (_err ?? ReadAssemblyResource("Err.css"));
			}
		}

		private string _quotes;

		/// <summary>
		/// Gets the file contents of "Quotes.css"
		/// </summary>
		public string Quotes 
		{
			get 
			{
				return _quotes = (_quotes ?? ReadAssemblyResource("Quotes.css"));
			}
		}

		

		private static string ReadAssemblyResource(string resourceFileName) 
		{
			string resourceName = "ExCSS.Tests.Stylesheets." + resourceFileName;

			try
            {
                TextReader reader = new StreamReader(Assembly.GetExecutingAssembly()
                    .GetManifestResourceStream(resourceName));
                return reader.ReadToEnd();
            }
            catch
            {
                return null;
            }
		}
    }
}
