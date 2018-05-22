/* MIT License
 * 
 * Copyright (c) 2018, Olaf Kober
 * https://github.com/Amarok79/InlayTester.Shared.Transports
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
*/

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
