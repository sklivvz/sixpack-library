using System;

namespace SixPack.Text
{
	/// <summary>
	/// Specifies how to search text
	/// </summary>
	[Flags]
	public enum TextSearchOptions
	{
		/// <summary>
		/// Case sensitive exact match
		/// </summary>
		None = 0,
		/// <summary>
		/// Partial matching
		/// </summary>
		Partial = 1,
		/// <summary>
		/// Case insensitive matching
		/// </summary>
		CaseInsensitive = 2,
		/// <summary>
		/// Use the text as a regular expression
		/// </summary>
		Regex = 4
	}
}