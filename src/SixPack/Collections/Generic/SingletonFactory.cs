// SingletonFactory.cs
//
//  Copyright (C) 2011 Antoine Aubry
//  Author: Antoine Aubry
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
using System.Threading;

namespace SixPack.Collections.Generic
{
	/// <summary>
	/// A factory that ensures that each object is created only once.
	/// </summary>
	/// <typeparam name="TKey">The type of the key.</typeparam>
	/// <typeparam name="TSingleton">The type of the singleton.</typeparam>
	public class SingletonFactory<TKey, TSingleton>
	{
		private readonly Func<TKey, TSingleton> _createInstance;
		private readonly IDictionary<TKey, TSingleton> _instances = new Dictionary<TKey, TSingleton>();
		private readonly ReaderWriterLockSlim _lock = new ReaderWriterLockSlim();

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonFactory&lt;TKey, TSingleton&gt;"/> class.
		/// </summary>
		/// <param name="createInstance">The create instance function.</param>
		public SingletonFactory(Func<TKey, TSingleton> createInstance)
		{
			_createInstance = createInstance;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="SingletonFactory&lt;TKey, TSingleton&gt;"/> class.
		/// </summary>
		public SingletonFactory()
		{
			_createInstance = key => (TSingleton)Activator.CreateInstance(typeof(TSingleton), key);
		}

		/// <summary>
		/// Gets the <typeparamref name="TSingleton"/> with the specified key. If the object has not been created yet,
		/// a new instance is created using the createInstance function that was specified in the constructor.
		/// </summary>
		/// <value></value>
		public TSingleton this[TKey key]
		{
			get
			{
				_lock.EnterReadLock();
				try
				{
					TSingleton singleton;
					if (_instances.TryGetValue(key, out singleton))
					{
						return singleton;
					}
				}
				finally
				{
					_lock.ExitReadLock();
				}

				_lock.EnterUpgradeableReadLock();
				try
				{
					TSingleton singleton;
					if (_instances.TryGetValue(key, out singleton))
					{
						return singleton;
					}

					_lock.EnterWriteLock();
					try
					{
						if (_instances.TryGetValue(key, out singleton))
						{
							return singleton;
						}
					}
					finally
					{
						_lock.ExitWriteLock();
					}

					singleton = _createInstance(key);
					_instances.Add(key, singleton);
					return singleton;
				}
				finally
				{
					_lock.ExitUpgradeableReadLock();
				}
			}
		}
	}
}