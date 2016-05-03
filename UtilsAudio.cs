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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace subtitleMemorize
{
	public static class UtilsAudio
	{
		public static double GetMaxVolume(String audioFile, StreamInfo audioStreamInfo) {
			if(audioStreamInfo.StreamTypeValue !=  StreamInfo.StreamType.ST_AUDIO)
				throw new Exception("Tried to get volume of non-audio-stream");

			String arguments = String.Format("-i \"{0}\" -map 0:{1} -af volumedetect -vn -f null /dev/null",
					audioFile,
					audioStreamInfo.StreamIndex);

			Console.WriteLine ("ffmpeg " + arguments);
			String stderr = UtilsCommon.StartProcessAndGetOutput(InstanceSettings.systemSettings.formatConvertCommand, arguments, true);
			String[] lines = stderr.Split('\n');
			for(int i = lines.Length - 1; i >= 0; i--) {
				String line = lines[i];
				if(line.Contains("max_volume")) {
					Match match = Regex.Match(line.Trim(), @"\[Parsed_volumedetect_0 @ (.*?)\] max_volume: (?<volume>[-.,0-9]+) dB$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
					if(!match.Success) throw new Exception("Output of \"" + InstanceSettings.systemSettings.formatConvertCommand + "\" with filter \"volumedetect\" could not be parsed");
					return Double.Parse(match.Groups["volume"].ToString(), CultureInfo.InvariantCulture);
				}
			}

			throw new Exception("Output of \"" + InstanceSettings.systemSettings.formatConvertCommand + "\" with filter \"volumedetect\" could not be parsed");
		}

		public static void NormalizeAudio(String filename, StreamInfo audioStreamInfo) {
			if(!filename.EndsWith("ogg")) throw new Exception("Only .ogg files are currently supported for normalizing!");
			double maxVolume = GetMaxVolume(filename, audioStreamInfo);
			const double targetVolume = -16; // in dB TODO: make audio normalize target configurable in settings

			String tmpFilename = InstanceSettings.temporaryFilesPath + Path.GetFileName(filename);
			String arguments = String.Format("-y -i \"{0}\" -af \"volume={1}dB\" -c:a libvorbis -vn \"{2}\"", filename, (-maxVolume + targetVolume).ToString(System.Globalization.CultureInfo.InvariantCulture), tmpFilename);
			Console.WriteLine ("ffmpeg " + arguments);

			UtilsCommon.StartProcessAndGetOutput(InstanceSettings.systemSettings.formatConvertCommand, arguments);

			// move new file to original position
			if(File.Exists(tmpFilename)) {
				File.Delete(filename);
				File.Move(tmpFilename, filename);
			}
		}
	}
}
