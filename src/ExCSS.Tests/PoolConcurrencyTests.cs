using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ExCSS.Tests;

public class PoolConcurrencyTests
{
    private static string ReadBootstrap()
    {
        var name = $"{typeof(PoolConcurrencyTests).Namespace}.bootstrap.css";
        using var stream = typeof(PoolConcurrencyTests).Assembly.GetManifestResourceStream(name);
        using var reader = new StreamReader(stream!);
        return reader.ReadToEnd();
    }

    [Fact]
    public async Task Parsing_Bootstrap_In_Parallel_Gives_Identical_Results()
    {
        var css = ReadBootstrap();
        var expected = new StylesheetParser().Parse(css).ToCss();

        var tasks = Enumerable.Range(0, 32).Select(_ => Task.Run(async () =>
        {
            var results = new string[4];
            for (var n = 0; n < results.Length; n++)
            {
                // Mix sync and async (which may resume on another thread) parsing.
                var sheet = n % 2 == 0
                    ? new StylesheetParser().Parse(css)
                    : await new StylesheetParser().ParseAsync(new MemoryStream(Encoding.UTF8.GetBytes(css)));
                results[n] = sheet.ToCss();
            }
            return results;
        })).ToArray();

        foreach (var result in await Task.WhenAll(tasks))
        foreach (var actual in result)
            Assert.Equal(expected, actual);
    }
}
