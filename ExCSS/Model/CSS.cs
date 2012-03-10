using System;
using System.Collections.Generic;

namespace BoneSoft.CSS {
	/// <summary></summary>
	public class CSS : ISelectorContainer {
		private string filename;
		private List<Selector> selectors = new List<Selector>();
		private List<Import> imports = new List<Import>();
		private List<MediaTag> medias = new List<MediaTag>();

		/// <summary></summary>
		public string FileName {
			get { return filename; }
			set { filename = value; }
		}

		/// <summary></summary>
		public List<Selector> Selectors {
			get { return selectors; }
			set { selectors = value; }
		}

		/// <summary></summary>
		public List<Import> Imports {
			get { return imports; }
			set { imports = value; }
		}

		/// <summary></summary>
		public List<MediaTag> MediaTags {
			get { return medias; }
			set { medias = value; }
		}
	}
}