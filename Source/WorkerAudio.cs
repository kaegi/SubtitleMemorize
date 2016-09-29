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
		public static void ExtractAudio(Settings settings, String path, List<Tuple<CardInfo, String>> allEntries) {
			foreach(var entry in allEntries) {
				CardInfo cardInfo = entry.Item1;
				if(!cardInfo.HasAudio()) continue;

				String outputAudioFilename = entry.Item2;
				String outputAudioFilepath = path + Path.DirectorySeparatorChar + outputAudioFilename;

				UtilsInputFiles.FileDesc audioFileDesc = cardInfo.episodeInfo.AudioFileDesc;
				var audioStreamInfo = cardInfo.episodeInfo.AudioStreamInfo;

				String arguments = String.Format ("-v quiet -y -i \"{0}\" -map 0:{1} -ss \"{2}\" -to \"{3}\" -vn -c:a libvorbis \"{4}\"",
					audioFileDesc.filename, // input file
					audioStreamInfo.StreamIndex, // audio stream index
					UtilsCommon.ToTimeArg(cardInfo.audioStartTimestamp), // start time
					UtilsCommon.ToTimeArg(cardInfo.audioEndTimestamp), // end time
					outputAudioFilepath // output file
				);
				UtilsCommon.StartProcessAndGetOutput(InstanceSettings.systemSettings.formatConvertCommand, arguments);
			}


		}
	}
}
