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

		[Row(TokenTypes.Encrypted)]
		[Row(TokenTypes.Hashed)]
		[Row(TokenTypes.Encrypted | TokenTypes.Hashed)]
		[RowTest]
		public void RoundtripObject(TokenTypes types)
		{
			var value = new MyClass
			{
				MyString = DataGenerator.RandomAsciiString(20),
				MyDate = DateTime.Now,
			};


			SecureTokenBuilder builder = new SecureTokenBuilder("p@ssw0rd", types);
			string token = builder.EncodeObject(value);
			object decoded = builder.DecodeObject(token);

			Assert.IsAssignableFrom(typeof(MyClass), decoded, "The deserialized object has the wrong type.");

			MyClass decodedObject = (MyClass)decoded;
			Assert.AreEqual(value.MyString, decodedObject.MyString, "The property MyString is incorrect.");
			Assert.AreEqual(value.MyDate, decodedObject.MyDate, "The property MyDate is incorrect.");
		}

		[Serializable]
		private class MyClass
		{
			public string MyString;
			public DateTime MyDate;
		}
	}
}