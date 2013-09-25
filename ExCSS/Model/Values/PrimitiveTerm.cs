using System;
using System.Globalization;
using ExCSS.Model.Values;

// ReSharper disable CheckNamespace
namespace ExCSS
// ReSharper restore CheckNamespace
{
    public class PrimitiveTerm : Term
    {
        private object _data;
        private UnitType _unit;

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
            Text = value.ToHtml();// value.ToCss();
            RuleValueType = RuleValueType.PrimitiveValue;
            _unit = UnitType.RGB;
            _data = value;
        }

        public UnitType PrimitiveType
        {
            get { return _unit; }
        }

        internal PrimitiveTerm SetFloatValue(UnitType unitType, Single value)
        {
            Text = value.ToString(CultureInfo.InvariantCulture) + ConvertUnitTypeToString(unitType);
            _unit = unitType;
            _data = value;

            return this;
        }

        internal Single? GetFloatValue(UnitType unitType)
        {
            if (_data is Single)
            {
                var value = (Single)_data;
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
                    Text = "url(" + value + ")";
                    break;

                default:
                    Text = value;
                    break;
            }

            _unit = unitType;
            _data = value;

            return this;
        }

        internal string GetStringValue()
        {
            var val = _data as string;

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
            return _data as Counter;
        }

        internal Rectangle GetRectValue()
        {
            return _data as Rectangle;
        }

        internal HtmlColor? GetRGBColorValue()
        {
            if (_unit == UnitType.RGB)
            {
                return (HtmlColor)_data;
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
