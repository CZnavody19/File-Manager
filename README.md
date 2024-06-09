# File manager

## Explorer

The explorer lists all files and directories (even the system ones) with their corresponding icon and (for files) size

You can open each file in its default application

The overall size of all files is also displayed together with the file and directory count

The explorer catches file and directory changes and refreshes automatically

You can also go to different drives and network shares

### Disclaimer: network shares are a bit slow and wonky

## History system

Each file view has its own history you can navigate with the back and forward buttons

## Search system

The search checks if the filename contains the searched text ignoring the case

The search clears with every load of a directory, this means even if a file change is detected

You can clear the search by searching an empty text

The search supports regex, to use it you need to prefix it with `<re>` (like this `<re>.*\.exe$` to find all `.exe` files)

### Warning: regex is a lot slower than normal searching

## Clipboard system

You can add both directories and files to the clipboard

You can add new items without removing the already copied

Cutting is also supported

## Known issues

-   When you have a connected network share the startup hangs sometimes
-   Network shares are a bit wonky in general
-   When copying a large amount of files, each error is displayed as a separate window
-   The size column in the file view is not aligned to the right side
-   Editing the path is a bit wonky when loading a large folder
-   The explorer shows everything, including for example the system page and swap files
