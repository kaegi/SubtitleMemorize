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
using System.Xml;

namespace subtitleMemorize
{
	[Serializable]
	public class StreamInfo
	{

		[Serializable]
		public class StreamType {

			public static readonly StreamType ST_SUBTITLE = new StreamType (0, "subtitle");
			public static readonly StreamType ST_AUDIO = new StreamType (1, "audio");
			public static readonly StreamType ST_VIDEO = new StreamType (2, "video");
			public static readonly StreamType ST_UNKNOWN = new StreamType (3, "unknown"); // others streams like fonts

			private readonly String plainString;

			private StreamType(int index, String plainString) {
				this.plainString = plainString;
			}

			/// <summary>
			/// Returns "audio", "video", "subtitle" or "unknown".
			/// </summary>
			/// <returns>The plain string.</returns>
			public String GetPlainString() {
				return plainString;
			}
		}

		private StreamType m_streamType; // ST_SUBTITLE, ST_VIDEO, ...
		private String m_language; // Japanese, English, ...
		private int m_streamIndex;
		private String m_streamName; // h264, ssa, ttf, ...
		private Dictionary<String, String> m_attributeDictionary;

		public StreamType StreamTypeValue {
			get { return m_streamType; }
		}

		public String StreamName {
			get { return m_streamName; }
		}

		public String Language {
			get { return GetLanguageByLanguageCode(m_language); }
		}

		public int StreamIndex {
			get { return m_streamIndex; }
		}


		private StreamInfo (int streamIndex, StreamType st, String streamName, Dictionary<String, String> attributeDictionary) {
			m_streamIndex = streamIndex;
			m_streamType = st;
			m_language = null;
			m_streamName = streamName;
			m_attributeDictionary = attributeDictionary;
		}


		/// <summary>
		/// ffmpeg generates a lot of information about streams as a key-value pair like
		///
		///		index:"7"
		///		codec_type:"attachment"
		///		...
		///
		/// This function returns the value for a key, or "null" if key was not found.
		/// </summary>
		public String GetAttribute(String name) {
			String ret = null;
			if(!m_attributeDictionary.TryGetValue(name, out ret)) return null;
			return ret;
		}

		/// <summary>
		/// See <see cref="GetAttribute"> for general information. This method finds the value
		/// as string in dicionary and then tries to parse it as int. Returns "null" if key was
		/// not found or there was an error while parsing.
		/// </summary>
		public Int32? GetAttributeInt(String name) {
			String val = GetAttribute(name);
			int ret = 0;
			if(!Int32.TryParse(val, out ret)) return null;
			return ret;
		}

		/// <summary>
		/// Returns true if ffmpeg created attribute with key "name".
		/// </summary>
		public bool HasAttribute(String name) {
			return m_attributeDictionary.ContainsKey(name);
		}

		/// <summary>
		/// Gets the language by short language code ("ja" -> "Japanese").
		/// </summary>
		/// <returns>Full language string.</returns>
		/// <param name="l">L.</param>
		private String GetLanguageByLanguageCode(String l) {
			return l; // TODO
		}


		/// <summary>
		/// Uses ffprobe/avprobe to query all audio, video and subtitle streams in a container file.
		/// </summary>
		public static List<StreamInfo> ReadAllStreams(String filename) {

			// use ffprobe/avprobe(?) to get nice XML-description of contents
			String stdout = UtilsCommon.StartProcessAndGetOutput(InstanceSettings.systemSettings.formatProberCommand, @"-v quiet -print_format xml -show_streams """ + filename + @"""");

			List<StreamInfo> allStreamInfos = new List<StreamInfo> ();
			StreamInfo lastStreamInfo = null;

			// use XmlReader to read all informations from "stream"-tags and "tag"-tags
			using (XmlReader reader = XmlReader.Create(new StringReader (stdout))) {
				while (reader.Read ()) {
					if (reader.NodeType != XmlNodeType.Element)
						continue;

					// the "stream"-tag contains most of the information needed as attributes
					if (reader.Name == "stream") {
						// get stream type
						StreamType streamType;
						switch(reader["codec_type"]) {
						case "video": streamType = StreamType.ST_VIDEO; break;
						case "audio": streamType = StreamType.ST_AUDIO; break;
						case "subtitle": streamType = StreamType.ST_SUBTITLE; break;
						default: streamType = StreamType.ST_UNKNOWN; break;
						}

						// read all other information into dictionary
						var attributeDictionary = new Dictionary<String, String>();
						for(int i = 0; i < reader.AttributeCount; i++) {
							reader.MoveToNextAttribute();
							attributeDictionary.Add(reader.Name, reader.Value);
						}

						StreamInfo streamInfo = new StreamInfo (Int32.Parse(reader["index"]), streamType, reader["codec_name"], attributeDictionary);
						allStreamInfos.Add (streamInfo);
						lastStreamInfo = streamInfo;
					}

					// the "tag"-tag provides additonal information (mainly language)
					if (reader.Name == "tag") {
						if (lastStreamInfo == null)
							continue;

						switch (reader ["key"]) {
						case "language":
							lastStreamInfo.m_language = reader ["value"] ?? "";
							break;
						default:
							break;
						}
					}
				}
			}

			return allStreamInfos;
		}
	}
}
