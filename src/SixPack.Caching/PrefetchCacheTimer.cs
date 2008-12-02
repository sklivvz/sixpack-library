// PrefetchCacheTimer.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi <marco.cecconi@gmail.com>
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
//

using System;
using SixPack.Diagnostics;

namespace SixPack.Caching
{

	internal class PrefetchCacheTimer
	{
		bool fetching;
		object fetchingLock;
		
		volatile int idleFetches;
		readonly int maxIdleFetches;
		object idleLock;
		
		public event EventHandler Fetching;
		
		protected void OnFetching(EventArgs e)
		{
			if (Fetching != null)
				Fetching(this, e);
		}
		
		public event EventHandler Idling;
		
		protected void OnIdling(EventArgs e)
		{
			if (Idling != null)
				Idling(this, e);
		}
		
#if DEBUG
		string sig;
#endif
		
		internal PrefetchCacheTimer(int maxIdleFetches)
		{
#if DEBUG
			Random r = new Random();
			sig = ((byte)r.Next()).ToString("X");
			Log.Instance.AddFormat("PrefetchCacheTimer ctor invoked, id <{0}>", sig);
#endif
			fetching = false;
			fetchingLock = new object();
			
			idleFetches = 0;
			idleLock = new object();
			
			this.maxIdleFetches = maxIdleFetches;
		}
		
		internal void Fetch(object stateInfo)
		{
#if DEBUG
			Random random = new Random();
			byte r = (byte)random.Next();
			string localsig = string.Format("{0} {1:X}", sig, r); 
			Log.Instance.AddFormat("PrefetchCacheTimer: Method '{0}', Fetching...", localsig);
#endif
			lock(idleLock)
			{
				idleFetches++;
				if (idleFetches>maxIdleFetches)
				{
#if DEBUG
					Log.Instance.AddFormat("PrefetchCacheTimer: Method '{0}', idle fetching limit hit, aborting!", localsig);
#endif
					OnIdling(EventArgs.Empty);
					return;					
				}
			}
			
			lock(fetchingLock)
			{
				if (fetching)
				{
#if DEBUG
					Log.Instance.AddFormat("PrefetchCacheTimer: Method '{0}', dobule fetch, aborting!", localsig);
#endif
					return;
				}
				fetching = true;
			}
			
			OnFetching(EventArgs.Empty);
			
			lock (fetchingLock)
			{
				fetching = false;
			}
#if DEBUG
			Log.Instance.AddFormat("PrefetchCacheTimer: Method '{0}', ...released lock!", localsig);
#endif
		}
		
		public void ResetIdleFetches()
		{
#if DEBUG
			Log.Instance.AddFormat("PrefetchCacheTimer: ResetIdleFetches '{0}', before was {1}", sig, idleFetches);
#endif
			lock(idleLock)
			{
				idleFetches = 0;
			}
		}
		
		public int IdleFetches
		{
			get { return idleFetches; }
		}
	}

}
