iLBC2PCM.NET
============

Convert iLBC compressed audio data wrapped in a CAF file to uncompressed linear PCM and store it into a WAV container.

Content
-------

1) iLBC

C# fork of the original iLBC source codes. These files have been created by Walking Cat (see copyright information below)
These classes are actually used by the test application.

2) iLBCConverter

Trying to pack the original iLBC C-Sources into a DLL to be imported by other projects. The Sourcefiles have been extracted from the iLBC specification file (rfc3951.txt).
This project cannot be compiled by now due to a linker error. If somebody has a clue to fix this, please send a pull request!

3) iLBCTest

The actual Testapplication. It includes Classes for a CAF reader, a class for a WAV writer and the main class where some sound is converted.
Within the debug build folder are some sound files for the test.

Copyright
---------

1) For copyright information on the iLBC-C#-Library, please visit https://ilbcnet.codeplex.com/

2) For copyright information about iLBC itself, please visit http://www.ilbcfreeware.org

3) All files that are not included within the above licenses, are available unter MIT license:

Copyright (c) 2012 Christoph Graumann

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.


