# Tutorial
To get back to the introduction page and how-to-install, click [here](README.md).

## Step-by-step guide
Unfortunally, this is still on the TODO list. Pull requests are greatly welcome.

### How to import into Anki
-   Import `AnkiTemplate.apkg` into Anki. This will create the SubtitleMemorize card type - you can safely delete the dummy card.

-   Now import the generated `.tsv` file. Make sure you `Allow HTML in fields`! Field 10 should be mapped to `Tags`.

-   Copy the files inside the generated `*_audio` and `*_snapshots` folders into `~/Documents/Anki/Username/collection.media/`.

-   That's it! You are good to go!

## Advanced features
### Batch processing
You can process multiple input files at once, all you have to do is to do is to select these files after pressing an `Add XXX`-button. Wildcards are also supported: instead of having to type/add `/home/user/Videos/ep001.mkv,/home/user/Videos/ep002.mkv,/home/user/Videos/ep003.mkv` etc.. you can just type `/home/user/Videos/ep???.mkv` or even `/home/user/Videos/ep*`. `?` is a single character, `*` stands for multiple characters.

By adding a comma but leaving out the filename you can create an episode that is missing information. For example a for single episode that does not have subtitles in your native languge (`/path/to/sub1,,/path/to/sub3`).


### NCalc search
The NCalc search allows you to easily query certain lines. Examples:

-   type in `duration > 10` and press enter. This will select all lines that
have a duration of more than 10 seconds. Pressing `Disable Line` will prevent the line from being exported.

-   type in `regex(',$')` and press enter. This will select all lines that end with a comma. By clicking on `Merge next` you can simply join all sentences together at the same time. Of course you can also query more than one ending character at the same time: `regex('(→|,)$')`.

-   type in `next_start - end < 2` and press enter. This will select all lines which have a time gap of less then two seconds to the next line. By clicking on `Merge next` you will obtain "packages" of full conversations.

-   type in `regex(',$') and next_start - end < 2` and press enter. This will select all lines that are selected in both example 2 and 3.

As you can see, this system is very flexible.

There are basically three different components. There are values (`duration` `end` `next_start` `',$'`), functions (`regex()` `contains()`) and operators (`<` `-` `and`). Every expression you type will be evaluated for every line and if the result is `true`, then the line gets selected (behaviour can be changed with `shift` and `ctrl`).

#### Values


-   `text1`, `sub1` (string): the text of the line in your target language

-   `text2`, `sub2` (string): the text of the line in your native language

-   `text`, `sub` (string): both lines combined into one, separated by a space character; `text1` + space  + `text2`

-   `actor`, `actors`, `name`, `names` (string): text that is displayed in third column; example: `<Paul> <Peter>`

-   `start`, `end` and `duration` (floating-point number): the start, end and duration of the line in seconds

-   `prev_start`, `prev_end` and `prev_duration` (floating-point number): the start, end and duration of the line before this line

-   `next_start`, `next_end` and `next_duration` (floating-point number): the start, end and duration of the line after this line

-   `isActive`, `active` (boolean): is `true` if line gets exported, `false` if line is deactivated (background is grey)

-   `number`, `episodeNumber` (integer): self-explanatory (for use with multiple files/batch processing)

-   integers as `42` and floating-point numbers such as `10.0`

-   strings as `'hello'`; be aware of escaping with `\`

    a single quotation mark is ``\'``, a backslash is `\\`, tabs are `\t`

#### Functions

-   `contains()` and its shorthand `c()`:

    Function expects one string or more than one string. Returns true if one of the first strings contains the last string (case insensitive). If only one argument is given, this function returns `true` if `text1` or `text2` contain the given string.

    Any line that contains `hello` in target-language-text: `c(text1, 'hello')`

    Any line that contains `hello`: `c(text1, text2, 'hello')` or `c('hello')`

    Any line that has actor `Peter`: `c(actors, '<Peter>')`

    Any line where the target-language-text contains the native-language-text (but why would you ever need that?): `c(text2, text1)`

-   `regex()` and its shorthand `r()`:

    Basically the same as `contains()` with the difference, that the last argument is a Perl-regex pattern instead of a simple string.

    Any line where the target-language-text begins with `A`: `r(text1, '^A')`

    Any line that only has actor `Peter`: `r(actors, '^<Peter>$')`

    Any line that only has actor `Paul` or `Peter`: `r(actors, '^<(Paul|Peter)>$')`

    Any line that contains a number: `r('[0-9]+')`

-   `time()`:

    Converts a time string to (floating-point) number of seconds.

    Any line that ends before minute 25 and 30.5 seconds: `end < time('25:30.5')`

#### Operators
-   numerical: `+` `-` `*` `\`

-   logical: `and` or `&&`, `or` or `||`

-   comparisons: `is` or `==` (yes, two equal signs), `<`, `>`

-   negation: `not` or `!` (for example: `not active` or `!c('hello')`)
