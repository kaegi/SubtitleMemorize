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
	public static class UtilsImage
	{
		/// <summary>
		/// Extracts an image from a video file at a given time and resizes it. Resizing preserves aspect ratio.
		/// </summary>
		public static bool GetImage(String videoFilename, double time, String outFilename, double scale=1) {
			String argumentString = String.Format ("-y -v quiet -an -ss {0} -i \"{1}\" -f image2 -vf \"scale=iw*{2}/3628800:ih*{2}/3628800\" -vframes 1 \"{3}\"",
					UtilsCommon.ToTimeArg(time),
					videoFilename,
					(int)(scale*3628800), // int is needed because double will create values with comma (e.g "0,1"); 3628800=10! (has many divisors)
					outFilename);
			UtilsCommon.StartProcessAndGetOutput(InstanceSettings.systemSettings.formatConvertCommand, argumentString);
			return true;
		}
	}
}
