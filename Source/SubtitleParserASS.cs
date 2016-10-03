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
using System.Text.RegularExpressions;
using System.Text;

namespace subtitleMemorize
{
	/// <summary>
	/// Parse .ass/.ssa files
	/// </summary>
	public class SubtitleParserASS : ISubtitleParser
	{
		public SubtitleParserASS ()
		{
		}

		/// <summary>
		/// The general idea is to create a regex based of the "Format: "-line in .ass file. Which
		/// then can be used to easily filter the required information (namely timestamps, text and
		/// actor).
		/// </summary>
		/// <param name="settings">Settings.</param>
		/// <param name="rawLines">Raw lines.</param>
		public List<LineInfo> parse(Settings settings, LinkedList<String> rawLines) {
			List<LineInfo> lines = new List<LineInfo> ();

			string formatRegex = GetFormatRegex (rawLines);
			if (formatRegex == null)
				return null;

			// parse every line with format regex and save lines in LineInfo
			foreach(string rawLine in rawLines) {
				Match lineMatch = Regex.Match(rawLine, formatRegex, RegexOptions.IgnoreCase | RegexOptions.Compiled);

				if (!lineMatch.Success)
					continue;

				string startTimeString = lineMatch.Groups ["StartTime"].ToString ().Trim ();
				string endTimeString = lineMatch.Groups ["EndTime"].ToString ().Trim ();
				string nameString = lineMatch.Groups ["Name"].ToString ().Trim ();
				string textString = lineMatch.Groups ["Text"].ToString ().Trim ();

				if (settings.IgnoreStyledSubLines &&
						textString.StartsWith ("{") // NOTE: this is a really big hint for styled subtitles but might create false-negatives -- research common patterns in subtitle files
						&& !textString.StartsWith ("{\\b1}") // bold
						&& !textString.StartsWith ("{\\u1}") // underline
						&& !textString.StartsWith ("{\\i1}") // italics
						&& !textString.StartsWith ("{\\an8}") // text align: up
						) {
					continue;
				}

				// remove styling in subtitles
				textString = Regex.Replace(textString, "{.*?}", "");


				if (textString == "")
					continue; // ignore lines without text

				// generate line info
				LineInfo li = new LineInfo(parseTime(startTimeString), parseTime(endTimeString), textString, new List<String>(new String[]{ nameString }));
				lines.Add(li);



			}


			return lines;
		}

		/// <summary>
		/// Parse SSA timestamps like "0:19:30.25" to seconds
		/// </summary>
		/// <returns>number of seconds in comparision to start of file</returns>
		public double parseTime(String timeString) {
			// Format: "0:00:00.00"
			// Format: "Hours:Minutes:Seconds.HSecs
			Match match = Regex.Match (timeString, @"^(?<Hours>\d):(?<Mins>\d\d):(?<Secs>\d\d).(?<HSecs>\d\d)$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			if (!match.Success) {
				throw new Exception ("Incorrect time format: \"" + timeString + "\"\nTo Fix:\n Open the subtitle file with Aegisub and select File | Save Subtitles");
			}

			DateTime time = new DateTime();
			try {
				time = time.AddHours(Int32.Parse(match.Groups["Hours"].ToString().Trim()));
				time = time.AddMinutes(Int32.Parse(match.Groups["Mins"].ToString().Trim()));
				time = time.AddSeconds(Int32.Parse(match.Groups["Secs"].ToString().Trim()));
				time = time.AddMilliseconds(Int32.Parse(match.Groups["HSecs"].ToString().Trim()) * 10);
			} catch {
				throw new Exception ("Incorrect time format: \"" + timeString + "\"");
			}

			return time.TimeOfDay.TotalMilliseconds / 1000.0;
		}

		/// <summary>
		/// Find "Format: " line in "rawLines" and create a regex matching to infomation in this format.
		/// </summary>
		/// <returns>The format regex.</returns>
		/// <param name="rawLines">Raw lines.</param>
		public string GetFormatRegex(LinkedList<String> rawLines) {

			// find "Format: ..." line in "[Event]" Section
			bool eventSection = false;
			string formatLine = null;
			foreach(String line in rawLines) {
				if (line.StartsWith ("[Events]"))
					eventSection = true;
				else if (line.StartsWith ("Format:") && eventSection) {
					formatLine = line;
					break;
				}
			}

			if (formatLine == null)
				return null;

			// Example:
			// "Format: Layer, Start, End, Style, Name, MarginL, MarginR, MarginV, Effect, Text"
			// "Dialogue: 0,0:00:01.60,0:00:01.60,*Default,NTP,0,0,0,,"
			// "Dialogue: 0,0:00:01.60,0:00:01.60,*Default,NTP,0,0,0,,"
			// "Dialogue: 0,0:00:17.39,0:00:20.42,jp.sub,NTP,0,0,0,,ったく　遅いな"
			// "Dialogue: 0,0:00:21.77,0:00:23.62,jp.sub,NTP,0,0,0,,何やってんのよ"


			string regex = "^Dialogue:";
			string[] formatItems = formatLine.Split (new char[]{ ',' });
			foreach (string item in formatItems) {
				// Filter only important information
				if (item.Contains ("Name") || item.Contains ("Actor")) {
					regex += "(?<Name>.*?),";
				} else if (item.Contains ("Start")) {
					regex += "(?<StartTime>.*?),";
				} else if (item.Contains ("End")) {
					regex += "(?<EndTime>.*?),";
				} else if (item.Contains ("Text")) {
					regex += "(?<Text>.*)"; // should be last in format string; no comma or "?" in regex
				} else {
					// ignore until next comma
					regex += ".*?,";
				}
			}

			return regex;
		}

		/// <summary>
		/// Parse ASS files.
		/// </summary>
		/// <param name="settings">Settings.</param>
		/// <param name="stream">Stream.</param>
		public List<LineInfo> parse(Settings settings, Stream stream, Encoding encoding) {
			LinkedList<String> rawLines = new LinkedList<String> ();
			using(StreamReader reader = new StreamReader (stream, encoding)) {
				String line;
				while((line = reader.ReadLine()) != null) {
					rawLines.AddLast(line.Trim());
				}
			}

			return parse (settings, rawLines);
		}
	}
}
