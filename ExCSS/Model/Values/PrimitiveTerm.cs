using System;
using System.Globalization;
using ExCSS.Model.Values;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class PrimitiveTerm : Term
    {
        private object data;
        private UnitType unit;

        public PrimitiveTerm(UnitType unitType, string value)
        {
            RuleValueType = RuleValueType.PrimitiveValue;
            SetStringValue(unitType, value);
        }

        public PrimitiveTerm(UnitType unitType, Single value)
        {
            RuleValueType = RuleValueType.PrimitiveValue;
            SetFloatValue(unitType, value);
        }

        public PrimitiveTerm(string unit, Single value)
        {
            RuleValueType = RuleValueType.PrimitiveValue;
            var unitType = ConvertStringToUnitType(unit);
            SetFloatValue(unitType, value);
        }

        public PrimitiveTerm(HtmlColor value)
        {
            Text = value.ToCss();
            RuleValueType = RuleValueType.PrimitiveValue;
            unit = UnitType.RGB;
            data = value;
        }

        public UnitType PrimitiveType
        {
            get { return unit; }
        }

        internal PrimitiveTerm SetFloatValue(UnitType unitType, Single value)
        {
            Text = value.ToString(CultureInfo.InvariantCulture) + ConvertUnitTypeToString(unitType);
            unit = unitType;
            data = value;

            return this;
        }

        internal Single? GetFloatValue(UnitType unitType)
        {
            if (data is Single)
            {
                var value = (Single)data;
                //TODO Convert
                return value;
            }

            return null;
        }

        internal PrimitiveTerm SetStringValue(UnitType unitType, string value)
        {
            switch (unitType)
            {
                case UnitType.String:
                    Text = "'" + value + "'";
                    break;

                case UnitType.Uri:
                    Text = "url('" + value + "')";
                    break;

                default:
                    Text = value;
                    break;
            }

            unit = unitType;
            data = value;

            return this;
        }

        internal string GetStringValue()
        {
            var val = data as string;

            if (val != null)
            {
                var value = val;
                //TODO Convert
                return value;
            }

            return null;
        }

        internal Counter GetCounterValue()
        {
            return data as Counter;
        }

        internal Rectangle GetRectValue()
        {
            return data as Rectangle;
        }

        internal HtmlColor? GetRGBColorValue()
        {
            if (unit == UnitType.RGB)
            {
                return (HtmlColor)data;
            }

            return null;
        }

        internal static UnitType ConvertStringToUnitType(string unit)
        {
            switch (unit)
            {
                case "%": return UnitType.Percentage;
                case "em": return UnitType.Ems;
                case "cm": return UnitType.Centimeter;
                case "deg": return UnitType.Degree;
                case "grad": return UnitType.Grad;
                case "rad": return UnitType.Radian;
                case "turn": return UnitType.Turn;
                case "ex": return UnitType.Exs;
                case "hz": return UnitType.Hertz;
                case "in": return UnitType.Inch;
                case "khz": return UnitType.KiloHertz;
                case "mm": return UnitType.Millimeter;
                case "ms": return UnitType.Millisecond;
                case "s": return UnitType.Second;
                case "pc": return UnitType.Percent;
                case "pt": return UnitType.Point;
                case "px": return UnitType.Pixel;
                case "vw": return UnitType.ViewportWidth;
                case "vh": return UnitType.ViewportHeight;
                case "vmin": return UnitType.ViewportMin;
                case "vmax": return UnitType.ViewportMax;
            }

            return UnitType.Unknown;
        }

        internal static string ConvertUnitTypeToString(UnitType unit)
        {
            switch (unit)
            {
                case UnitType.Percentage: return "%";
                case UnitType.Ems: return "em";
                case UnitType.Centimeter: return "cm";
                case UnitType.Degree: return "deg";
                case UnitType.Grad: return "grad";
                case UnitType.Radian: return "rad";
                case UnitType.Turn: return "turn";
                case UnitType.Exs: return "ex";
                case UnitType.Hertz: return "hz";
                case UnitType.Inch: return "in";
                case UnitType.KiloHertz: return "khz";
                case UnitType.Millimeter: return "mm";
                case UnitType.Millisecond: return "ms";
                case UnitType.Second: return "s";
                case UnitType.Percent: return "pc";
                case UnitType.Point: return "pt";
                case UnitType.Pixel: return "px";
                case UnitType.ViewportWidth: return "vw";
                case UnitType.ViewportHeight: return "vh";
                case UnitType.ViewportMin: return "vmin";
                case UnitType.ViewportMax: return "vmax";
            }

            return string.Empty;
        }
    }
}
