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

namespace subtitleMemorize
{
	public class EpisodeInfo
	{
		private readonly int m_index; // ranges from [0..numberOfEpisodes-1]
		private readonly int m_number; // ranges from [firstEpisodeNumber..firstEpisodeNumber+numberOfEpisode-1]
		private readonly UtilsInputFiles.FileDesc m_videoFileDesc;
		private readonly UtilsInputFiles.FileDesc m_audioFileDesc;
		private readonly UtilsInputFiles.FileDesc[] m_subsFileDesc = new UtilsInputFiles.FileDesc[2];
		private readonly StreamInfo m_videoStreamInfo;
		private readonly StreamInfo m_audioStreamInfo;
		private readonly StreamInfo[] m_subStreamInfos = new StreamInfo[2];

		public int Index {
			get { return m_index; }
		}

		/// <summary>
		/// An episode number is "index + first episode index (choosen by user)".
		/// </summary>
		/// <value>The number.</value>
		public int Number {
			get { return m_number; }
		}

		public UtilsInputFiles.FileDesc VideoFileDesc {
			get {
				return m_videoFileDesc;
			}
		}

		public UtilsInputFiles.FileDesc AudioFileDesc {
			get {
				return m_audioFileDesc;
			}
		}	

		public UtilsInputFiles.FileDesc[] SubsFileDesc {
			get {
				return m_subsFileDesc;
			}
		}

		public StreamInfo VideoStreamInfo {
			get {
				return m_videoStreamInfo;
			}
		}

		public StreamInfo AudioStreamInfo {
			get {
				return m_audioStreamInfo;
			}
		}

		public StreamInfo[] SubsStreamInfos {
			get {
				return m_subStreamInfos;
			}
		}



		public EpisodeInfo (int index,
				int number,
				UtilsInputFiles.FileDesc videoFileDesc,
				UtilsInputFiles.FileDesc audioFileDesc,
				UtilsInputFiles.FileDesc sub1FileDesc,
				UtilsInputFiles.FileDesc sub2FileDesc,
				StreamInfo videoStreamInfo,
				StreamInfo audioStreamInfo,
				StreamInfo sub1StreamInfo,
				StreamInfo sub2StreamInfo)
		{
			m_index = index;
			m_number = number;
			m_videoFileDesc = videoFileDesc;
			m_audioFileDesc = audioFileDesc;
			m_subsFileDesc[0] = sub1FileDesc;
			m_subsFileDesc[1] = sub2FileDesc;
			m_videoStreamInfo = videoStreamInfo;
			m_audioStreamInfo = audioStreamInfo;
			m_subStreamInfos[0] = sub1StreamInfo;
			m_subStreamInfos[1] = sub2StreamInfo;
		}
	}
}

