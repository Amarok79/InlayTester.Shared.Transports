![CI](https://github.com/Amarok79/InlayTester.Shared.Transports/workflows/CI/badge.svg)
[![NuGet](https://img.shields.io/nuget/v/InlayTester.Shared.Transports.svg?logo=)](https://www.nuget.org/packages/InlayTester.Shared.Transports/)

# Introduction

This library is available as NuGet package:
[InlayTester.Shared.Transports](https://www.nuget.org/packages/InlayTester.Shared.Transports/)

The package provides strong-named binaries for .NET Standard 2.0, .NET 6.0, and .NET 8.0. Tests are performed
with .NET Framework 4.8, .NET 6.0, and .NET 8.0.

For development, you need *Visual Studio 2022*. For running the tests, you need to
install [com0com](https://sourceforge.net/projects/com0com/) and set up a serial port pair with names "COMA" and "COMB".
This virtual serial port pair is used throughout unit tests.

# Types of Interest

### Transports

An abstraction of serial communication that can be opened and closed and that can send and receive (binary) data. The
abstraction is mainly there to make writing tests easier by enabling one to mock the `ITransport` interface.

````cs
    var settings = new SerialTransportSettings {
        PortName = "COM3",
        Baud = 19200,
        DataBits = 8,
        Parity = Parity.Even,
        StopBits = StopBits.One,
        Handshake = Handshake.None
    };

    using(ITransport transport = Transport.Create(settings))
    {
        // register event handler for received data
        transport.Received.Subscribe(x => {
            Console.WriteLine("Received: {0}", x);
        });

        // a transport can be opened and closed multiple times, but diposed only once
        transport.Open();
        transport.Close();
        transport.Open();

        // send data
        transport.Send(BufferSpan.From(0x11, 0x22, 0x33));
    }
````

Internally, transports use the excellent [SerialPortStream](https://github.com/jcurl/SerialPortStream) library for
serial communication.

If you want to get logging information for transport operations, you can specify an **ILogger** on **Transport.Create()**.

````cs
    var logger = // obtain from Microsoft.Extensions.Logging.Abstractions

    using(ITransport transport = Transport.Create(settings, logger))
    {
    }
````

If you want to monitor or even manipulate data being sent and received, you can implement **ITransportHooks** and
provide your implementation on `Transport.Create(..)`.

````cs
public sealed class FooTransportHooks : ITransportHooks
{
    public void BeforeSend(ref BufferSpan data)
    {
        Console.WriteLine("SENT: " + data.ToString());
    }

    public void AfterReceived(ref BufferSpan data)
    {
        Console.WriteLine("RECEIVED: " + data.ToString());
    }
}

var transport = Transport.Create(settings, new FooTransportHooks());
````
