using System;
using System.Collections.Generic;

namespace BoneSoft.CSS {
	/// <summary></summary>
	public class Import {
		private PropertyValue url;
		private Media media = Media.None;

		/// <summary></summary>
		public PropertyValue Url {
			get { return this.url; }
			set { this.url = value; }
		}

		/// <summary></summary>
		public Media Media {
			get { return this.media; }
			set { this.media = value; }
		}

		/// <summary></summary>
		/// <returns></returns>
		public override string ToString() {
			return string.Format("@import url('{0}'){1};", url.Value, media != Media.None ? " " + media.ToString() : "");
		}
	}
}