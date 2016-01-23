using System;

namespace subs2srs4linux
{
	public class LineInfo : IComparable<LineInfo>
	{
		public DateTime endTime;
		public DateTime startTime;
		public String name;
		public String text;

		public LineInfo (DateTime startTime, DateTime endTime, String name, String text)
		{
			this.startTime = startTime;
			this.endTime = endTime;
			this.name = name;
			this.text = text;
		}

		/// <summary>
		/// Compare lines based on their Start Times.
		/// </summary>
		public int CompareTo(LineInfo other)
		{
			return startTime < other.startTime ? -1 : 1;
		}

	}
}

