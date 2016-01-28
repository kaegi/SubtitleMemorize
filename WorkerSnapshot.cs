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
	public static class WorkerSnapshot
	{
		public static List<String> ExtractSnaphots(Settings settings, String path, List<UtilsSubtitle.EntryInformation> allEntries) {
			List<String> snapshotFieldValues = new List<string>(allEntries.Count);
			Dictionary<UtilsInputFiles.FileDesc, StreamInfo> choosenStreamInfo = new Dictionary<UtilsInputFiles.FileDesc, StreamInfo> ();
			for(int i = 0; i < allEntries.Count; i++) {
				UtilsSubtitle.EntryInformation entryInformation = allEntries[i];

				// create file at given path
				String outputSnapshotFilename = entryInformation.GetKey () + ".jpg";
				String outputSnapshotFilepath = path + Path.DirectorySeparatorChar + outputSnapshotFilename;

				// value that will be imported into Anki/SRS-Programs-Field
				// TODO: make this flexible
				snapshotFieldValues.Add("<img src=\"" + outputSnapshotFilename + "\"]");

				// get file with snapshot information -> video
				UtilsInputFiles.FileDesc videoFileDesc = entryInformation.episodeInfo.VideoFileDesc;

				// the operation of reading all streams is slow (ca 1s) because it analyses whole the file -> cache result
				StreamInfo videoStreamInfo = null;
				if (choosenStreamInfo.ContainsKey (videoFileDesc)) {
					videoStreamInfo = choosenStreamInfo [videoFileDesc];
				} else {
					videoStreamInfo = UtilsVideo.ChooseStreamInfo (videoFileDesc.filename, videoFileDesc.properties, StreamInfo.StreamType.ST_VIDEO);
					choosenStreamInfo.Add (videoFileDesc, videoStreamInfo);
				}

				// extract image
				DateTime timeStamp = UtilsCommon.GetMiddleTime (entryInformation.startTimestamp, entryInformation.endTimestamp);
				UtilsImage.GetImage (videoFileDesc.filename, timeStamp, outputSnapshotFilepath);
			}
			return snapshotFieldValues;
		}
	}
}

