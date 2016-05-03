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

namespace subtitleMemorize
{
	public static class WorkerAudio
	{
		public static List<String> ExtractAudio(Settings settings, String path, List<CardInfo> allEntries) {
			List<String> audioFieldValues = new List<string>(allEntries.Count);
			List<String> audioFilepaths = new List<string>(allEntries.Count);
			for(int i = 0; i < allEntries.Count; i++) {
				CardInfo CardInfo = allEntries[i];

				String outputAudioFilename = CardInfo.GetKey () + ".ogg";
				String outputAudioFilepath = path + Path.DirectorySeparatorChar + outputAudioFilename;
				audioFieldValues.Add("[sound:" + outputAudioFilename + "]");
				audioFilepaths.Add(outputAudioFilepath);

				UtilsInputFiles.FileDesc audioFileDesc = CardInfo.episodeInfo.AudioFileDesc;
				var audioStreamInfo = CardInfo.episodeInfo.AudioStreamInfo;

				String arguments = String.Format ("-v quiet -y -i \"{0}\" -map 0:{1} -ss \"{2}\" -to \"{3}\" -vn -c:a libvorbis \"{4}\"",
					audioFileDesc.filename, // input file
					audioStreamInfo.StreamIndex, // audio stream index
					UtilsCommon.ToTimeArg(CardInfo.startTimestamp), // start time
					UtilsCommon.ToTimeArg(CardInfo.endTimestamp), // end time
					outputAudioFilepath // output file
				);
				Console.WriteLine ("ffmpeg " + arguments);
				UtilsCommon.StartProcessAndGetOutput(InstanceSettings.systemSettings.formatConvertCommand, arguments);
			}
			if(settings.NormalizeAudio) {
				foreach(String filepath in audioFilepaths) {
					var audioStreamInfos = StreamInfo.ReadAllStreams(filepath);
					audioStreamInfos.RemoveAll(streamInfo => streamInfo.StreamTypeValue != StreamInfo.StreamType.ST_AUDIO);
					if(audioStreamInfos.Count != 1) {
						Console.WriteLine("Skipped normalizing file \"{0}\" because it contains {1} audio streams", filepath, audioStreamInfos.Count);
						continue;
					}
					try {
						UtilsAudio.NormalizeAudio(filepath, audioStreamInfos[0]);
					} catch(Exception e) {
						Console.WriteLine(e.ToString());
						continue;
					}
				}
			}
			return audioFieldValues;
		}
	}
}
