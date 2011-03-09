// AssemblyInfo.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi <marco.cecconi@gmail.com>
//  Author: Antoine Aubry <aaubry@gmail.com>
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
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly: AssemblyTitle(AssemblyInfoData.ProjectName + " - " + AssemblyInfoData.BuildDescription)]
[assembly: AssemblyDescription(AssemblyInfoData.ProjectName)]
[assembly: AssemblyConfiguration(AssemblyInfoData.BuildDescription)]
[assembly : AssemblyProduct("SixPack Library")]
[assembly: AssemblyCompany("SixPack")]
[assembly: AssemblyCopyright("Copyright © 2007, 2008, 2009, 2010, 2011")]
[assembly : AssemblyTrademark("")]
[assembly : AssemblyCulture("")]
[assembly : CLSCompliant(true)]
[assembly : ComVisible(false)]
[assembly : NeutralResourcesLanguage("en")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

// Ensure that builds for different plaforms get different version numbers, to avoid versioning issues.
[assembly: AssemblyVersion("1.1.2." + AssemblyInfoData.Revision)]
[assembly: AssemblyFileVersion("1.1.2." + AssemblyInfoData.Revision)]

#region AssemblyInfoData
internal static partial class AssemblyInfoData
{
	public const string BuildDescription = _targetPlatform + " " + _debugMode;

#if DEBUG
	private const string _debugMode = "debug";
#else
	private const string _debugMode = "release";
#endif

	private const string _targetPlatform = ".net 3.5";
	public const string Revision = "35";
}
#endregion
