using System;
using System.Collections.Generic;

namespace BoneSoft.CSS {
	/// <summary></summary>
	public class MediaTag : ISelectorContainer {
		private List<Selector> selectors = new List<Selector>();
		private Media media;

		/// <summary></summary>
		public List<Selector> Selectors {
			get { return this.selectors; }
			set { this.selectors = value; }
		}

		/// <summary></summary>
		public Media Media {
			get { return this.media; }
			set { this.media = value; }
		}
	}
}