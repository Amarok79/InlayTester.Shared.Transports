// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports;


[TestFixture]
public class Test_SerialTransportSettings
{
    [Test]
    public void Construction_Default()
    {
        // act
        var settings = new SerialTransportSettings();

        // assert
        Check.That(settings.PortName).IsEqualTo("COM1");

        Check.That(settings.Baud).IsEqualTo(9600);

        Check.That(settings.DataBits).IsEqualTo(8);

        Check.That(settings.Parity).IsEqualTo(Parity.None);

        Check.That(settings.StopBits).IsEqualTo(StopBits.One);

        Check.That(settings.Handshake).IsEqualTo(Handshake.None);

        Check.That(settings.ToString()).IsEqualTo("COM1,9600,8,None,One,None");
    }

    [Test]
    public void Construction()
    {
        // act
        var settings = new SerialTransportSettings {
            PortName = "COM12",
            Baud = 38400,
            DataBits = 7,
            Parity = Parity.Even,
            StopBits = StopBits.Two,
            Handshake = Handshake.XOnXOff,
        };

        // assert
        Check.That(settings.PortName).IsEqualTo("COM12");

        Check.That(settings.Baud).IsEqualTo(38400);

        Check.That(settings.DataBits).IsEqualTo(7);

        Check.That(settings.Parity).IsEqualTo(Parity.Even);

        Check.That(settings.StopBits).IsEqualTo(StopBits.Two);

        Check.That(settings.Handshake).IsEqualTo(Handshake.XOnXOff);

        Check.That(settings.ToString()).IsEqualTo("COM12,38400,7,Even,Two,XOnXOff");
    }

    [Test]
    public void Construction_Copy()
    {
        // act
        var org = new SerialTransportSettings {
            PortName = "COM12",
            Baud = 38400,
            DataBits = 7,
            Parity = Parity.Even,
            StopBits = StopBits.Two,
            Handshake = Handshake.XOnXOff,
        };

        var settings = new SerialTransportSettings(org);

        // assert
        Check.That(settings.PortName).IsEqualTo("COM12");

        Check.That(settings.Baud).IsEqualTo(38400);

        Check.That(settings.DataBits).IsEqualTo(7);

        Check.That(settings.Parity).IsEqualTo(Parity.Even);

        Check.That(settings.StopBits).IsEqualTo(StopBits.Two);

        Check.That(settings.Handshake).IsEqualTo(Handshake.XOnXOff);
    }
}
