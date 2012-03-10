using System;
using System.Drawing;

namespace BoneSoft.CSS {
	/// <summary></summary>
	public class PropertyValue {
		private ValueType type;
		private Unit unit;
		private string value;

		/// <summary></summary>
		public ValueType Type {
			get { return this.type; }
			set { this.type = value; }
		}

		/// <summary></summary>
		public Unit Unit {
			get { return this.unit; }
			set { this.unit = value; }
		}

		/// <summary></summary>
		public string Value {
			get { return this.value; }
			set { this.value = value; }
		}

		/// <summary></summary>
		/// <returns></returns>
		public override string ToString() {
			System.Text.StringBuilder txt = new System.Text.StringBuilder(value);
			if (type == ValueType.Unit) {
				txt.Append(unit.ToString().ToLower());
			}
			txt.Append(" [");
			txt.Append(type.ToString());
			txt.Append("]");
			return txt.ToString();
		}

		public bool IsColor {
			get {
				if (((type == ValueType.Hex) || (type == ValueType.String && value.StartsWith("#"))) && (value.Length == 6 || (value.Length == 7 && value.StartsWith("#")))) {
					bool hex = true;
					foreach (char c in value) {
						if (!char.IsDigit(c) && c != '#'
							&& c != 'a' && c != 'A'
							&& c != 'b' && c != 'B'
							&& c != 'c' && c != 'C'
							&& c != 'd' && c != 'D'
							&& c != 'e' && c != 'E'
							&& c != 'f' && c != 'F'
						) {
							return false;
						}
					}
					return hex;
				} else if (type == ValueType.String) {
					bool number = true;
					foreach (char c in value) {
						if (!char.IsDigit(c)) {
							number = false;
							break;
						}
					}
					if (number) { return false; }

					try {
						KnownColor kc = (KnownColor)Enum.Parse(typeof(KnownColor), value, true);
						return true;
					} catch {}
				}
				return false;
			}
		}

		public Color ToColor() {
			string hex = "000000";
			if (type == ValueType.Hex) {
				if (value.Length == 7 && value.StartsWith("#")) {
					hex = value.Substring(1);
				} else if (value.Length == 6) {
					hex = value;
				}
			} else {
				try {
					KnownColor kc = (KnownColor)Enum.Parse(typeof(KnownColor), value, true);
					Color c = Color.FromKnownColor(kc);
					return c;
				} catch {}
			}
			int r = DeHex(hex.Substring(0, 2));
			int g = DeHex(hex.Substring(2, 2));
			int b = DeHex(hex.Substring(4));
			return Color.FromArgb(r, g, b);
		}
		private int DeHex(string input) {
			int val;
			int result = 0;
			for (int i = 0; i < input.Length; i++) {
				string chunk = input.Substring(i, 1).ToUpper();
				switch (chunk) {
					case "A":
						val = 10; break;
					case "B":
						val = 11; break;
					case "C":
						val = 12; break;
					case "D":
						val = 13; break;
					case "E":
						val = 14; break;
					case "F":
						val = 15; break;
					default:
						val = int.Parse(chunk); break;
				}
				if (i == 0) {
					result += val * 16;
				} else {
					result += val;
				}
			}
			return result;
		}
	}
}