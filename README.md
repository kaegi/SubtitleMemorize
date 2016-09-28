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
Dependencies:

-   `mono` to run C# code
-   `ffmpeg` for do media-related work (splitting video, extracting images/audio from video, rescaling images, normalizing audio, etc.)
-   `GTK+ 3` for the GUI

After installing these dependencies run following commands:

```bash
# building
git clone https://github.com/ChangSpivey/SubtitleMemorize
cd SubtitleMemorize
xbuild /p:Configuration=Release

# running
cd bin/Release
mono SubtitleMemorize.exe
```

In the future I am going to provide Linux binaries.


Related Projects
============
Projects that have similar goals and were updated recently:

-   [substudy](https://github.com/emk/substudy)
-   [movies2anki](https://github.com/kelciour/movies2anki)


Status
============
This software has been successfully used to create a high-quality Anki deck. Because it is still in beta, some rough edges and unfinished features are nonetheless remaining. Any help, especially on documentation, would be greatly appreciated!
