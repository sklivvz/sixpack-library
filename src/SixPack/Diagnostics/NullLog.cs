using System;

namespace SixPack.Diagnostics
{
	/// <summary>
	/// An <see cref="ILog"/> implementation that does nothing.
	/// </summary>
	public sealed class NullLog : LogBase
	{
		/// <summary>
		/// Adds a line to the log
		/// </summary>
		/// <param name="text">The message to add</param>
		/// <param name="logLevel">The severity of the corresponding message</param>
		public override void Add(string text, LogLevel logLevel)
		{
			// Do nothing
		}
	}
}