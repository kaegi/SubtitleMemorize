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

namespace subtitleMemorize
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

		public class SubtitleMatcherCache {

			private List<ExtendedLineInfo> extendedLineInfos1;
			private List<ExtendedLineInfo> extendedLineInfos2;

			public SubtitleMatcherCache(object extendedLineInfos1, object extendedLineInfos2) {
				this.extendedLineInfos1 = (List<ExtendedLineInfo>)extendedLineInfos1;
				this.extendedLineInfos2 = (List<ExtendedLineInfo>)extendedLineInfos2;
			}


			/// <summary>
			/// Shift all lines that were given as parameters to SubtitleMatcher that created
			/// this object by "shiftTime".
			/// </summary>
			public void ShiftTime(double shiftTime, bool shiftList1, bool shiftList2) {
				if(shiftList1) {
					foreach(var line in extendedLineInfos1) {
						line.lineInfo.startTime += shiftTime;
						line.lineInfo.endTime += shiftTime;
					}
				}

				if(shiftList2) {
					foreach(var line in extendedLineInfos2) {
						line.lineInfo.startTime += shiftTime;
						line.lineInfo.endTime += shiftTime;
					}
				}
			}

			public object ExtendedLineInfo1 { get { return extendedLineInfos1; } }
			public object ExtendedLineInfo2 { get { return extendedLineInfos2; } }
		}

		public static SubtitleMatcherCache GetParameterCache(IEnumerable<LineInfo> lines1, IEnumerable<LineInfo> lines2) {

			// LineInfo -> ExtendedLineInfo
			var extendedLineInfos1 = new List<ExtendedLineInfo>();
			var extendedLineInfos2 = new List<ExtendedLineInfo>();
			foreach(var line in lines1) extendedLineInfos1.Add(new ExtendedLineInfo(line));
			foreach(var line in lines2) extendedLineInfos2.Add(new ExtendedLineInfo(line));

			return new SubtitleMatcherCache(extendedLineInfos1, extendedLineInfos2);
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
		public static LinkedList<BiMatchedLines> MatchSubtitles(SubtitleMatcherCache cache) {

			List<ExtendedLineInfo> extendedLineInfos1 = (List<ExtendedLineInfo>)cache.ExtendedLineInfo1;
			List<ExtendedLineInfo> extendedLineInfos2 = (List<ExtendedLineInfo>)cache.ExtendedLineInfo2;

			// find matchings for every line in list1 to list2 and reverse
			FindMatching (extendedLineInfos1, extendedLineInfos2);

			RemoveOverlappings (extendedLineInfos1);
			RemoveOverlappings (extendedLineInfos2);

			var finalMappings = FindBidirectionalMapping (extendedLineInfos1, extendedLineInfos2);

			return finalMappings;
		}

		/// <summary>
		/// This creates a list of good matching lines between primaryList and secondaryList.
		/// </summary>
		/// <param name="list1">List1.</param>
		/// <param name="list2">List2.</param>
		private static void FindMatching(List<ExtendedLineInfo> list1, List<ExtendedLineInfo> list2) {
			int i1 = 0, i2 = 0;

			foreach(var line in list1) {
				line.matchingLines.Clear();
				line.alreadyUsedInBidirectionalSearch = false;
			}

			foreach(var line in list2) {
				line.matchingLines.Clear();
				line.alreadyUsedInBidirectionalSearch = false;
			}

			while(i1 < list1.Count) {
				int i2_reset = i2;

				while(i2 < list2.Count) {
					// node2 does not overlap with node1? Then proceed with next node1.
					if(list1[i1].EndTime < list2[i2].StartTime) break;

					// node1 and node2 overlap!
					if (UtilsCommon.OverlappingScore(list1[i1], list2[i2]) >= 0.4) {
						list1[i1].matchingLines.AddLast (list2[i2]);
						list2[i2].matchingLines.AddLast (list1[i1]);
					}

					// try combination (node1, next node2) in next inner iteration
					i2++;
				}

				// do node1 and next node1 overlap? Then all node2's we handled in this iteration might
				// overlap with next node1 -> reset node2 to start of this iteration.
				if(i1 + 1 != list1.Count && list1[i1 + 1].StartTime <= list1[i1].EndTime) i2 = i2_reset;
				i1++;
			}
/*
			foreach(var primaryListLine in primaryList) {
				primaryListLine.matchingLines.Clear();
				primaryListLine.alreadyUsedInBidirectionalSearch = false;

				Action<ExtendedLineInfo, ExtendedLineInfo> matchLines = delegate(ExtendedLineInfo thisLine, ExtendedLineInfo secondaryListLine) {
					if (UtilsCommon.OverlappingScore(thisLine, secondaryListLine) < 0.4) return;
					thisLine.matchingLines.AddLast (secondaryListLine);
				};

				secondaryListCache.DoForOverlapping(primaryListLine, matchLines);
			}
			*/
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
		static void RemoveOverlappings (List<ExtendedLineInfo> primaryList)
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

		private static LinkedList<BiMatchedLines> FindBidirectionalMapping (List<ExtendedLineInfo> mappingForLines1, List<ExtendedLineInfo> mappingForLines2)
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
	}
}
