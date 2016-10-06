Introduction
============

`SubtitleMemorize` is a tool that generates a list of images, audio files, lines and
translations of these lines by using video and subtitle files.
This is especially useful for language learning, as the resulting files can be imported into
spaced repetition software
like [Anki](http://ankisrs.net/ "Link to Anki homepage"). To make it even more effective, all generated cards can be used directly in the Anki plugin [MorphMan](https://github.com/ChangSpivey/MorphMan), which sort the massive amounts of cards based on the cards difficulty and the words you know.

This tool currently only runs on Linux, but is deliberately built solely on top of portable technologies.

![Image](/Images/SubtitleMemorize_In_Action.png)

Installation
============
To make `SubtitleMemorize` work, you have to install its dependencies first. Scroll down to find the necessary installation commands for your distribution. These are in detail:

-   `mono` to run .NET executable
-   `ffmpeg` for do media-related work (splitting video, extracting images/audio from video, rescaling images, normalizing audio, etc.)
-   `GTK+ 3` for the GUI
-   `mpv` for live audio preview (optional)


After that, download and extract the latest version of SubtitleMemorize from [here](https://github.com/ChangSpivey/SubtitleMemorize/releases). To start the program, execute the file `SubtitleMemorize.sh` or `bin/SubtitleMemorize.exe` in the extracted directory.

###### Ubuntu 16.04 LTS
```bash
sudo apt-get install libav-tools libavcodec-extra mono-complete mpv
```

###### Arch Linux

```bash
sudo pacman -S mono ffmpeg gtk3 mpv
```

###### Windows and macOS
Your know-how is needed! If you managed to install the dependencies on these platforms and successfully created a SubtitleMemorize deck, please create an issue on the GitHub page, so I can add an installation tutorial.


Tutorial
============
Click [here](TUTORIAL.md) for examples how to import files into Anki and how use the more advanced features.

MorphMan integration
============
The generated cards are ready to be used in the Anki plugin [MorphMan](https://github.com/ChangSpivey/MorphMan). This will make your learing-with-movies much more effective. No additional configuration is needed.

Building From Source
============


After installing the above mentioned dependencies for your distribution run following commands:

```bash
# building
git clone https://github.com/ChangSpivey/SubtitleMemorize
cd SubtitleMemorize
xbuild /p:Configuration=Release

# running
cd bin/Release
mono SubtitleMemorize.exe
```

Related Projects
============
Projects that have similar goals and were updated recently:

-   [subs2srs](http://subs2srs.sourceforge.net/) (SubtitleMemorize is inspired by subs2srs; they share some code)
-   [substudy](https://github.com/emk/substudy)
-   [movies2anki](https://github.com/kelciour/movies2anki)


Status
============
This software has been successfully used to create a high-quality Anki deck. Because it is still in beta, some rough edges and unfinished features are nonetheless remaining. Any help, especially on documentation, would be greatly appreciated!
