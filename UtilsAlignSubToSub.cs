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

namespace subtitleMemorize
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

			public RemainingSlice(LinkedList<LineInfo> referenceListData, LinkedList<LineInfo> listToChangeData) {
				referenceListLines = referenceListData;
				listToChangeLines = listToChangeData;
			}
		}

		/// <summary>
		/// By calling this function, all references to LineInfo in "listToChange" passed in
		/// constructor can be changed. Retiming includes removing/introducing commercial breaks.
		///
		/// By using the "noSplitting=true" option, the whole list gets shifted as one.
		/// </summary>
		public void Retime(bool noSplitting=false) {

			var queue = new Queue<RemainingSlice>();

			// initilize first slice (with all lines)
			RemainingSlice fullSlice = new RemainingSlice(new LinkedList<LineInfo>(m_referenceList), new LinkedList<LineInfo>(m_listToChange));
			queue.Enqueue(fullSlice);

			// just find best offset
			if(noSplitting) {
				double bestOffset = FindBestOffset(fullSlice);
				UtilsSubtitle.ShiftByTime(m_listToChange, bestOffset);
				return;
			}


			// Only lines that are in the same slice (list of lines on both sides) can be matched.
			// When processing the slices, a consecutive row of lines is recognized as "good". Two new
			// slices (one left of the right of the row) are then created.
			//
			// This queue will process all these slices that have some elements in them.
			while(queue.Count > 0) {
				RemainingSlice slice = queue.Dequeue();
				//if(slice.listToChangeLines.Count == 0 || slice.referenceListLines.Count == 0) {
				//	// T O D O: these shouldn't just be moved out of the way but left in another slice
				//	// this is more debug code than anything else
				//	UtilsSubtitle.ShiftByTime(slice.listToChangeLines, 10000);
				//	UtilsSubtitle.ShiftByTime(slice.referenceListLines, 10000);
				//	continue;
				//}

				double bestOffset = FindBestOffset(slice);
				ApplyOffset(slice, bestOffset, queue);
			}
		}

		/// <summary>
		/// Find best offset for "listToChange" so it matches "referenceList". There can be
		/// multiple "peaks" if one subtitle has additional breaks the other does not have.
		/// Only one of them will be returned.
		/// </summary>
		private double FindBestOffset(RemainingSlice slice) {

			// the tuple is (offset, rating)
			var firstPassList = new List<OffsetRatingTuple>(5);
			FindGoodOffsets(slice, 0, 0.3, 1000, firstPassList);

			// find fine grained offsets around approximated offsets
			var secondPassList = new List<OffsetRatingTuple>(5);
			foreach(var offsetRatingTuple in firstPassList)
				FindGoodOffsets(slice, offsetRatingTuple.offset, 0.01, 90, secondPassList);

			return secondPassList[0].offset;
		}

		private class OffsetRatingTuple : IComparable<OffsetRatingTuple> {
			public double offset;
			public double rating;

			public OffsetRatingTuple(double offset, double rating) {
				this.offset = offset;
				this.rating = rating;
			}

			public int CompareTo(OffsetRatingTuple x) {
				return rating < x.rating ? 1 : -1;
			}
		}

		/// <summary>
		/// This function test "iterations"-times different offsets and saves the best in "returnList".
		/// The offsets will be around "centerOffset" and "2 * stepSize" is the  in every direction.
		/// The returnList will be filled until "Count==Capacity". At every time, "returnList" is sorted
		/// by rating ("returnList[0]" has the best rating of all tested offsets).
		/// </summary>
		private void FindGoodOffsets(RemainingSlice slice, double centerOffset, double stepSize, int iterations, List<OffsetRatingTuple> returnList) {

			var subtitleMatcherParams = SubtitleMatcher.GetParameterCache(slice.referenceListLines, slice.listToChangeLines);

			int sign = 1; // will alternate every iteration
			for(int iteration = 0; iteration < iterations; iteration++) {
				double offset = sign * (stepSize * iteration) + centerOffset;
				sign *= -1;

				double averageRating = GetRatingOfOffset(offset, subtitleMatcherParams);

				if(returnList.Count < returnList.Capacity) {
					returnList.Add(new OffsetRatingTuple(offset, averageRating));

					// if all entries are filled they have to be sorted by how good they are
					// (at index 0 the rating is best)
					if(returnList.Count == returnList.Capacity)
						returnList.Sort();
				} else {
					// is value better than worst in return list?
					if(averageRating > returnList[returnList.Count - 1].rating) {
						returnList[returnList.Count - 1].offset = offset;
						returnList[returnList.Count - 1].rating = averageRating;

						// bubble sort in sorted list
						for(int i = returnList.Count - 1; i >= 1; i--) {
							// move value up (rating at lower index is higher) or end loop?
							if(returnList[i].rating < returnList[i - 1].rating) break;

							var tmp = returnList[i];
							returnList[i] = returnList[i - 1];
							returnList[i - 1] = tmp;
						}
					}
				}
			}
		}

		/// <summary>
		/// Returns the minimum time of all lines in BiMatchedLines.
		/// XXX: move this to BiMatchedLines?
		/// </summary>
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

		/// <summary>
		/// This function will...
		/// a) move all lines to change in slice by offset
		/// b) match the lines in "listToChange" and "referenceList"
		/// c) find a threshold value for matchings
		/// d) find the longest row of consecutive matchings that are above the threshold
		/// e) create slices for lines before and lines after this "good row"
		/// f) reset offset of these remaining lines and put slice into queue
		/// </summary>
		private void ApplyOffset(RemainingSlice slice, double offset, Queue<RemainingSlice> queue) {
			UtilsSubtitle.ShiftByTime(slice.listToChangeLines, offset);

			// match lines
			var subtitleMatcherParameters = SubtitleMatcher.GetParameterCache(slice.referenceListLines, slice.listToChangeLines);
			var biMatchedLinesLinkedList = SubtitleMatcher.MatchSubtitles(subtitleMatcherParameters);
			var biMatchedLinesList = new List<SubtitleMatcher.BiMatchedLines>(biMatchedLinesLinkedList);
			biMatchedLinesList.Sort(delegate(SubtitleMatcher.BiMatchedLines x,SubtitleMatcher.BiMatchedLines y) {
				return GetStartTime(x) < GetStartTime(y) ? -1 : 1;
			});


			// --------------------------------------------------
			// find threshold rating
			double averageRating = 0;
			int numRatings = 0;
			foreach(var biMatchedLines in biMatchedLinesList) {
				double rating = RateBiMatchedLines(biMatchedLines);
				if(rating > 0.0001) { // ignore "zero" ratings
					averageRating += rating;
					numRatings++;
				}
			}
			averageRating /= numRatings;
			double thresholdValue = averageRating * 0.8;

			// --------------------------------------------------
			// Find longest row over threshold rating.
			//
			// Zero ratings may be inbetween good ratings when some
			// lines couldn't get matched (for example street-sign
			// translations that aren't in subtitle file in native language).
			// These are stepped over: There can be a limited number of zero ratings
			// ratings in the row except at the beginning and the end (these will
			// get matched at a different time if possible).
			int numGoodMatched = 0;
			int currentRowStart = 0;
			int allowedZeroRatings = 0;


			int maxNumGoodMatched = -1;
			int bestRowStart = 0;
			for(int index = 0; index < biMatchedLinesList.Count; index++) {
				var biMatchedLines = biMatchedLinesList[index];
				double rating = RateBiMatchedLines(biMatchedLines);

				if(rating < thresholdValue) {
					// step over zero ratings
					if(rating > 0.000001 || allowedZeroRatings-- < 0)
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

					allowedZeroRatings = 4;
				}
			}

			// could good row be found?
			if(maxNumGoodMatched == -1) return;

			// create new slices left and right
			RemainingSlice newSlice;

			// left slice
			newSlice = GetSubSlice(biMatchedLinesList, 0, bestRowStart);
			//UtilsSubtitle.ShiftByTime(newSlice.listToChangeLines, -offset); // by disabling this code, remaining slices are now embedded in greater slices
			queue.Enqueue(newSlice);

			// right slice
			newSlice = GetSubSlice(biMatchedLinesList, bestRowStart + maxNumGoodMatched + 1, biMatchedLinesList.Count);
			//UtilsSubtitle.ShiftByTime(newSlice.listToChangeLines, -offset);
			queue.Enqueue(newSlice);
		}

		/// <summary>
		/// Generate a slice from a part of matched lines list.
		/// </summary>
		private RemainingSlice GetSubSlice(List<SubtitleMatcher.BiMatchedLines> biMatchedLinesList, int start, int end) {

			// everything after the "good row" has be to matched
			var remainingSliceReferenceList = new LinkedList<LineInfo>();
			var remainingSliceToChangeList = new LinkedList<LineInfo>();
			for(int index = start; index < end; index++) {
				var biMatchedLines = biMatchedLinesList[index];
				foreach(var lineInfo in biMatchedLines.listlines[0]) remainingSliceReferenceList.AddLast(lineInfo);
				foreach(var lineInfo in biMatchedLines.listlines[1]) remainingSliceToChangeList.AddLast(lineInfo);
			}
			return new RemainingSlice(remainingSliceReferenceList, remainingSliceToChangeList);
		}


		/// <summary>
		/// Shift all lines in "list to change" by "offset", create a matching, rate the matching and shift lines back.
		/// </summary>
		public double GetRatingOfOffset(double offset, SubtitleMatcher.SubtitleMatcherCache subtitleMatcherParams) {
			/*
			 * Other ideas for rating:
			 * 		bonus value for censecutive good bi-match ratings
			 */

			// move timings of every line
			subtitleMatcherParams.ShiftTime(offset, false, true);

			// match lines
			var biMatchedLinesList = SubtitleMatcher.MatchSubtitles(subtitleMatcherParams);
			double finalRating = 0;

			foreach(var biMatchedLines in biMatchedLinesList) {
				double rating = RateBiMatchedLines(biMatchedLines);

				// use rating^3 because that way small values will become smaller
				finalRating += rating * rating * rating;
			}

			// shift timings back
			subtitleMatcherParams.ShiftTime(-offset, false, true);

			return finalRating;
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
