using System;
using System.Globalization;

namespace SixPack.Diagnostics
{
	/// <summary>
	/// Basic implementation of the <see cref="ILog"/> interface.
	/// </summary>
	public abstract class LogBase : ILog
	{
		/// <summary>
		/// Logs an exception
		/// </summary>
		/// <param name="exception">The exception to log</param>
		public void HandleException(Exception exception)
		{
			HandleException(exception, LogLevel.Error);
		}

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		public void AddFormat(string format, object arg0)
		{
			AddFormat(format, new object[1] { arg0 });
		}

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="logLevel">The log level.</param>
		public void AddFormat(string format, object arg0, LogLevel logLevel)
		{
			AddFormat(format, new object[1] { arg0 }, logLevel);
		}

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="arg1">Another object.</param>
		public void AddFormat(string format, object arg0, object arg1)
		{
			AddFormat(format, new object[2] { arg0, arg1 });
		}

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="arg1">Another object.</param>
		/// <param name="logLevel">The log level.</param>
		public void AddFormat(string format, object arg0, object arg1, LogLevel logLevel)
		{
			AddFormat(format, new object[2] { arg0, arg1 }, logLevel);
		}

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="arg1">Another object.</param>
		/// <param name="arg2">Yet another object.</param>
		public void AddFormat(string format, object arg0, object arg1, object arg2)
		{
			AddFormat(format, new object[3] { arg0, arg1, arg2 });
		}

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="arg0">The object.</param>
		/// <param name="arg1">Another object.</param>
		/// <param name="arg2">Yet another object.</param>
		/// <param name="logLevel">The log level.</param>
		public void AddFormat(string format, object arg0, object arg1, object arg2, LogLevel logLevel)
		{
			AddFormat(format, new object[3] { arg0, arg1, arg2 }, logLevel);
		}

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The object array.</param>
		/// <param name="logLevel">The log level.</param>
		public void AddFormat(string format, object[] args, LogLevel logLevel)
		{
			try
			{
				Add(String.Format(CultureInfo.InvariantCulture, format, args), logLevel);
			}
			catch (FormatException fe)
			{
				HandleException(fe, LogLevel.Critical);
			}
			catch (ArgumentNullException ane)
			{
				HandleException(ane, LogLevel.Critical);
			}
		}

		/// <summary>
		/// Adds a line to the log with format.
		/// </summary>
		/// <param name="format">The format.</param>
		/// <param name="args">The object array.</param>
		public void AddFormat(string format, object[] args)
		{
			try
			{
				Add(String.Format(CultureInfo.InvariantCulture, format, args));
			}
			catch (FormatException fe)
			{
				HandleException(fe, LogLevel.Critical);
			}
			catch (ArgumentNullException ane)
			{
				HandleException(ane, LogLevel.Critical);
			}
		}

		/// <summary>
		/// Logs an exception
		/// </summary>
		/// <param name="exception">The exception to log</param>
		/// <param name="logLevel">The severity of the corresponding message</param>
		public void HandleException(Exception exception, LogLevel logLevel)
		{
			Add("Exception occurred:\n" + exception, logLevel);
		}

		/// <summary>
		/// Adds a line to the log
		/// </summary>
		/// <param name="text">The message to add</param>
		public void Add(string text)
		{
			Add(text, LogLevel.Debug);
		}

		/// <summary>
		/// Adds a line to the log
		/// </summary>
		/// <param name="text">The message to add</param>
		/// <param name="logLevel">The severity of the corresponding message</param>
		public abstract void Add(string text, LogLevel logLevel);
	}
}