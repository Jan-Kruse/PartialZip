# PartialZip

[![License: LGPL v3](https://img.shields.io/badge/License-LGPL%20v3-blue.svg)](https://github.com/Jan-Kruse/PartialZip/blob/master/LICENSE)
[![Nuget](https://img.shields.io/nuget/v/PartialZip.svg?colorB=green&style=flat)](https://www.nuget.org/packages/PartialZip/1.0.1)

PartialZip is a .NET-Standard library to download specific files from remote .zip archives.

PartialZip is based on [libfragmentzip](https://github.com/tihmstar/libfragmentzip) by [@tihmstar](https://twitter.com/tihmstar).

## External Dependencies

None

## Installation

Nuget:
```
PM> Install-Package PartialZip
```

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

## How it works

- PartialZip sends a HTTP `HEAD` request  to the archive url to retrieve the `Content-Length` and `Accept-Range` Header.
    - If the web server does not support byte range requests, an exception is thrown.
- PartialZip uses the content length to get the `End Of Central Directory` record. If certain values reach their maximum limit, PartialZip can determine if it is a ZIP64 file.
    - If it is a ZIP64 file, PartialZip gets the `ZIP64 End Of Central Directory Locator` right before the `End Of Central Directory`.
    - With the locator, PartialZip can compute the byte ranges for the `ZIP64 End Of Central Directory`
- The `(ZIP64) End Of Central Directory` record tells PartialZip the start of the `Central Directory`, its size and its element count.
- Now, PartialZip can get the `Central Directory` containing the filenames, sizes and offsets for all files in the archive.
- To download a specific file, PartialZip gets the `Central Directory` record for the file to compute the byte ranges for the `Local File Header` of the file. The `Local File Header` contains information about the compression and other values used to compute the start offset of the file data.
- PartialZip can now download the byte range for the file data and inflate it in case of `Deflate` compression.

Further reading: [.ZIP File Format Specification](https://pkware.cachefly.net/webdocs/casestudies/APPNOTE.TXT) by PKWARE Inc.

## Credits

[@tihmstar](https://twitter.com/tihmstar) for [libfragmentzip](https://github.com/tihmstar/libfragmentzip)

## Contributors
[BuIlDaLiBlE](https://github.com/BuIlDaLiBlE) 