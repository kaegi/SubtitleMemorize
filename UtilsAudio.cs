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
using System.Text.RegularExpressions;

namespace subs2srs4linux
{
	public static class UtilsAudio
	{
		public static double GetVolume(String audioFile, StreamInfo audioStreamInfo, double fromSeconds, double timeSpanSeconds) {
			if(audioStreamInfo.StreamTypeValue !=  StreamInfo.StreamType.ST_AUDIO)
				throw new Exception("Tried to get volume of non-audio-stream");

			String arguments = String.Format("-ss {0} -t {1} -i \"{2}\" -map 0:{3} -af volumedetect -vn -f null /dev/null",
					UtilsCommon.ToTimeArg(fromSeconds),
					UtilsCommon.ToTimeArg(timeSpanSeconds),
					audioFile,
					audioStreamInfo.StreamIndex);

			String stderr = UtilsCommon.CallExeAndGetStdout(InstanceSettings.systemSettings.formatConvertCommand, arguments, false);
			String[] lines = stderr.Split('\n');
			for(int i = lines.Length - 1; i >= 0; i--) {
				String line = lines[i];
				if(line.Contains("mean_volume")) {
					Match match = Regex.Match(line.Trim(), @"\[Parsed_volumedetect_0 @ (.*?)\] mean_volume: (?<volume>.*) dB$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
					if(!match.Success) throw new Exception("Output of \"" + InstanceSettings.systemSettings.formatConvertCommand + "\" with filter \"volumedetect\" could not be parsed");
					return Double.Parse(match.Groups["volume"].ToString());
				}
			}

			throw new Exception("Output of \"" + InstanceSettings.systemSettings.formatConvertCommand + "\" with filter \"volumedetect\" could not be parsed");
		}
	}
}
