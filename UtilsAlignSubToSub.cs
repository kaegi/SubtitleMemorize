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
			listToChange.Sort();
			referenceList.Sort();
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

			public int position = 0; // TODO: debug value

			public RemainingSlice(LinkedList<LineInfo> referenceListData, LinkedList<LineInfo> listToChangeData) {
				referenceListLines = referenceListData;
				listToChangeLines = listToChangeData;
			}
		}

		public void Retime() {

			var stackOfSubtitleListParts = new Queue<RemainingSlice>();

			// initilize first slice (with all lines)
			RemainingSlice fullSlice = new RemainingSlice(new LinkedList<LineInfo>(m_referenceList), new LinkedList<LineInfo>(m_listToChange));
			stackOfSubtitleListParts.Enqueue(fullSlice);

			while(stackOfSubtitleListParts.Count > 0) {
				RemainingSlice slice = stackOfSubtitleListParts.Dequeue();
				if(slice.listToChangeLines.Count == 0 || slice.referenceListLines.Count == 0) continue;
				HandleSlice(slice, stackOfSubtitleListParts);
			}
		}

		private void HandleSlice(RemainingSlice slice, Queue<RemainingSlice> queue) {

			double bestOffset = 0;
			double bestOffsetRating = 0;

			const double offsetPerIteration = 0.1;
			int sign = 1; // will alternate every iteration
			for(int iteration = 0; iteration < 1600; iteration++) {
				double offset = sign * (offsetPerIteration * iteration);
				sign *= -1;

				double averageRating = GetRatingOfOffset(offset, slice.referenceListLines, slice.listToChangeLines);

				// is this offset better than the current offset?
				if(averageRating >= bestOffsetRating) {
					bestOffset = offset;
					bestOffsetRating = averageRating;
				}
			}

			//Console.WriteLine(bestOffset + ", " + bestOffsetRating + "; " + slice.referenceListLines.Count + " -> " + slice.listToChangeLines.Count + "; pos: " + slice.position);
			ApplyOffset(slice, bestOffset, queue);

		}

		private double GetStartTime(SubtitleMatcher.BiMatchedLines biMatchedLines) {
			double startTime = 0;
			bool initialized = false;
			foreach (var lineList in biMatchedLines.listlines)
				foreach (LineInfo line in lineList)
					if (line.StartTime < startTime || !initialized) {
						startTime = line.StartTime;
						initialized = true;
					}

			return startTime;
		}

		private void ApplyOffset(RemainingSlice slice, double offset, Queue<RemainingSlice> queue) {
			UtilsSubtitle.ShiftByTime(slice.listToChangeLines, offset);

			// match lines
			var biMatchedLinesLinkedList = SubtitleMatcher.MatchSubtitles(slice.referenceListLines, slice.listToChangeLines);
			var biMatchedLinesList = new List<SubtitleMatcher.BiMatchedLines>(biMatchedLinesLinkedList);
			biMatchedLinesList.Sort(delegate(SubtitleMatcher.BiMatchedLines x,SubtitleMatcher.BiMatchedLines y) {
				return GetStartTime(x) < GetStartTime(y) ? -1 : 1;
			});


			// find average rating TODO: value is not perfect for this task (should be higher)
			double averageRating = 0;
			int numRatings = 0;
			foreach(var biMatchedLines in biMatchedLinesList) {
				double rating = RateBiMatchedLines(biMatchedLines);
				if(rating > 0.0001) {
					averageRating += rating;
					numRatings++;
				}
			}
			averageRating /= numRatings;

			double thresholdValue = averageRating * 0.5;

			// find longest row over average value
			int numGoodMatched = 0;
			int currentRowStart = 0;

			int maxNumGoodMatched = -1;
			int bestRowStart = 0;
			for(int index = 0; index < biMatchedLinesList.Count; index++) {
				var biMatchedLines = biMatchedLinesList[index];
				double rating = RateBiMatchedLines(biMatchedLines);

				if(rating < thresholdValue) {
					if(rating > 0.000001)
						numGoodMatched = 0; // not a zero rating
				} else {
					// update row start/end
					if(numGoodMatched == 0) currentRowStart = index;
					numGoodMatched = index - currentRowStart + 1;

					// save best value
					if(numGoodMatched > maxNumGoodMatched) {
						maxNumGoodMatched = numGoodMatched;
						bestRowStart = currentRowStart;
					}
				}
			}


			if(maxNumGoodMatched == -1) return;

			{
				// everything in front of the "good row" can be matched
				var remainingSliceReferenceList = new LinkedList<LineInfo>();
				var remainingSliceToChangeList = new LinkedList<LineInfo>();
				for(int index = 0; index < bestRowStart; index++) {
					var biMatchedLines = biMatchedLinesList[index];
					foreach(var lineInfo in biMatchedLines.listlines[0]) remainingSliceReferenceList.AddLast(lineInfo);
					foreach(var lineInfo in biMatchedLines.listlines[1]) remainingSliceToChangeList.AddLast(lineInfo);
				}
				UtilsSubtitle.ShiftByTime(remainingSliceToChangeList, -offset); // correct offsets back
				RemainingSlice sliceLeft = new RemainingSlice(remainingSliceReferenceList, remainingSliceToChangeList);
				sliceLeft.position = slice.position;
				queue.Enqueue(sliceLeft);
			}

			{
				// everything after the "good row" has to matched
				var remainingSliceReferenceList = new LinkedList<LineInfo>();
				var remainingSliceToChangeList = new LinkedList<LineInfo>();
				for(int index = bestRowStart + maxNumGoodMatched + 1; index < biMatchedLinesList.Count; index++) {
					var biMatchedLines = biMatchedLinesList[index];
					foreach(var lineInfo in biMatchedLines.listlines[0]) remainingSliceReferenceList.AddLast(lineInfo);
					foreach(var lineInfo in biMatchedLines.listlines[1]) remainingSliceToChangeList.AddLast(lineInfo);
				}
				UtilsSubtitle.ShiftByTime(remainingSliceToChangeList, -offset); // correct offsets back
				RemainingSlice sliceRight = new RemainingSlice(remainingSliceReferenceList, remainingSliceToChangeList);
				sliceRight.position = slice.position + bestRowStart + maxNumGoodMatched + 1;
				queue.Enqueue(sliceRight);
			}
		}


		/// <summary>
		/// Shift all lines in "list to change" by "offset", create a matching, rate the matching and shift lines back.
		/// </summary>
		public double GetRatingOfOffset(double offset, IEnumerable<LineInfo> referenceList, IEnumerable<LineInfo> listToChange) {
			/*
			 * Other ideas for rating:
			 * 		bonus value for censecutive good bi-match ratings
			 */

			// move timings of every line
			UtilsSubtitle.ShiftByTime(listToChange, offset);

			// match lines
			var biMatchedLinesList = SubtitleMatcher.MatchSubtitles(referenceList, listToChange);
			double averageRating = 0;
			int numRatings = 1;

			foreach(var biMatchedLines in biMatchedLinesList) {
				double rating = RateBiMatchedLines(biMatchedLines);

				averageRating += rating * rating * rating;
			}

			averageRating /= numRatings;

			// shift timings back
			UtilsSubtitle.ShiftByTime(listToChange, -offset);

			return averageRating;
		}

		/// <summary>
		/// Returns a value between 0 and 1 how good the matching is.
		/// </summary>
		public double RateBiMatchedLines(SubtitleMatcher.BiMatchedLines biMatchedLines) {
			// [shared times between lines from list1 and list2] divided by [whole time]
			// ...because this value is between 0 and 0.5 we have to multiply it with 2

			// calculate "all time"
			double sharedTime = 0;
			double allTimeSpanTime = 0;
			for(int listIndex = 0; listIndex < 2; listIndex++) {
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
