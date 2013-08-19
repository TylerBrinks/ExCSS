using System;
using System.Globalization;

namespace ExCSS.Model
{
    internal sealed class PrimitiveTerm : Term
    {
        private Object data;
        private UnitType unit;

        internal PrimitiveTerm(UnitType unitType, string value)
        {
            RuleValueType = RuleValueType.PrimitiveValue;
            SetStringValue(unitType, value);
        }

        internal PrimitiveTerm(UnitType unitType, Single value)
        {
            RuleValueType = RuleValueType.PrimitiveValue;
            SetFloatValue(unitType, value);
        }

        internal PrimitiveTerm(string unit, Single value)
        {
            RuleValueType = RuleValueType.PrimitiveValue;
            var unitType = ConvertStringToUnitType(unit);
            SetFloatValue(unitType, value);
        }

        internal PrimitiveTerm(HtmlColor value)
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

        public PrimitiveTerm SetFloatValue(UnitType unitType, Single value)
        {
            Text = value.ToString(CultureInfo.InvariantCulture) + ConvertUnitTypeToString(unitType);
            unit = unitType;
            data = value;

            return this;
        }

        public Single? GetFloatValue(UnitType unitType)
        {
            if (data is Single)
            {
                var value = (Single)data;
                //TODO Convert
                return value;
            }

            return null;
        }

        public PrimitiveTerm SetStringValue(UnitType unitType, string value)
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

        public string GetStringValue()
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

        public Counter GetCounterValue()
        {
            return data as Counter;
        }

        public Rectangle GetRectValue()
        {
            return data as Rectangle;
        }

        public HtmlColor? GetRGBColorValue()
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
