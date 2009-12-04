using System;
using System.Collections.Generic;
using SixPack;
using SixPack.Security.Cryptography;
using SixPack.Collections.Generic;
using SixPack.Text;

namespace SixPack.Samples
{
	public class Program
	{
		public static void RunBigInteger()
		{
BigInteger value = new BigInteger("1234DEADBEEF", 16);

const string charSet = "ABCDEFGHIJKLMNOP";

string valueInLetters = value.ToString(charSet);
Console.WriteLine("Value converted to base 16 using letters:\n{0}\n", valueInLetters);

BigInteger valueFromLetters = new BigInteger(valueInLetters, charSet);
Console.WriteLine("Value parsed from base 16 using letters:\n{0}\n", valueFromLetters.ToString(16));
		}

		public static void RunComparer() {
List<int> items = new List<int> { 1, 7, 4, 5, 2, 9, 8 };

items.Sort(GenericComparer<int>.Natural);

foreach(int item in items) Console.Write(item);
Console.WriteLine();
// Output is 1245789

items.Sort(GenericComparer<int>.Reverse);

foreach (int item in items) Console.Write(item);
Console.WriteLine();
// Output is 9875421
		}

		public static void Main()
		{
			RunToken();
		}

		private static void RunToken()
		{
SecureTokenBuilder tokenBuilder = new SecureTokenBuilder("some random secret string");

int userId = 12345;
DateTime expirationDate = DateTime.Now.AddDays(1);

string encodedToken = tokenBuilder.EncodeArray(userId, expirationDate);
Console.WriteLine(encodedToken);
// Output is riUNBja3yrFdL8Lpxb63H_gC-NM2PdvRIwk7FKI53Z382hh1DDcfLQJl6O67PQhGQ2z4GGd0zVA

object[] decodedToken = tokenBuilder.DecodeArray(encodedToken);
Console.WriteLine("UserId: {0}, ExpirationDate: {1}", decodedToken[0], decodedToken[1]);
// Output is UserId: 12345, ExpirationDate: 26-11-2009 13:00:20
		}

		private static void RunHash()
		{
string userName = "John Smith";
string documentNumnber = "0001234";

string cacheFileName = TextUtilities.Hash(userName, documentNumnber);
Console.WriteLine(cacheFileName);
// Output is cdK0B5P7yM0x2u8Fdou77QhB8Zk
		}
	}
}
