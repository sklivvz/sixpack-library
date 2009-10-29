using System;
using MbUnit.Framework;
using SixPack.Diagnostics;
using System.IO;

namespace SixPack.UnitTests.Diagnostics
{
	[TestFixture]
	public class LogTests
	{
		[Test]
		public void TestCustomLogger()
		{
			StringWriter buffer = new StringWriter();
			var output = Console.Out;
			Console.SetOut(buffer);
			try
			{
				Log.Instance.Add("Testing the log");
				Assert.AreEqual("DEBUG\tTesting the log", buffer.ToString().TrimEnd('\r', '\n'));
			}
			finally
			{
				Console.SetOut(output);
			}
		}
	}

	public class ConsoleLogger : LogBase
	{
		public override void Add(string text, LogLevel logLevel)
		{
			Console.WriteLine("{0}\t{1}", logLevel.ToString().ToUpperInvariant(), text);
		}
	}
}