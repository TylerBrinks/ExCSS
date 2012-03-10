using ExCSS.Model;

namespace ExCSS
{
    public static class Extensions
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
                case Unit.None:
                    return "";
            }

            return unit.ToString().ToLower();
        }
    }
}
