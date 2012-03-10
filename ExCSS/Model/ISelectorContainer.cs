using System;
using System.Collections.Generic;

namespace BoneSoft.CSS {
	/// <summary></summary>
	public interface ISelectorContainer {
		/// <summary></summary>
		List<Selector> Selectors { get; set; }
	}
}