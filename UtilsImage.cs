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
	public static class UtilsImage
	{
		public static bool GetImage(String videoFilename, DateTime time, String outFilename) {
			// TODO: correct scaling
			// TODO: ffmpeg -> settings-value
			String argumentString = String.Format ("-y -v quiet -an -ss {0} -i \"{1}\" -f image2 -vf \"{2}\" -vframes 1 \"{3}\"", UtilsCommon.ToTimeArg(time), videoFilename, "scale=300:-1", outFilename);
			UtilsCommon.CallExeAndGetStdout (InstanceSettings.systemSettings.formatConvertCommand, argumentString);
			return true;
		}
	}
}

