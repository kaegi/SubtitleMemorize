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

namespace subs2srs4linux
{
	/// <summary>
	/// This class provides a way of retiming every single line in
	/// a list to another line list (of a different subtitle file)
	/// solely based on comparing their timestamps.
	///
	/// Commercial breaks are supported as well.
	/// </summary>
	public class UtilsAlignSubToSub
	{
		private List<LineInfo> m_listToChange;
		private List<LineInfo> m_referenceList;

		public UtilsAlignSubToSub (List<LineInfo> listToChange, List<LineInfo> referenceList)
		{
			m_listToChange = listToChange;
			m_referenceList = referenceList;
		}

		public void Retime() {
			List<UtilsSubtitle.LineContainer> lineContainer1 = UtilsSubtitle.GetNonOverlappingTimeSpans(m_listToChange);
			List<UtilsSubtitle.LineContainer> lineContainer2 = UtilsSubtitle.GetNonOverlappingTimeSpans(m_referenceList);
		}
	}
}

