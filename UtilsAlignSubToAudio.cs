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

namespace subtitleMemorize {
	class UtilsAlignSubToAudio {
#pragma warning disable 0414, 0169
		private static double m_stepSize = 0.5;
		private List<UtilsSubtitle.LineContainer<LineInfo>> m_lineContainers;
		private StreamInfo m_audioStreamInfo;
		private UtilsInputFiles.FileDesc m_audioFileDesc;
#pragma warning restore 0414, 0169

		public UtilsAlignSubToAudio(List<LineInfo> lines, UtilsInputFiles.FileDesc audioFile) {
			// TODO: m_lineContainers = UtilsSubtitle.GetNonOverlappingTimeSpans<LineInfo>(lines);
			//m_audioStreamInfo = UtilsVideo.ChooseStreamInfo(audioFile.filename, audioFile.properties, StreamInfo.StreamType.ST_AUDIO);
			m_audioFileDesc = audioFile;
		}

		/// <summary>
		/// Computes the time in seconds for that the lines have to be shifted fit optimal to the audio.
		/// </summary>
		public double GetBestShiftValue() {
			Console.WriteLine("Align to audio is currently not implemented");
			return 0;
		}
	}
}
