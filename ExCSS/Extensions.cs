using ExCSS.Model;

namespace ExCSS
{
    internal static class Extensions
    {
        public static string ToUnitString(this Unit unit)
        {
            switch (unit)
            {
                case Unit.Percent:
                    return "%";

                case Unit.Khz:
                case Unit.Hz:

                    return unit.ToString();

                case Unit.None: // Account for empty units.  i.e. border: 10px 0 0 5px;
                    return " ";
            }

            return unit.ToString().ToLower();
        }
    }
}
