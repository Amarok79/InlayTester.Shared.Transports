// Copyright (c) 2018, Olaf Kober <olaf.kober@outlook.com>

using System.IO.Ports;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports
{
	[TestFixture]
	public class Test_SerialTransportSettings
	{
		[Test]
		public void Construction_Default()
		{
			var settings = new SerialTransportSettings();

			Check.That(settings.PortName)
				.IsNull();
			Check.That(settings.Baud)
				.IsEqualTo(9600);
			Check.That(settings.DataBits)
				.IsEqualTo(8);
			Check.That(settings.Parity)
				.IsEqualTo(Parity.None);
			Check.That(settings.StopBits)
				.IsEqualTo(StopBits.One);
			Check.That(settings.Handshake)
				.IsEqualTo(Handshake.None);
		}

		[Test]
		public void Construction()
		{
			var settings = new SerialTransportSettings {
				PortName = "COM12",
				Baud = 38400,
				DataBits = 7,
				Parity = Parity.Even,
				StopBits = StopBits.Two,
				Handshake = Handshake.XOnXOff,
			};

			Check.That(settings.PortName)
				.IsEqualTo("COM12");
			Check.That(settings.Baud)
				.IsEqualTo(38400);
			Check.That(settings.DataBits)
				.IsEqualTo(7);
			Check.That(settings.Parity)
				.IsEqualTo(Parity.Even);
			Check.That(settings.StopBits)
				.IsEqualTo(StopBits.Two);
			Check.That(settings.Handshake)
				.IsEqualTo(Handshake.XOnXOff);
		}

		[Test]
		public void Construction_CopyConstructor()
		{
			var org = new SerialTransportSettings {
				PortName = "COM12",
				Baud = 38400,
				DataBits = 7,
				Parity = Parity.Even,
				StopBits = StopBits.Two,
				Handshake = Handshake.XOnXOff,
			};

			var settings = new SerialTransportSettings(org);

			Check.That(settings.PortName)
				.IsEqualTo("COM12");
			Check.That(settings.Baud)
				.IsEqualTo(38400);
			Check.That(settings.DataBits)
				.IsEqualTo(7);
			Check.That(settings.Parity)
				.IsEqualTo(Parity.Even);
			Check.That(settings.StopBits)
				.IsEqualTo(StopBits.Two);
			Check.That(settings.Handshake)
				.IsEqualTo(Handshake.XOnXOff);
		}
	}
}
