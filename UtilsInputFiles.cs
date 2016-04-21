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
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.IO;

namespace subtitleMemorize
{
	public class UtilsInputFiles
	{

		[Serializable]
		public class FileDesc {
			public readonly String filename; // can be whitespace
			public readonly Dictionary<String, String> properties;

			public FileDesc(String filename, Dictionary<String, String> properties) {
				this.filename = filename ?? "";
				this.properties = properties;
			}
		};

		private class DataEntry {
			public String filename;

			/// <summary>
			/// The properties of file like text encoding, important stream for container formats, etc.
			///
			/// This dictionary can be null, which means that this file has the same properties as previous file.
			/// In case the dictionary of the first file is null it means that no properties are set.
			/// </summary>
			public Dictionary<String, String> properties;

			public DataEntry(String filename, Dictionary<String, String> properties) {
				this.filename = filename;
				this.properties = properties;
			}
		};

		private List<DataEntry> m_dataEntries = new List<DataEntry>();

		// ParserStates -> expresses most recently finished token
		enum PS {
			PLIST_OPEN, 					// expect ">" or key
			PLIST_KEY,						// expect "="
			PLIST_ASSIGN,					// expect value
			PLIST_VALUE,					// expect comma or
			PLIST_COMMA, 					// expect key
			PLIST_CLOSE,					// expect filename or ","

			FLIST_COMMA,					// expect filename, property list or ","
			FLIST_FILENAME,					// expect filename

			NUM_PARSER_STATES
		};

		// TokenType
		enum TT {
			OPEN,					// "<"
			CLOSE,					// ">"
			COMMA,					// ","
			ASSIGN,					// "="
			STRING,					// every other string

			NUM_TOKEN_TYPES
		};

		private TT StringToTokenType(String str) {
			switch (str) {
			case "<": return TT.OPEN;
			case ">": return TT.CLOSE;
			case ",": return TT.COMMA;
			case "=": return TT.ASSIGN;
			default: return TT.STRING;
			}
		}



		public UtilsInputFiles (String inputString) {
			// nothing to parse?
			if (String.IsNullOrWhiteSpace (inputString))
				return;

			// tokenize input strings which are easier to parse (see documentation of function)
			String[] tokens = TokenizeInputString (inputString);

			// -----------------------------------------------------------------
			// parse tokens

			PS parserState;
			String lastKey = "";
			Dictionary<String, String> curDict = null;

			// parse tokens with "Finite-State Machine"
			Action<String>[,] stateMachineMapping = new Action<String>[(int)PS.NUM_PARSER_STATES, (int)TT.NUM_TOKEN_TYPES];
			for (int parserStateIndex = 0; parserStateIndex < (int)PS.NUM_PARSER_STATES; parserStateIndex++) {
				stateMachineMapping [parserStateIndex, (int)TT.OPEN]	= delegate { throw new Exception("Unexpected '<'"); };
				stateMachineMapping [parserStateIndex, (int)TT.CLOSE]	= delegate { throw new Exception("Unexpected '>'"); };
				stateMachineMapping [parserStateIndex, (int)TT.COMMA]	= delegate { throw new Exception("Unexpected ','"); };
				stateMachineMapping [parserStateIndex, (int)TT.ASSIGN]	= delegate { throw new Exception("Unexpected '='"); };
				stateMachineMapping [parserStateIndex, (int)TT.STRING]	= delegate(String str) { throw new Exception("Unexpected string \"" + str + "\""); };
			}

			// some more functional goodness
			Action<PS, TT, PS, Action<String>> map = delegate(PS _parserState, TT _tokenType, PS _nextParserState, Action<string> _action) {
				stateMachineMapping[(int)_parserState, (int)_tokenType] = delegate(String str) {
					if(_action != null) _action(str);
					parserState = _nextParserState;
				};
			};


			// create connections/mappings for state machine
			map (PS.PLIST_OPEN, 		TT.CLOSE,		PS.PLIST_CLOSE,		  null                                                                                     );
			map (PS.PLIST_OPEN, 		TT.STRING,	PS.PLIST_KEY,		    (String str)   => { lastKey = str; }                                                     );
			map (PS.PLIST_KEY, 			TT.ASSIGN,	PS.PLIST_ASSIGN, 	  null                                                                                     );
			map (PS.PLIST_ASSIGN,		TT.STRING,	PS.PLIST_VALUE,		  (String str) => { curDict.Add(lastKey, str); }                                           );
			map (PS.PLIST_VALUE,		TT.COMMA,		PS.PLIST_COMMA,		  null                                                                                     );
			map (PS.PLIST_VALUE,		TT.CLOSE,		PS.PLIST_CLOSE,		  null                                                                                     );
			map (PS.PLIST_COMMA,		TT.STRING,	PS.PLIST_KEY,		    (String str) => { lastKey = str; }                                                       );
			map (PS.PLIST_CLOSE,		TT.STRING,	PS.FLIST_FILENAME,	(String str) => { m_dataEntries.Add (new DataEntry (str, curDict)); curDict = null; }    );
			map (PS.FLIST_COMMA, 		TT.OPEN,		PS.PLIST_OPEN,		  (String str) => { curDict = new Dictionary<String, String>(); }                          );
			map (PS.FLIST_COMMA, 		TT.STRING, 	PS.FLIST_FILENAME,	(String str) => { m_dataEntries.Add (new DataEntry (str, curDict)); curDict = null; }    );
			map (PS.FLIST_COMMA, 		TT.COMMA, 	PS.FLIST_COMMA,		  (String str) => { m_dataEntries.Add (new DataEntry (null, curDict)); curDict = null; }   );
			map (PS.FLIST_FILENAME, TT.COMMA, 	PS.FLIST_COMMA,		  null                                                                                     );


			// follow state machine to end
			parserState = PS.FLIST_COMMA;
			foreach (String token in tokens)
				stateMachineMapping [(int)parserState, (int)StringToTokenType (token)] (EscapeSequencesToNormalChar (token));


			// depending on last state there could be errors
			switch (parserState) {
			case PS.PLIST_CLOSE:
			case PS.FLIST_COMMA:
				// insert missing file
				m_dataEntries.Add (new DataEntry (null, curDict));
				break;

			case PS.PLIST_OPEN:
			case PS.PLIST_COMMA:
			case PS.PLIST_KEY:
			case PS.PLIST_ASSIGN:
			case PS.PLIST_VALUE:
				throw new Exception ("Property list didn't get finished! Missing '>'!");

			case PS.FLIST_FILENAME:
				// add null-file
				break;
			}



		}

		/// <summary>
		/// Splits the input string on special charcters
		/// '&lt;', &gt;', '=' and ','. In case these characters
		/// are precedented by '\' they become normal characters and the
		/// string will not be split at this point.
		///
		/// &lt;encoding=utf-8,&gt;
		/// </summary>
		/// <example></example>
		/// <returns>The on markers.</returns>
		private String[] TokenizeInputString(String inputStr) {
			LinkedList<String> strings = new LinkedList<String> ();


			int beginInterval = 0;
			int intervalLength = 0;
			bool nextIsEscapeChar = false;
			while (true) {
				char c = inputStr [beginInterval + intervalLength];

				if (nextIsEscapeChar) {
					if (c != '<' && c != '>' && c != '=' && c != ',' && c != '\\')
						throw new Exception ("Unknow escape character '" + c.ToString () + "'");
					nextIsEscapeChar = false;
					// nothing special, because this character is normal
				} else if (c == '\\') {
					nextIsEscapeChar = true;
				} else {
					if (c == '<' || c == '>' || c == '=' || c == ',') {
						if(intervalLength > 0) strings.AddLast (inputStr.Substring (beginInterval, intervalLength)); // text before special character
						strings.AddLast (inputStr.Substring (beginInterval + intervalLength, 1)); // special character

						// reset interval values
						beginInterval = beginInterval + intervalLength + 1;
						intervalLength = 0;

						if (beginInterval >= inputStr.Length)
							break;

						// counter next increment
						intervalLength--;
					}
				}


				// next character
				intervalLength++;

				if (beginInterval + intervalLength >= inputStr.Length)
					break;
			}

			// unhandled escape char
			if (nextIsEscapeChar)
				throw new Exception ("Unexpected EOL (unfinished escape sequence)");

				strings.AddLast (inputStr.Substring (beginInterval));

			// remove first and last empty strings
			while (strings.First.Value == "")
				strings.RemoveFirst ();
			while (strings.Last.Value == "")
				strings.RemoveLast ();


			return strings.ToArray ();
		}


		public override string ToString ()
		{
			StringBuilder strBuilder = new StringBuilder ();
			bool isFirstFile = true;

			foreach (DataEntry dataEntry in m_dataEntries) {
				if (!isFirstFile)
					strBuilder.Append (",");
				isFirstFile = false;

				// only add properties when there is a properties-dictionary
				if (dataEntry.properties != null) {
					// example properties: "<encoding=utf-8,stream=0>"
					strBuilder.Append("<");
					bool isFirstProperty = true;
					foreach(KeyValuePair<String, String> property in dataEntry.properties) {
						// add comma beween two properties
						if (!isFirstProperty)
							strBuilder.Append (",");
						isFirstProperty = false;

						strBuilder.Append (SpecialCharacterToEscapeSequence(property.Key));
						strBuilder.Append ("=");
						strBuilder.Append (SpecialCharacterToEscapeSequence(property.Value));
					}
					strBuilder.Append(">");
				}

				// append file
				strBuilder.Append (SpecialCharacterToEscapeSequence(dataEntry.filename));
			}


			return strBuilder.ToString ();
		}

		/// <summary>
		/// Gets the file descriptions. It resolves wildcards so that
		/// this function returns a list of usable files.
		/// </summary>
		/// <returns>The file descriptions.</returns>
		public List<FileDesc> GetFileDescriptions() {
			// TODO: Glob files
			Dictionary<String, String> currentDictionary = new Dictionary<String, String>();
			List<FileDesc> fileDescs = new List<FileDesc>();
			foreach (DataEntry dataEntry in m_dataEntries) {
				currentDictionary = dataEntry.properties ?? currentDictionary;

				if (String.IsNullOrWhiteSpace (dataEntry.filename)) {
					fileDescs.Add (new FileDesc (null, new Dictionary<String,String> (currentDictionary)));
				} else {
					// resolve wildcards
					fileDescs.AddRange(AddFileDescToList(false, dataEntry.filename, currentDictionary));
				}
			}

			return fileDescs;
		}

		List<FileDesc> AddFileDescToList (bool isFillerFile, string filePath, Dictionary<string, string> dictionary)
		{
			List<FileDesc> fileDescs = new List<FileDesc> ();
			String dirPath = Path.GetDirectoryName (filePath);
			String filename = Path.GetFileName (filePath);



			if (!filename.Contains ("*") || !Directory.Exists (dirPath)) {
				fileDescs.Add (new FileDesc (filePath, new Dictionary<string, string> (dictionary)));
			} else {
				// TODO: more regex features
				String regex = "^" + filename.Replace ("*", "(.*)") + "$";
				String[] allDirFilenames = Directory.GetFiles (dirPath);
				foreach (String thisFilePath in allDirFilenames) {
					String thisFilename = Path.GetFileName (thisFilePath);

					if (Regex.Match (thisFilename, regex, RegexOptions.Compiled | RegexOptions.IgnoreCase).Success) {
						fileDescs.Add (new FileDesc (thisFilePath, new Dictionary<string, string> (dictionary)));
					}
				}


			}
			return fileDescs;
		}

		private static String SpecialCharacterToEscapeSequence(String str) {
			if (str == null)
				return null;
			return str.Replace(@"\", @"\\").Replace(@",", @"\,").Replace(@"<", @"\<").Replace(@">", @"\>").Replace(@"=", @"\=");
		}


		private static String EscapeSequencesToNormalChar(String str) {
			if (str == null)
				return null;
			return str.Replace(@"\\", @"\").Replace(@"\,", @",").Replace(@"\<", @"<").Replace(@"\>", @">").Replace(@"\=", @"=");
		}

		public void AddFile(String filename) {
			m_dataEntries.Add (new DataEntry (filename, null));
		}

		public void AddFileWithProperties(String filename, Dictionary<String,String> dict) {
			m_dataEntries.Add (new DataEntry (filename, new Dictionary<string, string>(dict)));
		}

		public void SetPropertiesOfFirstFile (string key, string value)
		{
			// create filler entry
			if (m_dataEntries.Count == 0)
				m_dataEntries.Add (new DataEntry (null, null));

			m_dataEntries [0].properties = m_dataEntries [0].properties ?? new Dictionary<string, string> ();
			m_dataEntries [0].properties [key] = value;
		}
	}
}
