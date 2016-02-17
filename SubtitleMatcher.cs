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
		private class ExtendedLineInfo : ITimeSpan
		{
			public readonly LineInfo lineInfo;
			public readonly LinkedList<ExtendedLineInfo> matchingLines = new LinkedList<ExtendedLineInfo>(); // matching lines in other language
			public bool alreadyUsedInBidirectionalSearch = false;

			public ExtendedLineInfo(LineInfo li) {
				lineInfo = li;
			}


			public double StartTime {
				get { return lineInfo.StartTime; }
			}

			public double EndTime {
				get { return lineInfo.EndTime; }
			}

		}

		/// <summary>
		/// Indices refer to lines in two list. This represents
		/// a many-to-many (N:M) relationship of this list.
		/// </summary>
		public class BiMatchedLines
		{
			public readonly LinkedList<LineInfo>[] listlines = new LinkedList<LineInfo>[2];

			public BiMatchedLines() {
				listlines[0] = new LinkedList<LineInfo>();
				listlines[1] = new LinkedList<LineInfo>();
			}
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
		public static LinkedList<BiMatchedLines> MatchSubtitles(IEnumerable<LineInfo> lines1, IEnumerable<LineInfo> lines2) {

			// LineInfo -> ExtendedLineInfo
			var extendedLineInfos1 = new LinkedList<ExtendedLineInfo>();
			var extendedLineInfos2 = new LinkedList<ExtendedLineInfo>();
			foreach(var line in lines1) extendedLineInfos1.AddLast(new ExtendedLineInfo(line));
			foreach(var line in lines2) extendedLineInfos2.AddLast(new ExtendedLineInfo(line));

			// find matchings for every line in list1 to list2 and reverse
			FindMatching (extendedLineInfos1, extendedLineInfos2);

			RemoveOverlappings (extendedLineInfos1);
			RemoveOverlappings (extendedLineInfos2);

			var finalMappings = FindBidirectionalMapping (extendedLineInfos1, extendedLineInfos2);

			return finalMappings;
		}

		/// <summary>
		/// This creates a list of good matching subtitles from list1 to every element from list2.
		/// </summary>
		/// <param name="list1">List1.</param>
		/// <param name="list2">List2.</param>
		private static void FindMatching(LinkedList<ExtendedLineInfo> list1, LinkedList<ExtendedLineInfo> list2) {
			// TODO: this has O(n²) time -> optimize
			foreach (ExtendedLineInfo list1line in list1) {
				foreach (ExtendedLineInfo list2line in list2) {
					if (UtilsCommon.OverlappingScore(list1line, list2line) < 0.4)
						continue;
					list1line.matchingLines.AddLast (list2line);
					list2line.matchingLines.AddLast (list1line);
				}
			}
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
		static void RemoveOverlappings (LinkedList<ExtendedLineInfo> primaryList)
		{
			foreach (ExtendedLineInfo eli in primaryList) {

				ExtendedLineInfo leastFittingLine;
				do {
					leastFittingLine = null;

					// the function will group all overlapping lines together, so we delete the worst
					// line from that container and recalculate the containers (because overlapping could
					// have been stopped)
					var nonOverlappingLineContainers = UtilsSubtitle.GetNonOverlappingTimeSpans (eli.matchingLines, 0.2);
					foreach (var lineContainer in nonOverlappingLineContainers) {
						if (lineContainer.TimeSpans.Count > 1) {
							leastFittingLine = GetLeastFittingLine(eli.lineInfo, lineContainer.TimeSpans);
							break;
						}
					}

					// rebuild matching lines without the removed line
					if(leastFittingLine != null) {
						eli.matchingLines.Remove(leastFittingLine);
						continue;
					}
				} while(leastFittingLine != null);
			}
		}

		/// <summary>
		/// Returns the line from "matching lines" that has the least overlapping rating with "lineInfo".
		/// </summary>
		private static ExtendedLineInfo GetLeastFittingLine(LineInfo lineInfo, LinkedList<ExtendedLineInfo> matchingLines)
		{
			double leastFittingOverlappingScore =  0.5;
			ExtendedLineInfo leastFittingLine = null;
			foreach(ExtendedLineInfo matchingLine in matchingLines) {

				double currentOverlappingScore = UtilsCommon.OverlappingScore (lineInfo, matchingLine);
				if (leastFittingLine == null || (currentOverlappingScore < leastFittingOverlappingScore)) {
					leastFittingOverlappingScore = currentOverlappingScore;
					leastFittingLine = matchingLine;
				}
			}

			return leastFittingLine;
		}

		private static LinkedList<BiMatchedLines> FindBidirectionalMapping (LinkedList<ExtendedLineInfo> mappingForLines1, LinkedList<ExtendedLineInfo> mappingForLines2)
		{
			Func<ExtendedLineInfo, Boolean, BiMatchedLines> bfsSearch = delegate(ExtendedLineInfo lineInfo, Boolean isInFirstList) {


				// first value in tuple: the index of the line in respective list; second value: true -> list1, false -> list2
				var bfsList = new Queue<Tuple<ExtendedLineInfo, bool>> ();
				BiMatchedLines bimatchedLines = new BiMatchedLines ();

				// all matching lines that are meant to be together are a connected component in the bipartite graph of lines in list1 and list2 -> find
				// these with breadth-first search
				bfsList.Enqueue(new Tuple<ExtendedLineInfo, bool>(lineInfo, isInFirstList));
				while (bfsList.Count > 0) {
					Tuple<ExtendedLineInfo, Boolean> currentTuple = bfsList.Dequeue();
					ExtendedLineInfo currentLineInfo = currentTuple.Item1;

					// this value was already handled so it can be skipped
					if(currentLineInfo.alreadyUsedInBidirectionalSearch)
						continue;
					currentLineInfo.alreadyUsedInBidirectionalSearch = true;

					bimatchedLines.listlines[currentTuple.Item2 ? 0 : 1].AddLast(currentLineInfo.lineInfo);

					foreach (var oppositeLineMatching in currentLineInfo.matchingLines)
						bfsList.Enqueue(new Tuple<ExtendedLineInfo, bool> (oppositeLineMatching, !currentTuple.Item2)); // this line is in the opposite list
				}

				return (bimatchedLines.listlines[0].Count > 0 || bimatchedLines.listlines[1].Count > 0) ? bimatchedLines : null;
			};

			var returnList = new LinkedList<BiMatchedLines> ();

			// handle all lines in list1
			foreach(ExtendedLineInfo line in mappingForLines1) {
				BiMatchedLines bml = bfsSearch (line, true);
				if (bml != null) returnList.AddLast (bml);
			}

			// handle all lines in list2
			foreach(ExtendedLineInfo line in mappingForLines2) {
				BiMatchedLines bml = bfsSearch (line, false);
				if (bml != null) returnList.AddLast (bml);
			}

			return returnList;
		}


		public static List<UtilsSubtitle.EntryInformation> GetEntryInformation(Boolean ignoreSingleSubLines, EpisodeInfo episodeInfo, IEnumerable<BiMatchedLines> matchedLinesList) {
			var returnList = new LinkedList<UtilsSubtitle.EntryInformation> ();

			foreach (SubtitleMatcher.BiMatchedLines matchedLines in matchedLinesList) {

				// ignore line when no counterpart was found
				bool deactivated = false;
				if (matchedLines.listlines [0].Count == 0 || matchedLines.listlines [1].Count == 0)
					deactivated = true;

				bool timestamspUninitialized = true;
				double startTimestamp = 0;
				double endTimestamp = 0;


				Func<IEnumerable<LineInfo>, String> catenateString = delegate(IEnumerable<LineInfo> lineInfos) {
					StringBuilder thisStringBuilder = new StringBuilder();
					foreach(var thisLineInfo in lineInfos) {
						thisStringBuilder.Append (thisLineInfo.text + "|");

						// adjust timestamps to this line
						if(timestamspUninitialized) {
							// initialize timestamps
							startTimestamp = thisLineInfo.StartTime;
							endTimestamp = thisLineInfo.EndTime;
							timestamspUninitialized = false;
						} else {
							startTimestamp = Math.Min(startTimestamp, thisLineInfo.StartTime);
							endTimestamp = Math.Max(endTimestamp, thisLineInfo.EndTime);
						}
					}
					return thisStringBuilder.ToString();
				};

				String sub1string = catenateString (matchedLines.listlines [0]);
				String sub2string = catenateString (matchedLines.listlines [1]);

				var entryInfo = new UtilsSubtitle.EntryInformation (sub1string, sub2string, episodeInfo, startTimestamp, endTimestamp);
				entryInfo.isActive = !deactivated;
				returnList.AddLast (entryInfo);
			}


			var retList = new List<UtilsSubtitle.EntryInformation>(returnList);
			retList.Sort();
			return retList;
		}
	}
}

