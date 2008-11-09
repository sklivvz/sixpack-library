// TimerSample.cs 
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

namespace SixPack.Diagnostics
{
	/// <summary>
	/// Value that can be used to calculate a time interval.
	/// </summary>
	public struct TimerSample
	{
		internal readonly long Value;

		/// <summary>
		/// Initializes a new instance of the <see cref="TimerSample"/> struct.
		/// </summary>
		/// <param name="value">The value.</param>
		public TimerSample(long value)
		{
			Value = value;
		}
	}
}
