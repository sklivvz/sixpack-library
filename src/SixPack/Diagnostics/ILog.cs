using System;

namespace SixPack.Diagnostics
{
	/// <summary>
	/// Defines the interface for any log implementation.
	/// </summary>
	public interface ILog
	{
		/// <summary>
		/// Logs an exception
		/// </summary>
		/// <param name="exception">The exception to log</param>
		void HandleException(Exception exception);

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		void AddFormat(string format, object arg0);

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="logLevel">The log level.</param>
		void AddFormat(string format, object arg0, LogLevel logLevel);

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="arg1">Another object.</param>
		void AddFormat(string format, object arg0, object arg1);

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="arg1">Another object.</param>
		/// <param name="logLevel">The log level.</param>
		void AddFormat(string format, object arg0, object arg1, LogLevel logLevel);

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="arg1">Another object.</param>
		/// <param name="arg2">Yet another object.</param>
		void AddFormat(string format, object arg0, object arg1, object arg2);

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="arg1">Another object.</param>
		/// <param name="arg2">Yet another object.</param>
		/// <param name="logLevel">The log level.</param>
		void AddFormat(string format, object arg0, object arg1, object arg2, LogLevel logLevel);

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The object array.</param>
		/// <param name="logLevel">The log level.</param>
		void AddFormat(string format, object[] args, LogLevel logLevel);

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The object array.</param>
		void AddFormat(string format, object[] args);

		/// <summary>
		/// Logs an exception
		/// </summary>
		/// <param name="exception">The exception to log</param>
		/// <param name="logLevel">The severity of the corresponding message</param>
		void HandleException(Exception exception, LogLevel logLevel);

		/// <summary>
		/// Adds a line to the log
		/// </summary>
		/// <param name="text">The message to add</param>
		void Add(string text);

		/// <summary>
		/// Adds a line to the log
		/// </summary>
		/// <param name="text">The message to add</param>
		/// <param name="logLevel">The severity of the corresponding message</param>
		void Add(string text, LogLevel logLevel);
	}
}