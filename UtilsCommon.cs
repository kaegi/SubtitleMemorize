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
using System.Diagnostics;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace subs2srs4linux
{
	public class UtilsCommon
	{
		private UtilsCommon ()
		{

		}

		// TODO: Rename
		public static string CallExeAndGetStdout(string exePath, string args, bool stdout=true) {
			Process process = new Process();

			try {
				process.StartInfo.FileName = exePath;
				process.StartInfo.Arguments = args;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.Start();

				if(stdout)
					return process.StandardOutput.ReadToEnd();
				else
					return process.StandardError.ReadToEnd();
			} catch {
				try {
					process.Kill();
				} catch(Exception) {
					// can't do anything anymore -> just ignore it
				}
			}

			return null; // failed
		}

		/// <summary>
		/// Filler function (possible future use). Try to find path by executable name.
		/// This includes relative paths and absolute/system directory paths.
		/// 
		/// This function does nothing else besides returning the initial "exeName" because
		/// calling a process by name already searches the system directories ("/usr/bin/", ...).
		/// </summary>
		/// <returns>The exe path.</returns>
		/// <param name="exeName">Exe name.</param>
		public static string FindExePath(String exeName) {
			return exeName;
		}

		public static string StartProcessGetStdout(String exeName, String args) {
			String exePath = FindExePath (exeName);
			return CallExeAndGetStdout (exePath, args);
		}


		/// <summary>
		/// The MIME mappings that are relevant for this project (video files and subtitles).
		/// </summary>
		private static readonly IDictionary<string, string> _mimeMappings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{

			{ ".*", "application/octet-stream" },
			{ ".mkv", "video/x-matroska" },
			{ ".mk3d", "video/x-matroska" },
			{ ".mka", "video/x-matroska" },
			{ ".mks", "video/x-matroska" },
			{ ".ssa", "text/x-ssa" },
			{ ".ass", "text/x-ass" },
			{ ".srt", "application/x-subrip" },
		};

		/// <summary>
		/// Gets the mimetype by filename.
		/// 
		/// Code from:
		/// 	https://github.com/mono/mono/blob/master/mcs/class/System.Web/System.Web/MimeMapping.cs
		/// </summary>
		/// <returns>The mimetype by filename.</returns>
		/// <param name="filename">Filename.</param>
		public static string GetMimetypeByFilename(string filename) {

			if (filename == null) {
				throw new ArgumentNullException ("filename");
			}
				
			string contentType = null;
			string extension = Path.GetExtension (filename);
			if (_mimeMappings.TryGetValue (extension, out contentType)) {
				return contentType;
			}
			return _mimeMappings [".*"];
		}

		public enum FileType {
			FT_VIDEO,
			FT_AUDIO,
			FT_SUBTITLE,
			FT_UNKNOWN
		}

		private readonly static String[] m_videoMimeTypes = {
			"video/x-matroska",
		};

		private readonly static String[] m_audioMimeTypes = {
			"audio/x-matroska",
		};

		private readonly static String[] m_subtitleMimeTypes = {
			"text/x-ass",
			"text/x-ssa",
			"application/x-subrip",
		};

		/// <summary>
		/// Assings mime type a file type of "FT_VIDEO", "FT_AUDIO" or "FT_SUBTILES" (and "FT_UNKNOWN" for other).
		/// </summary>
		/// <returns>The file type by MIME type.</returns>
		/// <param name="mimeType">MIME type.</param>
		public static FileType GetFileTypeByMimeType(String mimeType) {
			if (m_videoMimeTypes.Any (mimeType.Equals))
				return FileType.FT_VIDEO;
			if (m_audioMimeTypes.Any (mimeType.Equals))
				return FileType.FT_AUDIO;
			if (m_subtitleMimeTypes.Any (mimeType.Equals))
				return FileType.FT_SUBTITLE;
			return FileType.FT_UNKNOWN;
		}

		/// <summary>
		/// Shorthand for "GetFileTypeByMimeType (GetMimetypeByFilename(filename))"
		/// </summary>
		/// <returns>The file type by filename.</returns>
		/// <param name="filename">Filename.</param>
		public static FileType GetFileTypeByFilename(String filename) {
			return GetFileTypeByMimeType (GetMimetypeByFilename (filename));
		}

		/// <summary>
		/// Gets the middle time of two time stamps.
		/// </summary>
		/// <returns>The middle time.</returns>
		/// <param name="startTimestamp">Start timestamp.</param>
		/// <param name="endTimestamp">End timestamp.</param>
		public static DateTime GetMiddleTime (DateTime startTimestamp, DateTime endTimestamp)
		{
			DateTime middleTime = new DateTime ();
			return middleTime.AddMilliseconds (startTimestamp.TimeOfDay.TotalMilliseconds / 2 + endTimestamp.TimeOfDay.TotalMilliseconds / 2);
		}

		public static String ToTimeArg (double seconds)
		{
			String sign = seconds < 0 ? "-" : "";
			int milliseconds 	= (int)(seconds * 1000.0) % 1000;
			int iseconds 		= (int)(seconds * 1.0) % 60;
			int minutes 		= (int)(seconds / 60.0) % 60;
			int totalHours 		= (int)(seconds / (60.0  * 60.0));

			// Example: 00:00:07.920
			return String.Format("{0}{1:00.}:{2:00.}:{3:00.}.{4:000.}",
				sign,			 // {0}
				totalHours,      // {1}
				minutes,         // {2}
				iseconds,        // {3}
				milliseconds);   // {4}
		}


		public static String ToTimeArg (DateTime time)
		{
			// Example: 00:00:07.920
			return String.Format("{0:00.}:{1:00.}:{2:00.}.{3:000.}",
				(int)time.TimeOfDay.TotalHours,      // {0}
				(int)time.TimeOfDay.Minutes,         // {1}
				(int)time.TimeOfDay.Seconds,         // {2}
				(int)time.TimeOfDay.Milliseconds);   // {3}
		}

		/// <summary>
		/// Returns md5 hash that only contains 0-9 and A-F.
		/// </summary>
		/// <returns>The hash M d5.</returns>
		public static string GetHashMD5 (String filename) {
			using (var md5 = MD5.Create ()) {
				using (var stream = File.OpenRead (filename)) {
					return System.BitConverter.ToString(md5.ComputeHash (stream)).Replace("-", "");
				}
			}
		}

		/// <summary>
		/// Creates a checksum (only containing 0-9 and A-F) by using file size and modification date.
		/// </summary>
		/// <returns>The date size checksum.</returns>
		/// <param name="filename">Filename.</param>
		public static string GetDateSizeChecksum(String filename) {
			FileInfo fileInfo = new FileInfo (filename);
			long fileLength = fileInfo.Length;
			double modificationDate = fileInfo.LastWriteTimeUtc.TimeOfDay.TotalMilliseconds;
			String fileLengthChecksum = BitConverter.ToString (BitConverter.GetBytes (fileLength)).Replace("-", "");
			String modificationTimeChecksum = BitConverter.ToString (BitConverter.GetBytes (modificationDate)).Replace("-", "");
			return fileLengthChecksum + "_" + modificationTimeChecksum;
		}


		/// <summary>
		/// Returns a DateTime-Object that has the time of a - b.
		/// </summary>
		public static DateTime GetTimeDiff(DateTime a, DateTime b) {
			DateTime result = new DateTime();
			return result.AddMilliseconds(a.TimeOfDay.TotalMilliseconds - b.TimeOfDay.TotalMilliseconds);
		}

		/// <summary>
		/// Returns true if "a" and "b" overlap, false otherwise. For both
		/// time spans the basic assumption is "start less or equal end".
		/// </summary>
		public static bool IsOverlapping(ITimeSpan a, ITimeSpan b) {
			return a.EndTime >= b.StartTime && a.StartTime <= b.EndTime;
		}


		/// <summary>
		/// Align subtitle line by constant value, to audio or to other subtitle based on settings in "perSubSettings".
		/// </summary>
		public static void AlignSub (List<LineInfo> lineList, List<LineInfo> referenceList, EpisodeInfo epInfo, Settings settings, PerSubtitleSettings perSubSettings)
		{
			switch(perSubSettings.AlignMode) {
			case PerSubtitleSettings.AlignModes.ByConstantValue:
				UtilsSubtitle.ShiftByTime (lineList, perSubSettings.SubDelay);
				break;
			case PerSubtitleSettings.AlignModes.ToAudio:
				UtilsAlignSubToAudio alignToAudio = new UtilsAlignSubToAudio (lineList, epInfo.AudioFileDesc);
				UtilsSubtitle.ShiftByTime (lineList, alignToAudio.GetBestShiftValue());
				break;
			case PerSubtitleSettings.AlignModes.ToSubtitle:
				UtilsAlignSubToSub alignToSub = new UtilsAlignSubToSub(lineList, referenceList);
				alignToSub.Retime();
				break;
			}
		}


		/// <summary>
		/// Returns number of seconds for string in following format:
		/// [sign] [number] [:number]*n [.number]
		///
		/// so valid inputs are:
		///
		/// [nothing]
		/// 1
		/// 1.523454325
		/// .523
		/// -10:1.453
		/// 10:01.453
		/// -00:00.1
		/// 00:00
		/// 10:10:40:10:10
		/// ...
		///
		/// The number behind '.' is in the range of 0-1 seconds. The first number in front of the '.' is the number of seconds.
		/// Every number to the left is multiplied by 60. Examples:
		///
		/// [nothing] -> 0 seconds
		/// 1 -> 1 second
		/// -1.5 -> -1.5 seconds
		/// 1:1.5 -> 61.5 seconds
		/// </summary>
		public static double ParseTimeString(String s) {
			// safty checks
			if(String.IsNullOrWhiteSpace(s)) return 0;
			s = s.Trim();

			// only one dot is allowed
			int numberOfDots = s.Length - s.Replace(".", "").Length;
			if(numberOfDots > 1) throw new FormatException();

			// split string in two parts: one in front of dot and one behind
			String[] splitByDot = null;
			if(numberOfDots == 0) splitByDot = new String[] { s, "" };
			else splitByDot = (" " + s + " ").Split('.'); // these spaces ensure that there are two Strings in array

			// parse all values in front of dot
			String[] splitByColon = splitByDot[0].Split(':');
			int factor = 1;
			double seconds = 0.0;
			for(int i = splitByColon.Length - 1; i >= 0; i--) {
				if(!String.IsNullOrWhiteSpace(splitByColon[i]))
					seconds += factor * Int32.Parse(splitByColon[i]);
				factor *= 60;
			}

			// parse value behind dot
			String fracSeconds = splitByDot[1];
			fracSeconds.Trim();
			if(fracSeconds != "")
				seconds += Int32.Parse(splitByDot[1]) / Math.Pow(10, fracSeconds.Length);

			return seconds;
		}
	}
}

