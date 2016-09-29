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
		public static void ExtractSnaphots(Settings settings, String path, List<Tuple<CardInfo, String>> allEntries) {
			foreach(var entry in allEntries) {
				var cardInfoNameTuple = entry;
				var cardInfo = cardInfoNameTuple.Item1;
				if(!cardInfo.HasImage()) continue;

				// create file at given path
				String outputSnapshotFilename = cardInfoNameTuple.Item2;
				String outputSnapshotFilepath = path + Path.DirectorySeparatorChar + outputSnapshotFilename;

				// get file with snapshot information -> video
				UtilsInputFiles.FileDesc videoFileDesc = cardInfo.episodeInfo.VideoFileDesc;

				// extract image
				double scaling = UtilsVideo.GetMaxScalingByStreamInfo(cardInfo.episodeInfo.VideoStreamInfo, settings.RescaleWidth, settings.RescaleHeight, settings.RescaleMode);
				double timeStamp = UtilsCommon.GetMiddleTime (cardInfo);
				UtilsImage.GetImage (videoFileDesc.filename, timeStamp, outputSnapshotFilepath, scaling);
			}
		}
	}
}
