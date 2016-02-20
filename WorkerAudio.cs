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

namespace subs2srs4linux
{
	public static class WorkerAudio
	{
		public static List<String> ExtractAudio(Settings settings, String path, List<UtilsSubtitle.EntryInformation> allEntries) {
			List<String> audioFieldValues = new List<string>(allEntries.Count);
			Dictionary<UtilsInputFiles.FileDesc, StreamInfo> choosenStreamInfo = new Dictionary<UtilsInputFiles.FileDesc, StreamInfo> ();
			for(int i = 0; i < allEntries.Count; i++) {
				UtilsSubtitle.EntryInformation entryInformation = allEntries[i];

				String outputAudioFilename = entryInformation.GetKey () + ".ogg";
				String outputAudioFilepath = path + Path.DirectorySeparatorChar + outputAudioFilename;
				audioFieldValues.Add("[sound:" + outputAudioFilename + "]");

				UtilsInputFiles.FileDesc audioFileDesc = entryInformation.episodeInfo.AudioFileDesc;


				// the operation of reading all streams is slow (ca 1s) because it analyses whole the file -> cache result
				StreamInfo audioStreamInfo = null;
				if (choosenStreamInfo.ContainsKey (audioFileDesc)) {
					audioStreamInfo = choosenStreamInfo [audioFileDesc];
				} else {
					audioStreamInfo = UtilsVideo.ChooseStreamInfo (audioFileDesc.filename, audioFileDesc.properties, StreamInfo.StreamType.ST_AUDIO);
					choosenStreamInfo.Add (audioFileDesc, audioStreamInfo);
				}


				String arguments = String.Format ("-v quiet -y -i \"{0}\" -map 0:{1} -ss \"{2}\" -to \"{3}\" -vn -c:a libvorbis \"{4}\"",
					audioFileDesc.filename, // input file
					audioStreamInfo.StreamIndex, // audio stream index
					UtilsCommon.ToTimeArg(entryInformation.startTimestamp), // start time
					UtilsCommon.ToTimeArg(entryInformation.endTimestamp), // end time
					outputAudioFilepath // output file
				);
				Console.WriteLine ("ffmpeg " + arguments);
				UtilsCommon.StartProcessAndGetOutput(InstanceSettings.systemSettings.formatConvertCommand, arguments);
			}
			return audioFieldValues;
		}
	}
}
