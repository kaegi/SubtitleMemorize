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
		/// Creates an entry information for every BiMatchedLine object.
		/// </summary>
		public static List<UtilsSubtitle.EntryInformation> GetEntryInformation(Settings settings, EpisodeInfo episodeInfo, IEnumerable<SubtitleMatcher.BiMatchedLines> matchedLinesList) {
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

				catenateString (matchedLines.listlines [0]);
				catenateString (matchedLines.listlines [1]);

				var entryInfo = new UtilsSubtitle.EntryInformation (
																	matchedLines.listlines[0].ToList(),
																	matchedLines.listlines[1].ToList(),
																	episodeInfo,
																	startTimestamp, endTimestamp,
																	startTimestamp - settings.AudioPaddingBefore,
																	endTimestamp + settings.AudioPaddingAfter);
				entryInfo.isActive = !deactivated;
				returnList.AddLast (entryInfo);
			}


			var retList = new List<UtilsSubtitle.EntryInformation>(returnList);
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
			// TODO: remove lines with filler characters like notes etc.
			// TODO: unify lines with "->", etc. character

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
		public static LinkedList<LineContainer<T>> GetNonOverlappingTimeSpans<T>(LinkedList<T> lines, double threshold=0) where T : ITimeSpan {
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

			// TODO sort

			return containers;
		}

		/// <summary>
		/// This class is closely related to the cards that will be generated.
		/// Every EntryInformation-Instance, that isn't filtered away will be used
		/// for exactly one card.
		/// </summary>
		public class EntryInformation : IComparable<ITimeSpan>, ITimeSpan {
			public List<LineInfo> targetLanguageLines;
			public List<LineInfo> nativeLanguageLines;
			public EpisodeInfo episodeInfo;
			public double startTimestamp;
			public double endTimestamp;
			public double audioStartTimestamp;
			public double audioEndTimestamp;
			public bool isActive;

			public double Duration {
				get { return UtilsCommon.GetTimeSpanLength(this); }
			}

			public EntryInformation(List<LineInfo> targetLanguageLines,
									List<LineInfo> nativeLanguageLines,
									EpisodeInfo episodeInfo,
									double startTimestamp,
									double endTimestamp,
									double audioStartTimestamp,
									double audioEndTimestamp) {
				this.targetLanguageLines = targetLanguageLines;
				this.nativeLanguageLines = nativeLanguageLines;
				this.episodeInfo         = episodeInfo;
				this.startTimestamp      = startTimestamp;
				this.endTimestamp        = endTimestamp;
				this.audioStartTimestamp = audioStartTimestamp;
				this.audioEndTimestamp   = audioEndTimestamp;
				this.isActive            = true;
			}

			/// <summary>
			/// Unifies two EntryInformation into one (merging). The two EntryInformation have to be compatible
			/// which can be checked with IsMergePossbile().
			/// </summary>
			public EntryInformation(EntryInformation first, EntryInformation second) {
				this.targetLanguageLines = first.targetLanguageLines.Concat(second.targetLanguageLines).ToList();
				this.nativeLanguageLines = first.nativeLanguageLines.Concat(second.nativeLanguageLines).ToList();
				this.episodeInfo          = first.episodeInfo;
				this.startTimestamp       = Math.Min(first.startTimestamp, second.startTimestamp);
				this.endTimestamp         = Math.Max(first.endTimestamp, second.endTimestamp);
				this.isActive             = first.isActive || second.isActive;
			}

			public List<LineInfo> GetListByLanguageType(UtilsCommon.LanguageType languageType) {
				return languageType == UtilsCommon.LanguageType.NATIVE ? nativeLanguageLines : targetLanguageLines;
			}

			/// <summary>
			/// Replaces strings in line infos by regex.
			/// </summary>
			public void DoRegexReplace(UtilsCommon.LanguageType languageType, String pattern, String to) {
				foreach(var line in GetListByLanguageType(languageType)) {
					line.text = Regex.Replace(line.text, pattern, to);
				}
			}

			/// <summary>
			/// Checks whether two EntryInformation instances can be merged. (If they
			/// are not in the same episode, in which episode is the new EntryInformation?)
			/// </summary>
			public static bool IsMergePossbile(EntryInformation a, EntryInformation b) {
				if(a.episodeInfo != b.episodeInfo) return false;
				return true;
			}

			/// <summary>
			/// Returns some string that identifies this entry information.
			/// </summary>
			/// <returns>The key.</returns>
			public String GetKey() {
				String str = String.Format ("{0:000.}", episodeInfo.Number) + "__" + UtilsCommon.ToTimeArg (startTimestamp) + "__" + UtilsCommon.ToTimeArg (endTimestamp);
				return Regex.Replace (str, "[^a-zA-Z0-9]", "_");
			}

			public double StartTime {
				get { return startTimestamp; }
			}

			public double EndTime {
				get { return endTimestamp; }
			}

			/// <summary>
			/// Compare lines based on their Start Times.
			/// </summary>
			public int CompareTo(ITimeSpan other) {
				if(StartTime == other.StartTime) return 0;
				return StartTime < other.StartTime ? -1 : 1;
			}

			private string ToString(UtilsCommon.LanguageType languageType, String separator="\n") {
				String str = "";
				bool isFirst = true;
				foreach(var line in GetListByLanguageType(languageType)) {
					if(isFirst) {
						str += line.text;
						isFirst = false;
					} else {
						str += separator + line.text;
					}
				}
				return str;
			}

			public string ToMultiLine(UtilsCommon.LanguageType languageType) {
				return ToString(languageType, "\n");
			}

			public string ToSingleLine(UtilsCommon.LanguageType languageType) {
				return ToString(languageType, " ");
			}

			private List<String> GetActors(UtilsCommon.LanguageType languageType) {
				var result = new List<String>();
				foreach(var line in targetLanguageLines) {
					result.Add(line.name);
				}
				return result;
			}

			public List<String> GetActors() {
				var list = GetActors(UtilsCommon.LanguageType.TARGET).Concat(GetActors(UtilsCommon.LanguageType.NATIVE)).Distinct().ToList();
				list.Sort();
				return list;
			}

			public String GetActorString() {
				StringBuilder stringBuilder = new StringBuilder();
				var actors = GetActors();
				if(actors.Count > 0) stringBuilder.Append(actors[0]);
				foreach(var actor in actors.Skip(1)) {
					stringBuilder.Append(", ");
					stringBuilder.Append(actor);
				}
				return stringBuilder.ToString();
			}

			/// <summary>
			/// Updates LineInfos. Strings for different LineInfos are separated by '\n'.
			/// Returns false if text could not be parsed.
			/// </summary>
			public bool SetLineInfosByMultiLineString(UtilsCommon.LanguageType languageType, string text) {
				var lines = GetListByLanguageType(languageType);
				var stringsRows = text.Split('\n').ToList();
				if(stringsRows.Count != lines.Count()) { return false; }
				for(int i = 0; i < lines.Count(); i++) { lines[i].text = stringsRows[i]; }
				return true;
			}
		}
	}
}
