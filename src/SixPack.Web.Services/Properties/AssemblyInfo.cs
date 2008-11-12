// AssemblyInfo.cs 
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
using System.Reflection;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

[assembly : AssemblyTitle("SixPack.Web.Services")]
#if DEBUG
[assembly : AssemblyConfiguration("Debug")]
[assembly : AssemblyDescription("SixPack Library Web.Services - Debug build")]
#else
[assembly: AssemblyConfiguration("Release")]
[assembly : AssemblyDescription("SixPack Library Web.Services - Release build")]
#endif
[assembly: AssemblyCompany("SixPack")]
[assembly : AssemblyProduct("SixPack Library")]
[assembly : AssemblyCopyright("Copyright © 2007, 2008")]
[assembly : AssemblyTrademark("")]
[assembly : AssemblyCulture("")]
[assembly : CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.

[assembly : ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM

[assembly : Guid("787a6aea-743d-4f23-b976-6e8f48bfbeaf")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Revision and Build Numbers 
// by using the '*' as shown below:

[assembly : AssemblyVersion("1.0.0.*")]
[assembly : AssemblyFileVersion("1.0.0.1")]
