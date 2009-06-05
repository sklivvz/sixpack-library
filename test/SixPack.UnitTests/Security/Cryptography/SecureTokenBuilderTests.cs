using System;
using MbUnit.Framework;
using SixPack.Security.Cryptography;
using System.Security.Authentication;

namespace SixPack.UnitTests.Security.Cryptography
{
	[TestFixture]
	public class SecureTokenBuilderTests
	{
		[Row("Hello world!", TokenTypes.Encrypted)]
		[Row("Hello world!", TokenTypes.Hashed)]
		[Row("Hello world!", TokenTypes.Encrypted | TokenTypes.Hashed)]
		[RowTest]
		public void Roundtrip(string text, TokenTypes types)
		{
			SecureTokenBuilder builder = new SecureTokenBuilder("p@ssw0rd", types);
			string token = builder.EncodeToken(text);
			string decoded = builder.DecodeToken(token);

			Assert.AreEqual(text, decoded);
		}

		[Test]
		[ExpectedException(typeof(AuthenticationException))]
		public void TamperWithAuthenticator()
		{
			SecureTokenBuilder builder = new SecureTokenBuilder("p@ssw0rd", TokenTypes.Hashed);
			string token = builder.EncodeToken("Hello world");

			token = token.Substring(0, 4) + token.Substring(4, 8) + token.Substring(8);
			
			builder.DecodeToken(token);
		}
	}
}