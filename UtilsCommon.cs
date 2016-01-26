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

		public static string CallExeAndGetStdout(string exePath, string args) {
			Process process = new Process();

			try {
				process.StartInfo.FileName = exePath;
				process.StartInfo.Arguments = args;
				process.StartInfo.UseShellExecute = false;
				process.StartInfo.CreateNoWindow = true;
				process.StartInfo.RedirectStandardOutput = true;
				process.Start();

				return process.StandardOutput.ReadToEnd();
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
	}
}

