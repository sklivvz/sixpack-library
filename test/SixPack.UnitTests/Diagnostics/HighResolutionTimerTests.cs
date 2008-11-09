// HighResolutionTimerTests.cs 
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
using MbUnit.Framework;
using SixPack.Diagnostics;

namespace SixPack.UnitTests.Diagnostics
{
	[TestFixture]
	public class HighResolutionTimerTests
	{
		private const double MeasurementTime = 500.0;
		private const double Variation = 5.0 / 100.0;
		private const double TimeToMilliseconds = 1.0 / 1000000.0;
		private const double TimeToSeconds = 1.0 / 1000000000.0;

		private static double Wait()
		{
			DateTime startTime = DateTime.Now;
			double elapsedTime;
			do
			{
				elapsedTime = DateTime.Now.Subtract(startTime).TotalMilliseconds;
			} while (elapsedTime < MeasurementTime);
			return elapsedTime;
		}

		[Test]
		public void StartStop()
		{
			HighResolutionTimer timer = new HighResolutionTimer();

			timer.Start();
			double elapsedTime = Wait();
			timer.Stop();

			double duration = timer.Duration(1) * TimeToMilliseconds;

			Console.WriteLine("DateTime time: {0:N3}, Timer time: {1:N3}", elapsedTime, duration);

			Assert.Between(
				duration,
				elapsedTime * (1.0 - Variation),
				elapsedTime * (1.0 + Variation)
			);
		}

		[Test]
		public void MultipleSamples()
		{
			HighResolutionTimer timer = new HighResolutionTimer();

			TimerSample sample1 = HighResolutionTimer.Sample();
			double elapsedTime1 = Wait();
			TimerSample sample2 = HighResolutionTimer.Sample();
			double elapsedTime2 = Wait();
			TimerSample sample3 = HighResolutionTimer.Sample();

			double duration1 = HighResolutionTimer.Duration(sample1, sample2) * TimeToMilliseconds;
			Console.WriteLine("1 -> DateTime time: {0:N3}, Timer time: {1:N3}", elapsedTime1, duration1);

			Assert.Between(
				duration1,
				elapsedTime1 * (1.0 - Variation),
				elapsedTime1 * (1.0 + Variation)
			);

			double duration2 = HighResolutionTimer.Duration(sample2, sample3) * TimeToMilliseconds;
			Console.WriteLine("2 -> DateTime time: {0:N3}, Timer time: {1:N3}", elapsedTime2, duration2);

			Assert.Between(
				duration2,
				elapsedTime1 * (1.0 - Variation),
				elapsedTime1 * (1.0 + Variation)
			);
		}

		[Test]
		public void ConvertTime()
		{
			HighResolutionTimer timer = new HighResolutionTimer();

			timer.Start();
			Wait();
			timer.Stop();

			double duration = timer.Duration(1);
			Assert.Between(
				HighResolutionTimer.TimeToMilliseconds(duration),
				duration * TimeToMilliseconds * (1.0 - Variation),
				duration * TimeToMilliseconds * (1.0 + Variation)
			);
			Assert.Between(
				HighResolutionTimer.TimeToSeconds(duration),
				duration * TimeToSeconds * (1.0 - Variation),
				duration * TimeToSeconds * (1.0 + Variation)
			);
			Assert.Between(
				HighResolutionTimer.TimeToTimeSpan(duration).TotalSeconds,
				duration * TimeToSeconds * (1.0 - Variation),
				duration * TimeToSeconds * (1.0 + Variation)
			);
		}

		[Test]
		public void MeasureCallTime()
		{
			// Warm up
			HighResolutionTimer.Sample();

			TimerSample start = HighResolutionTimer.Sample();
			TimerSample end = HighResolutionTimer.Sample();
			Console.WriteLine(HighResolutionTimer.Duration(start, end));

			HighResolutionTimer timer = new HighResolutionTimer();
			timer.Start();
			timer.Stop();
			Console.WriteLine(timer.Duration(1));
		}
	}
}
