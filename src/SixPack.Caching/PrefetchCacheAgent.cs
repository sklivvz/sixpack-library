// PrefetchCacheAgent.cs
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
using System.Threading;

using SixPack.Diagnostics;

namespace SixPack.Caching
{
	internal class PrefetchCacheAgent<TReturn> : IDisposable, IPrefetchCacheAgent
	{

		readonly PrefetchCacheContent<TReturn> content;
		readonly Timer timer;
		readonly RefreshFunction<TReturn> myDelegate;
		readonly int cacheTime;
		readonly PrefetchCacheTimer timerFunction;
		readonly string name;
#if DEBUG
		readonly string sig;
#endif
		private bool disposed;
		private Exception exception;

		/// <value>
		/// Fired when the content has been refreshed 20 times without access.
		/// </value>
		public event EventHandler Idling;

		/// <value>
		/// Indicates whether the cache has any content.
		/// </value>
		public bool HasContent
		{
			get
			{
				return content.ReturnMessage != null;
			}
		}
		/// <value>
		/// The content of the cache.
		/// </value>
		public IPrefetchCacheContent Content
		{
			get
			{
				timerFunction.ResetIdleFetches();
				return content;
			}
		}
		/// <value>
		/// The exception which occurred, if any.
		/// </value>
		public Exception Exception
		{
			get
			{
				return exception;
			}
		}
		/// <value>
		/// The name of this Agent in cache.
		/// </value>
		public string Name
		{
			get
			{
				return name;
			}
		}
		/// <summary>
		/// Creates a new instance of <see cref="PrefetchCacheAgent{TReturn}"/>
		/// </summary>
		/// <param name="name">
		/// The name of this agent in cache
		/// </param>
		/// <param name="del">
		/// A <see cref="RefreshFunction{TReturn}"/> which will be executed repeatedly each interval
		/// to refresh the content.
		/// </param>
		/// <param name="cacheTime">
		/// The length of time between attempts to refresh the content
		/// </param>		
		public PrefetchCacheAgent(string name, RefreshFunction<TReturn> del, int cacheTime)
		{
#if DEBUG
			sig = name;
			Log.Instance.AddFormat("PrefetchCacheAgent - ctor invoked, id <{0}>, spawning...", sig);
#endif
			myDelegate = del;
			this.cacheTime = cacheTime;
			this.name = name;

			content = new PrefetchCacheContent<TReturn>();
			// Spawn Update Thread
			timerFunction = new PrefetchCacheTimer(10);

			timerFunction.Fetching += timerFunction_OnFetching;
			timerFunction.Idling += timerFunction_OnIdling;

			TimerCallback timerDelegate = timerFunction.Fetch;
			timer = new Timer(timerDelegate, null, 0, cacheTime * 1000);
#if DEBUG
			Log.Instance.AddFormat("PrefetchCacheAgent - ...timer for '{0}' spawned!", sig);
#endif
		}

		/// <summary>
		/// Waits until the content is not null.
		/// </summary>
		public void WaitForNotNullContent(int millisecondMaxWait)
		{
#if DEBUG
			Log.Instance.AddFormat("WaitForNotNullContent - {0} - acquiring lock...", sig);
#endif
			// Waits for an (eventually still in progress) cache fetching cycle to complete.
			lock (content)
			{
#if DEBUG
				Log.Instance.AddFormat("WaitForNotNullContent - {0} - ...got lock...", sig);
#endif
				if (!HasContent)
				{
#if DEBUG
					Log.Instance.AddFormat("WaitForNotNullContent - {0} - No contentent, calling Monitor.Wait()...", sig);
#endif
					Monitor.Wait(content, millisecondMaxWait);
#if DEBUG
					Log.Instance.AddFormat("WaitForNotNullContent - {0} - ...Monitor.Wait() set me free!", sig);
#endif
				}
			}
#if DEBUG
			Log.Instance.AddFormat("WaitForNotNullContent - {0} - ...released lock!", sig);
#endif
		}

		/// <summary>
		/// Waits until the content is refreshed.
		/// </summary>
		public void WaitForFreshContent(int millisecondMaxWait)
		{
#if DEBUG
			Log.Instance.AddFormat("WaitForFreshContent - {0} - acquiring lock...", sig);
#endif
			// Waits for an (eventually still in progress) cache fetching cycle to complete.
			lock (content)
			{
#if DEBUG
				Log.Instance.AddFormat("WaitForFreshContent - {0} - ...got lock...", sig);
#endif
				if (DateTime.Now > content.ExpiryDate)
				{
#if DEBUG
					Log.Instance.AddFormat("WaitForFreshContent - {0} - No contentent, calling Monitor.Wait()...", sig);
#endif
					Monitor.Wait(content, millisecondMaxWait);
#if DEBUG
					Log.Instance.AddFormat("WaitForFreshContent - {0} - ...Monitor.Wait() set me free!", sig);
#endif
				}
			}
#if DEBUG
			Log.Instance.AddFormat("WaitForFreshContent - {0} - ...released lock!", sig);
#endif
		}

		protected void OnIdling(EventArgs e)
		{
			if (Idling != null)
				Idling(this, e);
		}

		/// <summary>
		/// Disposes this instance.
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!disposed)
			{
				if (disposing)
				{
					timer.Dispose();
				}
			}
			disposed = true;
		}

		private void timerFunction_OnIdling(object sender, EventArgs e)
		{
			if (timerFunction.IdleFetches > 20)
			{
#if DEBUG
				Log.Instance.AddFormat("PrefetchCacheAgent: {0} Idle, committing seppuku...", sig);
#endif
				OnIdling(EventArgs.Empty);
			}
		}

		private void timerFunction_OnFetching(object sender, EventArgs e)
		{
			try
			{
				TReturn res = myDelegate();
#if DEBUG
				Log.Instance.AddFormat("PrefetchCacheAgent: {0} Acquiring lock...", sig);
#endif
				lock (content)
				{
					content.ReturnMessage = res;
					content.ExpiryDate = DateTime.Now.AddSeconds(cacheTime);
#if DEBUG
					Log.Instance.AddFormat("PrefetchCacheAgent: {0} Pulsing lock...", sig);
#endif
					// Signal back that we have fresh content.
					Monitor.PulseAll(content);
#if DEBUG
					Log.Instance.AddFormat("PrefetchCacheAgent: {0} ...Done!", sig);
#endif
				}
			}
			// we are catching exceptions because we are in a separate thread.
			// the exception is then stored in the main class so it can be
			// handled after the pulse all is executed.
			catch (Exception err)
			{
#if DEBUG
				Log.Instance.HandleException(err);
#endif
				exception = err;
				lock (content)
				{
#if DEBUG
					Log.Instance.AddFormat("PrefetchCacheAgent: {0} Error: Pulsing lock...", sig);
#endif
					// Signal back that we have fresh content.
					Monitor.PulseAll(content);
#if DEBUG
					Log.Instance.AddFormat("PrefetchCacheAgent: {0} Error: ...Done!", sig);
#endif
				}
				// If we don't have any content we need to commit seppuku so we will be invoked again at next call.
				OnIdling(EventArgs.Empty);
			}
		}
	}
}
