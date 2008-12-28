// HighResolutionTimer.cs 
//
//  Copyright (C) 2008 Fullsix Marketing Interactivo LDA
//  Author: Marco Cecconi
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
using System.ComponentModel;

namespace SixPack.Diagnostics
{
	/// <summary>
	/// High resolution timer class
	/// </summary>
	public class HighResolutionTimer
	{
		private static readonly double scaleFactor = CalculateScaleFactor();
		private const double Multiplier = 1000000000.0;
		private long start;
		private long stop;

		private const double TimeToMillisecondsScale = 1000.0 / Multiplier;
		private const double TimeToSecondsScale = 1.0 / Multiplier;
		private const double TimeToTimeSpanScale = 10000000.0 / Multiplier;

		private static double CalculateScaleFactor()
		{
			long frequency;
			if (SafeNativeMethods.QueryPerformanceFrequency(out frequency) == false)
			{
				// Frequency not supported
				throw new Win32Exception();
			}

			return Multiplier / frequency;
		}

		/// <summary>
		/// Returns a value that can be used to calculate the time interval between two samples.
		/// </summary>
		public static TimerSample Sample()
		{
			long sample;
			SafeNativeMethods.QueryPerformanceCounter(out sample);
			return new TimerSample(sample);
		}

		/// <summary>
		/// Gets the time elapsed between <paramref name="startTime"/> and <paramref name="endTime" />.
		/// </summary>
		/// <param name="startTime">The start time.</param>
		/// <param name="endTime">The end time.</param>
		/// <returns></returns>
		/// <remarks>
		/// This method is safe for using in a multithreaded environment.
		/// </remarks>
		public static double Duration(TimerSample startTime, TimerSample endTime)
		{
			return Duration(startTime.Value, endTime.Value);
		}

		/// <summary>
		/// Starts the timer.
		/// </summary>
		public void Start()
		{
			SafeNativeMethods.QueryPerformanceCounter(out start);
		}

		/// <summary>
		/// Stops the timer.
		/// </summary>
		public void Stop()
		{
			SafeNativeMethods.QueryPerformanceCounter(out stop);
		}

		/// <summary>
		/// Returns the elapsed duration given the number of iterations
		/// </summary>
		/// <param name="iterations">The iterations.</param>
		/// <returns></returns>
		public double Duration(int iterations)
		{
			return Duration(start, stop) / iterations;
		}

		private static double Duration(long startTime, long endTime)
		{
			return (endTime - startTime) * scaleFactor;
		}

		/// <summary>
		/// Converts the specified time to seconds.
		/// </summary>
		/// <param name="time">The time.</param>
		/// <returns></returns>
		public static double TimeToSeconds(double time)
		{
			return time * TimeToSecondsScale;
		}

		/// <summary>
		/// Converts the specified time to milliseconds.
		/// </summary>
		/// <param name="time">The time.</param>
		/// <returns></returns>
		public static double TimeToMilliseconds(double time)
		{
			return time * TimeToMillisecondsScale;
		}

		/// <summary>
		/// Converts the specified time to <see cref="TimeSpan" />.
		/// </summary>
		/// <param name="time">The time.</param>
		/// <returns></returns>
		public static TimeSpan TimeToTimeSpan(double time)
		{
			return new TimeSpan((long)(time * TimeToTimeSpanScale));
		}
	}
}
