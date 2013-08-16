using System.IO;
using ExCSS.Model;
using NUnit.Framework;

namespace ExCSS.Tests
{
    [TestFixture]
    public class ImportRuleFixture
    {
        [Test]
        public void Parser_Reads_Imports()
        {
            var parser = new Parser(File.OpenRead(@"C:\Everything\Source\git\ExCSS\ExCSS.Tests\Stylesheets\test.css"));
            
            parser.Parse();
        }
    }
}
