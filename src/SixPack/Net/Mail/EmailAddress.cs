// EmailAddress.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Copyright (C) 2008 Marco Cecconi
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

using System;
using System.ComponentModel;
using System.Net;
using System.Security.Permissions;
using System.Text.RegularExpressions;
using System.Globalization;
using SixPack.Diagnostics;
using SixPack.Net.Sockets;

namespace SixPack.Net.Mail
{
	/// <summary>
	/// Validates an email address
	/// </summary>
	public static class EmailAddress
	{
		/// <summary>
		/// Determines whether the specified email address is valid.
		/// </summary>
		/// <param name="emailAddress">The email address.</param>
		/// <param name="syntaxValidationMode">The syntax validation mode.</param>
		/// <param name="semanticValidationMode">The semantic validation mode.</param>
		/// <returns>
		/// 	<c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string emailAddress, MailSyntaxValidationMode syntaxValidationMode, MailSemanticValidationMode semanticValidationMode)
		{
			return
				IsValid(emailAddress, syntaxValidationMode) &&
				IsValid(emailAddress, semanticValidationMode);
		}

		/// <summary>
		/// Determines whether the specified email address is valid.
		/// </summary>
		/// <param name="emailAddress">The email address.</param>
		/// <param name="syntaxValidationMode">The syntax validation mode.</param>
		/// <returns>
		/// 	<c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string emailAddress, MailSyntaxValidationMode syntaxValidationMode)
		{
			switch (syntaxValidationMode)
			{
				case MailSyntaxValidationMode.None:
					return true;

				case MailSyntaxValidationMode.Simple:
					return simpleEmailAddressPattern.IsMatch(emailAddress);

				case MailSyntaxValidationMode.Rfc2822:
					return rfc2822EmailAddressPattern.IsMatch(emailAddress);

				default:
					throw new NotImplementedException(
						String.Format(
							CultureInfo.InvariantCulture,
							"Validation mode {0} is not implemented.",
							syntaxValidationMode
						)
					);
			}
		}

		/// <summary>
		/// Determines whether the specified email address is valid.
		/// </summary>
		/// <param name="emailAddress">The email address.</param>
		/// <param name="semanticValidationMode">The semantic validation mode.</param>
		/// <returns>
		/// 	<c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string emailAddress, MailSemanticValidationMode semanticValidationMode)
		{
			if (semanticValidationMode == MailSemanticValidationMode.None)
			{
				return true;
			}

			Match match = emailParser.Match(emailAddress);
			if (!match.Success)
			{
				return false;
			}

			string server = match.Groups["server"].Value;

			switch (semanticValidationMode)
			{
				case MailSemanticValidationMode.DnsLookup:
					return emailDNSLookup(server);

				case MailSemanticValidationMode.ServerLookup:
					return emailServerLookup(server);

				case MailSemanticValidationMode.UserLookup:
					return emailUserLookup(server, emailAddress);

				default:
					throw new NotImplementedException(
						string.Format(
							CultureInfo.InvariantCulture,
							"Validation Mode {0} is not implemented.",
							semanticValidationMode
						)
					);
			}
		}

		/// <summary>
		/// Determines whether the specified email address is valid.
		/// </summary>
		/// <param name="emailAddress">The email address.</param>
		/// <returns>
		/// 	<c>true</c> if the specified email address is valid; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsValid(string emailAddress)
		{
			return IsValid(emailAddress, MailSyntaxValidationMode.Simple, MailSemanticValidationMode.None);
		}

		#region Private implementation
		/// <summary>
		/// Regex expression to validate an Email address according to RFC 2822
		/// </summary>
		private static readonly Regex rfc2822EmailAddressPattern = new Regex(
			@"^(?:(?:\r
)?[ \t])*(?:(?:(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*))*@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*|(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)*\<(?:(?:\r
)?[ \t])*(?:@(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*(?:,@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*)*:(?:(?:\r
)?[ \t])*)?(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*))*@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*\>(?:(?:\r
)?[ \t])*)|(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)*:(?:(?:\r
)?[ \t])*(?:(?:(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*))*@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*|(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)*\<(?:(?:\r
)?[ \t])*(?:@(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*(?:,@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*)*:(?:(?:\r
)?[ \t])*)?(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*))*@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*\>(?:(?:\r
)?[ \t])*)(?:,\s*(?:(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*))*@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*|(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)*\<(?:(?:\r
)?[ \t])*(?:@(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*(?:,@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*)*:(?:(?:\r
)?[ \t])*)?(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|""(?:[^\""\r\\]|\\.|(?:(?:\r
)?[ \t]))*""(?:(?:\r
)?[ \t])*))*@(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*)(?:\.(?:(?:\r
)?[ \t])*(?:[^()<>@,;:\\"".\[\] \000-\031]+(?:(?:(?:\r
)?[ \t])+|\Z|(?=[\[""()<>@,;:\\"".\[\]]))|\[([^\[\]\r\\]|\\.)*\](?:(?:\r
)?[ \t])*))*\>(?:(?:\r
)?[ \t])*))*)?;\s*)$",
			RegexOptions.Compiled | RegexOptions.CultureInvariant
		);

		/// <summary>
		/// Regex expression to validate an Email address in a simple, but efficient manner
		/// </summary>
		private static readonly Regex simpleEmailAddressPattern = new Regex(
			@"^[A-Za-z0-9\._%+&\-]+@[A-Za-z0-9\.\-]+\.[A-Za-z]{2,5}$",
			RegexOptions.Compiled | RegexOptions.CultureInvariant
		);

		private static readonly Regex emailParser = new Regex("^(?<user>.+)@(?<server>.+)$", RegexOptions.Compiled | RegexOptions.ExplicitCapture);

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static bool emailUserLookup(string server, string email)
		{
			IPAddress[] mx;
			try
			{
				mx = Dns.GetResolvedMXRecords(server);
			}
			catch (Win32Exception w32e)
			{
				Log.Instance.HandleException(w32e, LogLevel.Debug);
				return false;
			}
			if (mx.Length == 0)
				return false;

			foreach (IPAddress record in mx)
			{
				SmtpClient smtp = new SmtpClient(record);
				try
				{
					if (smtp.Connect().ResponseCode == SmtpResponseCode.ServiceReady &&
						smtp.Hello().ResponseCode == SmtpResponseCode.Completed)
					{
						SmtpResponse res = smtp.Verify(email);
						if (res.ResponseCode == SmtpResponseCode.Completed || res.ResponseCode == SmtpResponseCode.UserNotLocalForward ||
							res.ResponseCode == SmtpResponseCode.CannotVerify || res.ResponseCode == SmtpResponseCode.UserNotLocalForward)
							return true;
					}
				}
				catch (ApplicationException)
				{
					continue;
				}
			}
			return false;
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static bool emailServerLookup(string server)
		{
			IPAddress[] mx;
			try
			{
				mx = Dns.GetResolvedMXRecords(server);
			}
			catch (Win32Exception w32e)
			{
				Log.Instance.HandleException(w32e, LogLevel.Debug);
				return false;
			}
			if (mx.Length == 0)
				return false;
			foreach (IPAddress record in mx)
			{
				SmtpClient smtp = new SmtpClient(record);
				try
				{
					if (smtp.Connect().ResponseCode == SmtpResponseCode.ServiceReady &&
						smtp.Hello().ResponseCode == SmtpResponseCode.Completed)
						return true;
				}
				catch (ApplicationException)
				{
					continue;
				}
			}
			return false;
		}

		[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		private static bool emailDNSLookup(string server)
		{
			IPAddress[] mx;
			try
			{
				mx = Dns.GetResolvedMXRecords(server);
			}
			catch (Win32Exception w32e)
			{
				Log.Instance.HandleException(w32e, LogLevel.Debug);
				return false;
			}
			return (mx.Length > 0);
		}
		#endregion
	}
}
