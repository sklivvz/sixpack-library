// UnsafeNativeMethods.cs 
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
using System.Runtime.InteropServices;

namespace SixPack.Net
{
	internal static class UnsafeNativeMethods
	{
		[DllImport("dnsapi", EntryPoint = "DnsQuery_W", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		internal static extern int DnsQuery([MarshalAs(UnmanagedType.VBByRefStr)] ref string pszName,
		                                    QueryTypes wType, QueryOptions options, int aipServers, ref IntPtr ppQueryResults,
		                                    int pReserved);

		[DllImport("dnsapi", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern void DnsRecordListFree(IntPtr pRecordList, int FreeType);

		#region Nested type: MXRecord

		[StructLayout(LayoutKind.Sequential)]
		internal struct MXRecord
		{
			public int dwReserved;
			public int dwTtl;
			public int flags;
			public short Pad;
			public IntPtr pName;
			public IntPtr pNameExchange;
			public IntPtr pNext;
			public short wDataLength;
			public short wPreference;
			public short wType;
		}

		#endregion

		#region Nested type: QueryOptions

		internal enum QueryOptions
		{
			//DNS_QUERY_ACCEPT_TRUNCATED_RESPONSE = 1,
			DNS_QUERY_BYPASS_CACHE = 8 //,
			//DNS_QUERY_DONT_RESET_TTL_VALUES = 0x100000,
			//DNS_QUERY_NO_HOSTS_FILE = 0x40,
			//DNS_QUERY_NO_LOCAL_NAME = 0x20,
			//DNS_QUERY_NO_NETBT = 0x80,
			//DNS_QUERY_NO_RECURSION = 4,
			//DNS_QUERY_NO_WIRE_QUERY = 0x10,
			//DNS_QUERY_RESERVED = -16777216,
			//DNS_QUERY_RETURN_MESSAGE = 0x200,
			//DNS_QUERY_STANDARD = 0,
			//DNS_QUERY_TREAT_AS_FQDN = 0x1000,
			//DNS_QUERY_USE_TCP_ONLY = 2,
			//DNS_QUERY_WIRE_ONLY = 0x100
		}

		#endregion

		#region Nested type: QueryTypes

		internal enum QueryTypes
		{
			DNS_TYPE_MX = 15
		}

		#endregion
	}
}
