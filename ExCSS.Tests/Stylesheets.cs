using System.Dynamic;
using System.IO;
using System.Reflection;

namespace ExCSS.Tests
{
    public class Stylesheets : DynamicObject
    {
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            try
            {
                TextReader reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("ExCSS.Tests.Stylesheets." + binder.Name + ".css"));
                result = reader.ReadToEnd();

                return true;
            }
            catch
            {
                result = null;
                return false;
            }
        }
    }
}
