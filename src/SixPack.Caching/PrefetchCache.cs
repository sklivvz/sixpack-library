// PrefetchCache.cs
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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace SixPack.Caching
{
	/// <summary>
	/// A <see cref="Delegate"/> that will be executed to refresh the cache.
	/// </summary>
	/// <returns>
	/// The delegate returns an object of type represented by TReturn that is going to be cached.
	/// </returns>
	public delegate TReturn FetchingExecutor<TReturn>();

	/// <summary>
	/// A caching system with prefetching.
	/// </summary>
	public static class PrefetchCache
	{
		/// <summary>
		/// Tries to get the result of the <see cref="FetchingExecutor{TReturn}"/> from the cache. If it can't it starts a new prefetch thread in
		/// the background.
		/// </summary>
		/// <param name="del">
		/// A <see cref="FetchingExecutor{TReturn}"/> which will be prefetched by the class.
		/// </param>
		/// <param name="cacheTime">
		/// The number of seconds of cache life as a <see cref="System.Int32"/>/
		/// </param>
		/// <param name="prefetchCacheOptions">
		/// A <see cref="PrefetchCacheOptions"/> flags, which specify the behavior of this cache.
		/// </param>
		/// <param name="args">
		/// A series of <see cref="System.Object"/>s which represent the parameters upon which the <see cref="FetchingExecutor{TReturn}"/>
		/// operates. The same delegate with different parameters will be cached separately.
		/// </param>
		/// <returns>
		/// A TReturn, the result of the cache lookup.
		/// </returns>
		public static TReturn Get<TReturn>(FetchingExecutor<TReturn> del, int cacheTime, PrefetchCacheOptions prefetchCacheOptions,
		                                   params object[] args)
		{
			if (cacheTime > 0)
			{
				StackTrace trace = new StackTrace(1, false);
				MethodBase caller = trace.GetFrame(0).GetMethod();
				StringBuilder invokeSig =
					new StringBuilder(caller.DeclaringType.FullName.GetHashCode().ToString("X", CultureInfo.InvariantCulture));
				invokeSig.Append(caller.Name.GetHashCode().ToString("X", CultureInfo.InvariantCulture));
				invokeSig.AppendFormat(" {0}.{1}", caller.DeclaringType.FullName.Split(',')[0], caller.Name);
				foreach (object o in args)
				{
					invokeSig.AppendFormat(CultureInfo.InvariantCulture, " <{0}>", o.GetHashCode());
					Console.WriteLine("{0} - {1}", o.GetType().FullName, o);
				}

				return PrefetchCacheController.GetFromCache(invokeSig.ToString(), del, cacheTime, prefetchCacheOptions);
			}

			return del();
		}
	}
}