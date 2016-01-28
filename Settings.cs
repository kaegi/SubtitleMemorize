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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace subs2srs4linux
{
	/// <summary>
	/// Settings for one program instance.
	/// </summary>
	public static class InstanceSettings {
		public static String temporaryFilesPath = System.IO.Path.GetTempPath() + "subs2srs4linux/";

		public static SystemSettings systemSettings = new SystemSettings();
	}

	/// <summary>
	/// These values will be read from a xml file (through serialization) in the same path as the program.
	/// </summary>
	[Serializable]
	public class SystemSettings { 
		public float overlappingThreshold_InterSub = 0.4f;
		public float overlappingThreshold_InSub = 0.01f;

		public String formatProberCommand = "ffprobe";
		public String formatConvertCommand = "ffmpeg";

		public String preLoadedSettings = null;
	}
	
	[Serializable]
	public class PerSubtitleSettings {
		private int m_subDelay = 100; // in ms

		public int SubDelay {
			get { return m_subDelay; }
			set { m_subDelay = value; }
		}

		public PerSubtitleSettings() {
		}
	}
	
	[Serializable]
	public class Settings
	{
		private string m_targetFilePath;
		private string m_outputDirectoryPath;
		private string m_nativeFilePath;
		private string m_videoFilePath;

		private string m_deckName;
		private int m_firstEpisodeNumber;

		private PerSubtitleSettings[] m_perSubtitleSettings = { new PerSubtitleSettings(), new PerSubtitleSettings() };

		private bool m_ignoreStyledSubLines = true;
		private bool m_ignoreSingleLines = true; // single = lines without an obvious counterpart

		public string TargetFilePath {
			get { return m_targetFilePath;  }
			set { m_targetFilePath = value; }
		}

		public string OutputDirectoryPath {
			get { return m_outputDirectoryPath; }
			set { m_outputDirectoryPath = value; }
		}

		public string NativeFilePath {
			get { return m_nativeFilePath; }
			set { m_nativeFilePath = value; }
		}

		public string VideoFilePath {
			get { return m_videoFilePath; }
			set { m_videoFilePath = value; }
		}

		public string DeckName {
			get { return m_deckName; }
			set { m_deckName = value; }
		}

		public string DeckNameModified {
			get { return m_deckName == null ? null : m_deckName.Trim ().Replace (' ', '_'); }
		}

		public bool IgnoreStyledSubLines {
			get { return m_ignoreStyledSubLines; }
			set { m_ignoreStyledSubLines = value; }
		}

		public bool IgnoreSingleSubLines {
			get { return m_ignoreSingleLines; }
			set { m_ignoreSingleLines = value; }
		}

		public int FirstEpisodeNumber {
			get { return m_firstEpisodeNumber; }
			set { m_firstEpisodeNumber = value; }
		}


		public PerSubtitleSettings[] PerSubtitleSettings {
			get { return m_perSubtitleSettings; }
		}


		public Settings ()
		{
		}


		/// <summary>
		/// Sets the settings in instance to default values.
		/// </summary>
		public Settings DeepCopy(Settings original) {
			using (var ms = new MemoryStream())
			{
				var formatter = new BinaryFormatter();
				formatter.Serialize(ms, original);
				ms.Position = 0;

				return (Settings) formatter.Deserialize(ms);
			}
		}
	}
}

