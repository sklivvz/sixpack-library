// CachedContextProperty.cs 
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

using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Messaging;
using System.Security.Permissions;

namespace SixPack.ComponentModel
{
	/// <summary>
	/// Experimental
	/// </summary>
	public class CachedContextProperty : IContextProperty, IContributeServerContextSink
	{
		#region IContextProperty Members

		/// <summary>
		/// Called when the context is frozen.
		/// </summary>
		/// <param name="newContext">The context to freeze.</param>
		/// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public void Freeze(Context newContext)
		{
			// do nothing
		}

		/// <summary>
		/// Returns a Boolean value indicating whether the context property is compatible with the new context.
		/// </summary>
		/// <param name="newCtx">The new context in which the <see cref="T:System.Runtime.Remoting.Contexts.ContextProperty"></see> has been created.</param>
		/// <returns>
		/// true if the context property can coexist with the other context properties in the given context; otherwise, false.
		/// </returns>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public bool IsNewContextOK(Context newCtx)
		{
			//Log.Instance.AddFormat("CachedAttribute IsContextOK (newCtx={0})", newCtx);
			return true; //??
		}

		/// <summary>
		/// Gets the name of the property under which it will be added to the context.
		/// </summary>
		/// <value></value>
		/// <returns>The name of the property.</returns>
		/// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
		public string Name
		{
			[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
			get { return "CachedContextProperty"; }
		}

		#endregion

		#region IContributeServerContextSink Members

		/// <summary>
		/// Takes the first sink in the chain of sinks composed so far, and then chains its message sink in front of the chain already formed.
		/// </summary>
		/// <param name="nextSink">The chain of sinks composed so far.</param>
		/// <returns>The composite sink chain.</returns>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public IMessageSink GetServerContextSink(IMessageSink nextSink)
		{
			IMessageSink cacheSink = new CachedSink(nextSink);
			return cacheSink;
		}

		#endregion
	}
}
