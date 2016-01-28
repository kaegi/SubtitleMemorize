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
using System.Xml;
using System.Xml.Serialization;
using Gtk;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace subs2srs4linux
{
	class MainClass
	{
		////////////////// AUTO-GENERATED CODE BEGIN //////////////////////
		#pragma warning disable 0414 // private field assigned but not used
		private Gtk.Action m_action1;
		private Gtk.Adjustment m_episodeAdjustment;
		private Gtk.Image m_image1;
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
		private Gtk.Frame m_frame10;
		private Gtk.Alignment m_alignment10;
		private Gtk.Box m_box6;
		private Gtk.CheckButton m_checkbutton1;
		private Gtk.ComboBox m_combobox1;
		private Gtk.CellRendererText m_cellrenderertextSelectEpisode;
		private Gtk.Label m_label21;
		private Gtk.Frame m_frame11;
		private Gtk.Alignment m_alignment11;
		private Gtk.Box m_box7;
		private Gtk.Frame m_frame12;
		private Gtk.Box m_box11;
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
		private Gtk.Label m_label1;
		private Gtk.Frame m_frame2;
		private Gtk.Alignment m_alignment2;
		private Gtk.Box m_box9;
		private Gtk.Frame m_frame3;
		private Gtk.Alignment m_alignment3;
		private Gtk.Box m_box3;
		private Gtk.RadioButton m_radiobutton1;
		private Gtk.RadioButton m_radiobutton2;
		private Gtk.Label m_label7;
		private Gtk.Frame m_frame4;
		private Gtk.Alignment m_alignment4;
		private Gtk.Grid m_grid2;
		private Gtk.Label m_label8;
		private Gtk.Label m_label9;
		private Gtk.Entry m_entrySpanStart;
		private Gtk.Entry m_entrySpanStart1;
		private Gtk.CheckButton m_checkbuttonUseSpan;
		private Gtk.Frame m_frame5;
		private Gtk.Alignment m_alignment5;
		private Gtk.Grid m_grid3;
		private Gtk.Label m_label6;
		private Gtk.Label m_label10;
		private Gtk.SpinButton m_spinbuttonSub1TimeShift;
		private Gtk.SpinButton m_spinbuttonSub2TimeShift;
		private Gtk.Label m_label12;
		private Gtk.Label m_label13;
		private Gtk.CheckButton m_checkbuttonUseTimeShift;
		private Gtk.Label m_label11;
		private Gtk.Box m_box4;
		private Gtk.Frame m_frame7;
		private Gtk.Alignment m_alignment7;
		private Gtk.Box m_box8;
		private Gtk.Frame m_frame8;
		private Gtk.Alignment m_alignment8;
		private Gtk.Entry m_entryDeckName;
		private Gtk.Label m_label15;
		private Gtk.Frame m_frame6;
		private Gtk.Alignment m_alignment6;
		private Gtk.SpinButton m_spinbuttonEpisodeNumber;
		private Gtk.Label m_label16;
		private Gtk.Label m_label14;
		private Gtk.Frame m_frame9;
		private Gtk.Alignment m_alignment9;
		private Gtk.Box m_box10;
		private Gtk.Button m_buttonPreview;
		private Gtk.Button m_buttonGo;
		private Gtk.Label m_label19;
		private Gtk.Window m_windowProgressInfo;
		private Gtk.Alignment m_alignment12;
		private Gtk.Box m_box14;
		private Gtk.ProgressBar m_progressbarProgressInfo;
		private Gtk.Button m_buttonCancelOperation;

		private void InitializeGtkObjects(Gtk.Builder b) {
			m_action1 = (Gtk.Action) b.GetObject("action1");
			m_episodeAdjustment = (Gtk.Adjustment) b.GetObject("episode_adjustment");
			m_image1 = (Gtk.Image) b.GetObject("image1");
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
			m_frame10 = (Gtk.Frame) b.GetObject("frame10");
			m_alignment10 = (Gtk.Alignment) b.GetObject("alignment10");
			m_box6 = (Gtk.Box) b.GetObject("box6");
			m_checkbutton1 = (Gtk.CheckButton) b.GetObject("checkbutton1");
			m_combobox1 = (Gtk.ComboBox) b.GetObject("combobox1");
			m_cellrenderertextSelectEpisode = (Gtk.CellRendererText) b.GetObject("cellrenderertext_select_episode");
			m_label21 = (Gtk.Label) b.GetObject("label21");
			m_frame11 = (Gtk.Frame) b.GetObject("frame11");
			m_alignment11 = (Gtk.Alignment) b.GetObject("alignment11");
			m_box7 = (Gtk.Box) b.GetObject("box7");
			m_frame12 = (Gtk.Frame) b.GetObject("frame12");
			m_box11 = (Gtk.Box) b.GetObject("box11");
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
			m_label1 = (Gtk.Label) b.GetObject("label1");
			m_frame2 = (Gtk.Frame) b.GetObject("frame2");
			m_alignment2 = (Gtk.Alignment) b.GetObject("alignment2");
			m_box9 = (Gtk.Box) b.GetObject("box9");
			m_frame3 = (Gtk.Frame) b.GetObject("frame3");
			m_alignment3 = (Gtk.Alignment) b.GetObject("alignment3");
			m_box3 = (Gtk.Box) b.GetObject("box3");
			m_radiobutton1 = (Gtk.RadioButton) b.GetObject("radiobutton1");
			m_radiobutton2 = (Gtk.RadioButton) b.GetObject("radiobutton2");
			m_label7 = (Gtk.Label) b.GetObject("label7");
			m_frame4 = (Gtk.Frame) b.GetObject("frame4");
			m_alignment4 = (Gtk.Alignment) b.GetObject("alignment4");
			m_grid2 = (Gtk.Grid) b.GetObject("grid2");
			m_label8 = (Gtk.Label) b.GetObject("label8");
			m_label9 = (Gtk.Label) b.GetObject("label9");
			m_entrySpanStart = (Gtk.Entry) b.GetObject("entry_span_start");
			m_entrySpanStart1 = (Gtk.Entry) b.GetObject("entry_span_start1");
			m_checkbuttonUseSpan = (Gtk.CheckButton) b.GetObject("checkbutton_use_span");
			m_frame5 = (Gtk.Frame) b.GetObject("frame5");
			m_alignment5 = (Gtk.Alignment) b.GetObject("alignment5");
			m_grid3 = (Gtk.Grid) b.GetObject("grid3");
			m_label6 = (Gtk.Label) b.GetObject("label6");
			m_label10 = (Gtk.Label) b.GetObject("label10");
			m_spinbuttonSub1TimeShift = (Gtk.SpinButton) b.GetObject("spinbutton_sub1_time_shift");
			m_spinbuttonSub2TimeShift = (Gtk.SpinButton) b.GetObject("spinbutton_sub2_time_shift");
			m_label12 = (Gtk.Label) b.GetObject("label12");
			m_label13 = (Gtk.Label) b.GetObject("label13");
			m_checkbuttonUseTimeShift = (Gtk.CheckButton) b.GetObject("checkbutton_use_time_shift");
			m_label11 = (Gtk.Label) b.GetObject("label11");
			m_box4 = (Gtk.Box) b.GetObject("box4");
			m_frame7 = (Gtk.Frame) b.GetObject("frame7");
			m_alignment7 = (Gtk.Alignment) b.GetObject("alignment7");
			m_box8 = (Gtk.Box) b.GetObject("box8");
			m_frame8 = (Gtk.Frame) b.GetObject("frame8");
			m_alignment8 = (Gtk.Alignment) b.GetObject("alignment8");
			m_entryDeckName = (Gtk.Entry) b.GetObject("entry_deck_name");
			m_label15 = (Gtk.Label) b.GetObject("label15");
			m_frame6 = (Gtk.Frame) b.GetObject("frame6");
			m_alignment6 = (Gtk.Alignment) b.GetObject("alignment6");
			m_spinbuttonEpisodeNumber = (Gtk.SpinButton) b.GetObject("spinbutton_episode_number");
			m_label16 = (Gtk.Label) b.GetObject("label16");
			m_label14 = (Gtk.Label) b.GetObject("label14");
			m_frame9 = (Gtk.Frame) b.GetObject("frame9");
			m_alignment9 = (Gtk.Alignment) b.GetObject("alignment9");
			m_box10 = (Gtk.Box) b.GetObject("box10");
			m_buttonPreview = (Gtk.Button) b.GetObject("button_preview");
			m_buttonGo = (Gtk.Button) b.GetObject("button_go");
			m_label19 = (Gtk.Label) b.GetObject("label19");
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
		private bool m_ignoreLineSelectionChanges = false;
		private List<EpisodeInfo> m_episodeInfo = new List<EpisodeInfo>();
		private List<UtilsSubtitle.EntryInformation> m_allEntryInfomation = new List<UtilsSubtitle.EntryInformation>(); // all entries from all episodes
		private List<UtilsSubtitle.EntryInformation> m_previewWindowEntries = new List<UtilsSubtitle.EntryInformation>(); // only entries that are currently shown
		private int m_selectedPreviewIndex = -1; // index of single selected/focused entry in "m_previewWindowEntries"

		// ##################################################################33
		// Variables for Subtitle-Options-Window
		private int m_subOptionsWindow_subIndex = 0;
		private List<int> m_subOptionsWindowStreamIndices = new List<int>();


		// ##################################################################33
		// Variables for Progress-Window
		private enum PendingOperation{
			GENERATE_PREVIEW, // "Preview"/"Refresh"
			GENERATE_OUTPUT, // "Go"
			NOTHING
		}
		private PendingOperation m_pendingOperation = PendingOperation.NOTHING;
		private InfoProgress m_progressAndCancellable = null;

		static private readonly string m_infobarLabelStandardMarkup = "Welcome to subs2srs4linux!" +
			" To see more information just hover the cursor over a button or field.\n" +
			"If any questions arise, please visit <span foreground=\"white\"><a href=\"https://www.github.com/\">https://www.github.com/</a></span>.";

		public MainClass() {
			GLib.ExceptionManager.UnhandledException += GlibUnhandledException;

			Gtk.Application.Init ();
			m_builder.AddFromString (ReadResourceString ("subs2srs4linux.Resources.gtk.glade"));
			m_builder.Autoconnect (this);

			InitializeGtkObjects (m_builder);
			ConnectEventsMainWindow ();
			ConnectEventsSubitleWindowOptions ();
			ConnectEventsPreviewWindowOptions ();
			ConnectEventsProgressWindow ();


			m_mainWindow.ShowAll();



			ReadGui (m_defaultSettings);
			m_subtitleOptionsWindow.HideOnDelete ();


			if (InstanceSettings.systemSettings.preLoadedSettings != null)
				LoadSaveStateFromFile (InstanceSettings.systemSettings.preLoadedSettings);

			// this has to be after "mainWindow.Show()", because otherwise the width of the window
			// is determined by the width of this text
			m_labelInInfobar.Markup = m_infobarLabelStandardMarkup;


			//on_button_preview_clicked(null, null);

			Application.Run ();
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
				if (String.IsNullOrWhiteSpace (fileDesc.filename))
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
				GoOrPreviewClicked(PendingOperation.GENERATE_PREVIEW);
			};


			// ----------------------------------------------------------------------------------------------------
			m_buttonGo.Clicked += delegate(object o, EventArgs args) {
				GoOrPreviewClicked(PendingOperation.GENERATE_OUTPUT);
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
			
				SelectEntry();
			};
				
			m_treeviewSelectionLines.Changed += delegate(object sender, EventArgs e) {
				SelectEntry();
			};

			m_buttonPlayContent.Clicked += delegate(object sender, EventArgs e) {
				if(m_selectedPreviewIndex < 0) return;

				UtilsSubtitle.EntryInformation entryInfo = m_previewWindowEntries[m_selectedPreviewIndex];
				EpisodeInfo episodeInfo = m_episodeInfo[entryInfo.episodeInfo.Index];
				String arguments = String.Format("--really-quiet --no-video --start={0} --end={1} \"{2}\"", UtilsCommon.ToTimeArg(entryInfo.startTimestamp), UtilsCommon.ToTimeArg(entryInfo.endTimestamp), episodeInfo.VideoFileDesc.filename);


				Thread thr = new Thread (new ThreadStart (delegate() {
					UtilsCommon.CallExeAndGetStdout("mpv", arguments);
				}));
				thr.Start ();
			};
		}


		private static List<EpisodeInfo> GenerateEpisodeInfos(Settings settings) {

			// get all filenames
			UtilsInputFiles sub1Files = new UtilsInputFiles (settings.TargetFilePath);
			UtilsInputFiles sub2Files = new UtilsInputFiles (settings.NativeFilePath);
			UtilsInputFiles videoFiles = new UtilsInputFiles (settings.VideoFilePath);

			// TODO: Handle different list lengths
			List<UtilsInputFiles.FileDesc> sub1FileDescs = sub1Files.GetFileDescriptions();
			List<UtilsInputFiles.FileDesc> sub2FileDescs = sub2Files.GetFileDescriptions();
			List<UtilsInputFiles.FileDesc> videoFileDescs = videoFiles.GetFileDescriptions();

			int numberOfEpisodes = sub1FileDescs.Count;
			if(numberOfEpisodes != sub2FileDescs.Count || numberOfEpisodes != videoFileDescs.Count)
				throw new Exception("Number of files in target languages and number of files in native language does not match.");

			// fill episode info
			List<EpisodeInfo> episodeFiles = new List<EpisodeInfo>();
			for(int episodeIndex = 0; episodeIndex < numberOfEpisodes; episodeIndex++)
				episodeFiles.Add(new EpisodeInfo(episodeIndex, episodeIndex + settings.FirstEpisodeNumber, videoFileDescs[episodeIndex], videoFileDescs[episodeIndex], sub1FileDescs[episodeIndex], sub2FileDescs[episodeIndex]));

			return episodeFiles;
		}

		private void GoOrPreviewClicked(PendingOperation previewOrGo) {


			// because this function/delegate is synchronized in gtk-thread -> guard with simple variable against pressing button two times
			if (!OpenProgressWindow (previewOrGo))
				return;

			// read all required information to class/struct, so that off-gtk-thread computation is possible
			Settings settings = new Settings ();
			ReadGui (settings);

			// quickly decide whether these inputs can be used for a run
			if (!IsSettingsValid (settings))
				return;

			Thread compuationThread = new Thread(new ThreadStart(delegate {

				InfoProgress progressInfo = new InfoProgress(ProgressHandler);
				m_progressAndCancellable = progressInfo;

				// find sub1, sub2, audio and video file per episode
				m_episodeInfo.Clear();
				m_episodeInfo.AddRange(GenerateEpisodeInfos(settings));

				// fill in progress sections
				for(int i = 0; i < m_episodeInfo.Count; i++)
					progressInfo.AddSection(String.Format("Episode {0:00.}: Extracting Sub1", i + 1), 1);
				for(int i = 0; i < m_episodeInfo.Count; i++)
					progressInfo.AddSection(String.Format("Episode {0:00.}: Extracting Sub2", i + 1), 1);
				for(int i = 0; i < m_episodeInfo.Count; i++)
					progressInfo.AddSection(String.Format("Episode {0:00.}: Matching subtitles", i + 1), 1);

				progressInfo.AddSection("Preparing data presentation", 1);
				progressInfo.StartProgressing();


				// read all sub-files, match them and create a list for user that can be presented in preview window
				m_allEntryInfomation.Clear();
				m_allEntryInfomation.AddRange(GenerateEntryInformation(settings, m_episodeInfo, progressInfo) ?? new List<UtilsSubtitle.EntryInformation>());

				if(!progressInfo.Cancelled) {

					// finish this last step
					progressInfo.ProcessedSteps(1);

					//infoProgress.ProcessedSteps(1);

					// choose to show all episodes
					SelectEpisodeForPreview(-1);

					if(previewOrGo == PendingOperation.GENERATE_PREVIEW)
						PopulatePreviewList();
					else
						ExportData(settings, progressInfo);
				}

				// close progress window, free pending operation variable
				CloseProgressWindow ();
			}));
			compuationThread.Start();

		}

		private void ExportData(Settings settings, InfoProgress progressInfo) {
			String tsvFilename = settings.OutputDirectoryPath + Path.DirectorySeparatorChar + settings.DeckName + ".tsv";
			String snapshotsPath = settings.OutputDirectoryPath + Path.DirectorySeparatorChar + settings.DeckName + "_snapshots" + Path.DirectorySeparatorChar;
			String audioPath = settings.OutputDirectoryPath + Path.DirectorySeparatorChar + settings.DeckName + "_audio" + Path.DirectorySeparatorChar;
			Console.WriteLine (tsvFilename);

			// extract images
			if(Directory.Exists(snapshotsPath)) Directory.Delete(snapshotsPath, true);
		 	Directory.CreateDirectory(snapshotsPath);
			List<String> snapshotFields = WorkerSnapshot.ExtractSnaphots(settings, snapshotsPath, m_allEntryInfomation);

			// extract audio
			if(Directory.Exists(audioPath)) Directory.Delete(audioPath, true);
			Directory.CreateDirectory(audioPath);
			List<String> audioFields = WorkerAudio.ExtractAudio(settings, audioPath, m_allEntryInfomation);


			// TODO: normalize audio

			using(var outputStream = new StreamWriter(tsvFilename)) {
				for (int i = 0; i < m_allEntryInfomation.Count; i++) {
					UtilsSubtitle.EntryInformation entryInfo = m_allEntryInfomation[i];

					String keyField = entryInfo.GetKey ();
					String audioField = audioFields [i];
					String imageField = snapshotFields [i];
					outputStream.WriteLine (keyField + "\t" + audioField + "\t" + entryInfo.targetLanguageString + "\t" + entryInfo.nativeLanguageString);
				}
			}
		}

		private void PopulatePreviewList() {

			Gtk.Application.Invoke(delegate {

				// populate subtitle list
				m_liststoreLines.Clear ();
				ShowAllSelectedEntryInformations();


				// select first entry
				m_treeviewSelectionLines.UnselectAll();
				TreeIter firstTreeIter = new TreeIter();
				m_liststoreLines.GetIterFirst(out firstTreeIter);
				m_treeviewSelectionLines.SelectIter(firstTreeIter);

				m_previewWindow.ShowAll ();
			});
		}

		/// <summary>
		/// This reads all subtitles, matches them and saves them to the "m_allEntryInfomation"
		/// </summary>
		/// <param name="settings">Settings.</param>
		private static List<UtilsSubtitle.EntryInformation> GenerateEntryInformation(Settings settings, List<EpisodeInfo> episodeInfos, InfoProgress progressInfo) {
			
			// read subtitles
			List<List<LineInfo>> lineInfosPerEpisode_TargetLanguage = ReadAllSubtitleFiles(settings, settings.PerSubtitleSettings[0], episodeInfos, 0, progressInfo);
			List<List<LineInfo>> lineInfosPerEpisode_NativeLanguage = ReadAllSubtitleFiles(settings, settings.PerSubtitleSettings[1], episodeInfos, 1, progressInfo);

			if (progressInfo.Cancelled)
				return null;

			List<UtilsSubtitle.EntryInformation> allEntryInformations = new List<UtilsSubtitle.EntryInformation> ();
			for(int episodeIndex = 0; episodeIndex < lineInfosPerEpisode_TargetLanguage.Count; episodeIndex++) {
				List<LineInfo> list1 = lineInfosPerEpisode_TargetLanguage[episodeIndex];
				List<LineInfo> list2 = lineInfosPerEpisode_NativeLanguage[episodeIndex];

				List<SubtitleMatcher.BiMatchedLines> matchedLinesList = SubtitleMatcher.MatchSubtitles(list1, list2);
				List<UtilsSubtitle.EntryInformation> thisEpisodeEntryInfos = SubtitleMatcher.GetEntryInformation(settings.IgnoreSingleSubLines, episodeInfos[episodeIndex], matchedLinesList, list1, list2);
				allEntryInformations.AddRange(thisEpisodeEntryInfos);

				progressInfo.ProcessedSteps (1);

				if (progressInfo.Cancelled)
					return null;
			}

			return allEntryInformations;
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

			// read properties of first file to get selected stream and 
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
		/// Finds first selected entry in preview list and then calls "SelectEntry(int)" with this entry number in another thread.
		/// </summary>
		private void SelectEntry () {
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
					SelectEntry(leastIndex);
				}));
				thr.Start ();
			}
		}

		/// <summary>
		/// Update image and text view to match "selectedIndex". Can be in a different thread then Gtk-Thread. It will extract the image, so 
		/// it can take 1-2 seconds.
		/// </summary>
		/// <param name="selectedIndex">Selected index.</param>
		private void SelectEntry (int selectedIndex)
		{
			// do not select currently selected entry again
			if (selectedIndex == m_selectedPreviewIndex || selectedIndex < 0 || selectedIndex >= m_previewWindowEntries.Count)
				return;
			
			m_selectedPreviewIndex = selectedIndex;
			UtilsSubtitle.EntryInformation entryInfo = m_previewWindowEntries [selectedIndex];

			Gtk.Application.Invoke (delegate {
				m_textviewTargetLanguage.Buffer.Text = entryInfo.targetLanguageString;
				m_textviewNativeLanguage.Buffer.Text = entryInfo.nativeLanguageString;
			});

			// wait and see if the selected image is still the same (if user scrolls through list, is highly unperformant to extract all images
			// that are only selected like 10ms)
			Thread.Sleep (150);
			if (selectedIndex != m_selectedPreviewIndex)
				return;

			UtilsInputFiles.FileDesc videoFilename = m_episodeInfo[entryInfo.episodeInfo.Index].VideoFileDesc;
			UtilsImage.GetImage(videoFilename.filename, UtilsCommon.GetMiddleTime(entryInfo.startTimestamp, entryInfo.endTimestamp), "/tmp/subs2srs.jpg");

			Gtk.Application.Invoke (delegate {
				if(selectedIndex == m_selectedPreviewIndex) // selection could have changed during the creation of the snapshot
					m_imagePreview.Pixbuf = new Gdk.Pixbuf ("/tmp/subs2srs.jpg", 300, 300);
			});
		}

		/// <summary>
		/// Links entries from episode "i" from "m_allEntryInfomation" to "m_previewWindowEntries". In case
		/// "i == -1", all entries are linked.
		/// </summary>
		/// <param name="i">The index.</param>
		private void SelectEpisodeForPreview (int i)
		{
			m_previewWindowEntries.Clear ();
			if (i == -1) // all episodes
				m_previewWindowEntries.AddRange (m_allEntryInfomation);
			else
				throw new NotImplementedException ();
		}

		void ShowAllSelectedEntryInformations ()
		{
			m_selectedPreviewIndex = -1;
			m_treeviewSelectionLines.UnselectAll ();
			m_liststoreLines.Clear ();
			foreach (UtilsSubtitle.EntryInformation entryInfo in m_previewWindowEntries)
				m_liststoreLines.AppendValues (entryInfo.targetLanguageString, entryInfo.nativeLanguageString);
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

			m_spinbuttonSub1TimeShift.Text = settings.PerSubtitleSettings [0].SubDelay.ToString();
			m_spinbuttonSub2TimeShift.Text = settings.PerSubtitleSettings [1].SubDelay.ToString();

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

			// read episode number
			int firstEpisodeNumber = 1;
			Int32.TryParse (m_spinbuttonEpisodeNumber.Text, out firstEpisodeNumber);
			settings.FirstEpisodeNumber = firstEpisodeNumber;

			try { settings.PerSubtitleSettings[0].SubDelay = Int32.Parse(m_spinbuttonSub1TimeShift.Text ?? "0"); } catch {}
			try { settings.PerSubtitleSettings[1].SubDelay = Int32.Parse(m_spinbuttonSub2TimeShift.Text ?? "0"); } catch {}
		}


		/// <summary>
		/// Validates all gui-fields in main window.
		/// </summary>
		/// <returns><c>true</c>, if output could be generated with entered information, <c>false</c> otherwise.</returns>
		private bool IsSettingsValid(Settings settings) {

			// TODO: Parse files with UtilsInputFiles

			if(String.IsNullOrWhiteSpace(settings.TargetFilePath)) {
				throw new Exception ("No subtitle file in your target language selected.");
			}

			// try to parse input file list
			ParseInputFilesString(settings.TargetFilePath, "Error in target language file entry: ");

			if(String.IsNullOrWhiteSpace(settings.OutputDirectoryPath))
				throw new Exception ("No output directory selected.");

			if(!Directory.Exists(settings.OutputDirectoryPath)) 
				throw new Exception ("Selected output directory does not exist.");
			

			if (!String.IsNullOrWhiteSpace (settings.NativeFilePath))
				ParseInputFilesString (settings.NativeFilePath, "Error in native language file entry: ");

			if (String.IsNullOrWhiteSpace(settings.DeckName)) {
				throw new Exception ("No deck name choosen.");
			}

			return true;
		
		}

		private UtilsInputFiles ParseInputFilesString(String str, String errorMessagePrefix) {
			UtilsInputFiles utilsInputFile;
			try {
				utilsInputFile = new UtilsInputFiles (m_entryTargetLanguage.Text);
			} catch (Exception ex) {
				throw new Exception (errorMessagePrefix + ex.Message);
			}
			return utilsInputFile;
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
			// ensure, the temporary path exists
			Directory.CreateDirectory(InstanceSettings.temporaryFilesPath);

			// read system settings
			try {
				XmlSerializer ser = new XmlSerializer(typeof(SystemSettings));
				using(TextReader writer = new StreamReader ("settings.s2s4l")) {
					InstanceSettings.systemSettings = (SystemSettings) ser.Deserialize(writer);
				}
			} catch {
				Console.WriteLine ("WARNING: failed to read \"setttings.s2s4l\" so default settings are used");
			}

			Console.WriteLine (InstanceSettings.systemSettings.overlappingThreshold_InterSub);

			new MainClass ();

			// write system settings (could be write protected -> ignore)
			try {
				XmlSerializer ser = new XmlSerializer (typeof(SystemSettings));
				using (TextWriter writer = new StreamWriter ("settings.s2s4l", false)) {
					ser.Serialize (writer, InstanceSettings.systemSettings);
				}
			} catch(Exception) {
			}
		}
	}
}
	
