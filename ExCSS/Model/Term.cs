using System;
using System.Drawing;
using System.Globalization;
using System.Linq;

namespace ExCSS.Model
{
    /// <summary>
    /// Represents a stylesheet term.
    /// </summary>
    public class Term
    {
        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            var builder = new System.Text.StringBuilder();
           
            switch (Type)
            {
                case TermType.Function:
                    builder.Append(Function.ToString());
                    break;

                case TermType.Url:
                    builder.AppendFormat("url('{0}')", Value);
                    break;

                case TermType.Unicode:
                    builder.AppendFormat("U\\{0}", Value.ToUpper());
                    break;

                case TermType.Hex:
                    builder.Append(Value.ToUpper());
                    break;

                default:
                    if (Sign.HasValue)
                    {
                        builder.Append(Sign.Value);
                    }

                    builder.Append(Value);

                    if (Unit.HasValue)
                    {
                        builder.Append(Unit.Value == Model.Unit.Percent ? "%" : Unit.Value.ToUnitString());
                    }
                    break;
            }

            return builder.ToString();
        }
        /// <summary>
        /// Gets the RGB value.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        private static int GetRGBValue(Term term)
        {
            try
            {
                if (term.Unit.HasValue && term.Unit.Value == Model.Unit.Percent)
                {
                    return (int)(255f * float.Parse(term.Value) / 100f);
                }

                return int.Parse(term.Value);
            }
            catch
            {
                ;
            }

            return 0;
        }
        /// <summary>
        /// Gets the hue.
        /// </summary>
        /// <param name="term">The term.</param>
        /// <returns></returns>
        private static int GetHueValue(Term term)
        {
            // 0 - 360
            try
            {
                return (int)(float.Parse(term.Value) * 255f / 360f);
            }
            catch
            {
                ;
            }

            return 0;
        }

        /// <summary>
        /// Converts HEX, functions, or known colors to a Color object
        /// </summary>
        /// <returns>The Color</returns>
        public Color ToColor()
        {
            var hex = "000000";
            if (Type == TermType.Hex)
            {
                if ((Value.Length == 7 || Value.Length == 4) && Value.StartsWith("#"))
                {
                    hex = Value.Substring(1);
                }
                else if (Value.Length == 6 || Value.Length == 3)
                {
                    hex = Value;
                }
            }
            else if (Type == TermType.Function)
            {
                if ((Function.Name.ToLower().Equals("rgb") && Function.Expression.Terms.Count == 3)
                    || (Function.Name.ToLower().Equals("rgba") && Function.Expression.Terms.Count == 4))
                {
                    int fr = 0, fg = 0, fb = 0;
                    for (var i = 0; i < Function.Expression.Terms.Count; i++)
                    {
                        if (Function.Expression.Terms[i].Type != TermType.Number)
                        {
                            return Color.Black;
                        }

                        switch (i)
                        {
                            case 0: 
                                fr = GetRGBValue(Function.Expression.Terms[i]); 
                                break;

                            case 1: 
                                fg = GetRGBValue(Function.Expression.Terms[i]);
                                break;

                            case 2: 
                                fb = GetRGBValue(Function.Expression.Terms[i]); 
                                break;
                        }
                    }
                    return Color.FromArgb(fr, fg, fb);
                }
                if ((Function.Name.ToLower().Equals("hsl") && Function.Expression.Terms.Count == 3)
                  || (Function.Name.Equals("hsla") && Function.Expression.Terms.Count == 4))
                {
                    int h = 0, s = 0, v = 0;
                    for (var i = 0; i < Function.Expression.Terms.Count; i++)
                    {
                        if (Function.Expression.Terms[i].Type != TermType.Number)
                        {
                            return Color.Black;
                        }

                        switch (i)
                        {
                            case 0: h = GetHueValue(Function.Expression.Terms[i]); break;
                            case 1: s = GetRGBValue(Function.Expression.Terms[i]); break;
                            case 2: v = GetRGBValue(Function.Expression.Terms[i]); break;
                        }
                    }

                    var hsv = new HSV(h, s, v);
                    return hsv.Color;
                }
            }
            else
            {
                try
                {
                    var color = (KnownColor)Enum.Parse(typeof(KnownColor), Value, true);
                    return Color.FromKnownColor(color);
                }
                catch
                {
                    ;
                }
            }

            if (hex.Length == 3)
            {
                hex = hex.Aggregate("", (current, c) =>
                    current + (c.ToString(CultureInfo.InvariantCulture) + c.ToString(CultureInfo.InvariantCulture)));
            }

            var r = DeHex(hex.Substring(0, 2));
            var g = DeHex(hex.Substring(2, 2));
            var b = DeHex(hex.Substring(4));

            return Color.FromArgb(r, g, b);
        }
        /// <summary>
        /// Converts HEX values to their integer counterparts.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>Int from a HEX string</returns>
        private static int DeHex(string input)
        {
            var result = 0;
            for (var i = 0; i < input.Length; i++)
            {
                var chunk = input.Substring(i, 1).ToUpper();
                int val;
                switch (chunk)
                {
                    case "A":
                        val = 10; 
                        break;

                    case "B":
                        val = 11; 
                        break;

                    case "C":
                        val = 12; 
                        break;

                    case "D":
                        val = 13; 
                        break;

                    case "E":
                        val = 14; 
                        break;

                    case "F":
                        val = 15; 
                        break;

                    default:
                        val = int.Parse(chunk); 
                        break;
                }

                if (i == 0)
                {
                    result += val * 16;
                }
                else
                {
                    result += val;
                }
            }

            return result;
        }

        /// <summary>
        /// Gets or sets the seperator.
        /// </summary>
        /// <value>
        /// The seperator.
        /// </value>
        public char? Seperator { get; set; }
        /// <summary>
        /// Gets or sets the sign.
        /// </summary>
        /// <value>
        /// The sign.
        /// </value>
        public char? Sign { get; set; }
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public TermType Type { get; set; }
        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }
        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public Unit? Unit { get; set; }
        /// <summary>
        /// Gets or sets the function.
        /// </summary>
        /// <value>
        /// The function.
        /// </value>
        public Function Function { get; set; }

        /// <summary>
        /// Gets or sets the seperator char.
        /// </summary>
        /// <value>
        /// The seperator char.
        /// </value>
        public string SeperatorChar
        {
            get { return Seperator.HasValue ? Seperator.Value.ToString(CultureInfo.InvariantCulture) : null; }
            set { Seperator = !string.IsNullOrEmpty(value) ? value[0] : '\0'; }
        }

        /// <summary>
        /// Gets or sets the sign char.
        /// </summary>
        /// <value>
        /// The sign char.
        /// </value>
        public string SignChar
        {
            get { return Sign.HasValue ? Sign.Value.ToString(CultureInfo.InvariantCulture) : null; }
            set { Sign = !string.IsNullOrEmpty(value) ? value[0] : '\0'; }
        }

        /// <summary>
        /// Gets or sets the unit string.
        /// </summary>
        /// <value>
        /// The unit string.
        /// </value>
        public string UnitString
        {
            get
            {
                return Unit.HasValue ? Unit.ToString() : null;
            }
            set { Unit = (Unit)Enum.Parse(typeof(Unit), value); }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is color.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is color; otherwise, <c>false</c>.
        /// </value>
        public bool IsColor
        {
            get
            {
                if (((Type == TermType.Hex) || (Type == TermType.String && Value.StartsWith("#")))
                    && (Value.Length == 6 || Value.Length == 3 || ((Value.Length == 7 || Value.Length == 4)
                    && Value.StartsWith("#"))))
                {
                    return Value.All(c => char.IsDigit(c) ||
                        c == '#' || c == 'a' || c == 'A' ||
                        c == 'b' || c == 'B' || c == 'c' || 
                        c == 'C' || c == 'd' || c == 'D' || 
                        c == 'e' || c == 'E' || c == 'f' || 
                        c == 'F');
                }

                switch (Type)
                {
                    case TermType.String:
                        {
                            var number = Value.All(char.IsDigit);
                            if (number) { return false; }

                            try
                            {
                                var kc = (KnownColor)Enum.Parse(typeof(KnownColor), Value, true);
                                return true;
                            }
                            catch
                            {
                                ;
                            }
                        }
                        break;

                    case TermType.Function:
                        if ((Function.Name.ToLower().Equals("rgb") && Function.Expression.Terms.Count == 3)
                            || (Function.Name.ToLower().Equals("rgba") && Function.Expression.Terms.Count == 4))
                        {
                            return Function.Expression.Terms.All(t => t.Type == TermType.Number);
                        }
                        if ((Function.Name.ToLower().Equals("hsl") && Function.Expression.Terms.Count == 3)
                            || (Function.Name.ToLower().Equals("hsla") && Function.Expression.Terms.Count == 4))
                        {
                            return Function.Expression.Terms.All(t => t.Type == TermType.Number);
                        }
                        break;
                }

                return false;
            }
        }
    }
}