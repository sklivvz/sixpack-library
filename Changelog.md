# Changes since version 1.2 #

  * Updated to .NET 3.5
  * Added SixPack.Reflection project. This project adds utilities for reflection and manipulation of expression trees.
  * Added SixPack.Data.Entity. This project adds helper methods that extend the Entity Framework.
  * Removed the obsolete HashSet type.
  * Added extension methods that implement BinarySearch on lists.
  * Added the CompositeKey type that can be used to represent a composite key in a collection.
  * Added the SixPack.Collections.Generic.Extensions namespace that provides helper methods for common needs when manipulating collections. These methods include:
    * IDictionary{K, ICollection{T}}.Add(), which simplifies implementing a multidictionary with a dictionary of collections.
    * IEnumerable{T}.ToTree(), which builds a tree from a sequence.
    * IEnumerable{T}.Index(), which adds properties to the items of a sequence, to simplify the treatment of special cases (first, last, alternating item) when rendering the sequence.
  * Added the TypeConverter class that can perform type conversions using all the standard mechanisms in the .NET framework, including:
    * TypeConverterAttribute;
    * conversion operators;
    * Convert.ChangeType;
    * conversion from and to nullable types;
    * special cases for DBNull, Enum, TimeSpan and Boolean.
  * Reusable implementation of GetHashCode, available from the HashCode and HashCodeBuilder classes.
  * String.DelimitWith() extension method that can concatenate any sequence into a string using a user-specified separator, item format string, prefix and suffix.

# Changes since version 1.1 beta 2 #

  * Added an overload of **BigInteger.ToString** that allows to specify the characters that will be used to represent the digits. Added the corresponding constructor.
```
BigInteger value = new BigInteger("1234DEADBEEF", 16);

const string charSet = "ABCDEFGHIJKLMNOP";
	
string valueInLetters = value.ToString(charSet);
Console.WriteLine("Value converted to base 16 using letters:\n{0}\n", valueInLetters);
// Output is BCDENOKNLOOP

BigInteger valueFromLetters = new BigInteger(valueInLetters, charSet);
Console.WriteLine("Value parsed from base 16 using letters:\n{0}\n", valueFromLetters.ToString(16));
// Output is 1234DEADBEEF
```

  * Added the class **RegularExpressionPatterns**, which contains general-purpose regular expression patterns.

  * Added the class **GenericComparer`<`TCompared`>`**, an implementation of IComparer`<`TCompared`>` that allows for natural and reverse ordering of elements that implement IComparable`<`TCompared`>`.
```
List<int> items = new List<int> { 1, 7, 4, 5, 2, 9, 8 };

items.Sort(GenericComparer<int>.Natural);

foreach(int item in items) Console.Write(item);
Console.WriteLine();
// Output is 1245789

items.Sort(GenericComparer<int>.Reverse);

foreach (int item in items) Console.Write(item);
Console.WriteLine();
// Output is 9875421
```

  * Added the method **TextUtilities.Hash** to easily calculate a hash of one or more strings.
```
string userName = "John Smith";
string documentNumnber = "0001234";

string cacheFileName = TextUtilities.Hash(userName, documentNumnber);
Console.WriteLine(cacheFileName);
// Output is cdK0B5P7yM0x2u8Fdou77QhB8Zk
```

  * Modified the **MailMessage** class. If the mail properties are not specified, it uses the .NET mail configuration (configuration/system.net/mailSettings).

  * Modified the **ServiceResult`<`T`>`** so that it no longer inherits **ServiceResult** because that caused problems in some SOAP clients, namely AS3.

  * Added the **SecureTokenBuilder** class. This class creates and validates tokens that can be encrypted and / or authenticated. The generated token contains only characters that are valid in a query string parameter.
```
SecureTokenBuilder tokenBuilder = new SecureTokenBuilder("some random secret string");

int userId = 12345;
DateTime expirationDate = DateTime.Now.AddDays(1);

string encodedToken = tokenBuilder.EncodeArray(userId, expirationDate);
Console.WriteLine(encodedToken);
// Output is riUNBja3yrFdL8Lpxb63H_gC-NM2PdvRIwk7FKI53Z382hh1DDcfLQJl6O67PQhGQ2z4GGd0zVA

object[] decodedToken = tokenBuilder.DecodeArray(encodedToken);
Console.WriteLine("UserId: {0}, ExpirationDate: {1}", decodedToken[0], decodedToken[1]);
// Output is UserId: 12345, ExpirationDate: 26-11-2009 13:00:20
```

  * Modified the PixelFormat used by the **ImageSizing** class so that it supports transparency.

  * Added a parameter to the **ServiceResult** class that controls whether to send the full exception to the client, to avoid information disclosure issues.

  * Fixed a bug in the **XmlRpc** class. It was not using the correct encoding.

  * Modified the **Log** class to allow to integrate with another logging system. The integration is done by creating a class that implements the **ILog** interface, and then registering that type in the appSettings section of the configuration file.
```
<!-- Replace the default log implementation by MyNamespace.MyLogImplementation -->
<!-- The class MyLogImplementation must implement ILog or inherit from LogBase -->
<add key="SixPackLogImplementation" value="MyNamespace.MyLogImplementation, MyAssembly"/>
```

  * Added a custom implementation of the ILog interface, **NullLog** that allows to completely disable the SixPack log.
```
<!-- Disable the SixPack log -->
<add key="SixPackLogImplementation" value="SixPack.Diagnostics.NullLog, SixPack"/>
```

  * Removed the build number from the assembly version because that causes many problems when using libraries that are compiled against an older (but compatible) version of SixPack.