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
using System.Text;
using System.Text.RegularExpressions;

namespace subs2srs4linux
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
			}

			lines.RemoveAll (item => item.text == "");
		}

		/// <summary>
		/// Shifts all lines/timestamps by a given time.
		/// </summary>
		static public void ShiftByTime(List<LineInfo> lines, double shiftValueInSecs) {
			TimeSpan shiftValue = TimeSpan.FromSeconds(shiftValueInSecs);
			foreach (LineInfo line in lines) {
				line.startTime += shiftValue;
				line.endTime += shiftValue;
			}
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
		public class LineContainer : ITimeSpan, IComparable<ITimeSpan> {
			private double m_startTime;
			private double m_endTime;
			private List<int> m_lineIndices = new List<int>();

			public LineContainer() { }

			/// <summary>
			/// Adds a line. The index is relative to a bigger list (for example to all
			/// lines of one subtitle file).
			/// </summary>
			public void AddLine(int index, double startTime, double endTime) {
				m_lineIndices.Add(index);
				m_startTime = Math.Min(startTime, m_startTime);
				m_endTime = Math.Max(endTime, m_endTime);
			}

			public List<int> LineIndices {
				get { return m_lineIndices; }
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
		public static List<LineContainer> GetNonOverlappingTimeSpans(List<LineInfo> lines) {
			var containers = new List<LineContainer>(new LineContainer[lines.Count]);
			var lineAlreadyAdded = new List<bool>(new bool[lines.Count]);
			for(int i = 0; i < lines.Count; i++) {
				if(lineAlreadyAdded[i]) continue;

				// create new container for this line
				LineContainer lineContainer = new LineContainer();
				lineContainer.AddLine(i, lines[i].StartTime, lines[i].EndTime);
				lineAlreadyAdded[i] = true;
				containers.Add(lineContainer);

restartLoop:
				for(int k = i + 1; k < lines.Count; k++) {
					if(lineAlreadyAdded[k]) continue;

					if(UtilsCommon.IsOverlapping(lines[k], lineContainer)) {
						lineContainer.AddLine(k, lines[k].StartTime, lines[k].EndTime);
						lineAlreadyAdded[k] = true;
						goto restartLoop;
					}
				}
			}

			containers.Sort();

			return containers;
		}

		/// <summary>
		/// This class is closely related to the cards that will be generated.
		/// Every EntryInformation-Instance, that isn't filtered away will be used
		/// for exactly one card.
		/// </summary>
		public class EntryInformation {
			public String targetLanguageString;
			public String nativeLanguageString;
			public EpisodeInfo episodeInfo;
			public DateTime startTimestamp;
			public DateTime endTimestamp;
			public bool isActive;

			public DateTime Duration {
				get { return UtilsCommon.GetTimeDiff(endTimestamp, startTimestamp); }
			}

			public EntryInformation(String targetLanguageString, String nativeLanguageString, EpisodeInfo episodeInfo, DateTime startTimestamp, DateTime endTimestamp) {
				this.targetLanguageString = targetLanguageString;
				this.nativeLanguageString = nativeLanguageString;
				this.episodeInfo = episodeInfo;
				this.startTimestamp = startTimestamp;
				this.endTimestamp = endTimestamp;
				this.isActive = true;
			}

			/// <summary>
			/// Returns some string that identifies this entry information.
			/// </summary>
			/// <returns>The key.</returns>
			public String GetKey() {
				String str = String.Format ("{0:000.}", episodeInfo.Number) + "__" + UtilsCommon.ToTimeArg (startTimestamp) + "__" + UtilsCommon.ToTimeArg (endTimestamp);
				return Regex.Replace (str, "[^a-zA-Z0-9]", "_");
			}
		}
	}
}
