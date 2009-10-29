// Log.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA 02111-1307 USA 
//

#define FILE_LOGGING
#define MAIL_LOGGING
#undef  DB_LOGGING

using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Diagnostics;
using System.Reflection;

namespace SixPack.Diagnostics
{
	/// <summary>
	/// Use this class to log messages for later consumption
	/// </summary>
	/// <remarks>
	/// If the appSettings configuration section contains a key named SixPackLogImplementation, and
	/// that entry contains the name of a type, the code uses that implementation instead of
	/// the default one. That implementation must implement the <see cref="ILog"/> interface and have
	/// a parameterless constructor.
	/// </remarks>
	public sealed class Log : LogBase
	{
		private const string COPYRIGHT = "(c) SixPack";
		private const string DEFAULT_FILELOGFORMAT = @"{0:yyyyMMddTHHmmss.ff} - {3} - {4} - {5} - {1} - {6} - {2}";

		private const string DEFAULT_MAILLOGFORMAT =
			@"
Date:           {0:yyyyMMddTHHmmss.ff}
Server:         {3}
Appdomain:      {4}
Originating IP: {5}
Severity:       {1}
Method name:	{6}
Message:

{2}";

		private const string RELEASE_DATE = "2008-11-08";
		private const string VERSION = "1.0";

		private static readonly object syncObject = new Object();
		private static readonly object syncRoot = new Object();

		private static volatile string CurrentAppDomain;
		private static volatile string CurrentServer;

#if DB_LOGGING
		private static volatile string DbLogFormat;
		private static volatile int DbLogLevel;
#endif

#if FILE_LOGGING

		private static volatile string FileLogFile;
		private static volatile string FileLogFormat;
		private static volatile int FileLogLevel;
		private static volatile string MailLogFormat;
#endif

#if MAIL_LOGGING
		private static volatile int MailLogLevel;
		private static volatile string MailLogRecipient;
		private static volatile string MailLogSender;
		private static volatile string MailLogSMTPServer;
		private static volatile string MailLogSubject;
#endif
		private static volatile ILog SoleInstance;
#if DEBUG
		private readonly bool EnableCallingMethodName;
#endif
		private Log()
		{
#if FILE_LOGGING
			try
			{
				FileLogLevel = Convert.ToInt32(ConfigurationManager.AppSettings["FileLogLevel"], CultureInfo.InvariantCulture);
			}
			catch (ApplicationException)
			{
				FileLogLevel = 0;
			}
			if (FileLogLevel > 0)
			{
				FileLogFile = ConfigurationManager.AppSettings["FileLogFile"];
				FileLogFormat = ConfigurationManager.AppSettings["FileLogFormat"];
				if (String.IsNullOrEmpty(FileLogFormat))
					FileLogFormat = DEFAULT_FILELOGFORMAT;
			}
#endif

#if DB_LOGGING
			try
			{
				DbLogLevel = Convert.ToInt32(ConfigurationManager.AppSettings["DbLogLevel"]);
			}
			catch
			{
				DbLogLevel = 0;
			}
			if (DbLogLevel > 0)
				DbLogFormat = ConfigurationManager.AppSettings["DbLogFormat"];

#endif

#if MAIL_LOGGING
			try
			{
				MailLogLevel = Convert.ToInt32(ConfigurationManager.AppSettings["MailLogLevel"], CultureInfo.InvariantCulture);
			}
			catch (ApplicationException)
			{
				MailLogLevel = 0;
			}
			if (MailLogLevel > 0)
			{
				MailLogSender = ConfigurationManager.AppSettings["MailLogSender"];
				MailLogRecipient = ConfigurationManager.AppSettings["MailLogRecipient"];
				MailLogSubject = ConfigurationManager.AppSettings["MailLogSubject"];
				MailLogSMTPServer = ConfigurationManager.AppSettings["MailLogSMTPServer"];
				if (String.IsNullOrEmpty(MailLogSender) || String.IsNullOrEmpty(MailLogRecipient) ||
					String.IsNullOrEmpty(MailLogSubject) || String.IsNullOrEmpty(MailLogSMTPServer))
					MailLogLevel = 0;
				MailLogFormat = ConfigurationManager.AppSettings["MailLogFormat"];
				if (String.IsNullOrEmpty(MailLogFormat))
					MailLogFormat = DEFAULT_MAILLOGFORMAT;
			}
#endif
#if DEBUG
			bool enableCaller;
			if (!bool.TryParse(ConfigurationManager.AppSettings["EnableCallingMethodName"], out enableCaller))
			{
				EnableCallingMethodName = enableCaller;
			}
#endif
			if (HttpContext.Current != null && HttpContext.Current.Server != null)
			{
				CurrentServer = HttpContext.Current.Server.MachineName;
			}
			else
			{
				CurrentServer = "UNKNOWN";
			}

			try
			{
				CurrentAppDomain = AppDomain.CurrentDomain.FriendlyName;
			}
			catch (ApplicationException)
			{
				CurrentAppDomain = "UNKNOWN";
			}

			Add("Log Started", LogLevel.Info);
			AddFormat("Version: {0}", VERSION, LogLevel.Debug);
			AddFormat("Copyright: {0}", COPYRIGHT, LogLevel.Debug);
			AddFormat("Release Date: {0}", RELEASE_DATE, LogLevel.Debug);
		}

#if FILE_LOGGING
		/// <summary>
		/// Gets the current log file.
		/// </summary>
		/// <value>The current log file.</value>
		[Obsolete("Will be removed from future versions.")]
		public static string CurrentLogFile
		{
			get
			{
				return String.Format(CultureInfo.InvariantCulture, FileLogFile, DateTime.Now);
			}
		}
#endif

		/// <summary>
		/// Gets the instance.
		/// </summary>
		/// <value>The instance.</value>
		public static ILog Instance
		{
			get
			{
				if (SoleInstance == null)
				{
					lock (syncRoot)
					{
						if (SoleInstance == null)
						{
							string logImplementation = ConfigurationManager.AppSettings["SixPackLogImplementation"];
							if (!string.IsNullOrEmpty(logImplementation))
							{
								Type logType = Type.GetType(logImplementation, true);
								SoleInstance = (ILog)Activator.CreateInstance(logType);
							}
							else
							{
								SoleInstance = new Log();
							}
						}
					}
				}
				return SoleInstance;
			}
		}

		/// <summary>
		/// Releases unmanaged resources and performs other cleanup operations before the
		/// <see cref="Log"/> is reclaimed by garbage collection.
		/// </summary>
		~Log()
		{
			Add("Log Stopped", LogLevel.Info);
		}

		/// <summary>
		/// Adds a line to the log
		/// </summary>
		/// <param name="text">The message to add</param>
		/// <param name="logLevel">The severity of the corresponding message</param>
		public override void Add(string text, LogLevel logLevel)
		{
			string ip;
			if (HttpContext.Current != null && HttpContext.Current.Request != null)
			{
				ip = HttpContext.Current.Request.UserHostAddress;
			}
			else
			{
				ip = "0.0.0.0";
			}

#if DEBUG
			string callerMethod;
			if (EnableCallingMethodName)
			{
				StackFrame stackFrame = new StackFrame(1, false);
				MethodBase method = stackFrame.GetMethod();
				callerMethod = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", method.DeclaringType.FullName, method.Name);
			}
			else
			{
				callerMethod = string.Empty;
			}
#else
			string callerMethod = string.Empty;
#endif

#if FILE_LOGGING
			if ((FileLogLevel & (int)logLevel) > 0)
			{
				string logFile = String.Format(CultureInfo.InvariantCulture, FileLogFile, DateTime.Now);
				string message =
					String.Format(CultureInfo.InvariantCulture, FileLogFormat,
								  new object[] { DateTime.Now, logLevel, text, CurrentServer, CurrentAppDomain, ip, callerMethod });
				lock (syncObject)
				{
					using (FileStream fileStream = new FileStream(logFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite))
					using (StreamWriter writer = new StreamWriter(fileStream))
					{
						writer.WriteLine(message);
						writer.Flush();
					}
				}
			}
#endif
#if DB_LOGGING
			if ((DbLogLevel & (long) Severity) > 0)
			{
				DatabaseGateway.Instance.AddLog(String.Format(DbLogFormat,DateTime.Now,logLevel,Text));
			}
#endif

#if MAIL_LOGGING
			if ((MailLogLevel & (long)logLevel) > 0)
			{
				MailMessage m = new MailMessage();
				m.From = new MailAddress(MailLogSender);
				m.To.Add(MailLogRecipient);
				m.Subject = String.Format(CultureInfo.InvariantCulture, MailLogSubject, logLevel, CurrentServer);
				string message =
					String.Format(CultureInfo.InvariantCulture, MailLogFormat,
								  new object[] { DateTime.Now, logLevel, text, CurrentServer, CurrentAppDomain, ip, callerMethod });

				m.Body = message;
				SmtpClient SmtpMail = new SmtpClient(MailLogSMTPServer);
				SmtpMail.Send(m);
			}
#endif
		}
	}
}
