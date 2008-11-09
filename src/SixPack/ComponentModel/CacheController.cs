// CacheController.cs 
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
using System.Collections.Generic;
using System.Globalization;					// Need for debug builds
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Caching;
using SixPack.Diagnostics;

namespace SixPack.ComponentModel
{
	internal static class CacheController
	{
		private static readonly bool isMultithreaded = Init();
		private static volatile Dictionary<string, object> localCache;
		private static volatile Dictionary<string, object> locks = new Dictionary<string, object>();

		private static bool Init()
		{
#if TRACE
            Console.WriteLine("[Cache] Starting.");
#endif
            Log.Instance.Add("[Cache] Starting.", LogLevel.Info);
			if (HttpContext.Current != null)
			{
#if TRACE
                Console.WriteLine("[Cache] Using HttpContext.Current.");
#endif
				Log.Instance.Add("[Cache] Using HttpContext.Current.", LogLevel.Info);
				return true;
			}
			else
			{
				localCache = new Dictionary<string, object>();
#if TRACE
                Console.WriteLine("[Cache] Using local caching. Cached items will not expire.");
#endif
                Log.Instance.Add("[Cache] Using local caching. Cached items will not expire.", LogLevel.Warning);
				return false;
			}
		}

		internal static IMethodReturnMessage GetFromCache(string cacheName, IMethodCallMessage methodMessage,
		                                                  IMessageSink nextSink, int cacheTime)
		{
			if (!locks.ContainsKey(cacheName))
			{
				lock (locks)
					if (!locks.ContainsKey(cacheName))
						locks[cacheName] = new object();
			}
			if (isMultithreaded)
			{
				object cachedContent = HttpRuntime.Cache[cacheName];
				if (cachedContent == null)
				{
					lock (locks[cacheName])
						cachedContent = HttpRuntime.Cache[cacheName];
					if (cachedContent == null)
					{
#if DEBUG
						Log.Instance.AddFormat("[Cache] {0} was a miss.", cacheName, LogLevel.Debug);
#endif
						IMethodReturnMessage mrm = (IMethodReturnMessage) nextSink.SyncProcessMessage(methodMessage);
                        if (mrm.Exception != null)
                            return mrm;
						cachedContent = mrm.ReturnValue;

						HttpRuntime.Cache.Insert(cacheName, cachedContent, null,
						                         DateTime.Now.AddSeconds(cacheTime),
						                         Cache.NoSlidingExpiration);
#if DEBUG
						Log.Instance.Add(
							String.Format(CultureInfo.InvariantCulture, "Total Memory used after load: {0}Mb.",
							              GC.GetTotalMemory(false) >> 20),
							LogLevel.Debug);
#endif
					}
#if DEBUG
					else
						Log.Instance.AddFormat("[Cache] {0} was a hit.", cacheName, LogLevel.Debug);
#endif
				}
#if DEBUG
				else
					Log.Instance.AddFormat("[Cache] {0} was a hit.", cacheName, LogLevel.Debug);
#endif
				return new ReturnMessage(cachedContent, null, 0, null, methodMessage);
			}
			else
			{
				if (!localCache.ContainsKey(cacheName))
				{
					lock (locks[cacheName])
						if (!localCache.ContainsKey(cacheName))
						{
#if DEBUG
							Log.Instance.AddFormat("[Cache] {0} was a miss.", cacheName, LogLevel.Debug);
#endif
							IMethodReturnMessage mrm = (IMethodReturnMessage) nextSink.SyncProcessMessage(methodMessage);
                            if (mrm.Exception != null)
                                return mrm;

							localCache.Add(cacheName, mrm.ReturnValue);
#if DEBUG
							Log.Instance.Add(
								String.Format(CultureInfo.InvariantCulture, "Total Memory used after load: {0}Mb.",
								              GC.GetTotalMemory(false) >> 20),
								LogLevel.Debug);
#endif
						}
#if DEBUG
						else
							Log.Instance.AddFormat("[Cache] {0} was a hit.", cacheName, LogLevel.Debug);
#endif
				}
#if DEBUG
				else
					Log.Instance.AddFormat("[Cache] {0} was a hit.", cacheName, LogLevel.Debug);
#endif
				return
					new ReturnMessage(localCache[cacheName], new object[] {localCache[cacheName]}, 1, methodMessage.LogicalCallContext,
					                  methodMessage);
			}
		}
	}
}
