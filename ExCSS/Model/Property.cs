using System;
using System.Collections.Generic;
using System.Drawing;

namespace BoneSoft.CSS {
	/// <summary></summary>
	public class Property {
		private string attribute;
		private bool isimportant = false;
		private List<PropertyValue> values = new List<PropertyValue>();

		/// <summary></summary>
		public string Attribute {
			get { return this.attribute; }
			set { this.attribute = value; }
		}

		/// <summary></summary>
		public bool Important {
			get { return isimportant; }
			set { isimportant = value; }
		}

		/// <summary></summary>
		public List<PropertyValue> Values {
			get { return this.values; }
			set { this.values = value; }
		}
	}
}