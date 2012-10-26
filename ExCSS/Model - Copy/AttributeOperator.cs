namespace ExCSS.Model
{
	public enum AttributeOperator
    {
        None,
		Equals,     // =
		InList,     // ~=
		Hyphenated, // |=
		EndsWith,   // $=
		BeginsWith, // ^=
		Contains    // *=
	}
}