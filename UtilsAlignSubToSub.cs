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
using System.Collections.Generic;

namespace subs2srs4linux
{
	/// <summary>
	/// This class provides a way of retiming every single line in
	/// a list to another line list (of a different subtitle file)
	/// solely based on comparing their timestamps.
	///
	/// Commercial breaks are supported as well.
	/// </summary>
	public class UtilsAlignSubToSub
	{
		private List<LineInfo> m_listToChange;
		private List<LineInfo> m_referenceList;

		public UtilsAlignSubToSub (List<LineInfo> listToChange, List<LineInfo> referenceList)
		{
			m_listToChange = listToChange;
			m_referenceList = referenceList;
		}

		/// <summary>
		/// All lines are in the two list that will get split multiple times.
		/// This slice represents one of these splits (it is a subset of the two lists).
		/// Only lines in same slices can get matched.
		/// </summary>
		private class RemainingSlice {
			public LinkedList<LineInfo> referenceListLines;
			public LinkedList<LineInfo> listToChangeLines;

			public RemainingSlice(IEnumerable<LineInfo> referenceListData, IEnumerable<LineInfo> listToChangeData) {
				referenceListLines = new LinkedList<LineInfo>(referenceListData);
				listToChangeData = new LinkedList<LineInfo>(listToChangeData);
			}
		}

		public void Retime() {

			double bestOffset = 0;
			double bestOffsetRating = 0;

			var stackOfSubtitleListParts = new Queue<RemainingSlice>();

			// initilize first slice (with all lines)
			{
				RemainingSlice slice = new RemainingSlice(m_referenceList, m_listToChange);
				stackOfSubtitleListParts.Enqueue(slice);
			}

			while(stackOfSubtitleListParts.Count > 0) {
				RemainingSlice slice = stackOfSubtitleListParts.Dequeue();

			}


			const double offsetPerIteration = 0.05;
			int sign = 1; // will alternate every iteration
			for(int iteration = 0; iteration < 80; iteration++) {
				double offset = sign * (offsetPerIteration * iteration);
				sign *= -1;

				double averageRating = GetRatingOfOffset(offset);

				// is this offset better than the last?
				if(averageRating > bestOffsetRating) {
					bestOffset = offset;
					bestOffsetRating = averageRating;
					Console.WriteLine(offset + ", " + averageRating);
				}
			}

			UtilsSubtitle.ShiftByTime(m_listToChange, bestOffset);
		}


		/// <summary>
		/// Shift all lines in "list to change" by "offset", create a matching, rate the matching and shift lines back.
		/// </summary>
		public double GetRatingOfOffset(double offset) {

			// move timings of every line
			UtilsSubtitle.ShiftByTime(m_listToChange, offset);

			// match lines
			var bothLists = new List<LineInfo>[]{ m_referenceList, m_listToChange };
			var biMatchedLinesList = SubtitleMatcher.MatchSubtitles(m_referenceList, m_listToChange);
			double averageRating = 0;
			foreach(var biMatchedLines in biMatchedLinesList) {
				averageRating += RateBiMatchedLines(biMatchedLines, bothLists);
			}
			averageRating /= biMatchedLinesList.Count;

			// shift timings back
			UtilsSubtitle.ShiftByTime(m_listToChange, -offset);

			return averageRating;
		}

		/// <summary>
		/// Returns a value between 0 and 1 how good the matching is.
		/// </summary>
		public double RateBiMatchedLines(SubtitleMatcher.BiMatchedLines biMatchedLines, List<LineInfo>[] lists) {
			// [shared times between lines from list1 and list2] divided by [whole time]
			// ...because this value is between 0 and 0.5 we have to multiply it with 2

			// calculate "all time"
			double sharedTime = 0;
			double allTimeSpanTime = 0;
			for(int listIndex = 0; listIndex < 2; listIndex++) {
				List<LineInfo> currentList = lists[listIndex];

				foreach (var currentLine in biMatchedLines.listlines[listIndex]) {
					allTimeSpanTime += UtilsCommon.GetTimeSpanLength(currentLine);
				}
			}

			// calculate "shared time"
			foreach(var lineInfoA in biMatchedLines.listlines[0]) {
				foreach (var lineInfoB in biMatchedLines.listlines[1]) {
					sharedTime += UtilsCommon.OverlappingTimeSpan (lineInfoA, lineInfoB);
				}
			}

			return (sharedTime * 2) / allTimeSpanTime;
		}

	}
}

