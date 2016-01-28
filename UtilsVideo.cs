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
using System.IO;
using System.Collections.Generic;

namespace subs2srs4linux
{
	public static class UtilsVideo
	{
		public static bool ExtractStream(String videoFilePath, StreamInfo streamInfo, String newSubtitleFilePath) {
			
			String argumentString = String.Format ("-y -v quiet -i \"{0}\" -map 0:{1} \"{2}\"", videoFilePath, streamInfo.StreamIndex, newSubtitleFilePath);
			UtilsCommon.CallExeAndGetStdout ("ffmpeg", argumentString);
			return true; // TODO: exception instead of return value
		}


		/// <summary>
		/// This function searches a container file by streamType. Let's say you want to extract an audio stream from
		/// a file with two audio streams. To identify the right one, the key "stream" is searched in "properties". The
		/// value for this key should be an index (this index refers to an entry of an array created by "StreamInfo.ReadAllStreams()",
		/// but not to the actual in-file stream for ffmpeg).
		/// 
		/// In case there is no property given, the first stream of this type is selected (in array from "StreamInfo.ReadAllStreams()").
		/// 
		/// </summary>
		/// <returns>The stream info describing the requested stream</returns>
		/// <param name="filename">Filename.</param>
		/// <param name="properties">Properties.</param>
		/// <param name="streamType">Stream type.</param>
		public static StreamInfo ChooseStreamInfo(String filename, Dictionary<String, String> properties, StreamInfo.StreamType streamType) {

			List<StreamInfo> allStreams = StreamInfo.ReadAllStreams (filename);

			// find first stream of given type in file (after that we also know whether the file has any of these streams)
			int firstSuitableStream = -1;
			for (int i = 0; i < allStreams.Count; i++) {
				if (allStreams [i].StreamTypeValue == streamType) {
					firstSuitableStream = i;
					break;
				}
			}

			if (firstSuitableStream == -1)
				throw new Exception ("Container file \"" + filename + "\" does not contain any " + streamType.GetPlainString() + " streams");


			int streamIndex = 0;
			String streamIndexDictionaryString = "";
			if (properties.TryGetValue ("stream", out streamIndexDictionaryString)) {
				try {
					streamIndex = Int32.Parse (streamIndexDictionaryString);
				} catch (Exception) {
					throw new Exception ("Stream property is not an integer.");
				}

				if (streamIndex < 0 || streamIndex >= allStreams.Count || allStreams [streamIndex].StreamTypeValue != streamType) {
					throw new Exception ("Stream with index " + streamIndex + " is not a " + streamType.GetPlainString()  + " stream (but stream at index " + firstSuitableStream + " is).");
				}

			} else
				streamIndex = firstSuitableStream;

			return allStreams [streamIndex];
		}
	}
}

