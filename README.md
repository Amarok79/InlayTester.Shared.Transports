This library is available as NuGet package:
[Amarok.InlayTester.Shared.Transports](https://www.nuget.org/packages/Amarok.InlayTester.Shared.Transports/)

The library is compiled as *.NET Standard 2.0* library. Tests are performed with *.NET Framework 4.7.1* only. Currently, I have no plans to support .NET Core or older .NET Framework versions.

For development, you need *Visual Studio 2017* (v15.7.1 or later). For running the tests you need to install [com0com](https://sourceforge.net/projects/com0com/) and set up a serial port pair with names "COMA" and "COMB". These are used by unit tests.
