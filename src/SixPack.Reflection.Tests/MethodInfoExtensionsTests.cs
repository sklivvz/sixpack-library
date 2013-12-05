using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SixPack.Reflection.Tests
{
	[TestClass]
	public class MethodInfoExtensionsTests
	{
		public void InstanceMethod(string message)
		{
			throw new NotImplementedException(message);
		}

		public static void StaticMethod(string message)
		{
			throw new NotImplementedException(message);
		}

#if NET_40
		[TestMethod]
		[ExpectedException(typeof(NotImplementedException))]
		public void InvokeInstanceMethodPreservesException()
		{
			var method = MethodReference.Get(() => InstanceMethod(null));

			method.InvokeUnwrapped(this, new object[] { "kebas" });
		}

		[TestMethod]
		[ExpectedException(typeof(NotImplementedException))]
		public void InvokeStaticMethodPreservesException()
		{
			var method = MethodReference.Get(() => StaticMethod(null));

			method.InvokeUnwrapped(null, new object[] { "kebas" });
		}
#endif
	}
}