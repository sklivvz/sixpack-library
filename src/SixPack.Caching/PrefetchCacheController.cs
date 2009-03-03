// PrefetchCacheController.cs
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
using System.Collections.Generic;
using SixPack.Diagnostics;

namespace SixPack.Caching
{
	internal static class PrefetchCacheController
	{
		private static volatile Dictionary<string, IPrefetchCacheAgent> agents = new Dictionary<string, IPrefetchCacheAgent>();

		// Need to keep FxCop happy...
		private static int dummy = init();

		// FxCop does not like static constructors. It likes these things better.
		private static int init()
		{
			Log.Instance.Add("[PrefetchCache] Starting.", LogLevel.Info);
			return 0;
		}

		internal static TReturn GetFromCache<TReturn>(string cacheName, FetchingExecutor<TReturn> del, int cacheTime,
		                                              PrefetchCacheOptions prefetchCacheOptions)
		{
			if (!agents.ContainsKey(cacheName))
			{
				lock (agents)
				{
					if (!agents.ContainsKey(cacheName))
					{
#if DEBUG
						Log.Instance.AddFormat("GetFromCache: Method '{0}', Creating Agent ", cacheName);
#endif
						agents[cacheName] = new PrefetchCacheAgent<TReturn>(cacheName, del, cacheTime);
						agents[cacheName].Idling += agent_OnIdling;
					}
				}
			}

			IPrefetchCacheAgent agent = agents[cacheName];

			// If we don't have the content in cache, we either return null or wait for it to be retrieved
			if (!agent.HasContent)
			{
				// If we can, we return null and don't keep the caller waiting
				if ((prefetchCacheOptions & PrefetchCacheOptions.AllowNoResult) != 0)
				{
#if DEBUG
					Log.Instance.AddFormat("GetFromCache: Method '{0}', Returning null (allowed)", cacheName);
#endif
					return default(TReturn);
					//return new ReturnMessage(null, null, 0, methodMessage.LogicalCallContext, methodMessage);
				}

#if DEBUG
				Log.Instance.Add("Before wait for content (null check)");
#endif
				// We wait for the agent to retrieve the content
				agent.WaitForNotNullContent(10000);

				// If we, yet again, don't have content here, we must return an exception
				if (!agent.HasContent)
				{
					//return new ReturnMessage(
					throw new ContentNullException("Could not retrieve content from cache.", agent.Exception);
					//, methodMessage);
				}
				//return agent.Content.ReturnMessage;
			}

			// If caller doesn't care about expired items, we just give what we have
			// Or, if the content is up-to-date, we return it
			if ((prefetchCacheOptions & PrefetchCacheOptions.AllowExpired) != 0 || agent.Content.ExpiryDate >= DateTime.Now)
			{
#if DEBUG
				Log.Instance.AddFormat("GetFromCache: Method '{0}', Returning unchecked expiry (allowed)", cacheName);
#endif
				return (TReturn) agent.Content.ReturnMessage;
			}

			// The content must be up-to-date, but it's old.
			// We wait for new content

#if DEBUG
			Log.Instance.AddFormat("Before wait for content (expiry check) {0:s:fffff}, {1:s:fffff}", agent.Content.ExpiryDate,
			                       DateTime.Now);
#endif
			agent.WaitForFreshContent(10000);
#if DEBUG
			Log.Instance.AddFormat("After wait for content (expiry check) {0:s:fffff}, {1:s:fffff}", agent.Content.ExpiryDate,
			                       DateTime.Now);
#endif

			// If the content is expired we are DOOMED and we must return an exception
			if (agent.Content.ExpiryDate < DateTime.Now)
			{
				//return new ReturnMessage(
				throw new ContentExpiredException("Content in cache has expired.", agent.Exception);
				//, methodMessage);
			}
#if DEBUG
			Log.Instance.AddFormat("GetFromCache: Method '{0}', Returning real", cacheName);
#endif

			// All is well, return the content.

			return (TReturn) agent.Content.ReturnMessage;
		}

		private static void agent_OnIdling(object sender, EventArgs e)
		{
			// Need to keep FxCop happy...
			dummy = -dummy;
#if DEBUG
			Log.Instance.Add("CacheController: seppuku time!");
#endif
			IPrefetchCacheAgent agent = sender as IPrefetchCacheAgent;
						if (agent == null) return;
#if DEBUG
			Log.Instance.AddFormat("CacheController: Agent '{0}', removing from list", agent.Name);
#endif
			agents.Remove(agent.Name);
			IDisposable disposableAgent = sender as IDisposable;
						if (disposableAgent == null) return;

#if DEBUG
			Log.Instance.AddFormat("CacheController: Agent '{0}', disposing", agent.Name);
#endif
			disposableAgent.Dispose();
		}
	}
}