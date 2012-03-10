using System;
using System.Collections.Generic;

namespace BoneSoft.CSS {
	/// <summary></summary>
	public class Tag {
		private TagType tagtype;
		private string name;
		private string cls;
		private string pseudo;
		private string id;
		private char parentrel = '\0';
		private Tag subtag;
		private List<string> attribs = new List<string>();

		/// <summary></summary>
		public TagType TagType {
			get { return tagtype; }
			set { tagtype = value; }
		}

		/// <summary></summary>
		public bool IsIDSelector {
			//get { return (int)(this.tagtype & TagType.IDed) > 0; }
			get { return id != null; }
		}

		/// <summary></summary>
		public bool HasName {
			get { return name != null; }
		}

		/// <summary></summary>
		public bool HasClass {
			get { return cls != null; }
		}

		/// <summary></summary>
		public bool HasPseudoClass {
			get { return pseudo != null; }
		}

		/// <summary></summary>
		public string Name {
			get { return name; }
			set { name = value; }
		}

		/// <summary></summary>
		public string Class {
			get { return cls; }
			set { cls = value; }
		}

		/// <summary></summary>
		public string Pseudo {
			get { return pseudo; }
			set { pseudo = value; }
		}

		/// <summary></summary>
		public string Id {
			get { return id; }
			set { id = value; }
		}

		/// <summary></summary>
		public char ParentRelationship {
			get { return parentrel; }
			set { parentrel = value; }
		}

		/// <summary></summary>
		public Tag SubTag {
			get { return subtag; }
			set { subtag = value; }
		}

		/// <summary></summary>
		public List<string> Attributes {
			get { return attribs; }
			set { attribs = value; }
		}

		/// <summary></summary>
		/// <returns></returns>
		public override string ToString() {
			System.Text.StringBuilder txt = new System.Text.StringBuilder(ToShortString());

			if (subtag != null) {
				txt.Append(" ");
				txt.Append(subtag.ToString());
			}
			return txt.ToString();
		}

		/// <summary></summary>
		/// <returns></returns>
		public string ToShortString() {
			System.Text.StringBuilder txt = new System.Text.StringBuilder();
			if (parentrel != '\0') {
				txt.AppendFormat("{0} ", parentrel.ToString());
			}
			if (HasName) {
				txt.Append(name);
			}
			foreach (string atr in attribs) {
				txt.AppendFormat("[{0}]", atr);
			}
			if (HasClass) {
				txt.Append(".");
				txt.Append(cls);
			}
			if (IsIDSelector) {
				txt.Append("#");
				txt.Append(id);
			}
			if (HasPseudoClass) {
				txt.Append(":");
				txt.Append(pseudo);
			}
			return txt.ToString();
		}
	}
}