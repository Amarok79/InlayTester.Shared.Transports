// Copyright (c) 2018, Olaf Kober <olaf.kober@outlook.com>

using System;
using Common.Logging.Simple;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports
{
	[TestFixture]
	public class Test_Transport
	{
		[Test]
		public void Create()
		{
			var settings = new SerialTransportSettings {
				PortName = "COM12"
			};

			var transport = Transport.Create(settings);

			Check.That(transport)
				.IsInstanceOf<DefaultSerialTransport>();
		}

		[Test]
		public void Exception_For_NullSettings()
		{
			Check.ThatCode(() => Transport.Create(null))
				.Throws<ArgumentNullException>();
		}

		[Test]
		public void Create_With_Logger()
		{
			var settings = new SerialTransportSettings {
				PortName = "COM12"
			};

			var logger = new NoOpLogger();
			var transport = Transport.Create(settings, logger);

			Check.That(transport)
				.IsInstanceOf<DefaultSerialTransport>();
		}

		[Test]
		public void Exception_For_NullLogger()
		{
			var settings = new SerialTransportSettings {
				PortName = "COM12"
			};

			Check.ThatCode(() => Transport.Create(settings, null))
				.Throws<ArgumentNullException>();
		}
	}
}
