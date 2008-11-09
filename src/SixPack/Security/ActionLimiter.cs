// ActionLimiter.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
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
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Web;
using System.Collections.Generic;
using System.Security;

namespace SixPack.Security
{
	/// <summary>
	/// Helper class to prevent a Web service method from being called too often
	/// </summary>
	public static class ActionLimiter
	{
		#region Private implementation
		#region CallTimings
		/// <summary>
		/// Keeps track of the last call time of each method
		/// </summary>
		[Serializable]
		private class CallTimings
		{
			private readonly Dictionary<string, DateTime> lastCallTimes = new Dictionary<string, DateTime>();

			/// <summary>
			/// Throws a <see cref="SecurityException"/> if the specified action is being performed too often.
			/// </summary>
			/// <param name="action">
			///	Name of the action that is being performed.
			/// </param>
			/// <param name="minTimeBetweenCalls"></param>
			public void LimitActionsPerUser(string action, TimeSpan minTimeBetweenCalls)
			{
				DateTime lastCallTime;
				if (!lastCallTimes.TryGetValue(action, out lastCallTime))
				{
					lastCallTimes.Add(action, DateTime.Now);
					return;
				}

				DateTime now = DateTime.Now;
				lastCallTimes[action] = now;
				if (now.Subtract(lastCallTime).CompareTo(minTimeBetweenCalls) < 0)
				{
					throw new SecurityException(string.Format(CultureInfo.InvariantCulture, "The Web method is being called too often. Action = '{0}'. Last call time = {1}. Min time between calls = {2}.", action, lastCallTime, minTimeBetweenCalls));
				}
			}
		}
		#endregion

		#region ISession
		private interface ITimingsStore
		{
			CallTimings Timings
			{
				get;
			}
		}

		private class SingleUserSession : ITimingsStore
		{
			private readonly CallTimings timings = new CallTimings();

			#region ITimingsStore Members
			public CallTimings Timings
			{
				get
				{
					return timings;
				}
			}
			#endregion
		}

		private class AspNetSession : ITimingsStore
		{
			private const string SessionKey = "WebServiceCalLimiter";

			#region ITimingsStore Members
			public CallTimings Timings
			{
				get
				{
					if (HttpContext.Current.Session == null)
					{
						throw new InvalidOperationException("The session does not exist.");
					}
					
					CallTimings timings = (CallTimings)HttpContext.Current.Session[SessionKey];
					if (timings == null)
					{
						timings = new CallTimings();
						HttpContext.Current.Session[SessionKey] = timings;
					}

					return timings;
				}
			}
			#endregion
		}

		private static readonly ITimingsStore timingsStore = InitializeTimingsStore();

		private static ITimingsStore InitializeTimingsStore()
		{
			if (HttpContext.Current != null)
			{
				return new AspNetSession();
			}
			else
			{
				return new SingleUserSession();
			}
		}

		#endregion

		private static readonly TimeSpan defaultTimeBetweenCalls = new TimeSpan(0, 0, 1);

		/// <summary>
		/// Notifies that an action is starting. If not enough time has passed since the current user performed that action, a <see cref="SecurityException"/> is thrown.
		/// </summary>
		/// <param name="action">
		///	Name of the action that is being performed. In case of a Web method, this is usually the namespace of the web service plus the name of the method.
		/// </param>
		/// <param name="minTimeBetweenCalls">The minimum time between calls.</param>
		public static void LimitActionsPerUser(string action, TimeSpan minTimeBetweenCalls)
		{
			timingsStore.Timings.LimitActionsPerUser(action, minTimeBetweenCalls);
		}

		/// <summary>
		/// Notifies that an action is starting. If not enough time has passed since the current user performed that action, a <see cref="SecurityException"/> is thrown.
		/// </summary>
		/// <param name="action">
		///	Name of the action that is being performed. In case of a Web method, this is usually the namespace of the web service plus the name of the method.
		/// </param>
		/// <remarks>
		/// Uses the default time between calls (1 second).
		/// </remarks>
		public static void LimitActionsPerUser(string action)
		{
			LimitActionsPerUser(action, defaultTimeBetweenCalls);
		}

		/// <summary>
		/// Notifies that an action is starting. If not enough time has passed since the current user performed that action, a <see cref="SecurityException"/> is thrown.
		/// </summary>
		/// <param name="minTimeBetweenCalls">The minimum time between calls.</param>
		/// <remarks>
		/// Uses the name of the calling method as the action name.
		/// </remarks>
		public static void LimitActionsPerUser(TimeSpan minTimeBetweenCalls)
		{
			StackFrame stackFrame = new StackFrame(1, false);
			MethodBase method = stackFrame.GetMethod();
			string action = string.Format(CultureInfo.InvariantCulture, "{0}.{1}", method.DeclaringType.FullName, method.Name);

			LimitActionsPerUser(action, minTimeBetweenCalls);
		}

		/// <summary>
		/// Notifies that an action is starting. If not enough time has passed since the current user performed that action, a <see cref="SecurityException"/> is thrown.
		/// </summary>
		/// <remarks>
		/// Uses the default time between calls (1 second) and the name of the calling method as the action name.
		/// </remarks>
		public static void LimitActionsPerUser()
		{
			LimitActionsPerUser(defaultTimeBetweenCalls);
		}
		#endregion
	}
}
