//  Copyright (C) 2011-2014 Christopher Brochtrup
//
//  This file is part of subs2srs.
//
//  subs2srs is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  subs2srs is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with subs2srs.  If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace subtitleMemorize
{
	/// <summary>
	/// Represents a text encoding.
	/// </summary>
	public class InfoEncoding
	{
		private string longName;
		private string shortName;
		private int codePage;

		// http://msdn.microsoft.com/en-us/library/system.text.encoding.aspx
		static private InfoEncoding[] encodingInfo = { 
	      new InfoEncoding( "Arabic (864)",                             "IBM864",                  864   ),
	      new InfoEncoding( "Arabic (ASMO 708)",                        "ASMO-708",                708   ),
	      new InfoEncoding( "Arabic (DOS)",                             "DOS-720",                 720   ),
	      new InfoEncoding( "Arabic (ISO)",                             "iso-8859-6",              28596 ),
	      new InfoEncoding( "Arabic (Mac)",                             "x-mac-arabic",            10004 ),
	      new InfoEncoding( "Arabic (Windows)",                         "windows-1256",            1256  ),
	      new InfoEncoding( "Baltic (DOS)",                             "ibm775",                  775   ),
	      new InfoEncoding( "Baltic (ISO)",                             "iso-8859-4",              28594 ),
	      new InfoEncoding( "Baltic (Windows)",                         "windows-1257",            1257  ),
	      new InfoEncoding( "Central European (DOS)",                   "ibm852",                  852   ),
	      new InfoEncoding( "Central European (ISO)",                   "iso-8859-2",              28592 ),
	      new InfoEncoding( "Central European (Mac)",                   "x-mac-ce",                10029 ),
	      new InfoEncoding( "Central European (Windows)",               "windows-1250",            1250  ),
	      new InfoEncoding( "Chinese Simplified (EUC)",                 "EUC-CN",                  51936 ),
	      new InfoEncoding( "Chinese Simplified (GB18030)",             "GB18030",                 54936 ),
	      new InfoEncoding( "Chinese Simplified (GB2312)",              "gb2312",                  936   ),
	      new InfoEncoding( "Chinese Simplified (GB2312-80)",           "x-cp20936",               20936 ),
	      new InfoEncoding( "Chinese Simplified (HZ)",                  "hz-gb-2312",              52936 ),
	      new InfoEncoding( "Chinese Simplified (ISO-2022)",            "x-cp50227",               50227 ),
	      new InfoEncoding( "Chinese Simplified (Mac)",                 "x-mac-chinesesimp",       10008 ),
	      new InfoEncoding( "Chinese Traditional (Big5)",               "big5",                    950   ),
	      new InfoEncoding( "Chinese Traditional (CNS)",                "x-Chinese-CNS",           20000 ),
	      new InfoEncoding( "Chinese Traditional (Eten)",               "x-Chinese-Eten",          20002 ),
	      new InfoEncoding( "Chinese Traditional (Mac)",                "x-mac-chinesetrad",       10002 ),
	      new InfoEncoding( "Croatian (Mac)",                           "x-mac-croatian",          10082 ),
	      new InfoEncoding( "Cyrillic (DOS)",                           "cp866",                   866   ),
	      new InfoEncoding( "Cyrillic (ISO)",                           "iso-8859-5",              28595 ),
	      new InfoEncoding( "Cyrillic (KOI8-R)",                        "koi8-r",                  20866 ),
	      new InfoEncoding( "Cyrillic (KOI8-U)",                        "koi8-u",                  21866 ),
	      new InfoEncoding( "Cyrillic (Mac)",                           "x-mac-cyrillic",          10007 ),
	      new InfoEncoding( "Cyrillic (Windows)",                       "windows-1251",            1251  ),
	      new InfoEncoding( "Estonian (ISO)",                           "iso-8859-13",             28603 ),
	      new InfoEncoding( "Europa",                                   "x-Europa",                29001 ),
	      new InfoEncoding( "French Canadian (DOS)",                    "IBM863",                  863   ),
	      new InfoEncoding( "German (IA5)",                             "x-IA5-German",            20106 ),
	      new InfoEncoding( "Greek (DOS)",                              "ibm737",                  737   ),
	      new InfoEncoding( "Greek (ISO)",                              "iso-8859-7",              28597 ),
	      new InfoEncoding( "Greek (Mac)",                              "x-mac-greek",             10006 ),
	      new InfoEncoding( "Greek (Windows)",                          "windows-1253",            1253  ),
	      new InfoEncoding( "Greek, Modern (DOS)",                      "ibm869",                  869   ),
	      new InfoEncoding( "Hebrew (DOS)",                             "DOS-862",                 862   ),
	      new InfoEncoding( "Hebrew (ISO-Logical)",                     "iso-8859-8-i",            38598 ),
	      new InfoEncoding( "Hebrew (ISO-Visual)",                      "iso-8859-8",              28598 ),
	      new InfoEncoding( "Hebrew (Mac)",                             "x-mac-hebrew",            10005 ),
	      new InfoEncoding( "Hebrew (Windows)",                         "windows-1255",            1255  ),
	      new InfoEncoding( "IBM EBCDIC (Arabic)",                      "IBM420",                  20420 ),
	      new InfoEncoding( "IBM EBCDIC (Cyrillic Russian)",            "IBM880",                  20880 ),
	      new InfoEncoding( "IBM EBCDIC (Cyrillic Serbian-Bulgarian)",  "cp1025",                  21025 ),
	      new InfoEncoding( "IBM EBCDIC (Denmark-Norway)",              "IBM277",                  20277 ),
	      new InfoEncoding( "IBM EBCDIC (Denmark-Norway-Euro)",         "IBM01142",                1142  ),
	      new InfoEncoding( "IBM EBCDIC (Finland-Sweden)",              "IBM278",                  20278 ),
	      new InfoEncoding( "IBM EBCDIC (Finland-Sweden-Euro)",         "IBM01143",                1143  ),
	      new InfoEncoding( "IBM EBCDIC (France)",                      "IBM297",                  20297 ),
	      new InfoEncoding( "IBM EBCDIC (France-Euro)",                 "IBM01147",                1147  ),
	      new InfoEncoding( "IBM EBCDIC (Germany)",                     "IBM273",                  20273 ),
	      new InfoEncoding( "IBM EBCDIC (Germany-Euro)",                "IBM01141",                1141  ),
	      new InfoEncoding( "IBM EBCDIC (Greek Modern)",                "cp875",                   875   ),
	      new InfoEncoding( "IBM EBCDIC (Greek)",                       "IBM423",                  20423 ),
	      new InfoEncoding( "IBM EBCDIC (Hebrew)",                      "IBM424",                  20424 ),
	      new InfoEncoding( "IBM EBCDIC (Icelandic)",                   "IBM871",                  20871 ),
	      new InfoEncoding( "IBM EBCDIC (Icelandic-Euro)",              "IBM01149",                1149  ),
	      new InfoEncoding( "IBM EBCDIC (International)",               "IBM500",                  500   ),
	      new InfoEncoding( "IBM EBCDIC (International-Euro)",          "IBM01148",                1148  ),
	      new InfoEncoding( "IBM EBCDIC (Italy)",                       "IBM280",                  20280 ),
	      new InfoEncoding( "IBM EBCDIC (Italy-Euro)",                  "IBM01144",                1144  ),
	      new InfoEncoding( "IBM EBCDIC (Japanese katakana)",           "IBM290",                  20290 ),
	      new InfoEncoding( "IBM EBCDIC (Korean Extended)",             "x-EBCDIC-KoreanExtended", 20833 ),
	      new InfoEncoding( "IBM EBCDIC (Multilingual Latin-2)",        "IBM870",                  870   ),
	      new InfoEncoding( "IBM EBCDIC (Spain)",                       "IBM284",                  20284 ),
	      new InfoEncoding( "IBM EBCDIC (Spain-Euro)",                  "IBM01145",                1145  ),
	      new InfoEncoding( "IBM EBCDIC (Thai)",                        "IBM-Thai",                20838 ),
	      new InfoEncoding( "IBM EBCDIC (Turkish Latin-5)",             "IBM1026",                 1026  ),
	      new InfoEncoding( "IBM EBCDIC (Turkish)",                     "IBM905",                  20905 ),
	      new InfoEncoding( "IBM EBCDIC (UK)",                          "IBM285",                  20285 ),
	      new InfoEncoding( "IBM EBCDIC (UK-Euro)",                     "IBM01146",                1146  ),
	      new InfoEncoding( "IBM EBCDIC (US-Canada)",                   "IBM037",                  37    ),
	      new InfoEncoding( "IBM EBCDIC (US-Canada-Euro)",              "IBM01140",                1140  ),
	      new InfoEncoding( "IBM Latin-1",                              "IBM01047",                1047  ),
	      new InfoEncoding( "IBM Latin-1",                              "IBM00924",                20924 ),
	      new InfoEncoding( "IBM5550 Taiwan",                           "x-cp20003",               20003 ),
	      new InfoEncoding( "Icelandic (DOS)",                          "ibm861",                  861   ),
	      new InfoEncoding( "Icelandic (Mac)",                          "x-mac-icelandic",         10079 ),
	      new InfoEncoding( "ISCII Assamese",                           "x-iscii-as",              57006 ),
	      new InfoEncoding( "ISCII Bengali",                            "x-iscii-be",              57003 ),
	      new InfoEncoding( "ISCII Devanagari",                         "x-iscii-de",              57002 ),
	      new InfoEncoding( "ISCII Gujarati",                           "x-iscii-gu",              57010 ),
	      new InfoEncoding( "ISCII Kannada",                            "x-iscii-ka",              57008 ),
	      new InfoEncoding( "ISCII Malayalam",                          "x-iscii-ma",              57009 ),
	      new InfoEncoding( "ISCII Oriya",                              "x-iscii-or",              57007 ),
	      new InfoEncoding( "ISCII Punjabi",                            "x-iscii-pa",              57011 ),
	      new InfoEncoding( "ISCII Tamil",                              "x-iscii-ta",              57004 ),
	      new InfoEncoding( "ISCII Telugu",                             "x-iscii-te",              57005 ),
	      new InfoEncoding( "ISO-6937",                                 "x-cp20269",               20269 ),
	      new InfoEncoding( "Japanese (EUC)",                           "euc-jp",                  51932 ),
	      new InfoEncoding( "Japanese (JIS 0208-1990 and 0212-1990)",   "EUC-JP",                  20932 ),
	      new InfoEncoding( "Japanese (JIS)",                           "iso-2022-jp",             50220 ),
	      new InfoEncoding( "Japanese (JIS-Allow 1 byte Kana - SO/SI)", "iso-2022-jp",             50222 ),
	      new InfoEncoding( "Japanese (JIS-Allow 1 byte Kana)",         "csISO2022JP",             50221 ),
	      new InfoEncoding( "Japanese (Mac)",                           "x-mac-japanese",          10001 ),
	      new InfoEncoding( "Japanese (Shift-JIS)",                     "shift_jis",               932   ),
	      new InfoEncoding( "Korean",                                   "ks_c_5601-1987",          949   ),
	      new InfoEncoding( "Korean (EUC)",                             "euc-kr",                  51949 ),
	      new InfoEncoding( "Korean (ISO)",                             "iso-2022-kr",             50225 ),
	      new InfoEncoding( "Korean (Johab)",                           "Johab",                   1361  ),
	      new InfoEncoding( "Korean (Mac)",                             "x-mac-korean",            10003 ),
	      new InfoEncoding( "Korean Wansung",                           "x-cp20949",               20949 ),
	      new InfoEncoding( "Latin 3 (ISO)",                            "iso-8859-3",              28593 ),
	      new InfoEncoding( "Latin 9 (ISO)",                            "iso-8859-15",             28605 ),
	      new InfoEncoding( "Nordic (DOS)",                             "IBM865",                  865   ),
	      new InfoEncoding( "Norwegian (IA5)",                          "x-IA5-Norwegian",         20108 ),
	      new InfoEncoding( "OEM Cyrillic",                             "IBM855",                  855   ),
	      new InfoEncoding( "OEM Multilingual Latin I",                 "IBM00858",                858   ),
	      new InfoEncoding( "OEM United States",                        "IBM437",                  437   ),
	      new InfoEncoding( "Portuguese (DOS)",                         "IBM860",                  860   ),
	      new InfoEncoding( "Romanian (Mac)",                           "x-mac-romanian",          10010 ),
	      new InfoEncoding( "Swedish (IA5)",                            "x-IA5-Swedish",           20107 ),
	      new InfoEncoding( "T.61",                                     "x-cp20261",               20261 ),
	      new InfoEncoding( "TCA Taiwan",                               "x-cp20001",               20001 ),
	      new InfoEncoding( "TeleText Taiwan",                          "x-cp20004",               20004 ),
	      new InfoEncoding( "Thai (Mac)",                               "x-mac-thai",              10021 ),
	      new InfoEncoding( "Thai (Windows)",                           "windows-874",             874   ),
	      new InfoEncoding( "Turkish (DOS)",                            "ibm857",                  857   ),
	      new InfoEncoding( "Turkish (ISO)",                            "iso-8859-9",              28599 ),
	      new InfoEncoding( "Turkish (Mac)",                            "x-mac-turkish",           10081 ),
	      new InfoEncoding( "Turkish (Windows)",                        "windows-1254",            1254  ),
	      new InfoEncoding( "Ukrainian (Mac)",                          "x-mac-ukrainian",         10017 ),
	      new InfoEncoding( "Unicode",                                  "utf-16",                  1200  ),
	      new InfoEncoding( "Unicode (Big endian)",                     "unicodeFFFE",             1201  ),
	      new InfoEncoding( "Unicode (UTF-32 Big endian)",              "utf-32BE",                12001 ),
	      new InfoEncoding( "Unicode (UTF-32)",                         "utf-32",                  12000 ),
	      new InfoEncoding( "Unicode (UTF-7)",                          "utf-7",                   65000 ),
	      new InfoEncoding( "Unicode (UTF-8)",                          "utf-8",                   65001 ),
	      new InfoEncoding( "US-ASCII",                                 "us-ascii",                20127 ),
	      new InfoEncoding( "Vietnamese (Windows)",                     "windows-1258",            1258  ),
	      new InfoEncoding( "Wang Taiwan",                              "x-cp20005",               20005 ),
	      new InfoEncoding( "Western European (DOS)",                   "ibm850",                  850   ),
	      new InfoEncoding( "Western European (IA5)",                   "x-IA5",                   20105 ),
	      new InfoEncoding( "Western European (ISO)",                   "iso-8859-1",              28591 ),
	      new InfoEncoding( "Western European (Mac)",                   "macintosh",               10000 ),
	      new InfoEncoding( "Western European (Windows)",               "Windows-1252",            1252  )
	    };

		/// <summary>
		/// The long name of the encoding. Example for Shift-JIS: Japanese (Shift-JIS).
		/// </summary>
		public string LongName {
			get { return longName; }
		}

		/// <summary>
		/// The short name of the encoding. Example for Shift-JIS: shift_jis.
		/// </summary>
		public string ShortName {
			get { return shortName; }
		}

		/// <summary>
		/// The code page of the encoding. Example for Shift-JIS: 932.
		/// </summary>
		public int CodePage {
			get { return codePage; }
		}


		public InfoEncoding (string longName, string shortName, int codePage)
		{
			this.longName = longName;
			this.shortName = shortName;
			this.codePage = codePage;
		}


		public static InfoEncoding[] getEncodings ()
		{
			return encodingInfo;
		}


		/// <summary>
		/// Convert a long name to a short name.
		/// </summary>
		public static string longToShort (string longName)
		{
			string shortName = "utf-8";

			foreach (InfoEncoding encoding in encodingInfo) {
				if (encoding.longName == longName) {
					shortName = encoding.shortName;
					break;
				}
			}

			return shortName;
		}


		/// <summary>
		/// Convert a short name to a long name.
		/// </summary>
		public static string shortToLong (string shortName)
		{
			string longName = "Unicode (UTF-8)";

			foreach (InfoEncoding encoding in encodingInfo) {
				if (encoding.shortName == shortName) {
					longName = encoding.longName;
					break;
				}
			}

			return longName;
		}


		/// <summary>
		/// Is the provided short name a valid encoding?
		/// </summary>
		public static bool isValidShortEncoding (string shortName)
		{
			bool valid = false;

			foreach (InfoEncoding encoding in encodingInfo) {
				if (encoding.shortName == shortName) {
					valid = true;
					break;
				}
			}

			return valid;
		}



	}
}
