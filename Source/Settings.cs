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

namespace subtitleMemorize
{
	/// <summary>
	/// Settings for one program instance.
	/// </summary>
	public static class InstanceSettings {
		public static String temporaryFilesPath = System.IO.Path.GetTempPath() + "subtitleMemorize/";

		public static String settingsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SubtitileMemorize");
		public static String systemSettingFilePath = Path.Combine(settingsFolder, "settings.smem");

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

		public double normalizeTargetVolume = -16;

		public String preLoadedSettings = null;

		public double subToSubAlign_minGoodMatchingThreshold = 0.6;
	}

	[Serializable]
	public class PerSubtitleSettings {
		/// for automatic subtitle timing correction
		public enum AlignModes {
			ByConstantValue,
			ToSubtitle,
			ToAudio,
		}

		private AlignModes m_alignMode = AlignModes.ByConstantValue;
		private bool m_useTmingsOfThisSub;
		private double m_subDelay = 0; // in seconds

		public bool UseTimingsOfThisSub {
			get { return m_useTmingsOfThisSub; }
			set { m_useTmingsOfThisSub = value; }
		}
		public double SubDelay {
			get { return m_subDelay; }
			set { m_subDelay = value; }
		}

		public AlignModes AlignMode {
			get { return m_alignMode; }
			set { m_alignMode = value; }
		}

		public PerSubtitleSettings() {
		}
	}

	[Serializable]
	public class Settings
	{
		public enum RescaleModeEnum {
			NoRescaling,
			Downscale,
			UpscaleAndDownscale,
		}

		private string m_targetFilePath;
		private string m_outputDirectoryPath;
		private string m_nativeFilePath;
		private string m_videoFilePath;

		private string m_deckName;
		private int m_firstEpisodeNumber;

		private PerSubtitleSettings[] m_perSubtitleSettings = { new PerSubtitleSettings(), new PerSubtitleSettings() };

		private bool m_ignoreStyledSubLines = true;
		private bool m_ignoreSingleLines = true; // single = lines without an obvious counterpart

		private bool m_normalizeAudio = true;

		private bool m_exportAudio = true;
		private bool m_exportImages = true;

		private RescaleModeEnum m_rescaleMode = RescaleModeEnum.Downscale;

		private int m_imageRescaleWidth = 640;
		private int m_imageRescaleHeight = 360;

		private double m_audioPaddingBefore = 0;
		private double m_audioPaddingAfter = 0;


		public double AudioPaddingBefore {
			get { return m_audioPaddingBefore;  }
			set { m_audioPaddingBefore = value; }
		}

		public double AudioPaddingAfter {
			get { return m_audioPaddingAfter;  }
			set { m_audioPaddingAfter = value; }
		}

		public int RescaleHeight {
			get { return m_imageRescaleHeight;  }
			set { m_imageRescaleHeight = value; }
		}

		public int RescaleWidth {
			get { return m_imageRescaleWidth;  }
			set { m_imageRescaleWidth = value; }
		}

		public RescaleModeEnum RescaleMode {
			get { return m_rescaleMode; }
			set { m_rescaleMode = value; }
		}

		public bool NormalizeAudio {
			get { return m_normalizeAudio;  }
			set { m_normalizeAudio = value; }
		}

		public bool ExportImages {
			get { return m_exportImages;  }
			set { m_exportImages = value; }
		}

		public bool ExportAudio {
			get { return m_exportAudio;  }
			set { m_exportAudio = value; }
		}

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
