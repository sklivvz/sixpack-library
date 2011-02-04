using System;
using System.Collections.Generic;
using System.Reflection;
using MbUnit.Framework;

namespace SixPack.Reflection.Tests
{
	[TestFixture]
	public class TypeNameTests
	{
		[RowTest]
		[Row(typeof(string), "String")]
		[Row(typeof(IEnumerable<string>), "IEnumerable<String>")]
		[Row(typeof(int[, ,][]), "Int32[][,,]")]
		[Row(typeof(char*), "Char*")]
		public void ParseTypes(Type type, string expectedName)
		{
			Console.WriteLine("-----------------------------------------------------");
			Console.WriteLine(type.AssemblyQualifiedName);
			Console.WriteLine("-----------------------------------------------------");

			var typeName = new TypeName(type.AssemblyQualifiedName);

			Console.WriteLine(type.Assembly.FullName);
			Console.WriteLine(typeName.AssemblyName.FullName);
			Console.WriteLine();
			Assert.AreEqual(type.Assembly.FullName, typeName.AssemblyName.FullName, type.AssemblyQualifiedName);

			Console.WriteLine(type.AssemblyQualifiedName);
			Console.WriteLine(typeName.AssemblyQualifiedName);
			Console.WriteLine();
			Assert.AreEqual(type.AssemblyQualifiedName, typeName.AssemblyQualifiedName, type.AssemblyQualifiedName);

			Console.WriteLine(type.FullName);
			Console.WriteLine(typeName.FullName);
			Console.WriteLine();
			Assert.AreEqual(type.FullName, typeName.FullName, type.AssemblyQualifiedName);

			Console.WriteLine(expectedName);
			Console.WriteLine(typeName.ToString());
			Console.WriteLine();
			Assert.AreEqual(expectedName, typeName.ToString(), type.AssemblyQualifiedName);

			Console.WriteLine("-----------------------------------------------------");
		}

		[Test]
		public void TypeReference()
		{
			var method = typeof(TypeNameTests).GetMethod("TypeReferenceHelper", BindingFlags.Static | BindingFlags.NonPublic);
			var args = method.GetParameters();

			ParseTypes(args[0].ParameterType, "String&");
			ParseTypes(args[1].ParameterType, "IEnumerable<Int32>&");
			ParseTypes(args[2].ParameterType, "Char*&");
		}

		unsafe private static void TypeReferenceHelper(ref string x, ref IEnumerable<int> y, ref char* z)
		{
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