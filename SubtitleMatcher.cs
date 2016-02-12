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
using System.Text;

namespace subs2srs4linux
{
	public static class SubtitleMatcher
	{
		private class ExtendedLineInfo
		{
			public readonly LineInfo lineInfo;
			public readonly List<Int32> matchingLines = new List<Int32>(); // matching lines in subtitle of 
			public bool alreadyUsedInBidirectionalSearch = false;

			public ExtendedLineInfo(LineInfo li) {
				lineInfo = li;
			}
		}

		/// <summary>
		/// Indices refer to lines in two list. This represents
		/// a many-to-many (N:M) relationship of this list.
		/// </summary>
		public class BiMatchedLines
		{
			public readonly List<Int32>[] listlines = { new List<int>(), new List<int>() };
		}

		/// <summary>
		/// Matches the two subtitles. The problem is that no two subtiles have the same timestamps or the same number of lines (they can be split, omitted, ...).
		/// This algotithm tries to find matching lines nontheless. The general algorithm works as following:
		/// 
		/// Compare every "line1" in list1 with every "line2" in list2. Generate a score (between 0 and 1) how good they overlap. If the score is above a certain threshold, there
		/// will be an edge (as in graph theory) beween line1 and line2. There result is a bipartite graph that has many connected compontens. Every connected component is then a
		/// mached line.
		/// 
		/// There is another step that removes matchings where two lines in same list overlap. See documentation of "RemoveOverlappings()" for more information.
		/// 
		/// </summary>
		/// <returns>The subtitles.</returns>
		/// <param name="lines1">Lines1.</param>
		/// <param name="lines2">Lines2.</param>
		public static List<BiMatchedLines> MatchSubtitles(List<LineInfo> lines1, List<LineInfo> lines2) {
			
			lines1.Sort (delegate(LineInfo x, LineInfo y) {
				if(x.startTime < y.startTime) return -1;
				else if(x.startTime.Equals(y.startTime)) return 0;
				else return 1;
			});

			lines2.Sort (delegate(LineInfo x, LineInfo y) {
				if(x.startTime < y.startTime) return -1;
				else if(x.startTime.Equals(y.startTime)) return 0;
				else return 1;
			});

			// find matching for lines1 and then for lines2
			List<ExtendedLineInfo> mappingForLines1 = FindMatching (lines1, lines2);
			List<ExtendedLineInfo> mappingForLines2 = FindMatching (lines2, lines1);

			RemoveOverlappings (mappingForLines1, mappingForLines2);
			RemoveOverlappings (mappingForLines2, mappingForLines1);

			List<BiMatchedLines> finalMappings = FindBidirectionalMapping (mappingForLines1, mappingForLines2);

			return finalMappings;
		}

		/// <summary>
		/// This creates a list of good matching subtitles from list2 to every element from list1.
		/// </summary>
		/// <returns>The returned has list1.Count Elements. Every element (with position "i") is a list of numbers indicating the matching lines in list2 to the i-th line in list1.</returns>
		/// <param name="list1">List1.</param>
		/// <param name="list2">List2.</param>
		private static List<ExtendedLineInfo> FindMatching(List<LineInfo> list1, List<LineInfo> list2) {
			// TODO: this has O(n²) time -> optimize
			List<ExtendedLineInfo> matchings = new List<ExtendedLineInfo>(list1.Count);
			foreach (LineInfo list1line in list1) {
				ExtendedLineInfo thisLineInfo = new ExtendedLineInfo (list1line);

				for (int list2Index = 0; list2Index < list2.Count; list2Index++) {
					LineInfo list2line = list2[list2Index]; 

					if (OverlappingScore(list1line, list2line) < InstanceSettings.systemSettings.overlappingThreshold_InterSub)
						continue;
					
					thisLineInfo.matchingLines.Add (list2Index);
				}

				matchings.Add (thisLineInfo);
			}

			return matchings;
		}

		/// <summary>
		/// Removes the overlappings in one list.
		/// 
		/// Example:
		/// 
		/// sub1 is in list1
		/// sub2 and sub3 are in list2
		/// 
		/// sub1 overlaps with sub2, sub3
		///  + sub2 and sub3 overlap
		/// 
		/// An example of this in the real worlds are announcements while the main characters are speaking.
		/// We obviously only want to map the character-dialog and the announcement to its own translation.
		/// 
		/// </summary>
		static void RemoveOverlappings (List<ExtendedLineInfo> primaryList, List<ExtendedLineInfo> secondaryList)
		{
			foreach (ExtendedLineInfo eli in primaryList) {
				bool[] isMatchingLineOverlapping = null;
				while ((isMatchingLineOverlapping = GetInListOverlapping (eli.matchingLines, secondaryList)) != null) {
					DeleteLeastFittingOverlappingSubtitle (eli.lineInfo, eli.matchingLines, isMatchingLineOverlapping, secondaryList);
				}
			}
		}

		/// <summary>
		/// Gets an array of lines in "list" which overlap with another of "matchingLines" in this lines.
		/// </summary>
		/// <returns>null when there are no overlappings</returns>
		/// <param name="matchingLines">Matching lines.</param>
		/// <param name="list">List.</param>
		private static bool[] GetInListOverlapping (List<int> matchingLines, List<ExtendedLineInfo> list)
		{
			bool[] returnArray = null;
			for (int indexA = 0; indexA < matchingLines.Count; indexA++) {
				for (int indexB = indexA + 1; indexB < matchingLines.Count; indexB++) {
					if (OverlappingScore (list [matchingLines [indexA]].lineInfo, list [matchingLines [indexB]].lineInfo) > InstanceSettings.systemSettings.overlappingThreshold_InSub) {
						returnArray = returnArray ?? new bool[matchingLines.Count];
						returnArray [indexA] = true;
						returnArray [indexB] = true;
					}
				}
			}

			return returnArray;
		}

		/// <summary>
		/// Delete the entry from "matchingLines" that overlaps with in-subtitle-list-lines and
		/// has the least overlapping score with "lineInfo".
		/// </summary>
		private static void DeleteLeastFittingOverlappingSubtitle (LineInfo lineInfo, List<Int32> matchingLines, bool[] isOverlapping, List<ExtendedLineInfo> referencedList)
		{
			float leastFittingOverlappingScore =  0.5f;
			int leastFittingIndex = -1;
			for (int index = 0; index < matchingLines.Count; index++) {
				if (!isOverlapping [index])
					continue;

				float currentOverlappingScore = OverlappingScore (lineInfo, referencedList [matchingLines [index]].lineInfo);
				if (leastFittingIndex == -1 || (currentOverlappingScore < leastFittingOverlappingScore)) {
					leastFittingOverlappingScore = currentOverlappingScore;
					leastFittingIndex = index;
				}
			}

			// remove that choosen line from the matching
			matchingLines.RemoveAt (leastFittingIndex);
		}

		/// <summary>
		/// Score for overlapping of two subtitles between 0 and 1.
		/// 
		/// Corner cases:
		/// 	subtitles do not overlap -> 0
		/// 	subtitles fully overlap -> 1
		/// </summary>
		/// <returns>The score.</returns>
		/// <param name="a">The alpha component.</param>
		/// <param name="b">The blue component.</param>
		private static float OverlappingScore(LineInfo a, LineInfo b) {
			TimeSpan overlappingSpan = OverlappingTimeSpan (a, b);
			TimeSpan line1timeSpan = a.endTime - a.startTime;
			TimeSpan line2timeSpan = b.endTime - b.startTime;

			// ignore matchings if there is next to no overlapping 
			float line1score = (float)overlappingSpan.Ticks / line1timeSpan.Ticks;
			float line2score = (float)overlappingSpan.Ticks / line2timeSpan.Ticks;

			return (line1score + line2score) * 0.5f;
		}

		private static TimeSpan OverlappingTimeSpan(LineInfo a, LineInfo b) {
			if (!UtilsCommon.IsOverlapping (a, b))
				return new TimeSpan();

			//  |-----------------------|  a
			//    |-------------------|    b
			if(a.startTime <= b.startTime && a.endTime >= b.endTime) return b.endTime - b.startTime;


			//    |-------------------|    a
			//  |-----------------------|  b
			if(a.startTime >= b.startTime && a.endTime <= b.endTime) return a.endTime - a.startTime;

			// |-------------------|    	a
			//   |-----------------------|  b
			if(a.startTime <= b.startTime && a.endTime <= b.endTime) return a.endTime - b.startTime;

			//   |-----------------------|  a
			// |-------------------|    	b
			return b.endTime - a.startTime;
		}

		private static List<BiMatchedLines> FindBidirectionalMapping (List<ExtendedLineInfo> mappingForLines1, List<ExtendedLineInfo> mappingForLines2)
		{
			Func<Int32, Boolean, BiMatchedLines> bfsSearch = delegate(Int32 lineIndex, Boolean isInFirstList) {
				

				// first value in tuple: the index of the line in respective list; second value: true -> list1, false -> list2
				LinkedList<Tuple<Int32, Boolean>> bfsList = new LinkedList<Tuple<int, bool>> ();
				BiMatchedLines bimatchedLines = new BiMatchedLines ();

				// all matching lines that are meant to be together are a connected component in the bipartite graph of lines in list1 and list2 -> find
				// these with breadth-first search
				bfsList.AddLast(new Tuple<int, bool>(lineIndex, isInFirstList));
				while (bfsList.Count > 0) {
					Tuple<Int32, Boolean> currentTuple = bfsList.First.Value;
					ExtendedLineInfo currentLineInfo = currentTuple.Item2 ? mappingForLines1 [currentTuple.Item1] : mappingForLines2 [currentTuple.Item1];

					// remove first value
					bfsList.RemoveFirst();

					// this value was already handled so it can be skipped
					if(currentLineInfo.alreadyUsedInBidirectionalSearch)
						continue;
					currentLineInfo.alreadyUsedInBidirectionalSearch = true;

					bimatchedLines.listlines[currentTuple.Item2 ? 0 : 1].Add(currentTuple.Item1);

					foreach (Int32 oppositeLineMatching in currentLineInfo.matchingLines)
						bfsList.AddLast (new Tuple<int, bool> (oppositeLineMatching, !currentTuple.Item2)); // this line is in the opposite list
				}

				return (bimatchedLines.listlines[0].Count > 0 || bimatchedLines.listlines[1].Count > 0) ? bimatchedLines : null;
			};

			List<BiMatchedLines> returnList = new List<BiMatchedLines> ();
			List<ExtendedLineInfo>.Enumerator mappingForLinesEnum;

			// handle all lines in list1
			mappingForLinesEnum = mappingForLines1.GetEnumerator ();
			for (int index = 0; mappingForLinesEnum.MoveNext (); index++) {
				BiMatchedLines bml = bfsSearch (index, true);
				if (bml != null)
					returnList.Add (bml);
			}

			// handle all lines in list2
			mappingForLinesEnum = mappingForLines2.GetEnumerator ();
			for (int index = 0; mappingForLinesEnum.MoveNext (); index++) {
				BiMatchedLines bml = bfsSearch (index, false);
				if (bml != null)
					returnList.Add (bml);
			}

			return returnList;
		}


		public static List<UtilsSubtitle.EntryInformation> GetEntryInformation(Boolean ignoreSingleSubLines, EpisodeInfo episodeInfo, List<BiMatchedLines> matchedLinesList, List<LineInfo> list1, List<LineInfo> list2) {
			List<UtilsSubtitle.EntryInformation> returnList = new List<UtilsSubtitle.EntryInformation> ();

			foreach (SubtitleMatcher.BiMatchedLines matchedLines in matchedLinesList) {
					
				// ignore line when no counterpart was found
				if (ignoreSingleSubLines && (matchedLines.listlines [0].Count == 0 || matchedLines.listlines [1].Count == 0))
					continue;
				
				DateTime? startTimestamp = null;
				DateTime? endTimestamp = null;


				Func<List<Int32>, List<LineInfo>, String> catenateString = delegate(List<int> lineIndexList, List<LineInfo> lineInfoList) {
					StringBuilder thisStringBuilder = new StringBuilder();
					LineInfo thisLineInfo;
					List<Int32>.Enumerator lineIndexEnum;
					lineIndexEnum = lineIndexList.GetEnumerator ();
					while (lineIndexEnum.MoveNext ()) {
						thisLineInfo = lineInfoList [lineIndexEnum.Current];
						startTimestamp = (startTimestamp == null || thisLineInfo.startTime < startTimestamp ? thisLineInfo.startTime : startTimestamp);
						endTimestamp = (endTimestamp == null || thisLineInfo.endTime > endTimestamp ? thisLineInfo.endTime : endTimestamp);
						thisStringBuilder.Append (thisLineInfo.text + " | ");
					}
					return thisStringBuilder.ToString();
				};

				String sub1string = catenateString (matchedLines.listlines [0], list1);
				String sub2string = catenateString (matchedLines.listlines [1], list2);
				
				returnList.Add (new UtilsSubtitle.EntryInformation (sub1string, sub2string, episodeInfo, startTimestamp.GetValueOrDefault(), endTimestamp.GetValueOrDefault()));
			}

			return returnList;
		}
	}
}

