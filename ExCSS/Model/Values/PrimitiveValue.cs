using System;
using System.Globalization;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a CSS primitive value.
    /// </summary>
    sealed class PrimitiveValue : Value
    {
        #region Members

        Object data;
        UnitType unit;

        #endregion

        #region ctor

        internal PrimitiveValue(UnitType unitType, string value)
        {
            _type = RuleValueType.PrimitiveValue;
            SetStringValue(unitType, value);
        }

        internal PrimitiveValue(UnitType unitType, Single value)
        {
            _type = RuleValueType.PrimitiveValue;
            SetFloatValue(unitType, value);
        }

        internal PrimitiveValue(string unit, Single value)
        {
            _type = RuleValueType.PrimitiveValue;
            var unitType = ConvertStringToUnitType(unit);
            SetFloatValue(unitType, value);
        }

        internal PrimitiveValue(HtmlColor value)
        {
            _text = value.ToCss();
            _type = RuleValueType.PrimitiveValue;
            unit = UnitType.RGB;
            data = value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unit type of the value.
        /// </summary>
        public UnitType PrimitiveType
        {
            get { return unit; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Sets the primitive value to the given number.
        /// </summary>
        /// <param name="unitType">The unit of the number.</param>
        /// <param name="value">The value of the number.</param>
        /// <returns>The CSS primitive value instance.</returns>
        public PrimitiveValue SetFloatValue(UnitType unitType, Single value)
        {
            _text = value.ToString(CultureInfo.InvariantCulture) + ConvertUnitTypeToString(unitType);
            unit = unitType;
            data = value;
            return this;
        }

        /// <summary>
        /// Gets the primitive value's number if any.
        /// </summary>
        /// <param name="unitType">The unit of the number.</param>
        /// <returns>The value of the number if any.</returns>
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

        /// <summary>
        /// Sets the primitive value to the given string.
        /// </summary>
        /// <param name="unitType">The unit of the string.</param>
        /// <param name="value">The value of the string.</param>
        /// <returns>The CSS primitive value instance.</returns>
        public PrimitiveValue SetStringValue(UnitType unitType, string value)
        {
            switch (unitType)
            {
                case UnitType.String:
                    _text = "'" + value + "'";
                    break;
                case UnitType.Uri:
                    _text = "url('" + value + "')";
                    break;
                default:
                    _text = value;
                    break;
            }

            unit = unitType;
            data = value;
            return this;
        }

        /// <summary>
        /// Gets the primitive value's string if any.
        /// </summary>
        /// <returns>The value of the string if any.</returns>
        public string GetStringValue()
        {
            if (data is String)
            {
                var value = (String)data;
                //TODO Convert
                return value;
            }

            return null;
        }

        /// <summary>
        /// Gets the primitive value's counter if any.
        /// </summary>
        /// <returns>The value of the counter if any.</returns>
        public Counter GetCounterValue()
        {
            return data as Counter;
        }

        /// <summary>
        /// Gets the primitive value's rectangle if any.
        /// </summary>
        /// <returns>The value of the rectangle if any.</returns>
        public Rect GetRectValue()
        {
            return data as Rect;
        }

        /// <summary>
        /// Gets the primitive value's RGB color if any.
        /// </summary>
        /// <returns>The value of the RGB color if any.</returns>
        public HtmlColor? GetRGBColorValue()
        {
            if(unit == UnitType.RGB)
                return (HtmlColor)data;

            return null;
        }

        #endregion

        #region Helpers

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

        #endregion
    }
}
