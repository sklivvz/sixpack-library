// CachedAttribute.cs 
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
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Contexts;
using System.Security.Permissions;

namespace SixPack.ComponentModel
{
	/// <summary>
	/// Enables caching on the current class.
	/// </summary>
	/// <remarks>
	/// The methods that are to be cached should be marked with the <see cref="CachedMethodAttribute"/> attribute.
	/// The current class must inherit from <see cref="ContextBoundObject"/>.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class)]
	public sealed class CachedAttribute : ContextAttribute
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CachedAttribute"/> class.
		/// </summary>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public CachedAttribute()
			: base("CachedEnabled")
		{
			//Log.Instance.Add("CacheEnabled .ctor");
		}

		/// <summary>
		/// Gets the properties for new context.
		/// </summary>
		/// <param name="ctorMsg">The ctor.</param>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override void GetPropertiesForNewContext(IConstructionCallMessage ctorMsg)
		{
			if (ctorMsg == null)
				throw new ArgumentNullException("ctorMsg");

			IContextProperty cachedProperty = new CachedContextProperty();
			ctorMsg.ContextProperties.Add(cachedProperty);
		}

		/// <summary>
		/// Returns a Boolean value indicating whether the context parameter meets the context attribute's requirements.
		/// </summary>
		/// <param name="ctx">The context in which to check.</param>
		/// <param name="ctorMsg">The <see cref="T:System.Runtime.Remoting.Activation.IConstructionCallMessage"></see> to which to add the context property.</param>
		/// <returns>
		/// true if the passed in context is okay; otherwise, false.
		/// </returns>
		/// <exception cref="T:System.ArgumentNullException">Either ctx or ctorMsg is null. </exception>
		/// <PermissionSet><IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="Infrastructure"/></PermissionSet>
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.Infrastructure)]
		public override bool IsContextOK(Context ctx, IConstructionCallMessage ctorMsg)
		{
			//Log.Instance.AddFormat("CacheEnabled IsContextOK (ctx={0}, ctorMsg={1})", ctx, ctorMsg);
			return false; //??
		}
	}
}
