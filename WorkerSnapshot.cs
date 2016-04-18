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
	public static class WorkerSnapshot
	{
		public static List<String> ExtractSnaphots(Settings settings, String path, List<CardInfo> allEntries) {
			List<String> snapshotFieldValues = new List<string>(allEntries.Count);
			for(int i = 0; i < allEntries.Count; i++) {
				CardInfo CardInfo = allEntries[i];

				// create file at given path
				String outputSnapshotFilename = CardInfo.GetKey () + ".jpg";
				String outputSnapshotFilepath = path + Path.DirectorySeparatorChar + outputSnapshotFilename;

				// value that will be imported into Anki/SRS-Programs-Field
				// TODO: make this flexible
				snapshotFieldValues.Add("<img src=\"" + outputSnapshotFilename + "\"/>");

				// get file with snapshot information -> video
				UtilsInputFiles.FileDesc videoFileDesc = CardInfo.episodeInfo.VideoFileDesc;

				// extract image
				double timeStamp = UtilsCommon.GetMiddleTime (CardInfo);
				UtilsImage.GetImage (videoFileDesc.filename, timeStamp, outputSnapshotFilepath, 1);
			}
			return snapshotFieldValues;
		}
	}
}

