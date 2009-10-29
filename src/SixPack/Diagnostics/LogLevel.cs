using System;

namespace SixPack.Diagnostics
{
	/// <summary>
	/// Specifies the severity of a log message.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Will not be logged.
		/// </summary>
		None = 0,
		/// <summary>
		/// Debug
		/// </summary>
		Debug = 1,
		/// <summary>
		/// Information
		/// </summary>
		Info = 2,
		/// <summary>
		/// Warning
		/// </summary>
		Warning = 4,
		/// <summary>
		/// Error
		/// </summary>
		Error = 8,
		/// <summary>
		/// Critical error
		/// </summary>
		Critical = 16
	}
}
