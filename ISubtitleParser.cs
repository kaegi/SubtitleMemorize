using System;
using System.IO;

using System.Collections.Generic;
using System.Text;

namespace subs2srs4linux
{
	public interface ISubtitleParser
	{
		List<LineInfo> parse(Settings settings, Stream stream, Encoding encoding);
	}
}

