using System;
using System.Collections.Generic;
using MbUnit.Framework;

namespace SixPack.Reflection.Tests
{
	[TestFixture]
	public class TypeNameTests
	{
		[RowTest]
		[Row(typeof(string), "String")]
		[Row(typeof(IEnumerable<string>), "IEnumerable<String>")]
		[Row(typeof(int[,,][]), "Int32[][,,]")]
		public void ParseTypes(Type type, string expectedName)
		{
			var typeName = new TypeName(type.AssemblyQualifiedName);

			Assert.AreEqual(type.Assembly.FullName, typeName.AssemblyName.FullName, type.AssemblyQualifiedName);
			Assert.AreEqual(expectedName, typeName.ToString(), type.AssemblyQualifiedName);
		}

		[Test]
		public void InexistantAssembly()
		{
			var typeName = new TypeName(typeof(string).AssemblyQualifiedName.Replace("mscorlib", "dum\\my"));

			Assert.AreEqual(typeof(string).Assembly.FullName.Replace("mscorlib", "dummy"), typeName.AssemblyName.FullName);
			Assert.AreEqual("String", typeName.ToString());
		}
	}
}