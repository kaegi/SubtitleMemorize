// Copyright (C) 2016    Chang Spivey
//
// This program is free software; you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation; either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program; if not, write to the Free Software Foundation,
// Inc., 51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA
//

using System;

namespace subs2srs4linux
{
	public class LineInfo : IComparable<ITimeSpan>, ITimeSpan
	{
		public double startTime;
		public double endTime;
		public String name;
		public String text;

		public LineInfo (double startTime, double endTime, String name, String text)
		{
			this.startTime = startTime;
			this.endTime = endTime;
			this.name = name;
			this.text = text;
		}

		/// <summary>
		/// Compare lines based on their Start Times.
		/// </summary>
		public int CompareTo(ITimeSpan other) {
			if(StartTime == other.StartTime) return 0;
			return StartTime < other.StartTime ? -1 : 1;
		}

		public double StartTime {
			get { return startTime; }
		}

		public double EndTime {
			get { return endTime; }
		}
	}
}
