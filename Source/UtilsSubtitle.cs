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
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace subtitleMemorize
{
	public static class UtilsSubtitle
	{
		/// <summary>
		/// Removes special characters "\n" and "\t" and deletes blank lines.
		/// </summary>
		static public void BeautifyLines(List<LineInfo> lines) {
			foreach (LineInfo line in lines) {
				line.text = line.text.Replace ("\\N", " ");
				line.text = line.text.Replace ("\\n", " ");
				line.text = line.text.Replace ("\n", " ");
				line.text = line.text.Replace ("\t", " ");
				line.text = line.text.Trim();
				if(line.endTime < line.startTime) {
					var tmp = line.endTime;
					line.endTime = line.startTime;
					line.startTime = tmp;
				}
			}

			lines.RemoveAll (item => item.text == "");
		}

		/// <summary>
		/// Shifts all lines/timestamps by a given time.
		/// </summary>
		static public void ShiftByTime(IEnumerable<LineInfo> lines, double shiftValueInSecs) {
			foreach (LineInfo line in lines) {
				line.startTime += shiftValueInSecs;
				line.endTime += shiftValueInSecs;
			}
		}

		/// <summary>
		/// Creates an card information for LineInfo (without matching/without native language LineInfos).
		/// </summary>
		public static List<CardInfo> GetCardInfo(Settings settings, EpisodeInfo episodeInfo, List<LineInfo> lines) {
			var cardInfos = new List<CardInfo>();
			foreach(var line in lines) {
				var thisCardLineInfos = new List<LineInfo>();
				thisCardLineInfos.Add(line);
				cardInfos.Add(new CardInfo(thisCardLineInfos, new List<LineInfo>(), episodeInfo,
																	line.StartTime, line.EndTime,
																	line.StartTime - settings.AudioPaddingBefore,
																	line.EndTime + settings.AudioPaddingAfter));
			}
			return cardInfos;
		}

		/// <summary>
		/// Creates an card information for every BiMatchedLine object.
		/// </summary>
		public static List<CardInfo> GetCardInfo(Settings settings, EpisodeInfo episodeInfo, IEnumerable<SubtitleMatcher.BiMatchedLines> matchedLinesList) {
			var returnList = new LinkedList<CardInfo> ();

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

				catenateString (matchedLines.listlines [0]);
				catenateString (matchedLines.listlines [1]);

				var cardInfo = new CardInfo (
																	matchedLines.listlines[0].ToList(),
																	matchedLines.listlines[1].ToList(),
																	episodeInfo,
																	startTimestamp, endTimestamp,
																	startTimestamp - settings.AudioPaddingBefore,
																	endTimestamp + settings.AudioPaddingAfter);
				cardInfo.isActive = !deactivated;
				returnList.AddLast (cardInfo);
			}


			var retList = new List<CardInfo>(returnList);
			retList.Sort();
			return retList;
		}

		private static List<LineInfo> ParseSubtitleInVideoFile(Settings settings, string filename, Dictionary<String, String> properties) {
			StreamInfo subtitleStreamInfo = UtilsVideo.ChooseStreamInfo (filename, properties, StreamInfo.StreamType.ST_SUBTITLE);

			// new subtitle file
			String videoFileHash = UtilsCommon.GetDateSizeChecksum (filename);
			String newSubtitleFileName = videoFileHash + "_" + subtitleStreamInfo.StreamIndex + GetExtensionByStreamInfo (subtitleStreamInfo);
			string newSubtitleFilePath = InstanceSettings.temporaryFilesPath + newSubtitleFileName;

			// do not extract again when file was already extracted once
			if (!File.Exists (newSubtitleFilePath))
				UtilsVideo.ExtractStream (filename, subtitleStreamInfo, newSubtitleFilePath);

			return ParseSubtitle (settings, newSubtitleFilePath, properties);
		}

		public static List<LineInfo> ParseSubtitle(Settings settings, string filename, Dictionary<String, String> properties) {
			string mimeType = UtilsCommon.GetMimetypeByFilename (filename);

			// find right parser
			ISubtitleParser parser = null;
			switch (mimeType) {
			case "text/x-ass":
			case "text/x-ssa":
				parser = new SubtitleParserASS ();
				break;
			case "application/x-subrip":
				parser = new SubtitleParserSRT ();
				break;
			case "video/x-matroska":
				return ParseSubtitleInVideoFile (settings, filename, properties);
			case "":
				throw new Exception("File type/mime type could not be recognized for file \"" + filename + "\"!");
			default:
				throw new Exception("Unsupportet format (" + mimeType + ") for subtitle \"" + filename + "\"!");
			}

			// read encoding string from properties
			String encodingString = "utf-8";
			if(properties.ContainsKey("enc"))
				encodingString = properties["enc"];

			// read all lines
			using (var fileStream = new FileStream (filename, FileMode.Open)) {
				return parser.parse (settings, fileStream, Encoding.GetEncoding(encodingString));
			}
		}

		public static List<LineInfo> ParseSubtitleWithPostProcessing(Settings settings, PerSubtitleSettings perSubtitleSettings, string filename, Dictionary<String, String> properties) {
			List<LineInfo> lines = ParseSubtitle (settings, filename, properties);
			BeautifyLines (lines);
			return lines;
		}

		/// <summary>
		/// Determine extension for file (".ass" or ".srt", ...) usable for extracting subtitle streams.
		/// </summary>
		/// <returns>The full filename by stream info.</returns>
		public static String GetExtensionByStreamInfo(StreamInfo streamInfo) {
			return "." + streamInfo.StreamName;
		}


		/// <summary>
		/// This class provides an easy way to "group" lines and still have
		/// an time span for these lines. For example you could create
		/// "LineContainer"s for a subtitle file where each container
		/// does not overlap with another (one container then contains lines
		/// that overlap).
		/// </summary>
		public class LineContainer<T> : ITimeSpan, IComparable<ITimeSpan> where T : ITimeSpan {
			private double m_startTime;
			private double m_endTime;
			private LinkedList<T> m_timeSpans = new LinkedList<T>();

			public LineContainer() { }

			/// <summary>
			/// Adds a line. The index is relative to a bigger list (for example to all
			/// lines of one subtitle file).
			/// </summary>
			public void AddLine(T timeSpan) {
				m_timeSpans.AddLast(timeSpan);
				m_startTime = Math.Min(timeSpan.StartTime, m_startTime);
				m_endTime = Math.Max(timeSpan.EndTime, m_endTime);
			}

			public LinkedList<T> TimeSpans {
				get { return m_timeSpans; }
			}

			public double StartTime {
				get { return m_startTime; }
			}

			public double EndTime {
				get { return m_endTime; }
			}

			/// <summary>
			/// Compare lines based on their Start Times.
			/// </summary>
			public int CompareTo(ITimeSpan other) {
				if(StartTime == other.StartTime) return 0;
				return StartTime < other.StartTime ? -1 : 1;
			}
		}

		/// <summary>
		/// Returns the longest list of line containers, for which no line containers overlap. Addtionaly
		/// these containers are sorted by start time.
		/// </summary>
		public static List<LineContainer<T>> GetNonOverlappingTimeSpans<T>(LinkedList<T> lines, double threshold=0) where T : ITimeSpan {
			var containers = new LinkedList<LineContainer<T>>();
			var lineAlreadyAdded = new bool[lines.Count];
			var lineANode = lines.First;
			var lineBNode = lines.First;
			int lineAindex = 0;
			int lineBindex = 0;
			while(lineANode != null) {
				if (lineAlreadyAdded [lineAindex]) {
					lineAindex++;
					lineANode = lineANode.Next;
					continue;
				}

				// create new container for this line
				var lineContainer = new LineContainer<T>();
				lineContainer.AddLine(lineANode.Value);
				lineAlreadyAdded[lineAindex] = true;
				containers.AddLast(lineContainer);

restartLoop:
				lineBNode = lineANode.Next;
				lineBindex = lineAindex + 1;
				while(lineBNode != null) {
					if (lineAlreadyAdded [lineBindex]) {
						lineBindex++;
						lineBNode = lineBNode.Next;
						continue;
					}

					// just test treshold if line collides with container
					if(UtilsCommon.IsOverlapping(lineBNode.Value, lineContainer)) {
						foreach(ITimeSpan timeSpanInContainer in lineContainer.TimeSpans) {
							if(UtilsCommon.OverlappingScore(lineBNode.Value, timeSpanInContainer) > threshold) {
								lineContainer.AddLine(lineBNode.Value);
								lineAlreadyAdded[lineBindex] = true;
								goto restartLoop;
							}
						}
					}

					lineBindex++;
					lineBNode = lineBNode.Next;
				}

				lineAindex++;
				lineANode = lineANode.Next;
			}

			// XXX: is sort necessary
			var containerList = containers.ToList();
			containerList.Sort();

			return containerList;
		}

		public static Tuple<List<CardInfo>, List<CardInfo>> GetContextCards(int episodeIndex, ITimeSpan timeSpan, List<CardInfo> list, int maxNumberCards = 3, int maxNumberOfLines = -1, double maxSeconds = 15)
		{
			var previousLines = new List<CardInfo>();
			var nextLines = new List<CardInfo>();
			foreach (var card in list)
			{
				if (episodeIndex != card.episodeInfo.Index) continue;

				if (UtilsCommon.GetMinTimeSpanDistance(card, timeSpan) > maxSeconds) continue;
				if (UtilsCommon.DoesTimespanContain(card, timeSpan) || UtilsCommon.DoesTimespanContain(timeSpan, card)) continue;

				if(card.StartTime < timeSpan.StartTime) previousLines.Add(card);
				else nextLines.Add(card);
			}

			Comparison<CardInfo> comparer = delegate (CardInfo a, CardInfo b) {
				double aDistance = UtilsCommon.GetMinTimeSpanDistance(a, timeSpan);
				double bDistance = UtilsCommon.GetMinTimeSpanDistance(b, timeSpan);
				return aDistance < bDistance ? 1 : -1;
			};
			previousLines.Sort(comparer);
			previousLines = previousLines.Take(maxNumberCards).ToList();

			nextLines.Sort(comparer);
			nextLines = nextLines.Take(maxNumberCards).ToList();

			// sort by start times
			previousLines.Sort();
			nextLines.Sort();

			return new Tuple<List<CardInfo>, List<CardInfo>>(previousLines, nextLines);
		}

		public static String CardListToMultilineString(List<CardInfo> cards, UtilsCommon.LanguageType languageType) {
			var str = new StringBuilder();
			if(cards.Count > 0) {
				str.Append(cards[0].ToSingleLine(languageType));
			}
			foreach(var card in cards.Skip(1)) {
				str.Append(" --- ");
				str.Append(card.ToSingleLine(languageType));
			}
			return str.ToString();
		}

	}
}
