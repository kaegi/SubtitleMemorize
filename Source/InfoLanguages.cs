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

namespace subtitleMemorize
{
  public class InfoLanguages {

    private static List<InfoLanguages> GenerateLanguageList() {
      var languages = new List<InfoLanguages>();
      languages.Add(new InfoLanguages("Chinese", "chinese"));
      languages.Add(new InfoLanguages("Japanese", "japanese"));
      languages.Add(new InfoLanguages("English", "english"));
      languages.Add(new InfoLanguages("Spanish", "spanish"));
      languages.Add(new InfoLanguages("German", "german"));
      languages.Add(new InfoLanguages("Russian", "russian"));
      languages.Add(new InfoLanguages("Italian", "italian"));
      languages.Add(new InfoLanguages("Other language with spaces between words", "spaced_language"));
      languages.Add(new InfoLanguages("Other language (please create an issue on website)", ""));
      return languages;

    }

    public readonly static List<InfoLanguages> languages = GenerateLanguageList();

    public readonly string name;

    /// Tag that can be processed by MorphMan
    public readonly string tag;

    private InfoLanguages(String name, String tag) {
      this.name = name;
      this.tag = tag;
    }
  }
}
