using System;
using System.Globalization;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Security.Authentication;
using System.Text.RegularExpressions;
using SixPack.Text;
using System.Web.UI;

namespace SixPack.Security.Cryptography
{
	/// <summary>
	/// Encodes and decodes tokens. The tokens are encrypted and / or authenticated.
	/// </summary>
	public class SecureTokenBuilder
	{
		private static readonly byte[] signingSalt = new byte[] { 208, 52, 51, 190, 132, 109, 152, 156, 33, 210, 228, 234, 72, 90, 71, 14, 81, 113, 202, 74 };
		private static readonly byte[] encryptionSalt = new byte[] { 84, 98, 15, 41, 67, 141, 205, 231, 117, 33, 19, 6, 157, 51, 12, 30, 158, 42, 194, 20 };

		private readonly HMAC signingAlgorithm;
		private readonly SymmetricAlgorithm encryptionAlgorithm;

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureTokenBuilder"/> class.
		/// </summary>
		/// <param name="secretKey">The secret key.</param>
		public SecureTokenBuilder(string secretKey)
			: this(secretKey, TokenTypes.All, "HMACSHA256", "TripleDES")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureTokenBuilder"/> class.
		/// </summary>
		/// <param name="secretKey">The secret key.</param>
		/// <param name="types">The type of the token.</param>
		public SecureTokenBuilder(string secretKey, TokenTypes types)
			: this(secretKey, types, "HMACSHA256", "TripleDES")
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SecureTokenBuilder"/> class.
		/// </summary>
		/// <param name="secretKey">The secret key.</param>
		/// <param name="types">The type of the token.</param>
		/// <param name="hmacAlgorithmName">Name of the hmac algorithm.</param>
		/// <param name="encryptionAlgorithmName">Name of the encryption algorithm.</param>
		public SecureTokenBuilder(string secretKey, TokenTypes types, string hmacAlgorithmName, string encryptionAlgorithmName)
		{
			if (string.IsNullOrEmpty(secretKey))
			{
				throw new ArgumentNullException("secretKey");
			}

			if ((types & TokenTypes.Hashed) != 0)
			{
				if (string.IsNullOrEmpty(hmacAlgorithmName))
				{
					throw new ArgumentNullException("hmacAlgorithmName");
				}
				signingAlgorithm = HMAC.Create(hmacAlgorithmName);

				Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(secretKey, signingSalt);
				signingAlgorithm.Key = keyGenerator.GetBytes(128);
			}

			if ((types & TokenTypes.Encrypted) != 0)
			{
				if (string.IsNullOrEmpty(encryptionAlgorithmName))
				{
					throw new ArgumentNullException("encryptionAlgorithmName");
				}
				encryptionAlgorithm = SymmetricAlgorithm.Create(encryptionAlgorithmName);
				Rfc2898DeriveBytes keyGenerator = new Rfc2898DeriveBytes(secretKey, encryptionSalt);
				encryptionAlgorithm.Key = keyGenerator.GetBytes(encryptionAlgorithm.KeySize / 8);
				encryptionAlgorithm.IV = keyGenerator.GetBytes(encryptionAlgorithm.BlockSize / 8);
			}
		}

		private static T[] Concatenate<T>(T[] first, T[] second)
		{
			T[] result = new T[first.Length + second.Length];
			Array.Copy(first, result, first.Length);
			Array.Copy(second, 0, result, first.Length, second.Length);
			return result;
		}

		/// <summary>
		/// Makes a token with the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		/// <remarks>
		/// This class supports serializing any object graph, but is optimized for those containing strings, arrays, and hash tables.
		/// It offers second order optimization for many of the .NET Framework primitive types.
		/// </remarks>
		public string EncodeObject(object data)
		{
			using (StringWriter encodedData = new StringWriter(CultureInfo.InvariantCulture))
			{
				LosFormatter serializer = new LosFormatter();
				serializer.Serialize(encodedData, data);

				byte[] serializedData = Convert.FromBase64String(encodedData.ToString());
				return EncodeBytes(serializedData);
			}
		}

		/// <summary>
		/// Decodes the token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns>Returns the data of the token.</returns>
		public object DecodeObject(string token)
		{
			int dataLength;
			byte[] bytes = DecodeBytes(token, out dataLength);

			string base64Bytes = Convert.ToBase64String(bytes, 0, dataLength);
			LosFormatter serializer = new LosFormatter();
			using (var data = new StringReader(base64Bytes))
			{
				return serializer.Deserialize(data);
			}
		}

		/// <summary>
		/// Makes a token with the specified data.
		/// </summary>
		/// <param name="data">The data.</param>
		/// <returns></returns>
		public string EncodeToken(string data)
		{
			if (string.IsNullOrEmpty(data))
			{
				throw new ArgumentNullException("data");
			}

			byte[] bytes = Encoding.UTF8.GetBytes(data);

			return EncodeBytes(bytes);
		}

		/// <summary>
		/// Decodes the token.
		/// </summary>
		/// <param name="token">The token.</param>
		/// <returns>Returns the data of the token.</returns>
		public string DecodeToken(string token)
		{
			int dataLength;
			byte[] bytes = DecodeBytes(token, out dataLength);
			return Encoding.UTF8.GetString(bytes, 0, dataLength);
		}

		private string EncodeBytes(byte[] bytes)
		{
			if (encryptionAlgorithm != null)
			{
				MemoryStream buffer = new MemoryStream();
				using (var crypt = new CryptoStream(buffer, encryptionAlgorithm.CreateEncryptor(), CryptoStreamMode.Write))
				{
					crypt.Write(BitConverter.GetBytes(bytes.Length), 0, 4);
					crypt.Write(bytes, 0, bytes.Length);
				}
				bytes = buffer.ToArray();
			}

			if (signingAlgorithm != null)
			{
				byte[] mac = signingAlgorithm.ComputeHash(bytes);
				bytes = Concatenate(bytes, mac);
			}

			string base64 = Convert.ToBase64String(bytes);
			return base64.TrimEnd('=').Replace('+', '-').Replace('/', '_');
		}

		private byte[] DecodeBytes(string token, out int dataLength)
		{
			if (string.IsNullOrEmpty(token))
			{
				throw new ArgumentNullException("token");
			}

			string base64 = token.Replace('-', '+').Replace('_', '/').PadRight(((token.Length - 1) | 0x3) + 1, '=');

			byte[] bytes = Convert.FromBase64String(base64);
			dataLength = bytes.Length;

			if (signingAlgorithm != null)
			{
				int macOffset = bytes.Length - signingAlgorithm.HashSize / 8;
				byte[] mac = signingAlgorithm.ComputeHash(bytes, 0, macOffset);

				for (int i = 0; i < mac.Length; ++i)
				{
					if (mac[i] != bytes[macOffset + i])
					{
						throw new AuthenticationException("The token has an invalid MAC.");
					}
				}

				dataLength = macOffset;
			}

			if (encryptionAlgorithm != null)
			{
				MemoryStream buffer = new MemoryStream(bytes, 0, dataLength);
				using (var crypt = new CryptoStream(buffer, encryptionAlgorithm.CreateDecryptor(), CryptoStreamMode.Read))
				{
					byte[] lengthBytes = new byte[4];
					crypt.Read(lengthBytes, 0, lengthBytes.Length);
					dataLength = BitConverter.ToInt32(lengthBytes, 0);

					byte[] data = new byte[dataLength];
					crypt.Read(data, 0, data.Length);
					bytes = data;
				}
			}
			return bytes;
		}
	}
}