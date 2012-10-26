using System;
using System.Drawing;

namespace ExCSS.Model
{
    /// <summary>
    /// Converts to and from Hue/Saturation and RGB values from 0 to 255
    /// </summary>
	internal struct HSV 
    {
		private int _hue;
		private int _sat;
		private int _val;

        /// <summary>
        /// Initializes a new instance of the <see cref="HSV"/> struct.
        /// </summary>
        /// <param name="hue">The hue.</param>
        /// <param name="saturation">The saturation.</param>
        /// <param name="value">The value.</param>
		public HSV(int hue, int saturation, int value)
        {
			_hue = hue; 
            _sat = saturation; 
            _val = value;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="HSV"/> struct.
        /// </summary>
        /// <param name="color">The color.</param>
		public HSV(Color color) 
        {
			_hue = 0;
            _sat = 0; 
            _val = 0; 

            FromRGB(color);
		}

        /// <summary>
        /// Gets or sets the hue.
        /// </summary>
        /// <value>
        /// The hue.
        /// </value>
		public int Hue
        {
			get { return _hue; }
			set { _hue = value; }
		}

        /// <summary>
        /// Gets or sets the saturation.
        /// </summary>
        /// <value>
        /// The saturation.
        /// </value>
		public int Saturation 
        {
			get { return _sat; }
			set { _sat = value; }
		}

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
		public int Value 
        {
			get { return _val; }
			set { _val = value; }
		}

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
		public Color Color
        {
			get { return ToRGB(); }
			set { FromRGB(value); }
		}

        /// <summary>
        /// Sets HSV values based on a supplied color.
        /// </summary>
        /// <param name="color">The color.</param>
		private void FromRGB(Color color)
        {
		    var red = color.R / 255D;
            var green = color.G / 255D;
            var blue = color.B / 255D;

			double hue;
            double saturation;

		    var min = Math.Min(Math.Min(red, green), blue);
            var max = Math.Max(Math.Max(red, green), blue);

			var value = max;
			var delta = max - min;

			if (max == 0 || delta == 0) 
            {
				saturation = 0;
				hue = 0;
			} 
            else {

				saturation = delta / max;

				if (red == max)
                {
					hue = (60D * ((green - blue) / delta)) % 360D;
				} 
                else if (green == max) 
                {
					hue = 60D * ((blue - red) / delta) + 120D;
				} 
                else 
                {
					hue = 60D * ((red - green) / delta) + 240D;
				}
			}

			if (hue < 0) 
            {
				hue += 360D;
			}

			Hue = (int)(hue / 360D * 255D);
			Saturation = (int)(saturation * 255D);
			Value = (int)(value * 255D);
		}

        /// <summary>
        /// Converts the instance to a color using RGB values
        /// </summary>
        /// <returns></returns>
		private Color ToRGB()
        {
		    double red = 0;
			double green = 0;
			double blue = 0;

			var hue = (Hue / 255D * 360D) % 360D;
            var saturation = Saturation / 255D;
            var value = Value / 255D;

			if (saturation == 0) 
            {
				red = value;
				green = value;
				blue = value;
			} 
            else 
            {
                var sectorPos = hue / 60D;
                var sectorNumber = (int)(Math.Floor(sectorPos));

                var fractionalSector = sectorPos - sectorNumber;

                var p = value * (1D - saturation);
                var q = value * (1D - (saturation * fractionalSector));
                var t = value * (1D - (saturation * (1D - fractionalSector)));

				switch (sectorNumber) {
					case 0: 
                        red = value; 
                        green = t; 
                        blue = p; 
                        break;

					case 1: 
                        red = q; 
                        green = value; 
                        blue = p; 
                        break;

					case 2:
                        red = p;
                        green = value; 
                        blue = t;
                        break;

					case 3: 
                        red = p;
                        green = q; 
                        blue = value; 
                        break;

					case 4: 
                        red = t; 
                        green = p; 
                        blue = value;
                        break;

					case 5: 
                        red = value;
                        green = p;
                        blue = q;
                        break;
				}
			}

			return Color.FromArgb((int)(red * 255D), (int)(green * 255D), (int)(blue * 255D));
		}

        /// <summary>
        /// Implements the operator !=.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
		public static bool operator !=(HSV left, HSV right)
        {
			return !(left == right);
		}

        /// <summary>
        /// Implements the operator ==.
        /// </summary>
        /// <param name="left">The left.</param>
        /// <param name="right">The right.</param>
        /// <returns>
        /// The result of the operator.
        /// </returns>
		public static bool operator ==(HSV left, HSV right)
        {
			return (left.Hue == right.Hue && left.Value == right.Value && left.Saturation == right.Saturation);
		}
	}
}