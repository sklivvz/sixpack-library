using System;

namespace SixPack.Security.Cryptography
{
	/// <summary>
	/// Defines how a token is encoded.
	/// </summary>
	[Flags]
	public enum TokenTypes
	{
		/// <summary>
		/// Indicates that a MAC should be added to the token.
		/// </summary>
		Hashed = 1,

		/// <summary>
		/// Indicates that the token is encrypted.
		/// </summary>
		Encrypted = 2,

		/// <summary>
		/// Indicates that the token is encrypted and that a MAC is added to it.
		/// </summary>
		All = Hashed | Encrypted
	}
}