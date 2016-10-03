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
using System.Runtime.Serialization.Formatters.Binary;

namespace subtitleMemorize
{
	public class UtilsCommon
	{
		public enum LanguageType {
			/// language that is known to the user
			NATIVE,

			/// language that the user wants to learn
			TARGET,
		}

		private UtilsCommon ()
		{

		}

		/// <summary>
		/// Calls a executable file by path by usings C# "Process" class. All errors of executable
		/// will be ignored (the function just returns "null" in this case).
		/// </summary>
		private static string CallExeAndGetOutput(string exePath, string args, bool stderrInsteadOfStdout=false) {
			Process process = new Process();

			try {
				process.StartInfo.FileName = exePath;
				process.StartInfo.Arguments = args;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.StartInfo.RedirectStandardError = true;
				process.Start();

				if(stderrInsteadOfStdout) return process.StandardError.ReadToEnd();
				else return process.StandardOutput.ReadToEnd();
			} catch {
				try {
					process.Kill();
				} catch(Exception) {
					// can't do anything about it at this point -> just ignore it
				}
			}

			return null; // failed
		}

		/// <summary>
		/// Executes a command with arguments.
		/// </summary>
		public static string StartProcessAndGetOutput(String exeName, String args, bool stderrInsteadOfStdout=false) {
			return CallExeAndGetOutput (exeName, args, stderrInsteadOfStdout);
		}

		/// <summary>
		/// Find maximum scaling value so that image just fits into box. Giving a negative value will deactivate scaling in that
		///	dimension (width or maxWidth less 0 means only height will be used).
		/// </summary>
		public static double GetMaxScaling(double width, double height, double maxWidth, double maxHeight, bool scale1atMax=true) {
			double scaling1 = width >= 0 && maxWidth >= 0 ? maxWidth / width : 1000000.0;
			double scaling2 = height >= 0 && maxHeight >= 0 ? maxHeight / height : 1000000.0;
			double scaling = Math.Min(scaling1, scaling2);
			if(scale1atMax) scaling = Math.Min(scaling, 1.0);
			return scaling;
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
		public static double GetMiddleTime (ITimeSpan timeSpan)
		{
			return timeSpan.StartTime * 0.5 + timeSpan.EndTime * 0.5;
		}

		public static String ToTimeArg (double seconds)
		{
			String sign = seconds < 0 ? "-" : "";
			seconds = Math.Abs(seconds);
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
		/// Returns true if "a" and "b" overlap, false otherwise. For both
		/// time spans the basic assumption is "start less or equal end".
		/// </summary>
		public static bool IsOverlapping(ITimeSpan a, ITimeSpan b) {
			return a.EndTime >= b.StartTime && a.StartTime <= b.EndTime;
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
		public static double OverlappingScore(ITimeSpan a, ITimeSpan b) {
			double overlappingSpan = UtilsCommon.OverlappingTimeSpan (a, b);
			double line1timeSpan = a.EndTime - a.StartTime;
			double line2timeSpan = b.EndTime - b.StartTime;

			// ignore matchings if there is next to no overlapping
			double line1score = (double)overlappingSpan / (double)line1timeSpan;
			double line2score = (double)overlappingSpan / (double)line2timeSpan;

			return (line1score + line2score) * 0.5f;
		}




		/// <summary>
		/// Align subtitle line by constant value, to audio or to other subtitle based on settings in "perSubSettings".
		/// </summary>
		public static void AlignSub (List<LineInfo> lineList, List<LineInfo> referenceList, EpisodeInfo epInfo, Settings settings, PerSubtitleSettings thisSubSettings)
		{
			switch(thisSubSettings.AlignMode) {
			case PerSubtitleSettings.AlignModes.ByConstantValue:
				UtilsSubtitle.ShiftByTime (lineList, thisSubSettings.SubDelay);
				break;
			case PerSubtitleSettings.AlignModes.ToAudio:
				UtilsAlignSubToAudio alignToAudio = new UtilsAlignSubToAudio (lineList, epInfo.AudioFileDesc);
				UtilsSubtitle.ShiftByTime (lineList, alignToAudio.GetBestShiftValue());
				break;
			case PerSubtitleSettings.AlignModes.ToSubtitle:
				if(referenceList == null) throw new Exception("Can not align subtitle to other non-existent subtitle.");
				UtilsAlignSubToSub alignToSub = new UtilsAlignSubToSub(lineList, referenceList);
				alignToSub.Retime();
				break;
			}
		}

		/// <summary>
		/// Returns the length of a ITimeSpan, which is "End - Start".
		/// </summary>
		public static double GetTimeSpanLength(ITimeSpan timeSpan) {
			return timeSpan.EndTime - timeSpan.StartTime;
		}

		public static double OverlappingTimeSpan(ITimeSpan a, ITimeSpan b) {
			if (!UtilsCommon.IsOverlapping (a, b))
				return 0;

			//  |-----------------------|  a
			//    |-------------------|    b
			if(a.StartTime <= b.StartTime && a.EndTime >= b.EndTime) return b.EndTime - b.StartTime;


			//    |-------------------|    a
			//  |-----------------------|  b
			if(a.StartTime >= b.StartTime && a.EndTime <= b.EndTime) return a.EndTime - a.StartTime;

			// |-------------------|    	a
			//   |-----------------------|  b
			if(a.StartTime <= b.StartTime && a.EndTime <= b.EndTime) return a.EndTime - b.StartTime;

			//   |-----------------------|  a
			// |-------------------|    	b
			return b.EndTime - a.StartTime;
		}

		public static double GetMinTimeSpanDistance(ITimeSpan a, ITimeSpan b) {
			if(IsOverlapping(a, b)) return 0;

			if(a.EndTime <= b.StartTime) return b.StartTime - a.EndTime;
			else return a.StartTime - b.EndTime;
		}

		public static bool DoesTimespanContain(ITimeSpan reference, ITimeSpan probablyContainedTimespan) {
			return probablyContainedTimespan.StartTime >= reference.StartTime && probablyContainedTimespan.EndTime <= reference.EndTime;
		}

		// change format that it can be contained in a .tsv file and anki can use it
		public static String HTMLify(String str) {
			return str.Replace("\n", "<br/>").Replace("\t", " ");
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

		public static T DeepClone<T>(T obj)
		{
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, obj);
				ms.Position = 0;

				return (T) formatter.Deserialize(ms);
			}
		}

		public static List<int> GetListFromTo(int start, int end) {
			return (from number in Enumerable.Range(start, end) select number).ToList();
		}

		public static void ClearDirectory(string path)
		{
			if (Directory.Exists(path)) Directory.Delete(path, true);
			Directory.CreateDirectory(path);
		}

		public static bool IsFfmpegAvailable() {
			var output = UtilsCommon.StartProcessAndGetOutput("ffmpeg", "-version");
			Console.WriteLine(output);
			if(output == null) return false;
			return output.StartsWith("ffmpeg version");
		}

		public static bool IsAvconvAvailable() {
			var output = UtilsCommon.StartProcessAndGetOutput("avconv", "-version");
			if(output == null) return false;
			return output.StartsWith("avconv version");
		}
	}
}
