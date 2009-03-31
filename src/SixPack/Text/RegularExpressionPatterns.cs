using System;
using System.Web;

namespace SixPack.Text
{
	/// <summary>
	/// Contains some general-purpose patterns that are usefull for validating input data.
	/// </summary>
	public static class RegularExpressionPatterns
	{
		/// <summary>
		/// Matches an hexadecimal string, composed of pairs of hexadecimal digits.
		/// </summary>
		public static readonly string HexString = @"^([0-9a-fA-F]{2})*$";

		/// <summary>
		/// Matches a base-64 string.
		/// </summary>
		public static readonly string Base64String = @"^([A-Za-z0-9+/]{4})*(([A-Za-z0-9+/]{3}=)|([A-Za-z0-9+/]{2}==)|([A-Za-z0-9+/]{1}===))?$";

		/// <summary>
		/// Matches a base-64 string, as encoded by <see cref="HttpServerUtility.UrlTokenEncode"/>.
		/// </summary>
		public static readonly string UrlTokenString = @"^[A-Za-z0-9-_]*$";
	}
}