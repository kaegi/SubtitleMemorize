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
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using System.Xml.Serialization;
using Gtk;

namespace subtitleMemorize
{
  class MainClass
  {
    ////////////////// AUTO-GENERATED CODE BEGIN //////////////////////
    #pragma warning disable 0414 // private field assigned but not used
    private Gtk.Adjustment m_adjustmentAudioPaddingAfter;
    private Gtk.Adjustment m_adjustmentAudioPaddingBefore;
    private Gtk.Adjustment m_adjustmentMaxImageHeight;
    private Gtk.Adjustment m_adjustmentMaxImageWidth;
    private Gtk.Adjustment m_episodeAdjustment;
    private Gtk.Image m_image1;
    private Gtk.ListStore m_liststoreImageRescaleOptions;
    private Gtk.ListStore m_liststoreLanguages;
    private Gtk.ListStore m_liststoreLines;
    private Gtk.Window m_previewWindow;
    private Gtk.Box m_box5;
    private Gtk.Toolbar m_toolbarPreview;
    private Gtk.ToolButton m_toolbuttonGo;
    private Gtk.ToolButton m_toolbuttonRefresh;
    private Gtk.SeparatorToolItem m_toolbuttonSeparator1;
    private Gtk.ToolButton m_toolbuttonSelectAll;
    private Gtk.ToolButton m_toolbuttonSelectNone;
    private Gtk.ToolButton m_toolbuttonInvert;
    private Gtk.SeparatorToolItem m_toolbuttonSeparator2;
    private Gtk.ToolButton m_toolbuttonToggleActivation;
    private Gtk.ToolButton m_toolbuttonMerge;
    private Gtk.ScrolledWindow m_scrolledwindow1;
    private Gtk.TreeView m_treeviewLines;
    private Gtk.TreeSelection m_treeviewSelectionLines;
    private Gtk.TreeViewColumn m_treeviewcolumnTargetLanguage;
    private Gtk.CellRendererText m_cellrenderertextTargetLanguage;
    private Gtk.TreeViewColumn m_treeviewcolumnNativeLanguage;
    private Gtk.CellRendererText m_cellrenderertextNativeLanguage;
    private Gtk.TreeViewColumn m_treeviewcolumnActors;
    private Gtk.CellRendererText m_cellrenderertextActors;
    private Gtk.TreeViewColumn m_treeviewcolumnStart;
    private Gtk.CellRendererText m_cellrendertextStart;
    private Gtk.CellRendererText m_cellrendertextDuration;
    private Gtk.Frame m_frame10;
    private Gtk.Alignment m_alignment10;
    private Gtk.Grid m_grid4;
    private Gtk.Button m_buttonReplaceInSub2;
    private Gtk.Button m_buttonSelectLinesBySearch;
    private Gtk.Button m_buttonReplaceInSub1;
    private Gtk.Entry m_entryLinesSearch;
    private Gtk.Entry m_entryReplaceRegexTo;
    private Gtk.Entry m_entryReplaceRegexFrom;
    private Gtk.Label m_label5;
    private Gtk.Label m_label17;
    private Gtk.Label m_labelNumCardsSelected;
    private Gtk.Frame m_frame11;
    private Gtk.Alignment m_alignment11;
    private Gtk.Box m_box7;
    private Gtk.Frame m_frame12;
    private Gtk.Box m_box11;
    private Gtk.EventBox m_eventboxImagePreview;
    private Gtk.Image m_imagePreview;
    private Gtk.Button m_buttonPlayContent;
    private Gtk.Label m_label23;
    private Gtk.Frame m_frame13;
    private Gtk.Box m_box12;
    private Gtk.ScrolledWindow m_scrolledwindow2;
    private Gtk.TextView m_textviewTargetLanguage;
    private Gtk.ScrolledWindow m_scrolledwindow3;
    private Gtk.TextView m_textviewNativeLanguage;
    private Gtk.Label m_label24;
    private Gtk.Label m_label22;
    private Gtk.ListStore m_liststoreSubEncoding;
    private Gtk.ListStore m_liststoreSubStreams;
    private Gtk.Window m_subtitleOptionsWindow;
    private Gtk.Box m_box13;
    private Gtk.Frame m_frame14;
    private Gtk.ComboBox m_comboboxSubEncoding;
    private Gtk.CellRendererText m_cellrenderertextSubEncoding;
    private Gtk.Label m_label3;
    private Gtk.Frame m_frame15;
    private Gtk.ComboBox m_comboboxSubStream;
    private Gtk.CellRendererText m_cellrendertextSubStreams;
    private Gtk.Label m_label4;
    private Gtk.Button m_buttonSubOptionsApply;
    private Gtk.Adjustment m_numericalAdjustmentSub1;
    private Gtk.Adjustment m_numericalAdjustmentSub2;
    private Gtk.Window m_mainWindow;
    private Gtk.Box m_box1;
    private Gtk.MenuBar m_menubar1;
    private Gtk.MenuItem m_menuitem1;
    private Gtk.Menu m_menu1;
    private Gtk.ImageMenuItem m_imagemenuitemNew;
    private Gtk.ImageMenuItem m_imagemenuitemOpen;
    private Gtk.ImageMenuItem m_imagemenuitemSave;
    private Gtk.ImageMenuItem m_imagemenuitemSaveAs;
    private Gtk.SeparatorMenuItem m_separatormenuitem1;
    private Gtk.ImageMenuItem m_imagemenuitemClose;
    private Gtk.MenuItem m_menuitem3;
    private Gtk.Menu m_menu3;
    private Gtk.ImageMenuItem m_imagemenuitem10;
    private Gtk.InfoBar m_infobar1;
    private Gtk.ButtonBox m_infobarActionArea;
    private Gtk.Box m_infobarContentArea;
    private Gtk.Label m_labelInInfobar;
    private Gtk.Box m_box2;
    private Gtk.Frame m_frame1;
    private Gtk.Alignment m_alignment1;
    private Gtk.Grid m_grid1;
    private Gtk.Entry m_entryTargetLanguage;
    private Gtk.Entry m_entryOutputDirectory;
    private Gtk.Entry m_entryNativeLanguage;
    private Gtk.Entry m_entryVideoFile;
    private Gtk.Button m_buttonTargetLanguageChoose;
    private Gtk.Button m_buttonDirectoryChoose;
    private Gtk.Button m_buttonNativeLanguageChoose;
    private Gtk.Button m_buttonVideoFileChoose;
    private Gtk.Button m_buttonTargetLanguageOptions;
    private Gtk.Button m_buttonNativeLanguageOptions;
    private Gtk.Button m_buttonVideoOptions;
    private Gtk.Button m_buttonAudioChoose;
    private Gtk.Entry m_entryAudioFile;
    private Gtk.Label m_label1;
    private Gtk.Frame m_frame2;
    private Gtk.Alignment m_alignment2;
    private Gtk.Grid m_grid5;
    private Gtk.Expander m_expanderSubtitleOptions;
    private Gtk.Box m_box9;
    private Gtk.Frame m_frame4;
    private Gtk.Alignment m_alignment4;
    private Gtk.Grid m_grid2;
    private Gtk.Label m_label8;
    private Gtk.Label m_label9;
    private Gtk.ComboBoxText m_comboboxtextCorrectTimingsModeSub1;
    private Gtk.SpinButton m_spinbuttonSub1TimeShift;
    private Gtk.Label m_label6;
    private Gtk.Frame m_frame5;
    private Gtk.Alignment m_alignment5;
    private Gtk.Grid m_grid3;
    private Gtk.Label m_label7;
    private Gtk.Label m_label10;
    private Gtk.ComboBoxText m_comboboxtextCorrectTimingsModeSub2;
    private Gtk.SpinButton m_spinbuttonSub2TimeShift;
    private Gtk.Label m_label12;
    private Gtk.Frame m_frame3;
    private Gtk.Alignment m_alignment3;
    private Gtk.Box m_box3;
    private Gtk.CheckButton m_checkbuttonUseSub1Timings;
    private Gtk.CheckButton m_checkbuttonUseSub2Timings;
    private Gtk.Label m_label11;
    private Gtk.Label m_label20;
    private Gtk.Expander m_expanderAudioOptions;
    private Gtk.Box m_box15;
    private Gtk.Frame m_frame17;
    private Gtk.Alignment m_alignment14;
    private Gtk.Box m_box16;
    private Gtk.CheckButton m_checkbuttonNormalizeAudio;
    private Gtk.CheckButton m_checkbuttonDeactivateAudioClips;
    private Gtk.Label m_label26;
    private Gtk.Frame m_frame16;
    private Gtk.Alignment m_alignment13;
    private Gtk.Grid m_grid6;
    private Gtk.Label m_label28;
    private Gtk.Label m_label29;
    private Gtk.SpinButton m_spinbuttonAudioPaddingBefore;
    private Gtk.SpinButton m_spinbuttonAudioPaddingAfter;
    private Gtk.Label m_label27;
    private Gtk.Label m_label53;
    private Gtk.Expander m_expanderImageOptions;
    private Gtk.Box m_box17;
    private Gtk.Frame m_frame19;
    private Gtk.Alignment m_alignment16;
    private Gtk.SpinButton m_spinbuttonMaxImageWidth;
    private Gtk.SpinButton m_spinbuttonMaxImageHeight;
    private Gtk.ComboBox m_comboboxtextRescaleMode;
    private Gtk.Label m_label13;
    private Gtk.Label m_label2;
    private Gtk.Frame m_frame7;
    private Gtk.Alignment m_alignment7;
    private Gtk.Label m_label16;
    private Gtk.SpinButton m_spinbuttonEpisodeNumber;
    private Gtk.Entry m_entryDeckName;
    private Gtk.Label m_label15;
    private Gtk.Button m_buttonPreview;
    private Gtk.Label m_label18;
    private Gtk.Label m_label19;
    private Gtk.ComboBox m_comboboxTargetLanguage;
    private Gtk.Label m_label14;
    private Gtk.Window m_windowProgressInfo;
    private Gtk.Alignment m_alignment12;
    private Gtk.Box m_box14;
    private Gtk.ProgressBar m_progressbarProgressInfo;
    private Gtk.Button m_buttonCancelOperation;

    private void InitializeGtkObjects(Gtk.Builder b) {
      m_adjustmentAudioPaddingAfter = (Gtk.Adjustment) b.GetObject("adjustment_audio_padding_after");
      m_adjustmentAudioPaddingBefore = (Gtk.Adjustment) b.GetObject("adjustment_audio_padding_before");
      m_adjustmentMaxImageHeight = (Gtk.Adjustment) b.GetObject("adjustment_max_image_height");
      m_adjustmentMaxImageWidth = (Gtk.Adjustment) b.GetObject("adjustment_max_image_width");
      m_episodeAdjustment = (Gtk.Adjustment) b.GetObject("episode_adjustment");
      m_image1 = (Gtk.Image) b.GetObject("image1");
      m_liststoreImageRescaleOptions = (Gtk.ListStore) b.GetObject("liststore_image_rescale_options");
      m_liststoreLanguages = (Gtk.ListStore) b.GetObject("liststore_languages");
      m_liststoreLines = (Gtk.ListStore) b.GetObject("liststore_lines");
      m_previewWindow = (Gtk.Window) b.GetObject("preview_window");
      m_box5 = (Gtk.Box) b.GetObject("box5");
      m_toolbarPreview = (Gtk.Toolbar) b.GetObject("toolbar_preview");
      m_toolbuttonGo = (Gtk.ToolButton) b.GetObject("toolbutton_go");
      m_toolbuttonRefresh = (Gtk.ToolButton) b.GetObject("toolbutton_refresh");
      m_toolbuttonSeparator1 = (Gtk.SeparatorToolItem) b.GetObject("toolbutton_separator1");
      m_toolbuttonSelectAll = (Gtk.ToolButton) b.GetObject("toolbutton_select_all");
      m_toolbuttonSelectNone = (Gtk.ToolButton) b.GetObject("toolbutton_select_none");
      m_toolbuttonInvert = (Gtk.ToolButton) b.GetObject("toolbutton_invert");
      m_toolbuttonSeparator2 = (Gtk.SeparatorToolItem) b.GetObject("toolbutton_separator2");
      m_toolbuttonToggleActivation = (Gtk.ToolButton) b.GetObject("toolbutton_toggle_activation");
      m_toolbuttonMerge = (Gtk.ToolButton) b.GetObject("toolbutton_merge");
      m_scrolledwindow1 = (Gtk.ScrolledWindow) b.GetObject("scrolledwindow1");
      m_treeviewLines = (Gtk.TreeView) b.GetObject("treeview_lines");
      m_treeviewSelectionLines = (Gtk.TreeSelection) b.GetObject("treeview-selection_lines");
      m_treeviewcolumnTargetLanguage = (Gtk.TreeViewColumn) b.GetObject("treeviewcolumn_target_language");
      m_cellrenderertextTargetLanguage = (Gtk.CellRendererText) b.GetObject("cellrenderertext_target_language");
      m_treeviewcolumnNativeLanguage = (Gtk.TreeViewColumn) b.GetObject("treeviewcolumn_native_language");
      m_cellrenderertextNativeLanguage = (Gtk.CellRendererText) b.GetObject("cellrenderertext_native_language");
      m_treeviewcolumnActors = (Gtk.TreeViewColumn) b.GetObject("treeviewcolumn_actors");
      m_cellrenderertextActors = (Gtk.CellRendererText) b.GetObject("cellrenderertext_actors");
      m_treeviewcolumnStart = (Gtk.TreeViewColumn) b.GetObject("treeviewcolumn_start");
      m_cellrendertextStart = (Gtk.CellRendererText) b.GetObject("cellrendertext_start");
      m_cellrendertextDuration = (Gtk.CellRendererText) b.GetObject("cellrendertext_duration");
      m_frame10 = (Gtk.Frame) b.GetObject("frame10");
      m_alignment10 = (Gtk.Alignment) b.GetObject("alignment10");
      m_grid4 = (Gtk.Grid) b.GetObject("grid4");
      m_buttonReplaceInSub2 = (Gtk.Button) b.GetObject("button_replace_in_sub2");
      m_buttonSelectLinesBySearch = (Gtk.Button) b.GetObject("button_select_lines_by_search");
      m_buttonReplaceInSub1 = (Gtk.Button) b.GetObject("button_replace_in_sub1");
      m_entryLinesSearch = (Gtk.Entry) b.GetObject("entry_lines_search");
      m_entryReplaceRegexTo = (Gtk.Entry) b.GetObject("entry_replace_regex_to");
      m_entryReplaceRegexFrom = (Gtk.Entry) b.GetObject("entry_replace_regex_from");
      m_label5 = (Gtk.Label) b.GetObject("label5");
      m_label17 = (Gtk.Label) b.GetObject("label17");
      m_labelNumCardsSelected = (Gtk.Label) b.GetObject("label_num_cards_selected");
      m_frame11 = (Gtk.Frame) b.GetObject("frame11");
      m_alignment11 = (Gtk.Alignment) b.GetObject("alignment11");
      m_box7 = (Gtk.Box) b.GetObject("box7");
      m_frame12 = (Gtk.Frame) b.GetObject("frame12");
      m_box11 = (Gtk.Box) b.GetObject("box11");
      m_eventboxImagePreview = (Gtk.EventBox) b.GetObject("eventbox_image_preview");
      m_imagePreview = (Gtk.Image) b.GetObject("image_preview");
      m_buttonPlayContent = (Gtk.Button) b.GetObject("button_play_content");
      m_label23 = (Gtk.Label) b.GetObject("label23");
      m_frame13 = (Gtk.Frame) b.GetObject("frame13");
      m_box12 = (Gtk.Box) b.GetObject("box12");
      m_scrolledwindow2 = (Gtk.ScrolledWindow) b.GetObject("scrolledwindow2");
      m_textviewTargetLanguage = (Gtk.TextView) b.GetObject("textview_target_language");
      m_scrolledwindow3 = (Gtk.ScrolledWindow) b.GetObject("scrolledwindow3");
      m_textviewNativeLanguage = (Gtk.TextView) b.GetObject("textview_native_language");
      m_label24 = (Gtk.Label) b.GetObject("label24");
      m_label22 = (Gtk.Label) b.GetObject("label22");
      m_liststoreSubEncoding = (Gtk.ListStore) b.GetObject("liststore_sub_encoding");
      m_liststoreSubStreams = (Gtk.ListStore) b.GetObject("liststore_sub_streams");
      m_subtitleOptionsWindow = (Gtk.Window) b.GetObject("subtitle_options_window");
      m_box13 = (Gtk.Box) b.GetObject("box13");
      m_frame14 = (Gtk.Frame) b.GetObject("frame14");
      m_comboboxSubEncoding = (Gtk.ComboBox) b.GetObject("combobox_sub_encoding");
      m_cellrenderertextSubEncoding = (Gtk.CellRendererText) b.GetObject("cellrenderertext_sub_encoding");
      m_label3 = (Gtk.Label) b.GetObject("label3");
      m_frame15 = (Gtk.Frame) b.GetObject("frame15");
      m_comboboxSubStream = (Gtk.ComboBox) b.GetObject("combobox_sub_stream");
      m_cellrendertextSubStreams = (Gtk.CellRendererText) b.GetObject("cellrendertext_sub_streams");
      m_label4 = (Gtk.Label) b.GetObject("label4");
      m_buttonSubOptionsApply = (Gtk.Button) b.GetObject("button_sub_options_apply");
      m_numericalAdjustmentSub1 = (Gtk.Adjustment) b.GetObject("numerical_adjustment_sub1");
      m_numericalAdjustmentSub2 = (Gtk.Adjustment) b.GetObject("numerical_adjustment_sub2");
      m_mainWindow = (Gtk.Window) b.GetObject("main_window");
      m_box1 = (Gtk.Box) b.GetObject("box1");
      m_menubar1 = (Gtk.MenuBar) b.GetObject("menubar1");
      m_menuitem1 = (Gtk.MenuItem) b.GetObject("menuitem1");
      m_menu1 = (Gtk.Menu) b.GetObject("menu1");
      m_imagemenuitemNew = (Gtk.ImageMenuItem) b.GetObject("imagemenuitem_new");
      m_imagemenuitemOpen = (Gtk.ImageMenuItem) b.GetObject("imagemenuitem_open");
      m_imagemenuitemSave = (Gtk.ImageMenuItem) b.GetObject("imagemenuitem_save");
      m_imagemenuitemSaveAs = (Gtk.ImageMenuItem) b.GetObject("imagemenuitem_save_as");
      m_separatormenuitem1 = (Gtk.SeparatorMenuItem) b.GetObject("separatormenuitem1");
      m_imagemenuitemClose = (Gtk.ImageMenuItem) b.GetObject("imagemenuitem_close");
      m_menuitem3 = (Gtk.MenuItem) b.GetObject("menuitem3");
      m_menu3 = (Gtk.Menu) b.GetObject("menu3");
      m_imagemenuitem10 = (Gtk.ImageMenuItem) b.GetObject("imagemenuitem10");
      m_infobar1 = (Gtk.InfoBar) b.GetObject("infobar1");
      m_infobarActionArea = (Gtk.ButtonBox) b.GetObject("infobar-action_area");
      m_infobarContentArea = (Gtk.Box) b.GetObject("infobar-content_area");
      m_labelInInfobar = (Gtk.Label) b.GetObject("label_in_infobar");
      m_box2 = (Gtk.Box) b.GetObject("box2");
      m_frame1 = (Gtk.Frame) b.GetObject("frame1");
      m_alignment1 = (Gtk.Alignment) b.GetObject("alignment1");
      m_grid1 = (Gtk.Grid) b.GetObject("grid1");
      m_entryTargetLanguage = (Gtk.Entry) b.GetObject("entry_target_language");
      m_entryOutputDirectory = (Gtk.Entry) b.GetObject("entry_output_directory");
      m_entryNativeLanguage = (Gtk.Entry) b.GetObject("entry_native_language");
      m_entryVideoFile = (Gtk.Entry) b.GetObject("entry_video_file");
      m_buttonTargetLanguageChoose = (Gtk.Button) b.GetObject("button_target_language_choose");
      m_buttonDirectoryChoose = (Gtk.Button) b.GetObject("button_directory_choose");
      m_buttonNativeLanguageChoose = (Gtk.Button) b.GetObject("button_native_language_choose");
      m_buttonVideoFileChoose = (Gtk.Button) b.GetObject("button_video_file_choose");
      m_buttonTargetLanguageOptions = (Gtk.Button) b.GetObject("button_target_language_options");
      m_buttonNativeLanguageOptions = (Gtk.Button) b.GetObject("button_native_language_options");
      m_buttonVideoOptions = (Gtk.Button) b.GetObject("button_video_options");
      m_buttonAudioChoose = (Gtk.Button) b.GetObject("button_audio_choose");
      m_entryAudioFile = (Gtk.Entry) b.GetObject("entry_audio_file");
      m_label1 = (Gtk.Label) b.GetObject("label1");
      m_frame2 = (Gtk.Frame) b.GetObject("frame2");
      m_alignment2 = (Gtk.Alignment) b.GetObject("alignment2");
      m_grid5 = (Gtk.Grid) b.GetObject("grid5");
      m_expanderSubtitleOptions = (Gtk.Expander) b.GetObject("expander_subtitle_options");
      m_box9 = (Gtk.Box) b.GetObject("box9");
      m_frame4 = (Gtk.Frame) b.GetObject("frame4");
      m_alignment4 = (Gtk.Alignment) b.GetObject("alignment4");
      m_grid2 = (Gtk.Grid) b.GetObject("grid2");
      m_label8 = (Gtk.Label) b.GetObject("label8");
      m_label9 = (Gtk.Label) b.GetObject("label9");
      m_comboboxtextCorrectTimingsModeSub1 = (Gtk.ComboBoxText) b.GetObject("comboboxtext_correct_timings_mode_sub1");
      m_spinbuttonSub1TimeShift = (Gtk.SpinButton) b.GetObject("spinbutton_sub1_time_shift");
      m_label6 = (Gtk.Label) b.GetObject("label6");
      m_frame5 = (Gtk.Frame) b.GetObject("frame5");
      m_alignment5 = (Gtk.Alignment) b.GetObject("alignment5");
      m_grid3 = (Gtk.Grid) b.GetObject("grid3");
      m_label7 = (Gtk.Label) b.GetObject("label7");
      m_label10 = (Gtk.Label) b.GetObject("label10");
      m_comboboxtextCorrectTimingsModeSub2 = (Gtk.ComboBoxText) b.GetObject("comboboxtext_correct_timings_mode_sub2");
      m_spinbuttonSub2TimeShift = (Gtk.SpinButton) b.GetObject("spinbutton_sub2_time_shift");
      m_label12 = (Gtk.Label) b.GetObject("label12");
      m_frame3 = (Gtk.Frame) b.GetObject("frame3");
      m_alignment3 = (Gtk.Alignment) b.GetObject("alignment3");
      m_box3 = (Gtk.Box) b.GetObject("box3");
      m_checkbuttonUseSub1Timings = (Gtk.CheckButton) b.GetObject("checkbutton_use_sub1_timings");
      m_checkbuttonUseSub2Timings = (Gtk.CheckButton) b.GetObject("checkbutton_use_sub2_timings");
      m_label11 = (Gtk.Label) b.GetObject("label11");
      m_label20 = (Gtk.Label) b.GetObject("label20");
      m_expanderAudioOptions = (Gtk.Expander) b.GetObject("expander_audio_options");
      m_box15 = (Gtk.Box) b.GetObject("box15");
      m_frame17 = (Gtk.Frame) b.GetObject("frame17");
      m_alignment14 = (Gtk.Alignment) b.GetObject("alignment14");
      m_box16 = (Gtk.Box) b.GetObject("box16");
      m_checkbuttonNormalizeAudio = (Gtk.CheckButton) b.GetObject("checkbutton_normalize_audio");
      m_checkbuttonDeactivateAudioClips = (Gtk.CheckButton) b.GetObject("checkbutton_deactivate_audio_clips");
      m_label26 = (Gtk.Label) b.GetObject("label26");
      m_frame16 = (Gtk.Frame) b.GetObject("frame16");
      m_alignment13 = (Gtk.Alignment) b.GetObject("alignment13");
      m_grid6 = (Gtk.Grid) b.GetObject("grid6");
      m_label28 = (Gtk.Label) b.GetObject("label28");
      m_label29 = (Gtk.Label) b.GetObject("label29");
      m_spinbuttonAudioPaddingBefore = (Gtk.SpinButton) b.GetObject("spinbutton_audio_padding_before");
      m_spinbuttonAudioPaddingAfter = (Gtk.SpinButton) b.GetObject("spinbutton_audio_padding_after");
      m_label27 = (Gtk.Label) b.GetObject("label27");
      m_label53 = (Gtk.Label) b.GetObject("label53");
      m_expanderImageOptions = (Gtk.Expander) b.GetObject("expander_image_options");
      m_box17 = (Gtk.Box) b.GetObject("box17");
      m_frame19 = (Gtk.Frame) b.GetObject("frame19");
      m_alignment16 = (Gtk.Alignment) b.GetObject("alignment16");
      m_spinbuttonMaxImageWidth = (Gtk.SpinButton) b.GetObject("spinbutton_max_image_width");
      m_spinbuttonMaxImageHeight = (Gtk.SpinButton) b.GetObject("spinbutton_max_image_height");
      m_comboboxtextRescaleMode = (Gtk.ComboBox) b.GetObject("comboboxtext_rescale_mode");
      m_label13 = (Gtk.Label) b.GetObject("label13");
      m_label2 = (Gtk.Label) b.GetObject("label2");
      m_frame7 = (Gtk.Frame) b.GetObject("frame7");
      m_alignment7 = (Gtk.Alignment) b.GetObject("alignment7");
      m_label16 = (Gtk.Label) b.GetObject("label16");
      m_spinbuttonEpisodeNumber = (Gtk.SpinButton) b.GetObject("spinbutton_episode_number");
      m_entryDeckName = (Gtk.Entry) b.GetObject("entry_deck_name");
      m_label15 = (Gtk.Label) b.GetObject("label15");
      m_buttonPreview = (Gtk.Button) b.GetObject("button_preview");
      m_label18 = (Gtk.Label) b.GetObject("label18");
      m_label19 = (Gtk.Label) b.GetObject("label19");
      m_comboboxTargetLanguage = (Gtk.ComboBox) b.GetObject("combobox_target_language");
      m_label14 = (Gtk.Label) b.GetObject("label14");
      m_windowProgressInfo = (Gtk.Window) b.GetObject("window_progress_info");
      m_alignment12 = (Gtk.Alignment) b.GetObject("alignment12");
      m_box14 = (Gtk.Box) b.GetObject("box14");
      m_progressbarProgressInfo = (Gtk.ProgressBar) b.GetObject("progressbar_progress_info");
      m_buttonCancelOperation = (Gtk.Button) b.GetObject("button_cancel_operation");
    }
    #pragma warning restore 0414
    ////////////////// AUTO-GENERATED CODE END //////////////////////
    private readonly Settings m_defaultSettings = new Settings();
		private readonly Gtk.Builder m_builder = new Builder();
		private int m_numberOfInfobarLabelMarkupChanges = 0;


		// ##################################################################33
		// Variables for main window
		private String m_currentPath = null; // used for "Add ..."-Dialogs, so the last position gets remembered

		// ##################################################################33
		// Variables for Preview window
		private Settings m_previewSettings = null; // this set when preview window is shown and never changed until the preview window closes
		private bool m_ignoreLineSelectionChanges = false;
		private int m_selectedPreviewIndex = -1; // selected card in card list in preview window
		private bool m_previewWindow_isShiftPressed = false;
		private bool m_previewWindow_isControlPressed = false;
		private volatile bool m_ignoreBufferChanges = false;
		private PreviewListModel m_previewListModel = null;
		private int m_previewImageID = 0;

		// ##################################################################33
		// Variables for Subtitle-Options-Window
		private int m_subOptionsWindow_subIndex = 0;
		private List<int> m_subOptionsWindowStreamIndices = new List<int>();


		// ##################################################################33
		// Variables for Progress-Window
		private enum PendingOperation{
			GENERATE_PREVIEW, // "Preview"/"Refresh"
			NOTHING
		}
		private PendingOperation m_pendingOperation = PendingOperation.NOTHING;
		private InfoProgress m_progressAndCancellable = null;

		static private readonly string m_infobarLabelStandardMarkup = "Welcome to SubtitleMemorize!" +
			" To see more information just hover the cursor over a button or field.\n" +
			"If any questions arise, please visit <span foreground=\"white\"><a href=\"https://github.com/ChangSpivey/SubtitleMemorize\">https://github.com/ChangSpivey/SubtitleMemorize</a></span>.";

		public MainClass() {
			GLib.ExceptionManager.UnhandledException += GlibUnhandledException;

			Gtk.Application.Init ();
			m_builder.AddFromString (ReadResourceString ("SubtitleMemorize.Resources.gtk.glade"));
			m_builder.Autoconnect (this);

			InitializeGtkObjects (m_builder);
			ConnectEventsMainWindow ();
			ConnectEventsSubitleWindowOptions ();
			ConnectEventsPreviewWindowOptions ();
			ConnectEventsProgressWindow ();

      m_liststoreLanguages.Clear();
      foreach(var language in InfoLanguages.languages) { m_liststoreLanguages.AppendValues(language.name); }
      m_comboboxTargetLanguage.Active = 0;
			m_mainWindow.ShowAll();



			ReadGui (m_defaultSettings);
			m_subtitleOptionsWindow.HideOnDelete ();


			if (!String.IsNullOrWhiteSpace(InstanceSettings.systemSettings.preLoadedSettings))
				LoadSaveStateFromFile (InstanceSettings.systemSettings.preLoadedSettings);

			// this has to be after "mainWindow.Show()", because otherwise the width of the window
			// is determined by the width of this text
			m_labelInInfobar.Markup = m_infobarLabelStandardMarkup;
			//on_button_preview_clicked(null, null);

			Application.Run ();

			// close running processes
			if(m_progressAndCancellable != null) m_progressAndCancellable.Cancel();
		}

		/// <summary>
		/// Reads all subtitle files.
		/// </summary>
		/// <returns>Every entry in the outer list refers to exactly one subtitle file and for every file there is a list of all lines in it.</returns>
		/// <param name="settings">Settings.</param>
		/// <param name="attributedFilePaths">Attributed file path string.</param>
		private static List<List<LineInfo>> ReadAllSubtitleFiles (Settings settings, PerSubtitleSettings thisSubtitleSettings, List<EpisodeInfo> episodeInfos, int subtileIndex, InfoProgress progressInfo)
		{
			// read all lines in every subtitle file
			List<List<LineInfo>> lineInfosPerEpisode = new List<List<LineInfo>> ();
			foreach (EpisodeInfo episodeInfo in episodeInfos) {
				UtilsInputFiles.FileDesc fileDesc = episodeInfo.SubsFileDesc [subtileIndex];
				if (fileDesc == null || String.IsNullOrWhiteSpace (fileDesc.filename))
					lineInfosPerEpisode.Add (null);
				else
					lineInfosPerEpisode.Add (UtilsSubtitle.ParseSubtitleWithPostProcessing (settings, thisSubtitleSettings, fileDesc.filename, fileDesc.properties));
				progressInfo.ProcessedSteps (1);

				if (progressInfo.Cancelled)
					return null;
			}

			return lineInfosPerEpisode;
		}

		private void ConnectEventsMainWindow() {

			// ----------------------------------------------------------------------------------------------------
			m_imagemenuitemNew.ButtonReleaseEvent += delegate(object o, ButtonReleaseEventArgs args) {
				UpdateGui (m_defaultSettings);
			};

			m_imagemenuitemOpen.ButtonReleaseEvent += delegate(object o, ButtonReleaseEventArgs args) {
				Gtk.Application.Invoke(delegate {
					Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog ("Load configuration", m_mainWindow, FileChooserAction.Open,
						"_Cancel", ResponseType.Cancel,
						"_Open", ResponseType.Accept);
					fcd.LocalOnly = false;
					if(!String.IsNullOrWhiteSpace(m_currentPath)) fcd.SetFilename(m_currentPath);
					if ((Gtk.ResponseType)fcd.Run() == Gtk.ResponseType.Accept) {
						LoadSaveStateFromFile (fcd.Filename);
					}
					fcd.Destroy ();
				});
			};

			// ----------------------------------------------------------------------------------------------------
			Gtk.ButtonReleaseEventHandler imagemenuItemSave = delegate(object o, ButtonReleaseEventArgs arg) {
				Gtk.Application.Invoke(delegate {
					Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog ("Save configuration to file", m_mainWindow, FileChooserAction.Save,
						"_Cancel", ResponseType.Cancel,
						"_Save", ResponseType.Accept);
					fcd.DoOverwriteConfirmation = true;
					fcd.LocalOnly = false;
					if(!String.IsNullOrWhiteSpace(m_currentPath)) fcd.SetFilename(m_currentPath);
					if ((Gtk.ResponseType)fcd.Run() == Gtk.ResponseType.Accept) {
						SaveStateToFile (fcd.Filename);
					}
					fcd.Destroy ();
				});
			};

			// ----------------------------------------------------------------------------------------------------
			m_imagemenuitemSave.ButtonReleaseEvent += imagemenuItemSave;
			m_imagemenuitemSaveAs.ButtonReleaseEvent += imagemenuItemSave;

			// ----------------------------------------------------------------------------------------------------
			m_imagemenuitemClose.ButtonReleaseEvent += delegate(object obj, ButtonReleaseEventArgs args) {
				Application.Quit ();
			};


			// ----------------------------------------------------------------------------------------------------
			m_buttonTargetLanguageChoose.Clicked += delegate(object o, EventArgs e) {
				// Open file dialog
				String[] filenames = OpenFileChooser ("Select subtitles file in target language");
				if (filenames == null)
					return;

				m_entryTargetLanguage.Text = AddFilesToEntry(m_entryTargetLanguage.Text, filenames, "Error in target language file entry field: ");
			};

			// ----------------------------------------------------------------------------------------------------
			m_buttonDirectoryChoose.Clicked += delegate(object o, EventArgs e) {

				Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog ("Select output folder", m_mainWindow, FileChooserAction.SelectFolder,
					"_Cancel", ResponseType.Cancel,
					"_Choose", ResponseType.Accept);
				fcd.DoOverwriteConfirmation = true;
				fcd.SelectMultiple = true;
				if(!String.IsNullOrWhiteSpace(m_currentPath)) fcd.SelectFilename(m_currentPath);
				if ((Gtk.ResponseType)fcd.Run () == Gtk.ResponseType.Accept) {
					m_entryOutputDirectory.Text = fcd.Filename;
					m_currentPath = fcd.Filename;
				}
				fcd.Destroy ();
			};

			// ----------------------------------------------------------------------------------------------------
			m_buttonNativeLanguageChoose.Clicked += delegate(object o, EventArgs e) {
				// Open file dialog
				String[] filenames = OpenFileChooser ("Select subtitles file in native language");
				if (filenames == null)
					return;

				m_entryNativeLanguage.Text = AddFilesToEntry(m_entryNativeLanguage.Text, filenames, "Error in native language file entry field: ");
			};

			// ----------------------------------------------------------------------------------------------------
			m_buttonVideoFileChoose.Clicked += delegate(object o, EventArgs e) {
				// Open file dialog
				String[] filenames = OpenFileChooser ("Select video files");
				if (filenames == null)
					return;

				m_entryVideoFile.Text = AddFilesToEntry(m_entryVideoFile.Text, filenames, "Error in video file entry field: ");
			};


			// ----------------------------------------------------------------------------------------------------
			m_buttonTargetLanguageOptions.Clicked += delegate(object o, EventArgs e) {
				OpenSubtitleWindow(0);
			};

			// ----------------------------------------------------------------------------------------------------
			m_buttonNativeLanguageOptions.Clicked += delegate(object o, EventArgs e) {
				OpenSubtitleWindow(1);
			};

			// ----------------------------------------------------------------------------------------------------
			m_buttonPreview.Clicked += delegate(object o, EventArgs args) {
				PreviewButtonClicked();
			};

			// ----------------------------------------------------------------------------------------------------
			m_mainWindow.DeleteEvent += delegate(object obj, DeleteEventArgs args) {
				Application.Quit ();
			};
		}

		private void ConnectEventsSubitleWindowOptions() {

			m_subtitleOptionsWindow.DeleteEvent += delegate(object obj, DeleteEventArgs args) {
				m_subtitleOptionsWindow.Hide ();
				args.RetVal = true; // this prevents the window from being actually getting deleted
			};

			m_buttonSubOptionsApply.Clicked += delegate(object sender, EventArgs e) {

				Gtk.Entry currentEntry = m_subOptionsWindow_subIndex == 0 ? m_entryTargetLanguage : m_entryNativeLanguage;
				UtilsInputFiles allFiles = new UtilsInputFiles(currentEntry.Text);
				allFiles.SetPropertiesOfFirstFile("enc", InfoEncoding.getEncodings()[m_comboboxSubEncoding.Active].ShortName);

				if(m_comboboxSubStream.Active >= 0)
					allFiles.SetPropertiesOfFirstFile("stream", m_subOptionsWindowStreamIndices[m_comboboxSubStream.Active].ToString());

				//allFiles.SetPropertiesOfFirstFile("stream", );
				currentEntry.Text = allFiles.ToString();
				m_subtitleOptionsWindow.Hide();
			};
		}

		private void ConnectEventsProgressWindow() {

			m_buttonCancelOperation.Clicked += delegate {
				if(m_progressAndCancellable != null)
					m_progressAndCancellable.Cancel();
			};
		}

		/// <summary>
		/// Changes names of Search&Replace Buttons to "Select", "Deselect", ... dependent on
		/// Shift and Ctrl.
		/// </summary>
		private void UpdatePreviewWidgetLabels() {
			if(m_previewWindow_isShiftPressed) {
				if(m_previewWindow_isControlPressed) m_buttonSelectLinesBySearch.Label = "Deselect (inc)";
				else m_buttonSelectLinesBySearch.Label = "Select (inc)";
			} else {
				if(m_previewWindow_isControlPressed) m_buttonSelectLinesBySearch.Label = "Deselect ";
				else m_buttonSelectLinesBySearch.Label = "Select";
			}
			if(m_previewWindow_isControlPressed) {
				m_toolbuttonMerge.Label = "Merge prev";
			} else {
				m_toolbuttonMerge.Label = "Merge next";
			}
			if(m_previewWindow_isControlPressed) {
				m_toolbuttonToggleActivation.Label = "Enable Line";
			} else if(m_previewWindow_isShiftPressed) {
				m_toolbuttonToggleActivation.Label = "Toggle Activation";
			} else {
				m_toolbuttonToggleActivation.Label = "Disable Line";
			}
		}

		private void ConnectEventsPreviewWindowOptions() {

			// ----------------------------------------------------------------------------------------------------
			m_previewWindow.DeleteEvent += delegate(object o, DeleteEventArgs args) {
				m_previewWindow.Hide();
				args.RetVal = true; // prevents window from being actually deleted
			};

			// ----------------------------------------------------------------------------------------------------
			m_toolbuttonSelectAll.Clicked += delegate(object sender, EventArgs e) {
				m_treeviewSelectionLines.SelectAll();
			};

			// ----------------------------------------------------------------------------------------------------
			m_toolbuttonSelectNone.Clicked += delegate(object sender, EventArgs e) {
				m_treeviewSelectionLines.UnselectAll();
			};

			// ----------------------------------------------------------------------------------------------------
			m_toolbuttonInvert.Clicked += delegate(object sender, EventArgs e) {
				// get a all selected rows
				TreePath[] selectedTreePaths = m_treeviewSelectionLines.GetSelectedRows();
				bool[] wasLineSelected = new bool[m_liststoreLines.IterNChildren()];
				foreach(TreePath tp in selectedTreePaths)
					wasLineSelected[tp.Indices[0]] = true;

				// Gtk.TreeViews do not provide any easy mechanism to invert selection, so we just select
				// every non-selected row one by one
				m_ignoreLineSelectionChanges = true;
				m_treeviewSelectionLines.UnselectAll();
				for(int index = 0; index < wasLineSelected.Length; index++)
					if(!wasLineSelected[index])
						m_treeviewSelectionLines.SelectPath(new TreePath(new int[]{index}));
				m_ignoreLineSelectionChanges = false;

				SelectCard();
			};

			m_treeviewSelectionLines.Changed += delegate(object sender, EventArgs e) {
				var numLinesSelected = m_treeviewSelectionLines.CountSelectedRows();
				if(numLinesSelected < 2) m_labelNumCardsSelected.Text = numLinesSelected + " Card selected";
				else m_labelNumCardsSelected.Text = numLinesSelected + " Cards selected";
				SelectCard();
			};

			m_buttonReplaceInSub1.Clicked += delegate(object sender, EventArgs e) {
				PreviewWindowRegexReplace(true, false, m_previewWindow_isControlPressed, m_entryReplaceRegexFrom.Text, m_entryReplaceRegexTo.Text);
			};

			m_buttonReplaceInSub2.Clicked += delegate(object sender, EventArgs e) {
				PreviewWindowRegexReplace(false, true, m_previewWindow_isControlPressed, m_entryReplaceRegexFrom.Text, m_entryReplaceRegexTo.Text);
			};

			m_previewWindow.KeyPressEvent += delegate(object o, KeyPressEventArgs args) {
				if(args.Event.KeyValue == Gdk.Keyval.FromName("Shift_R") || args.Event.KeyValue == Gdk.Keyval.FromName("Shift_L"))
					m_previewWindow_isShiftPressed = true;
				if(args.Event.KeyValue == Gdk.Keyval.FromName("Control_R") || args.Event.KeyValue == Gdk.Keyval.FromName("Control_L"))
					m_previewWindow_isControlPressed = true;
				UpdatePreviewWidgetLabels();
			};

			m_previewWindow.KeyReleaseEvent += delegate(object o, KeyReleaseEventArgs args) {
				if(args.Event.KeyValue == Gdk.Keyval.FromName("Shift_R") || args.Event.KeyValue == Gdk.Keyval.FromName("Shift_L"))
					m_previewWindow_isShiftPressed = false;
				if(args.Event.KeyValue == Gdk.Keyval.FromName("Control_R") || args.Event.KeyValue == Gdk.Keyval.FromName("Control_L"))
					m_previewWindow_isControlPressed = false;
				UpdatePreviewWidgetLabels();
			};

			m_buttonSelectLinesBySearch.Clicked += delegate(object sender, EventArgs e) {
				PreviewWindowSelectLines(m_entryLinesSearch.Text, m_previewWindow_isShiftPressed, !m_previewWindow_isControlPressed);
			};

			m_entryLinesSearch.KeyReleaseEvent += delegate(object sender, KeyReleaseEventArgs e) {
				if(e.Event.KeyValue == Gdk.Keyval.FromName("Return")) {
					PreviewWindowSelectLines(m_entryLinesSearch.Text, m_previewWindow_isShiftPressed, !m_previewWindow_isControlPressed);
				}
			};

			m_buttonPlayContent.Clicked += delegate(object sender, EventArgs e) {
				if(m_selectedPreviewIndex < 0) return;

				CardInfo cardInfo = m_previewListModel.GetCardClone(m_selectedPreviewIndex);
				if(!cardInfo.HasAudio()) return;
				EpisodeInfo episodeInfo = cardInfo.episodeInfo;
				String arguments = String.Format("--really-quiet --no-video --start={0} --end={1} \"{2}\"",
						UtilsCommon.ToTimeArg(cardInfo.audioStartTimestamp),
						UtilsCommon.ToTimeArg(cardInfo.audioEndTimestamp),
						episodeInfo.VideoFileDesc.filename);
				// TODO: normalize audio for live audio play


				Thread thr = new Thread (new ThreadStart (delegate() {
					UtilsCommon.StartProcessAndGetOutput("mpv", arguments);
				}));
				thr.Start ();
			};

			m_toolbuttonToggleActivation.Clicked += delegate {
				// switch isActive field for every selected entry
				var updateItems = new List<int>();
				TreePath[] selectedTreePaths = m_treeviewSelectionLines.GetSelectedRows();
				foreach(TreePath treePath in selectedTreePaths) {
					updateItems.Add(treePath.Indices[0]);
				}

				// make internal changes visible to user
				var changeMode = PreviewListModel.BinaryChangeMode.Disable;
				if(m_previewWindow_isControlPressed) { changeMode = PreviewListModel.BinaryChangeMode.Enable; }
				else if(m_previewWindow_isShiftPressed) { changeMode = PreviewListModel.BinaryChangeMode.Toggle; }
				var changeList = m_previewListModel.UpdateCardActivation(updateItems, changeMode);
				UpdatePreviewListViewByChangeList(changeList);
			};

			m_toolbuttonGo.Clicked += delegate {
				new Thread(new ThreadStart(delegate {
					try {
						Console.WriteLine("Start computation");
						InfoProgress progressInfo = new InfoProgress(ProgressHandler);
						m_progressAndCancellable = progressInfo;
						Gtk.Application.Invoke(delegate { m_previewWindow.Hide(); m_windowProgressInfo.Show(); });
						m_previewListModel.ExportData(m_previewSettings, progressInfo);
						Gtk.Application.Invoke(delegate {  m_windowProgressInfo.Hide(); m_previewWindow.Show(); });
						Console.WriteLine("End computation");
					} catch(Exception e) {
						Console.WriteLine(e);
						SetErrorMessage(e.Message);
					}
				})).Start();

			};


			Action<UtilsCommon.LanguageType> onBufferChange = delegate(UtilsCommon.LanguageType languageType) {
				if(m_ignoreBufferChanges) return;


				var textview = GetTextViewByLanguageType(languageType);
				var cardInfo = m_previewListModel.GetCardClone(m_selectedPreviewIndex);
				cardInfo.SetLineInfosByMultiLineString(languageType, textview.Buffer.Text);
				var changeList = m_previewListModel.SetCard(m_selectedPreviewIndex, cardInfo);
				UpdatePreviewListViewByChangeList(changeList, false);
			};

			// ----------------------------------------------------------------------
			// change entries when text in preview textviews is changed
			m_textviewTargetLanguage.Buffer.Changed += delegate(object sender, EventArgs e) {
				onBufferChange(UtilsCommon.LanguageType.TARGET);
			};

			// ----------------------------------------------------------------------
			// change entries when text in preview textviews is changed
			m_textviewNativeLanguage.Buffer.Changed += delegate(object sender, EventArgs e) {
				onBufferChange(UtilsCommon.LanguageType.NATIVE);
			};

			// ----------------------------------------------------------------------
			// "merge line with next" or "merge selected"
			m_toolbuttonMerge.Clicked += delegate(object sender, EventArgs e) {
				var changeList = m_previewListModel.MergeLines(GetSelectedRows(), m_previewWindow_isControlPressed ? PreviewListModel.MergeMode.Prev : PreviewListModel.MergeMode.Next);
				UpdatePreviewListViewByChangeList(changeList);
			};

			m_eventboxImagePreview.ButtonReleaseEvent += delegate(object o, ButtonReleaseEventArgs args) {
				var imageWnd = new Gtk.Window("SubtitleMemorize - Image preview");
				var image = new Gtk.Image();

				// do not select currently selected entry again
				if (!m_previewListModel.IsIndexInRange(m_selectedPreviewIndex))
					return;

				// get references to classes that describe the video file and stream
				CardInfo cardInfo = m_previewListModel.GetCardClone(m_selectedPreviewIndex);
				UtilsInputFiles.FileDesc videoFilename = cardInfo.episodeInfo.VideoFileDesc;
				StreamInfo videoStreamInfo = cardInfo.episodeInfo.VideoStreamInfo;

				// get right scaling
				double scaling = UtilsVideo.GetMaxScalingByStreamInfo(videoStreamInfo, m_previewSettings.RescaleWidth, m_previewSettings.RescaleHeight, m_previewSettings.RescaleMode);

				// extract big image from video
				UtilsImage.GetImage(videoFilename.filename, UtilsCommon.GetMiddleTime(cardInfo), InstanceSettings.temporaryFilesPath + "subtitleMemorize_real.jpg", scaling);

				image.Pixbuf = new Gdk.Pixbuf (InstanceSettings.temporaryFilesPath + "subtitleMemorize_real.jpg");
				imageWnd.Add(image);
				imageWnd.ShowAll();
			};
		}

		public Gtk.TextView GetTextViewByLanguageType(UtilsCommon.LanguageType languageType) {
			return languageType == UtilsCommon.LanguageType.NATIVE ? m_textviewNativeLanguage : m_textviewTargetLanguage;
		}


		/// <summary>
		/// Selects preview lines in Gtk.TreeView based on a condtion like "episode=3 and contains(sub1, 'Bye')".
		/// </summary>
		/// <param name="conditionExpr">Condition expr.</param>
		/// <param name="isIncrementalSearch">Only change selection state for lines with matching expressions.</param>
		/// <param name="selectAction">true -> select matching lines, false -> deselect matching lines</param>
		public void PreviewWindowSelectLines(String conditionExpr, bool isIncrementalSearch, bool selectAction) {
			var resultList = m_previewListModel.EvaluateForEveryLine(conditionExpr);


			// go through whole list and evaluate the expression for every entry
			int index = 0;
			TreeIter treeIter;
			if(!m_liststoreLines.GetIterFirst(out treeIter)) return;
			do {
				// select if expression is evaluated positive, deselect if negative
				object result = resultList[index++];
				if(result is bool) {
					if(selectAction) {
						// standard action: select
						if((bool)result == true) m_treeviewSelectionLines.SelectIter(treeIter);
						else if(!isIncrementalSearch) m_treeviewSelectionLines.UnselectIter(treeIter);
					} else {
						// standard action: deselect
						if((bool)result == true) m_treeviewSelectionLines.UnselectIter(treeIter);
						else if(!isIncrementalSearch) m_treeviewSelectionLines.SelectIter(treeIter);
					}
				}
			} while(m_liststoreLines.IterNext(ref treeIter));
		}

		private List<int> GetSelectedRows() {
			TreePath[] selectedTreePaths = m_treeviewSelectionLines.GetSelectedRows();
			List<int> selectedRows = new List<int>();
			foreach(TreePath tp in selectedTreePaths)
				selectedRows.Add(tp.Indices[0]);
			return selectedRows;
		}


		/// <summary>
		/// Replace text in sub1 and/or sub2 by using regexes.
		/// </summary>
		private void PreviewWindowRegexReplace(bool inSub1, bool inSub2, bool onlyInSelected, String pattern, String replaceTo) {

			List<int> indices = null;
			if(onlyInSelected) indices = GetSelectedRows();
			else indices = UtilsCommon.GetListFromTo(0, m_previewListModel.GetLength());

			var changeList = m_previewListModel.RegexReplace(indices, inSub1, inSub2, pattern, replaceTo);
			UpdatePreviewListViewByChangeList(changeList);
		}

		/// <summary>
		/// XXX: documentation missing
		/// </summary>
		private TreeIter GetTreeIterByIndex(int index) {
			TreeIter treeIter;
			if(!m_liststoreLines.GetIterFirst(out treeIter)) throw new Exception("No entry in list");
			do {
				if(index == 0) return treeIter;
				index--;
			} while(m_liststoreLines.IterNext(ref treeIter));

			// index is not in list
			throw new ArgumentOutOfRangeException();
		}

		/// <summary>
		/// XXX: documentation missing
		/// </summary>
		private void UpdatePreviewListEntry(int index, CardInfo cardInfo, TreeIter? treeIter = null, bool updateSelectedEntryTextView=false) {
			if(!m_previewListModel.IsIndexInRange(index)) throw new ArgumentOutOfRangeException(); // nothing to update
			if(treeIter == null)
				treeIter = GetTreeIterByIndex(index);

			// if this entry is deactivated the line is colored grey with Pango's markup language
			String beginString = cardInfo.isActive ? "" : "<span foreground=\"white\" background=\"grey\">";
			String endString = cardInfo.isActive ? "" : "</span>";

			// set values in list
			m_liststoreLines.SetValue(treeIter.Value, 0, beginString + GLib.Markup.EscapeText(cardInfo.ToSingleLine(UtilsCommon.LanguageType.TARGET)) + endString);
			m_liststoreLines.SetValue(treeIter.Value, 1, beginString + GLib.Markup.EscapeText(cardInfo.ToSingleLine(UtilsCommon.LanguageType.NATIVE)) + endString);
			m_liststoreLines.SetValue(treeIter.Value, 2, beginString + GLib.Markup.EscapeText(cardInfo.GetActorString()) + endString);
			m_liststoreLines.SetValue(treeIter.Value, 3, beginString + GLib.Markup.EscapeText(UtilsCommon.ToTimeArg(cardInfo.StartTime)) + endString);
			m_liststoreLines.SetValue(treeIter.Value, 4, beginString + GLib.Markup.EscapeText(UtilsCommon.ToTimeArg(cardInfo.Duration)) + endString);

			if(updateSelectedEntryTextView && index == m_selectedPreviewIndex) {
				// regenerate image and set texts
				m_selectedPreviewIndex = -1;
				new Thread(new ThreadStart(delegate {
					SelectCard(index);
				})).Start();;
			}
		}

		/// <summary>
		/// This function sets all lines in the Gtk.TreeView to the values in m_cardInfos.
		/// </summary>
		private void UpdatePreviewListViewByChangeList(List<PreviewListModel.AtomicChange> list, bool updateTextBuffer = true) {
			int currentLine = -1, currentChangeIndex = 0;
			int numDeletions = 0;
			bool isNextIter = false;

			// Nothing to do?
			if(list.Count == 0) return;

			TreeIter treeIter;
			if(!m_liststoreLines.GetIterFirst(out treeIter)) return;
			do {
				currentLine++;
				if(list[currentChangeIndex].line != currentLine) {
					isNextIter = m_liststoreLines.IterNext(ref treeIter);
					continue;
				}

				var change = list[currentChangeIndex];
				switch(change.changeType) {
					case PreviewListModel.ChangeType.DataUpdate:
						UpdatePreviewListEntry(currentLine - numDeletions, change.cardInfo, treeIter, updateTextBuffer);
						isNextIter = m_liststoreLines.IterNext(ref treeIter);
						break;
					case PreviewListModel.ChangeType.LineDelete:
						isNextIter = m_liststoreLines.Remove(ref treeIter);
						numDeletions++;
						break;
					default: throw new NotImplementedException();
				}

				currentChangeIndex++;
			} while(isNextIter && currentChangeIndex < list.Count);
		}

		private static List<EpisodeInfo> GenerateEpisodeInfos(Settings settings) {

			// get all filenames
			UtilsInputFiles sub1Files = new UtilsInputFiles (settings.TargetFilePath);
			UtilsInputFiles sub2Files = new UtilsInputFiles (settings.NativeFilePath);
			UtilsInputFiles videoFiles = new UtilsInputFiles (settings.VideoFilePath);

			List<UtilsInputFiles.FileDesc> sub1FileDescs = sub1Files.GetFileDescriptions();
			List<UtilsInputFiles.FileDesc> sub2FileDescs = sub2Files.GetFileDescriptions();
			List<UtilsInputFiles.FileDesc> videoFileDescs = videoFiles.GetFileDescriptions();

			bool noSub2 = sub2FileDescs.Count == 0; // no subtitles in native language available
			bool noVideos = videoFileDescs.Count == 0; // no video files available
			bool noAudio = noVideos; // TODO

			int numberOfEpisodes = sub1FileDescs.Count;
			if(numberOfEpisodes != sub2FileDescs.Count && !noSub2)
				throw new Exception("Number of files in target languages and number of files in native language does not match.");
			if(numberOfEpisodes != videoFileDescs.Count && !noVideos)
				throw new Exception("Number of files in target languages and number of video files does not match.");

			// fill episode info
			List<EpisodeInfo> episodeFiles = new List<EpisodeInfo>();
			for(int episodeIndex = 0; episodeIndex < numberOfEpisodes; episodeIndex++) {
				int episodeNumber = episodeIndex + settings.FirstEpisodeNumber;
				var videoFileDesc = noVideos ? null : videoFileDescs[episodeIndex];
				var audioFileDesc = noAudio ? null : videoFileDescs[episodeIndex];
				var sub1FileDesc = sub1FileDescs[episodeIndex];
				var sub2FileDesc = noSub2 ? null : sub2FileDescs[episodeIndex];
				var videoStreamInfo = noVideos ? null : UtilsVideo.ChooseStreamInfo(videoFileDesc.filename, videoFileDesc.properties, StreamInfo.StreamType.ST_VIDEO);
				var audioStreamInfo = noAudio ? null : UtilsVideo.ChooseStreamInfo(audioFileDesc.filename, audioFileDesc.properties, StreamInfo.StreamType.ST_AUDIO);
				var sub1StreamInfo = UtilsVideo.ChooseStreamInfo(sub1FileDesc.filename, sub1FileDesc.properties, StreamInfo.StreamType.ST_SUBTITLE);
				var sub2StreamInfo = noSub2 ? null : UtilsVideo.ChooseStreamInfo(sub2FileDesc.filename, sub2FileDesc.properties, StreamInfo.StreamType.ST_SUBTITLE);
				episodeFiles.Add(new EpisodeInfo(episodeIndex, episodeNumber, videoFileDesc, audioFileDesc, sub1FileDesc, sub2FileDesc, videoStreamInfo, audioStreamInfo, sub1StreamInfo, sub2StreamInfo));
			}

			return episodeFiles;
		}

		private void ComputationThread(Settings settings) {

			InfoProgress progressInfo = new InfoProgress(ProgressHandler);
			m_progressAndCancellable = progressInfo;

			// find sub1, sub2, audio and video file per episode
			var episodeInfo = new List<EpisodeInfo>();
			episodeInfo.AddRange(GenerateEpisodeInfos(settings));

			// fill in progress sections
			for(int i = 0; i < episodeInfo.Count; i++)
			progressInfo.AddSection(String.Format("Episode {0:00.}: Extracting Sub1", i + 1), 1);
			for(int i = 0; i < episodeInfo.Count; i++)
			progressInfo.AddSection(String.Format("Episode {0:00.}: Extracting Sub2", i + 1), 1);
			for(int i = 0; i < episodeInfo.Count; i++)
			progressInfo.AddSection(String.Format("Episode {0:00.}: Matching subtitles", i + 1), 1);

			progressInfo.AddSection("Preparing data presentation", 1);
			progressInfo.StartProgressing();


			// read all sub-files, match them and create a list for user that can be presented in preview window
			var cardInfos = new List<CardInfo>();
			cardInfos.AddRange(GenerateCardInfo(settings, episodeInfo, progressInfo) ?? new List<CardInfo>());

			m_previewListModel = new PreviewListModel(cardInfos);

			if(!progressInfo.Cancelled) {

				// finish this last step
				progressInfo.ProcessedSteps(1);

				PopulatePreviewList();
			}

			// close progress window, free pending operation variable
			CloseProgressWindow ();

		}

		private void ComputationThreadSafe(Settings settings) {
			try {
				ComputationThread(settings);
			} catch(Exception e) {
				Gtk.Application.Invoke(delegate {
					Console.WriteLine(e);
					SetErrorMessage(e.Message);
					CloseProgressWindow();
					m_previewWindow.Hide();
				});
			}
		}

		private void PreviewButtonClicked() {
			if(this.m_pendingOperation != PendingOperation.NOTHING)
				return;
			this.m_pendingOperation = PendingOperation.GENERATE_PREVIEW;
			m_windowProgressInfo.Show();

			// read settings while handling errors
			Settings settings = new Settings ();
			try {
				// read all required information to class/struct, so that off-gtk-thread computation is possible
				settings = new Settings ();
				ReadGui (settings);

				// quickly decide whether these inputs can be used for a run
				IsSettingsValid (settings);

			} catch(Exception e) {
				SetErrorMessage (e.Message);
				CloseProgressWindow ();
				return;
			}

			m_previewSettings = settings;


			Thread compuationThread = new Thread(() => ComputationThreadSafe(settings));
			compuationThread.Start();

		}



		private void PopulatePreviewList() {

			Gtk.Application.Invoke(delegate {

				// populate subtitle list
				m_liststoreLines.Clear ();
				ShowAllSelectedCardInfos();


				m_treeviewSelectionLines.UnselectAll();



				m_previewWindow.ShowAll ();
			});
		}

		/// <summary>
		/// This reads all subtitles, matches them and saves them to the "m_cardInfos"
		/// </summary>
		/// <param name="settings">Settings.</param>
		private static List<CardInfo> GenerateCardInfo(Settings settings, List<EpisodeInfo> episodeInfos, InfoProgress progressInfo) {

			// read subtitles
			List<List<LineInfo>> lineInfosPerEpisode_TargetLanguage = ReadAllSubtitleFiles(settings, settings.PerSubtitleSettings[0], episodeInfos, 0, progressInfo);
			List<List<LineInfo>> lineInfosPerEpisode_NativeLanguage = ReadAllSubtitleFiles(settings, settings.PerSubtitleSettings[1], episodeInfos, 1, progressInfo);

			if (progressInfo.Cancelled)
				return null;

			List<CardInfo> allCardInfos = new List<CardInfo> ();
			for(int episodeIndex = 0; episodeIndex < lineInfosPerEpisode_TargetLanguage.Count; episodeIndex++) {
				List<LineInfo> list1 = lineInfosPerEpisode_TargetLanguage[episodeIndex];
				List<LineInfo> list2 = lineInfosPerEpisode_NativeLanguage[episodeIndex];
				var episodeInfo = episodeInfos[episodeIndex];

				if(episodeInfo.HasSub2()) {
					// make sure that "other" subtitle has been shifted when aligning "this" subtitle to it
					if(settings.PerSubtitleSettings[0].AlignMode == PerSubtitleSettings.AlignModes.ToSubtitle) {
						UtilsCommon.AlignSub(list2, list1, episodeInfo, settings, settings.PerSubtitleSettings[1]);
						UtilsCommon.AlignSub(list1, list2, episodeInfo, settings, settings.PerSubtitleSettings[0]);
					} else {
						UtilsCommon.AlignSub(list1, list2, episodeInfo, settings, settings.PerSubtitleSettings[0]);
						UtilsCommon.AlignSub(list2, list1, episodeInfo, settings, settings.PerSubtitleSettings[1]);
					}
					var subtitleMatcherParameters = SubtitleMatcher.GetParameterCache (list1, list2);
					var matchedLinesList = SubtitleMatcher.MatchSubtitles(subtitleMatcherParameters);
					var thisEpisodeCardInfos = UtilsSubtitle.GetCardInfo(settings, episodeInfo, matchedLinesList);
					allCardInfos.AddRange(thisEpisodeCardInfos);
				} else {
					UtilsCommon.AlignSub(list1, list2, episodeInfo, settings, settings.PerSubtitleSettings[0]);
					allCardInfos.AddRange(UtilsSubtitle.GetCardInfo(settings, episodeInfo, list1));
				}

				progressInfo.ProcessedSteps (1);

				if (progressInfo.Cancelled)
					return null;
			}

			return allCardInfos;
		}

		/// <summary>
		/// Opens the progress window and sets the current pending operation.
		/// </summary>
		/// <returns><c>true</c>, if progress window was opened, <c>false</c>, if there already is an operation pending.</returns>
		/// <param name="thisOperation">This operation.</param>
		private bool OpenProgressWindow(PendingOperation thisOperation) {
			// do not start another operation when there is already one
			if (m_pendingOperation != PendingOperation.NOTHING)
				return false;


			m_pendingOperation = thisOperation;

			m_windowProgressInfo.Show ();
			return true;
		}

		private void ProgressHandler(String name, double percentage) {
			Gtk.Application.Invoke (delegate {
				m_progressbarProgressInfo.Text = name ?? "No name";
				m_progressbarProgressInfo.Fraction = percentage;
			});
		}

		/// <summary>
		/// Closes/Hides the progress window.
		/// </summary>
		private void CloseProgressWindow() {
			Gtk.Application.Invoke (delegate {
				m_windowProgressInfo.Hide ();
				m_pendingOperation = PendingOperation.NOTHING;
			});
		}

		private void OpenSubtitleWindow(int subIndex) {
			m_subtitleOptionsWindow.Title = "Sub" + (subIndex + 1) + " Options";

			Gtk.Entry currentEntry = subIndex == 0 ? m_entryTargetLanguage : m_entryNativeLanguage;
			m_subOptionsWindow_subIndex = subIndex;

			// read properties of first file to get selected stream and show them in combobox
			UtilsInputFiles uif = new UtilsInputFiles(currentEntry.Text);
			List<UtilsInputFiles.FileDesc> fileDescs = uif.GetFileDescriptions ();

			String selectedEncoding = "utf-8";
			String selectedStreamString = null;
			if (fileDescs.Count > 0 && fileDescs [0].properties.ContainsKey ("enc"))
				selectedEncoding = fileDescs [0].properties ["enc"];
			if (fileDescs.Count > 0 && fileDescs [0].properties.ContainsKey ("stream"))
				selectedStreamString = fileDescs [0].properties ["stream"];

			int selectedStreamIndex = -1;
			if (!Int32.TryParse (selectedStreamString, out selectedStreamIndex))
				selectedStreamIndex = -1;

			// get all streams in first video file
			List<StreamInfo> firstVideoFileStreams = null;
			for (int index = 0; index < fileDescs.Count; index++) {
				String thisFilename = fileDescs [index].filename;

				if (!String.IsNullOrWhiteSpace (thisFilename) && UtilsCommon.GetFileTypeByFilename (thisFilename) == UtilsCommon.FileType.FT_VIDEO) {
					if (!File.Exists (thisFilename))
						throw new Exception ("File \"" + thisFilename + "\" does not exist");
					firstVideoFileStreams = StreamInfo.ReadAllStreams (thisFilename);

					break;
				}
			}

			// fill streams box
			int selectedEntry_SubStreams = -1;
			int numberOfEntries_SubStreams = 0;
			m_liststoreSubStreams.Clear();
			m_subOptionsWindowStreamIndices.Clear ();
			if (firstVideoFileStreams != null) {
				for (int index = 0; index < firstVideoFileStreams.Count; index++) {
					if (firstVideoFileStreams [index].StreamTypeValue == StreamInfo.StreamType.ST_SUBTITLE) {
						selectedEntry_SubStreams = selectedEntry_SubStreams == -1 ? 0 : selectedEntry_SubStreams;
						if (index == selectedStreamIndex)
							selectedEntry_SubStreams = numberOfEntries_SubStreams;
						numberOfEntries_SubStreams++;

						m_subOptionsWindowStreamIndices.Add (index);
						m_liststoreSubStreams.AppendValues ("Stream " + index + " - " + firstVideoFileStreams [index].Language);
					}
				}
			}
			if(selectedEntry_SubStreams >= 0)
				m_comboboxSubStream.Active = selectedEntry_SubStreams;


			// fill encodings in subtitle options
			m_liststoreSubEncoding.Clear();
			int encodingIndex = 0;
			int utf8index = -1;
			int selectedEncodingIndex = -1;
			foreach (InfoEncoding enc in InfoEncoding.getEncodings()) {
				if (enc.ShortName == "utf-8")
					utf8index = encodingIndex;
				if (enc.ShortName == selectedEncoding)
					selectedEncodingIndex = encodingIndex;
				m_liststoreSubEncoding.AppendValues (enc.LongName);
				encodingIndex++;
			}
			if (selectedEncodingIndex >= 0)
				m_comboboxSubEncoding.Active = selectedEncodingIndex;
			else if (utf8index >= 0)
				m_comboboxSubEncoding.Active = utf8index;
			else
				m_comboboxSubEncoding.Active = 0;


			m_subtitleOptionsWindow.ShowAll ();
		}

		/// <summary>
		/// Finds first selected entry in preview list and then calls "SelectCard(int)" with this entry number in another thread.
		/// </summary>
		private void SelectCard () {
			if(m_ignoreLineSelectionChanges)
				return;

			int leastIndex = -1;
			m_treeviewSelectionLines.SelectedForeach(delegate(ITreeModel model, TreePath path, TreeIter iter) {
				if(leastIndex == -1 || path.Indices[0] < leastIndex)
					leastIndex = path.Indices[0];
			});

			// only start thread if entry is not already selected
			if(m_selectedPreviewIndex != leastIndex) {
				Thread thr = new Thread (new ThreadStart (delegate() {
					SelectCard(leastIndex);
				}));
				thr.Start ();
			}
		}


		private void UpdatePreviewImage(int selectedIndex, CardInfo cardInfo) {
			// preview image does not need to be in full size (saves computation time)
			const int maxWidth = 300;
			const int maxHeight = 300;

			// get real scaling
			UtilsInputFiles.FileDesc videoFilename = cardInfo.episodeInfo.VideoFileDesc;
			var videoStreamInfo = cardInfo.episodeInfo.VideoStreamInfo;
			double videoScaling = UtilsVideo.GetMaxScalingByStreamInfo(videoStreamInfo, maxWidth, maxHeight, Settings.RescaleModeEnum.UpscaleAndDownscale);

			// extract small preview image
			UtilsImage.GetImage(videoFilename.filename, UtilsCommon.GetMiddleTime(cardInfo), InstanceSettings.temporaryFilesPath + "subtitleMemorize.jpg", videoScaling);

			Gtk.Application.Invoke (delegate {
				if(selectedIndex == m_selectedPreviewIndex) // selection could have changed during the creation of the snapshot
					m_imagePreview.Pixbuf = new Gdk.Pixbuf (InstanceSettings.temporaryFilesPath + "subtitleMemorize.jpg", maxWidth, maxHeight);
			});

		}
		/// <summary>
		/// Update image and text view to match "selectedIndex". Can be in a different thread then Gtk-Thread. It will extract the image, so
		/// it can take 1-2 seconds.
		/// </summary>
		/// <param name="selectedIndex">Selected index.</param>
		private void SelectCard (int selectedIndex)
		{
			// do not select currently selected entry again
			if (selectedIndex == m_selectedPreviewIndex || !m_previewListModel.IsIndexInRange(selectedIndex))
				return;

			m_selectedPreviewIndex = selectedIndex;
			CardInfo cardInfo = m_previewListModel.GetCardClone(m_selectedPreviewIndex);

			Gtk.Application.Invoke (delegate {
				m_ignoreBufferChanges = true;

				// assigning text to buffer sets cursor to end -> do not assign if text didn't change
				if(m_textviewTargetLanguage.Buffer.Text != cardInfo.ToMultiLine(UtilsCommon.LanguageType.TARGET))
					m_textviewTargetLanguage.Buffer.Text = cardInfo.ToMultiLine(UtilsCommon.LanguageType.TARGET);
				if(m_textviewNativeLanguage.Buffer.Text != cardInfo.ToMultiLine(UtilsCommon.LanguageType.NATIVE))
					m_textviewNativeLanguage.Buffer.Text = cardInfo.ToMultiLine(UtilsCommon.LanguageType.NATIVE);

				m_ignoreBufferChanges = false;
			});

			int previewImageID = ++m_previewImageID;

			if(cardInfo.HasImage()) {
				// wait and see if the selected image is still the same (if user scrolls through list, is highly unperformant to extract all images
				// that are discarded every 10ms)
				Thread.Sleep (150);
				if (previewImageID != m_previewImageID)
				return;

				UpdatePreviewImage(selectedIndex, cardInfo);

			}
		}

		void ShowAllSelectedCardInfos ()
		{
			m_selectedPreviewIndex = -1;
			m_treeviewSelectionLines.UnselectAll ();
			m_liststoreLines.Clear ();
			for(int i = 0; i < m_previewListModel.GetLength(); i++)
				m_liststoreLines.AppendValues ("", "", "", "", "");

			var updateList = m_previewListModel.GenerateFullUpdateList();
			UpdatePreviewListViewByChangeList(updateList);
		}


		private string ReadResourceString(String resourceName) {
			var assembly = Assembly.GetExecutingAssembly ();

			using (Stream stream = assembly.GetManifestResourceStream (resourceName))
			using (StreamReader reader = new StreamReader (stream)) {
				return reader.ReadToEnd ();
			}
		}

		/// <summary>
		/// Opens the file chooser to select multiple files.
		/// </summary>
		/// <returns>Array of file names or null, if dialog was cancled</returns>
		/// <param name="title">Title.</param>
		private String[] OpenFileChooser(String title) {
			String[] filenames = null;
			Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog (title, m_mainWindow, FileChooserAction.Open,
				"_Cancel", ResponseType.Cancel,
				"_Open", ResponseType.Accept);
			fcd.DoOverwriteConfirmation = true;
			fcd.SelectMultiple = true;
			if(!String.IsNullOrWhiteSpace(m_currentPath)) fcd.SetFilename (m_currentPath);
			if ((Gtk.ResponseType)fcd.Run() == Gtk.ResponseType.Accept) {
				filenames = fcd.Filenames;
				if(filenames != null && filenames.Length > 0)
					m_currentPath = filenames[0];
			}
			fcd.Destroy ();

			return filenames;
		}



		void on_imagemenuitem_open_activate(object o, EventArgs args) {
			Gtk.FileChooserDialog fcd = new Gtk.FileChooserDialog ("Open configuration file", m_mainWindow, FileChooserAction.Open,
				"_Cancel", ResponseType.Cancel,
				"_Open", ResponseType.Accept);
			fcd.DoOverwriteConfirmation = true;
			if ((Gtk.ResponseType)fcd.Run() == Gtk.ResponseType.Accept) {
				LoadSaveStateFromFile (fcd.Filename);
			}
			fcd.Destroy ();
		}

		private void SaveStateToFile(string filename) {
			m_currentPath = filename;

			Settings settings = new Settings ();
			ReadGui (settings);
			using (var fileStream = new FileStream(filename, FileMode.Create)) {
				BinaryFormatter fmt = new BinaryFormatter ();
				fmt.Serialize (fileStream, settings);
			}
		}

		private void LoadSaveStateFromFile(string filename) {
			m_currentPath = filename;

			using (FileStream fileStream = new FileStream (filename, FileMode.Open)) {
				BinaryFormatter fmt = new BinaryFormatter ();
				Settings settings = (Settings) fmt.Deserialize (fileStream);
				UpdateGui (settings);

				// read gui because some values might not be set according to the given value (e.g. in case a file does exist a FilechooserButton does not show this entry)
				ReadGui(settings);
			}
		}

		/// <summary>
		/// Updates the GUI, so that all GUI fields are now set to the values found in "settings".
		/// </summary>
		private void UpdateGui(Settings settings) {

			System.Action<Gtk.Entry, string> setEntryPath = (entry, path) => {
				entry.Text = path ?? "";
			};

			setEntryPath (m_entryTargetLanguage, settings.TargetFilePath);
			setEntryPath (m_entryOutputDirectory, settings.OutputDirectoryPath);
			setEntryPath (m_entryNativeLanguage, settings.NativeFilePath);
			setEntryPath (m_entryVideoFile, settings.VideoFilePath);

			m_adjustmentAudioPaddingBefore.Value = (int)(settings.AudioPaddingBefore * 1000);
			m_adjustmentAudioPaddingAfter.Value = (int)(settings.AudioPaddingAfter * 1000);

			m_spinbuttonSub1TimeShift.Text = ((int)(settings.PerSubtitleSettings [0].SubDelay * 1000)).ToString();
			m_comboboxtextCorrectTimingsModeSub1.Active = (int)settings.PerSubtitleSettings [0].AlignMode;
			m_checkbuttonUseSub1Timings.Active = settings.PerSubtitleSettings [0].UseTimingsOfThisSub;

			m_spinbuttonSub2TimeShift.Text = ((int)(settings.PerSubtitleSettings [1].SubDelay * 1000)).ToString();
			m_comboboxtextCorrectTimingsModeSub2.Active = (int)settings.PerSubtitleSettings [1].AlignMode;
			m_checkbuttonUseSub2Timings.Active = settings.PerSubtitleSettings [1].UseTimingsOfThisSub;

      m_comboboxTargetLanguage.Active = settings.TargetLanguageIndex;

			m_entryDeckName.Text = settings.DeckName ?? "";
			m_spinbuttonEpisodeNumber.Text = settings.FirstEpisodeNumber.ToString();
		}

		/// <summary>
		/// Reads the GUI fields and saves them to settings.
		/// </summary>
		private void ReadGui(Settings settings) {
			settings.TargetFilePath = m_entryTargetLanguage.Text;
			settings.OutputDirectoryPath = m_entryOutputDirectory.Text;
			settings.NativeFilePath = m_entryNativeLanguage.Text;
			settings.VideoFilePath = m_entryVideoFile.Text;
			settings.DeckName = m_entryDeckName.Text;

			settings.NormalizeAudio = m_checkbuttonNormalizeAudio.Active;
			settings.ExportAudio = !m_checkbuttonDeactivateAudioClips.Active;

			settings.RescaleWidth = (int)m_spinbuttonMaxImageWidth.Value;
			settings.RescaleHeight = (int)m_spinbuttonMaxImageHeight.Value;
			settings.RescaleMode = (Settings.RescaleModeEnum) m_comboboxtextRescaleMode.Active;

			// read episode number
			int firstEpisodeNumber = 1;
			Int32.TryParse (m_spinbuttonEpisodeNumber.Text, out firstEpisodeNumber);
			settings.FirstEpisodeNumber = firstEpisodeNumber;

			settings.AudioPaddingBefore = m_adjustmentAudioPaddingBefore.Value / 1000.0;
			settings.AudioPaddingAfter = m_adjustmentAudioPaddingAfter.Value / 1000.0;

      settings.TargetLanguageIndex = m_comboboxTargetLanguage.Active;

			settings.PerSubtitleSettings[0].AlignMode = (PerSubtitleSettings.AlignModes) m_comboboxtextCorrectTimingsModeSub1.Active;
			settings.PerSubtitleSettings[0].UseTimingsOfThisSub = m_checkbuttonUseSub1Timings.Active;
			try { settings.PerSubtitleSettings[0].SubDelay = Int32.Parse(m_spinbuttonSub1TimeShift.Text ?? "0") / 1000.0; } catch {}

			settings.PerSubtitleSettings[1].AlignMode = (PerSubtitleSettings.AlignModes) m_comboboxtextCorrectTimingsModeSub2.Active;
			settings.PerSubtitleSettings[1].UseTimingsOfThisSub = m_checkbuttonUseSub2Timings.Active;
			try { settings.PerSubtitleSettings[1].SubDelay = Int32.Parse(m_spinbuttonSub2TimeShift.Text ?? "0") / 1000.0; } catch {}
		}


		/// <summary>
		/// Validates all gui-fields in main window.
		/// </summary>
		/// <returns><c>true</c>, if output could be generated with entered information, <c>false</c> otherwise.</returns>
		private void IsSettingsValid(Settings settings) {

			// TODO: Parse files with UtilsInputFiles

			if(String.IsNullOrWhiteSpace(settings.TargetFilePath)) {
				throw new Exception ("No subtitle file in your target language selected.");
			}

			// try to parse input file list
			TryParseInputFilesString(settings.TargetFilePath, "Error in target language file entry: ");


			if(String.IsNullOrWhiteSpace(settings.OutputDirectoryPath))
				throw new Exception ("No output directory selected.");

			if(!Directory.Exists(settings.OutputDirectoryPath))
				throw new Exception ("Selected output directory does not exist.");


			if (!String.IsNullOrWhiteSpace (settings.NativeFilePath))
				TryParseInputFilesString (settings.NativeFilePath, "Error in native language file entry: ");

			if (String.IsNullOrWhiteSpace(settings.DeckName)) {
				throw new Exception ("No deck name choosen.");
			}
		}

		/// <summary>
		/// Tries to parse and evaluate an attributed file path. If one of the files does not exist, an
		/// exception will be thrown.
		/// </summary>
		/// <returns>The input files string.</returns>
		/// <param name="str">String.</param>
		/// <param name="errorMessagePrefix">Prefix for exception.</param>
		private void TryParseInputFilesString(String str, String errorMessagePrefix) {
			try {
				// parse attributed file string
				UtilsInputFiles utilsInputFile = new UtilsInputFiles (m_entryTargetLanguage.Text);

				// try to find all files
				foreach (UtilsInputFiles.FileDesc fileDesc in utilsInputFile.GetFileDescriptions ())
					if (!File.Exists (fileDesc.filename))
						throw new Exception("File \"" + fileDesc.filename + "\" does not exist!");

			} catch (Exception ex) {
				throw new Exception (errorMessagePrefix + ex.Message);
			}
		}

		private String AddFilesToEntry(String originalString, String[] filesToAdd, String errorMessagePrefix) {

			UtilsInputFiles utilsInputFile;
			try {
				utilsInputFile = new UtilsInputFiles (originalString);
			} catch (Exception ex) {
				SetErrorMessage (errorMessagePrefix + ex.Message);
				return originalString;
			}

			foreach (String filename in filesToAdd) {
				if (String.IsNullOrWhiteSpace (filename))
					continue;
				utilsInputFile.AddFile(filename);
			}

			return utilsInputFile.ToString();
		}

		private void SetErrorMessage(string msg) {
			Console.WriteLine (msg);
			SetMarkupOfLabelInInfo("<span foreground=\"red\"><b>" + msg + "</b></span>\nIf any questions arise or if you want to file a bug, please visit <span foreground=\"white\"><a href=\"https://www.github.com/\">https://www.github.com/</a></span>.");
			m_infobar1.ShowAll ();

			int currentChangeNumber = m_numberOfInfobarLabelMarkupChanges;

			GLib.Timeout.AddSeconds (8, delegate() {
				if(m_numberOfInfobarLabelMarkupChanges == currentChangeNumber)
					SetMarkupOfLabelInInfo(m_infobarLabelStandardMarkup);
				return false;
			});
		}

		private void SetMarkupOfLabelInInfo(String markup) {
			m_numberOfInfobarLabelMarkupChanges++;
			m_labelInInfobar.Markup = markup;
		}

		private void ClearErrorMessages() {

		}



		public void GlibUnhandledException(GLib.UnhandledExceptionArgs args) {

			if (args.ExceptionObject is Exception) {
				Exception e = (Exception) args.ExceptionObject;
				if(e.InnerException != null) e = e.InnerException;
				SetErrorMessage(e.Message);
			}
			Console.WriteLine (args.ExceptionObject.ToString ());
		}

		public static void Main (string[] args)
		{
			// ensure that the temporary path ("/tmp/subtitleMemorize") exists
			Directory.CreateDirectory(InstanceSettings.temporaryFilesPath);

			// read system settings
			try {
				XmlSerializer ser = new XmlSerializer(typeof(SystemSettings));
				using(TextReader writer = new StreamReader (InstanceSettings.systemSettingFilePath)) {
					InstanceSettings.systemSettings = (SystemSettings) ser.Deserialize(writer);
				}
			} catch {
				Console.WriteLine ("WARNING: failed to read \"{0}\" so default settings are used", InstanceSettings.systemSettingFilePath);
				InstanceSettings.systemSettings.AdjustFirstTimeSettings();
			}

			new MainClass ();

			// write system settings (could be write protected -> ignore)
			try {
				XmlSerializer ser = new XmlSerializer (typeof(SystemSettings));
				Directory.CreateDirectory(InstanceSettings.settingsFolder);
				using (TextWriter writer = new StreamWriter (InstanceSettings.systemSettingFilePath, false)) {
					ser.Serialize (writer, InstanceSettings.systemSettings);
				}
			} catch(Exception) {
				Console.WriteLine ("WARNING: failed to write \"{0}\"", InstanceSettings.systemSettingFilePath);
			}
		}
	}
}
