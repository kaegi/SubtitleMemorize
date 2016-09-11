using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using NCalc;
using System.Text.RegularExpressions;

namespace subtitleMemorize
{
    public class PreviewListModel
    {
        //private List<EpisodeInfo> m_episodeInfo = new List<EpisodeInfo>();
        private List<CardInfo> m_cardInfos = new List<CardInfo>(); // all entries from all episodes

        public enum ChangeType
        {
            // data of row was changed but it was left the way it was in the list
            DataUpdate,

            // list was deleted at this line
            LineDelete,

            // new line was created in front of given line number
            LineInsert
        }

        // how to switch binary flags
        public class BinaryChangeMode
        {
            public static readonly BinaryChangeMode Disable = new BinaryChangeMode(false, false);
            public static readonly BinaryChangeMode Enable = new BinaryChangeMode(true, true);
            public static readonly BinaryChangeMode Toggle = new BinaryChangeMode(true, false);
            public static readonly BinaryChangeMode Leave = new BinaryChangeMode(false, true);

            private readonly bool[] m_changeTo = new bool[2];
            public BinaryChangeMode(bool changeToFalse, bool changeToTrue)
            {
                m_changeTo[0] = changeToFalse;
                m_changeTo[1] = changeToTrue;
            }

            public bool Apply(bool input)
            {
                return m_changeTo[input ? 1 : 0];
            }
        }

        public class AtomicChange
        {
            public readonly int line;
            public readonly CardInfo cardInfo;
            public readonly ChangeType changeType;

            public AtomicChange(int line, CardInfo cardInfo, ChangeType changeType)
            {
                this.line = line;
                this.cardInfo = cardInfo;
                this.changeType = changeType;
            }
        }

        public PreviewListModel(List<CardInfo> cardInfos)
        {
            m_cardInfos = cardInfos;
            m_cardInfos.Sort();
        }

        public CardInfo GetCardClone(int index)
        {
            return UtilsCommon.DeepClone(m_cardInfos[index]);
        }

        public List<AtomicChange> SetCard(int index, CardInfo cardInfo)
        {
            m_cardInfos[index] = cardInfo;

            var changeList = new List<AtomicChange>();
            changeList.Add(new AtomicChange(index, cardInfo, ChangeType.DataUpdate));
            finalizeChangeList(changeList);
            return changeList;
        }

        private void finalizeChangeList(List<AtomicChange> changeList)
        {
            changeList.Sort(delegate (AtomicChange a, AtomicChange b)
            {
                return a.line < b.line ? -1 : 1;
            });
        }

        public List<AtomicChange> UpdateCardActivation(IEnumerable<int> items, BinaryChangeMode mode)
        {
            var changeList = new List<AtomicChange>();
            foreach (int item in items)
            {
                var cardInfo = m_cardInfos[item];
                bool previousState = cardInfo.isActive;
                cardInfo.isActive = mode.Apply(cardInfo.isActive);
                if (cardInfo.isActive != previousState)
                {
                    changeList.Add(new AtomicChange(item, cardInfo, ChangeType.DataUpdate));
                }
            }
            finalizeChangeList(changeList);
            return changeList;
        }

        public bool IsIndexInRange(int index)
        {
            return index >= 0 && index < m_cardInfos.Count;
        }


        /// <summary>
        /// There are three different modes for merging. Every selected line...
        ///		...will be merged with next line
        ///		...will be merged with previous line
        ///		...will be merged with all other selected lines
        /// </summary>
        public enum MergeMode
        {
            Selected,
            Next,
            Prev,
        }

        public List<AtomicChange> MergeLines(List<int> selectedIndices, MergeMode mergeMode)
        {
            var changeList = new List<AtomicChange>();

            // sort so consecutive indices can be recognized
            selectedIndices.Sort();

            // "merge prev" is like "merge next" for previous entry
            if (mergeMode == MergeMode.Prev)
            {
                for (int i = 0; i < selectedIndices.Count; i++) selectedIndices[i] -= 1;
            }

            // remove illegal indices
            selectedIndices.RemoveAll(delegate (int i) { return !IsIndexInRange(i); });

            // can't merge zero objects
            if (selectedIndices.Count <= 0) return changeList;

            // go through whole list and unify selected
            int inSelectedIndicesListIndex = 0;
            int numDeletedRows = 0; // every time a card information is deleted, all the following indices will decrease
            for (int currentListIndex = 0; currentListIndex <= selectedIndices[selectedIndices.Count - 1]; currentListIndex++)
            {
                // skip if line was not selected for merging
                if (currentListIndex < selectedIndices[inSelectedIndicesListIndex])
                {
                    continue;
                }

                // get references and indices to current CardInfo and next CardInfo
                int thisIndex = selectedIndices[inSelectedIndicesListIndex] - numDeletedRows;
                var thisCardInfo = m_cardInfos[thisIndex];

                // can't merge next if this entry is last
                if (thisIndex == m_cardInfos.Count - 1) break;

                int nextIndex = selectedIndices[inSelectedIndicesListIndex] - numDeletedRows + 1;
                var nextCardInfo = m_cardInfos[nextIndex];

                inSelectedIndicesListIndex++;

                // only merge if merging is possible
                if (!CardInfo.IsMergePossbile(thisCardInfo, nextCardInfo)) continue;
                nextCardInfo = new CardInfo(thisCardInfo, nextCardInfo);

                // "next entry" now contains both entries -> save this change
                m_cardInfos[nextIndex] = nextCardInfo;
                if (changeList.Count > 0 && changeList.Last().line == currentListIndex)
                    changeList[changeList.Count - 1] = new AtomicChange(currentListIndex, null, ChangeType.LineDelete);
                else
                    changeList.Add(new AtomicChange(currentListIndex, null, ChangeType.LineDelete));
                changeList.Add(new AtomicChange(currentListIndex + 1, nextCardInfo, ChangeType.DataUpdate));

                // remove entry that was unified into next
                m_cardInfos.RemoveAt(thisIndex);
                numDeletedRows++;
            }

            finalizeChangeList(changeList);
            return changeList;
        }


        /// <summary>
        /// XXX: Wrong documentation = Selects preview lines in Gtk.TreeView based on a condtion like "episode=3 and contains(sub1, 'Bye')".
        /// </summary>
        /// <param name="conditionExpr">Condition expr.</param>
        /// <param name="isIncrementalSearch">Only change selection state for lines with matching expressions.</param>
        /// <param name="selectAction">true -> select matching lines, false -> deselect matching lines</param>
        public List<bool> EvaluateForEveryLine(String conditionExpr)
        {
            // select all if expression is null
            var resultsList = Enumerable.Repeat(false, m_cardInfos.Count).ToList();
            if (String.IsNullOrWhiteSpace(conditionExpr))
            {
                return resultsList;
            }


            CardInfo infoSourceCard = null; // entry which will be used for evaluation an expression
            Expression expr = new Expression(conditionExpr);

            // resolve certain parameters in expression
            expr.EvaluateParameter += delegate (string name, ParameterArgs args)
            {
                switch (name)
                {
                    case "isActive": // fallthrough
                    case "active": args.Result = infoSourceCard.isActive; break;

                    case "number":
                    case "episodeNumber":
                    case "episode": args.Result = infoSourceCard.episodeInfo.Number; break;

                    case "text":
                    case "sub": args.Result = infoSourceCard.ToSingleLine(UtilsCommon.LanguageType.TARGET) + " " + infoSourceCard.ToSingleLine(UtilsCommon.LanguageType.NATIVE); break;

                    case "sub1":
                    case "text1": args.Result = infoSourceCard.ToSingleLine(UtilsCommon.LanguageType.TARGET); break;

                    case "sub2":
                    case "text2": args.Result = infoSourceCard.ToSingleLine(UtilsCommon.LanguageType.NATIVE); break;

                    case "actor":
                    case "actors":
                    case "name":
                    case "names": args.Result = infoSourceCard.GetActorString(); break;

                    case "start": args.Result = infoSourceCard.startTimestamp; break;
                    case "end": args.Result = infoSourceCard.endTimestamp; break;
                    case "duration": args.Result = infoSourceCard.Duration; break;
                }
            };
            // resolve certain functions in expression
            expr.EvaluateFunction += delegate (string name, FunctionArgs args)
            {
                switch (name)
                {
                    // an exmple for this function is "contains(sub1, 'some text')" that selects all lines, where sub1 contains 'some text'
                    case "c":
                    case "contains":
                        {
                            // two string parameters are expected
                            if (args.Parameters.Length < 1) return;
                            object[] arguments = args.EvaluateParameters();
                            foreach (var argument in arguments)
                                if (!(argument is String)) return;

                            String substring = (String)arguments[arguments.Length - 1];
                            args.HasResult = true;
                            args.Result = false;

                            bool result = false;
                            if (arguments.Length == 1)
                            {
                                result = infoSourceCard.ToSingleLine(UtilsCommon.LanguageType.NATIVE).Contains(substring);
                                if (result) { args.Result = result; goto contains_finished; }

                                result = infoSourceCard.ToSingleLine(UtilsCommon.LanguageType.TARGET).Contains(substring);
                                if (result) { args.Result = result; goto contains_finished; }
                            }
                            else {
                                // evaluate function
                                for (int i = 0; i < arguments.Length - 1; i++)
                                {
                                    String string0 = (String)arguments[i];
                                    result = string0.Contains(substring);
                                    if (result) { args.Result = result; goto contains_finished; }
                                }
                            }

                        contains_finished:;

                        }
                        break;

                    // search for regex; for example: "r(text1, 'hi|hello')"
                    // 'r('hi|hello')' searches both in sub1 and sub2
                    case "r":
                    case "regex":
                        {
                            // two string parameters are expected
                            if (args.Parameters.Length < 1) return;
                            object[] arguments = args.EvaluateParameters();
                            foreach (var argument in arguments)
                                if (!(argument is String)) return;

                            String regex = (String)arguments[arguments.Length - 1];
                            args.HasResult = true;
                            args.Result = false;

                            if (arguments.Length == 1)
                            {
                                try
                                {
                                    var match = Regex.Match(infoSourceCard.ToSingleLine(UtilsCommon.LanguageType.NATIVE), regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                    args.Result = match.Success;
                                    if (match.Success) goto finish_regex;
                                }
                                catch { }

                                try
                                {
                                    var match = Regex.Match(infoSourceCard.ToSingleLine(UtilsCommon.LanguageType.TARGET), regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                    args.Result = match.Success;
                                    if (match.Success) goto finish_regex;
                                }
                                catch { }
                            }
                            else if (arguments.Length > 1)
                            {
                                // try to match any text before regex argument
                                for (int i = 0; i < arguments.Length - 1; i++)
                                {
                                    try
                                    {
                                        String str = (String)arguments[0];
                                        var match = Regex.Match(str, regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                                        args.Result = match.Success;
                                        if (match.Success) goto finish_regex;
                                    }
                                    catch { }
                                }
                            }

                        finish_regex:;

                        }
                        break;

                    // example: time('25:10.50') returns the number of seconds of 25min 10secs 50hsecs
                    case "time":
                        {
                            if (args.Parameters.Length != 1) return;
                            object argumentObject = args.EvaluateParameters()[0];
                            if (!(argumentObject is String)) return;
                            String timeString = (String)argumentObject;

                            args.Result = UtilsCommon.ParseTimeString(timeString);
                            args.HasResult = true;
                        }
                        break;
                }
            };

            for(int i = 0; i < m_cardInfos.Count; i++) {
              // provide infos for expr.Evaluate()
              infoSourceCard = m_cardInfos[i];
              object result = expr.Evaluate();
              if(result is bool && result != null)
                resultsList[i] = (bool)result;
            }

            return resultsList;
        }

        public List<AtomicChange> RegexReplace(List<int> indices, bool inSub1, bool inSub2, string pattern, string replaceTo)
        {
            var changesList = new List<AtomicChange>();
            foreach (int index in indices)
            {
                CardInfo cardInfo = m_cardInfos[index];
                if (inSub1) cardInfo.DoRegexReplace(UtilsCommon.LanguageType.TARGET, pattern, replaceTo);
                if (inSub2) cardInfo.DoRegexReplace(UtilsCommon.LanguageType.NATIVE, pattern, replaceTo);
                changesList.Add(new AtomicChange(index, cardInfo, ChangeType.DataUpdate));
            }
            finalizeChangeList(changesList);
            return changesList;
        }

        public int GetLength()
        {
            return m_cardInfos.Count;
        }

        public String GetSnapshotFileName(Settings settings, CardInfo cardInfo) {
            return settings.DeckNameModified + "__" + cardInfo.GetKey() + ".jpg";
        }

        public String GetAudioFileName(Settings settings, CardInfo cardInfo) {
            return settings.DeckNameModified + "__" + cardInfo.GetKey() + ".ogg";
        }

        /** Returns a list of all cards that are activated -> all cards that are exported */
        public List<CardInfo> GetActiveCards() {
            // remove all entries that are deactivated
            List<CardInfo> activeCardInfos = new List<CardInfo>();
            activeCardInfos.AddRange(m_cardInfos);
            activeCardInfos.RemoveAll((CardInfo cardInfo) => !cardInfo.isActive);
            return activeCardInfos;
        }

        /** Generates a .tsv file */
        public void ExportTextFile(List<CardInfo> cardInfoList, Settings settings, InfoProgress progressInfo) {
            String tsvFilename = settings.OutputDirectoryPath + Path.DirectorySeparatorChar + settings.DeckName + ".tsv";
            Console.WriteLine(tsvFilename);

            // value that will be imported into Anki/SRS-Programs-Field => [sound:???.ogg] and <img src="???.jpg"/>
            var snapshotFields = new List<String>(cardInfoList.Count);
            var audioFields = new List<String>(cardInfoList.Count);

            foreach(var cardInfo in cardInfoList) {
              var outputSnapshotFilename = GetSnapshotFileName(settings, cardInfo);
              var outputAudioFilename = GetAudioFileName(settings, cardInfo);
              snapshotFields.Add("<img src=\"" + outputSnapshotFilename + "\"/>"); // TODO: make this flexible
              audioFields.Add("[sound:" + outputAudioFilename + "]"); // TODO: make this flexible
            }

            using (var outputStream = new StreamWriter(tsvFilename))
            {
                for (int i = 0; i < cardInfoList.Count; i++)
                {
                    CardInfo cardInfo = cardInfoList[i];

                    // TODO: generate a episode-filtered list for context card search (because it has O(n^2) steps)
                    var contextCardsTuple = UtilsSubtitle.GetContextCards(cardInfo.episodeInfo.Index, cardInfo, m_cardInfos);
                    var previousCards = contextCardsTuple.Item1;
                    var nextCards = contextCardsTuple.Item2;

                    var previousCardsNativeLanguage = UtilsSubtitle.CardListToMultilineString(previousCards, UtilsCommon.LanguageType.NATIVE);
                    var previousCardsTargetLanguage = UtilsSubtitle.CardListToMultilineString(previousCards, UtilsCommon.LanguageType.TARGET);

                    var nextCardsNativeLanguage = UtilsSubtitle.CardListToMultilineString(nextCards, UtilsCommon.LanguageType.NATIVE);
                    var nextCardsTargetLanguage = UtilsSubtitle.CardListToMultilineString(nextCards, UtilsCommon.LanguageType.TARGET);

                    String keyField = cardInfo.GetKey();
                    String audioField = audioFields[i];
                    String imageField = snapshotFields[i];
                    String tags = String.Format("{0} ep{1:000}", settings.DeckNameModified, cardInfo.episodeInfo.Number);
                    outputStream.WriteLine(UtilsCommon.HTMLify(keyField) + "\t" +
                                           UtilsCommon.HTMLify(imageField) + "\t"+
                                           UtilsCommon.HTMLify(audioField) + "\t" +
                                           UtilsCommon.HTMLify(cardInfo.ToSingleLine(UtilsCommon.LanguageType.TARGET)) + "\t" +
                                           UtilsCommon.HTMLify(cardInfo.ToSingleLine(UtilsCommon.LanguageType.NATIVE)) + "\t" +
                                           UtilsCommon.HTMLify(previousCardsTargetLanguage) + "\t" +
                                           UtilsCommon.HTMLify(previousCardsNativeLanguage) + "\t" +
                                           UtilsCommon.HTMLify(nextCardsTargetLanguage) + "\t" +
                                           UtilsCommon.HTMLify(nextCardsNativeLanguage) + "\t" +
                                           UtilsCommon.HTMLify(tags)
                                           );
                }
            }
        }


        public void ExportData(Settings settings, InfoProgress progressInfo)
        {
            var activeCardList = GetActiveCards();

            ExportTextFile(activeCardList, settings, progressInfo);

            var cardSnapshotNameTupleList = new List<Tuple<CardInfo, String>>(activeCardList.Count);
            var cardAudioNameTupleList = new List<Tuple<CardInfo, String>>(activeCardList.Count);
            foreach(var cardInfo in activeCardList) {
              cardSnapshotNameTupleList.Add(new Tuple<CardInfo, String>(cardInfo, GetSnapshotFileName(settings, cardInfo)));
              cardAudioNameTupleList.Add(new Tuple<CardInfo, String>(cardInfo, GetAudioFileName(settings, cardInfo)));
            }

            String snapshotsPath = settings.OutputDirectoryPath + Path.DirectorySeparatorChar + settings.DeckName + "_snapshots" + Path.DirectorySeparatorChar;
            String audioPath = settings.OutputDirectoryPath + Path.DirectorySeparatorChar + settings.DeckName + "_audio" + Path.DirectorySeparatorChar;

            // extract images
            UtilsCommon.ClearDirectory(snapshotsPath);
            WorkerSnapshot.ExtractSnaphots(settings, snapshotsPath, cardSnapshotNameTupleList);

            // extract audio
            UtilsCommon.ClearDirectory(audioPath);
            WorkerAudio.ExtractAudio(settings, audioPath, cardAudioNameTupleList);

            // TODO: normalize audio here instead of WorkerAudio
        }

        public List<AtomicChange> GenerateFullUpdateList()
        {
            var changeList = new List<AtomicChange>();
            for (int i = 0; i < m_cardInfos.Count; i++)
            {
                changeList.Add(new AtomicChange(i, m_cardInfos[i], ChangeType.DataUpdate));
            }
            return changeList;
        }
    }
}
