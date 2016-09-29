Introduction
============

`SubtitleMemorize` is a tool that generates a list of images, audio files, lines and
translations of these lines by using video and subtitle files.
This is especially useful for language learning, as the resulting files can be imported into
[spaced repetition software](https://en.wikipedia.org/wiki/Spaced_repetition "Link to Wikipedia")
like [Anki](http://ankisrs.net/ "Link to Anki homepage").

This tool currently only runs on Linux and is a similar to
[subs2srs](http://subs2srs.sourceforge.net/), which is Windows-only. These two projects
share some code, but otherwise they are independent.

![Image](/Images/SubtitleMemorize_In_Action.png)

Installation
============
To make `SubtitleMemorize` work, following dependencies are needed:

-   `mono` to run .NET executable
-   `ffmpeg` for do media-related work (splitting video, extracting images/audio from video, rescaling images, normalizing audio, etc.)
-   `GTK+ 3` for the GUI
-   `mpv` for live audio preview

Scroll down to find the necessary installation commands for your distribution.

After that, download and extract the latest version of SubtitleMemorize from [here](https://github.com/ChangSpivey/SubtitleMemorize/releases). To start the program, execute the file `SubtitleMemorize.sh` or `bin/SubtitleMemorize.exe` in the extracted directory.

###### Ubuntu 16.04 LTS
```bash
sudo apt-get install libav-tools libavcodec-extra mono-complete mpv
```

###### Arch Linux

```bash
sudo pacman -S mono ffmpeg gtk3 mpv
```

Building From Source
------------


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

-   [substudy](https://github.com/emk/substudy)
-   [movies2anki](https://github.com/kelciour/movies2anki)


Status
============
This software has been successfully used to create a high-quality Anki deck. Because it is still in beta, some rough edges and unfinished features are nonetheless remaining. Any help, especially on documentation, would be greatly appreciated!
