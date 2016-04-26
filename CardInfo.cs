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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace subtitleMemorize
{
	/// <summary>
	/// This class is closely related to the cards that will be generated.
	/// Every CardInfo-Instance, that isn't filtered away will be used
	/// for exactly one card.
	/// </summary>
	[Serializable]
	public class CardInfo : IComparable<CardInfo>, ITimeSpan {
		public List<LineInfo> targetLanguageLines;
		public List<LineInfo> nativeLanguageLines;
		public EpisodeInfo episodeInfo;
		public double startTimestamp;
		public double endTimestamp;
		public double audioStartTimestamp;
		public double audioEndTimestamp;
		public bool isActive;

		public double Duration {
			get { return UtilsCommon.GetTimeSpanLength(this); }
		}

		public CardInfo(List<LineInfo> targetLanguageLines,
										List<LineInfo> nativeLanguageLines,
			              EpisodeInfo episodeInfo,
			              double startTimestamp,
			              double endTimestamp,
			              double audioStartTimestamp,
			              double audioEndTimestamp) {
			this.targetLanguageLines = targetLanguageLines;
			this.nativeLanguageLines = nativeLanguageLines;
			this.episodeInfo         = episodeInfo;
			this.startTimestamp      = startTimestamp;
			this.endTimestamp        = endTimestamp;
			this.audioStartTimestamp = audioStartTimestamp;
			this.audioEndTimestamp   = audioEndTimestamp;
			this.isActive            = true;
		}

		/// <summary>
		/// Unifies two CardInfo into one (merging). The two CardInfo have to be compatible
		/// which can be checked with IsMergePossbile().
		/// </summary>
		public CardInfo(CardInfo first, CardInfo second) {
			this.targetLanguageLines = first.targetLanguageLines.Concat(second.targetLanguageLines).ToList();
			this.nativeLanguageLines = first.nativeLanguageLines.Concat(second.nativeLanguageLines).ToList();
			this.episodeInfo          = first.episodeInfo;
			this.startTimestamp       = Math.Min(first.startTimestamp, second.startTimestamp);
			this.endTimestamp         = Math.Max(first.endTimestamp, second.endTimestamp);
			this.audioStartTimestamp  = Math.Min(first.audioStartTimestamp, second.audioStartTimestamp);
			this.audioEndTimestamp		= Math.Max(first.audioEndTimestamp, second.audioEndTimestamp);
			this.isActive             = first.isActive || second.isActive;
		}

		public List<LineInfo> GetListByLanguageType(UtilsCommon.LanguageType languageType) {
			return languageType == UtilsCommon.LanguageType.NATIVE ? nativeLanguageLines : targetLanguageLines;
		}

		/// <summary>
		/// Replaces strings in line infos by regex.
		/// </summary>
		public void DoRegexReplace(UtilsCommon.LanguageType languageType, String pattern, String to) {
			foreach(var line in GetListByLanguageType(languageType)) {
				line.text = Regex.Replace(line.text, pattern, to);
			}
		}

		/// <summary>
		/// Checks whether two CardInfo instances can be merged. (If they
		/// are not in the same episode, in which episode is the new CardInfo?)
		/// </summary>
		public static bool IsMergePossbile(CardInfo a, CardInfo b) {
			if(a.episodeInfo != b.episodeInfo) return false;
			return true;
		}

		/// <summary>
		/// Returns some string that identifies this card information.
		/// </summary>
		/// <returns>The key.</returns>
		public String GetKey() {
			String str = String.Format ("{0:000.}", episodeInfo.Number) + "__" + UtilsCommon.ToTimeArg (startTimestamp) + "__" + UtilsCommon.ToTimeArg (endTimestamp);
			return Regex.Replace (str, "[^a-zA-Z0-9]", "_");
		}

		public double StartTime {
			get { return startTimestamp; }
		}

		public double EndTime {
			get { return endTimestamp; }
		}

		/// <summary>
		/// Compare lines based on their Start Times.
		/// </summary>
		public int CompareTo(CardInfo other) {
			if(this.episodeInfo.Index < other.episodeInfo.Index) return -1;
			if(this.episodeInfo.Index > other.episodeInfo.Index) return 1;
			if(StartTime == other.StartTime) return 0;
			return StartTime < other.StartTime ? -1 : 1;
		}

		private string ToString(UtilsCommon.LanguageType languageType, String separator="\n") {
			String str = "";
			bool isFirst = true;
			foreach(var line in GetListByLanguageType(languageType)) {
				if(isFirst) {
					str += line.text;
					isFirst = false;
				} else {
					str += separator + line.text;
				}
			}
			return str;
		}

		public string ToMultiLine(UtilsCommon.LanguageType languageType) {
			return ToString(languageType, "\n");
		}

		public string ToSingleLine(UtilsCommon.LanguageType languageType) {
			return ToString(languageType, " ");
		}

		private List<String> GetActors(UtilsCommon.LanguageType languageType) {
			var result = new List<String>();
			foreach(var line in GetListByLanguageType(languageType)) {
				if(String.IsNullOrWhiteSpace(line.name)) continue;
				result.Add(line.name);
			}
			return result;
		}

		public List<String> GetActors() {
			var list = GetActors(UtilsCommon.LanguageType.TARGET).Concat(GetActors(UtilsCommon.LanguageType.NATIVE)).Distinct().ToList();
			list.Sort();
			return list;
		}

		public String GetActorString() {
			StringBuilder stringBuilder = new StringBuilder();
			var actors = GetActors();
			foreach(var actor in actors) {
				stringBuilder.Append("<");
				stringBuilder.Append(actor);
				stringBuilder.Append("> ");
			}
			return stringBuilder.ToString();
		}

		/// <summary>
		/// Updates LineInfos. Strings for different LineInfos are separated by '\n'.
		/// Returns false if text could not be parsed.
		/// </summary>
		public bool SetLineInfosByMultiLineString(UtilsCommon.LanguageType languageType, string text) {
			var lines = GetListByLanguageType(languageType);
			var stringsRows = text.Split('\n').ToList();
			if(stringsRows.Count != lines.Count()) { return false; }
			for(int i = 0; i < lines.Count(); i++) { lines[i].text = stringsRows[i]; }
			return true;
		}
	}
}
