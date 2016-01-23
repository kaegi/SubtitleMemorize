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

namespace subs2srs4linux
{
	public class EpisodeInfo
	{
		private readonly UtilsInputFiles.FileDesc m_videoFileDesc;
		private readonly UtilsInputFiles.FileDesc m_audioFileDesc;
		private readonly UtilsInputFiles.FileDesc[] m_subsFileDesc = new UtilsInputFiles.FileDesc[2];

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

	

		public EpisodeInfo (UtilsInputFiles.FileDesc videoFileDesc, UtilsInputFiles.FileDesc audioFileDesc, UtilsInputFiles.FileDesc sub1FileDesc, UtilsInputFiles.FileDesc sub2FileDesc)
		{
			m_videoFileDesc = videoFileDesc;
			m_audioFileDesc = audioFileDesc;
			m_subsFileDesc[0] = sub1FileDesc;
			m_subsFileDesc[1] = sub2FileDesc;
		}
	}
}

