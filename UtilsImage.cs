using System;

namespace subs2srs4linux
{
	public static class UtilsImage
	{
		public static bool GetImage(String videoFilename, DateTime time, String outFilename) {
			// TODO: correct scaling
			// TODO: ffmpeg -> settings-value
			String argumentString = String.Format ("-y -v quiet -an -ss {0} -i \"{1}\" -f image2 -vf \"{2}\" -vframes 1 \"{3}\"", UtilsCommon.ToTimeArg(time), videoFilename, "scale=300:-1", outFilename);
			UtilsCommon.CallExeAndGetStdout (ConstantSettings.formatConvertCommand, argumentString);
			return true;
		}
	}
}

