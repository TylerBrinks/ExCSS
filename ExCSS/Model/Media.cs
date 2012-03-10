using System;
using System.Collections.Generic;
using System.Text;

namespace BoneSoft.CSS {
	/// <summary></summary>
	[Flags]
	public enum Media {
		/// <summary></summary>
		None = 0,
		/// <summary></summary>
		all = 1,
		/// <summary></summary>
		aural = 2,
		/// <summary></summary>
		braille = 4,
		/// <summary></summary>
		embossed = 8,
		/// <summary></summary>
		handheld = 16,
		/// <summary></summary>
		print = 32,
		/// <summary></summary>
		projection = 64,
		/// <summary></summary>
		screen = 128,
		/// <summary></summary>
		tty = 256,
		/// <summary></summary>
		tv = 512
	}
}