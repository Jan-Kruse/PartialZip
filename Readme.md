# PartialZip

[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://github.com/Jan-Kruse/PartialZip/blob/master/LICENSE) 

PartialZip is a .NET-Standard library to download specific files from remote .zip archives.

PartialZip is based on [libfragmentzip](https://github.com/tihmstar/libfragmentzip) by [@tihmstar](https://twitter.com/tihmstar).

## External Dependencies

None

## Installation

TODO

## Usage

The remote web server must support [HTTP range requests](https://developer.mozilla.org/en-US/docs/Web/HTTP/Range_requests).

Loading a list of all files in a .zip archive:

```csharp
IEnumerable<string> fileList = await PartialZipDownloader.GetFileList("https://www.example.com/archive.zip");
```

Downloading a specific file from a .zip archive:

```csharp
byte[] fileContent = await PartialZipDownloader.DownloadFile("https://www.example.com/archive.zip", "file.txt");
```

## Credits

[@tihmstar](https://twitter.com/tihmstar) for [libfragmentzip](https://github.com/tihmstar/libfragmentzip)