// Dns.cs 
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace SixPack.Net
{
	/// <summary>
	/// Dns provides advanced DNS lookup capabilities.
	/// </summary>
	public static class Dns
	{
		/// <summary>
		/// Gets the MX records.
		/// </summary>
		/// <param name="domain">The domain.</param>
		/// <returns></returns>
		[SecurityPermission(SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.UnmanagedCode)]
		public static string[] GetMXRecords(string domain)
		{
			IntPtr MXRecordsPointer = IntPtr.Zero;

			if (Environment.OSVersion.Platform != PlatformID.Win32NT)
				throw new NotSupportedException();
			List<string> entries = new List<string>();
			int retCode = UnsafeNativeMethods.DnsQuery(ref domain, UnsafeNativeMethods.QueryTypes.DNS_TYPE_MX,
			                                           UnsafeNativeMethods.QueryOptions.DNS_QUERY_BYPASS_CACHE, 0,
			                                           ref MXRecordsPointer, 0);
			try
			{
				if (retCode != 0)
					throw new Win32Exception(retCode);
				IntPtr currentMXRecordPointer = MXRecordsPointer;
				while (currentMXRecordPointer != IntPtr.Zero)
				{
					UnsafeNativeMethods.MXRecord currentMXRecord =
						(UnsafeNativeMethods.MXRecord)
						Marshal.PtrToStructure(currentMXRecordPointer, typeof (UnsafeNativeMethods.MXRecord));
					if (currentMXRecord.wType == 15)
					{
						string entry = Marshal.PtrToStringAuto(currentMXRecord.pNameExchange);
						//Log.Instance.AddFormat("Returned by MX query: <{0}>", entry, LogLevel.Debug);
						entries.Add(entry);
					}
					currentMXRecordPointer = currentMXRecord.pNext;
				}
			}
			finally
			{
				UnsafeNativeMethods.DnsRecordListFree(MXRecordsPointer, 0);
			}
			return entries.ToArray();
		}

		/// <summary>
		/// Gets the resolved MX records.
		/// </summary>
		/// <param name="domain">The domain.</param>
		/// <returns></returns>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
		public static IPAddress[] GetResolvedMXRecords(string domain)
		{
			string[] mxRecords = GetMXRecords(domain);
			List<IPAddress> resolved = new List<IPAddress>();
			foreach (string mxRecord in mxRecords)
			{
				//Log.Instance.AddFormat("Processing Record: <{0}>", mxRecord, LogLevel.Debug);
				IPAddress[] Records = System.Net.Dns.GetHostAddresses(mxRecord);
				foreach (IPAddress record in Records)
				{
					//Log.Instance.AddFormat("Found IP: <{0}>", record, LogLevel.Debug);
					if (!resolved.Contains(record))
					{
						//Log.Instance.AddFormat("Adding", LogLevel.Debug);
						resolved.Add(record);
					}
					else
					{
						//Log.Instance.AddFormat("Skipping", LogLevel.Debug);
					}
				}
			}
			return resolved.ToArray();
		}
	}
}
