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
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

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
		static public void ShiftByTime(List<LineInfo> lines, TimeSpan shiftValue) {
			foreach (LineInfo line in lines) {
				line.startTime += shiftValue;
				line.endTime += shiftValue;
			}
		}

		private static List<LineInfo> ParseSubtitleInVideoFile(Settings settings, string filename, Dictionary<String, String> properties) {
			
			List<StreamInfo> allStreams = StreamInfo.ReadAllStreams (filename);

			// find first subtitle in stream (we also know then whether the file has any subtitle streams)
			int firstSubtitleStream = -1;
			for (int i = 0; i < allStreams.Count; i++) {
				if (allStreams [i].StreamTypeValue == StreamInfo.StreamType.ST_SUBTITLE) {
					firstSubtitleStream = i;
					break;
				}
			}

			if (firstSubtitleStream == -1)
				throw new Exception ("Container file \"" + filename + "\" does not contain any subtitle streams");


			int streamIndex = 0;
			String streamIndexDictionaryString = "";
			if (properties.TryGetValue ("stream", out streamIndexDictionaryString)) {
				try {
					streamIndex = Int32.Parse (streamIndexDictionaryString);
				} catch (Exception) {
					throw new Exception ("Stream property is not an integer.");
				}

				if (streamIndex < 0 || streamIndex >= allStreams.Count || allStreams [streamIndex].StreamTypeValue != StreamInfo.StreamType.ST_SUBTITLE) {
					throw new Exception ("Stream with index " + streamIndex + " is not a subtitle stream (but stream at index " + firstSubtitleStream + " is).");
				}

			} else
				streamIndex = firstSubtitleStream;

			// new subtitle file
			String videoFileHash = UtilsCommon.GetDateSizeChecksum (filename);
			String newSubtitleFileName = videoFileHash + "_" + allStreams [streamIndex].StreamIndex + GetExtensionByStreamInfo (allStreams [streamIndex]);
			string newSubtitleFilePath = InstanceSettings.temporaryFilesPath + newSubtitleFileName;

			// do not extract again when file was already extracted once
			if (!File.Exists (newSubtitleFilePath))
				UtilsVideo.ExtractStream (filename, allStreams [streamIndex], newSubtitleFilePath);
			
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

			// shift subtitle
			TimeSpan timeShift = new TimeSpan(0, 0, 0, 0, perSubtitleSettings.SubDelay);
			foreach (LineInfo li in lines) {
				li.startTime += timeShift;
				li.endTime += timeShift;
			}
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
		/// This class is closely related to the cards that will be generated.
		/// Every EntryInformation-Instance, that isn't filtered away will be used
		/// for exactly one card.
		/// </summary>
		public struct EntryInformation {
			public String targetLanguageString;
			public String nativeLanguageString;
			public int episodeNumber;
			public DateTime startTimestamp;
			public DateTime endTimestamp;

			public EntryInformation(String targetLanguageString, String nativeLanguageString, int episodeNumber, DateTime startTimestamp, DateTime endTimestamp) {
				this.targetLanguageString = targetLanguageString;
				this.nativeLanguageString = nativeLanguageString;
				this.episodeNumber = episodeNumber;
				this.startTimestamp = startTimestamp;
				this.endTimestamp = endTimestamp;
			}

			/// <summary>
			/// Returns some string that identifies this entry information.
			/// </summary>
			/// <returns>The key.</returns>
			public String GetKey() {
				return String.Format ("{0:000.}", episodeNumber) + "_" + UtilsCommon.ToTimeArg (startTimestamp) + "_" + UtilsCommon.ToTimeArg (endTimestamp);
			}
		}
	}
}

