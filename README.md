### Introduction

This library is available as NuGet package:
[Amarok.InlayTester.Shared.Transports](https://www.nuget.org/packages/Amarok.InlayTester.Shared.Transports/)

The library is compiled as *.NET Standard 2.0* library. Tests are performed with *.NET Framework 4.7.1* only. Currently, I have no plans to support .NET Core or older .NET Framework versions.

For development, you need *Visual Studio 2017* (v15.7.1 or later). For running the tests you need to install [com0com](https://sourceforge.net/projects/com0com/) and set up a serial port pair with names "COMA" and "COMB". These are used by unit tests.

### Types of Interest

#### Transports

An abstraction of serial communication that can be opened and closed and that can send and receive (binary) data. The abstraction is mainly there to make writing tests easier by enabling one to mock the `ITransport` interface.

    var settings = new SerialTransportSettings {
        PortName = "COM3",
        Baud = 19200,
        DataBits = 8,
        Parity = Parity.Even,
        StopBits = StopBits.One,
        Handshake = Handshake.None
    };
    
    using(var transport = Transport.Create(settings))
    {
        // register event handler for received data
        transport.Received += (sender, e) => {
            Console.WriteLine("Received: {0}", e.Data);
        };

        // a transport can be opened and closed multiple times, but diposed only once
        transport.Open();
        transport.Close();
        transport.Open();
        
        // send data
        transport.Send(BufferSpan.From(0x11, 0x22, 0x33));
    }
