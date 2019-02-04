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
using Common.Logging;
using Common.Logging.Simple;
using Moq;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports
{
	public class Test_Transport
	{
		[TestFixture]
		public class Create_Settings
		{
			[Test, Category("com0com")]
			public void Create()
			{
				// act
				var settings = new SerialTransportSettings();
				var transport = Transport.Create(settings);

				// assert
				Check.That(transport)
					.IsInstanceOf<DefaultSerialTransport>();

				var transportImpl = (DefaultSerialTransport)transport;

				Check.That(transportImpl.Settings)
					.Not.IsSameReferenceAs(settings);
				Check.That(transportImpl.Logger)
					.IsInstanceOf<NoOpLogger>();
				Check.That(transportImpl.Hooks)
					.IsNull();
			}

			[Test]
			public void Exception_For_NullSettings()
			{
				Check.ThatCode(() => Transport.Create(null))
					.Throws<ArgumentNullException>();
			}
		}

		[TestFixture]
		public class Create_Settings_Hooks
		{
			[Test, Category("com0com")]
			public void Create()
			{
				// act
				var hooks = new Mock<ITransportHooks>();
				var settings = new SerialTransportSettings();
				var transport = Transport.Create(settings, hooks.Object);

				// assert
				Check.That(transport)
					.IsInstanceOf<DefaultSerialTransport>();

				var transportImpl = (DefaultSerialTransport)transport;

				Check.That(transportImpl.Settings)
					.Not.IsSameReferenceAs(settings);
				Check.That(transportImpl.Logger)
					.IsInstanceOf<NoOpLogger>();
				Check.That(transportImpl.Hooks)
					.IsSameReferenceAs(hooks.Object);
			}

			[Test]
			public void Exception_For_NullSettings()
			{
				var hooks = new Mock<ITransportHooks>().Object;

				Check.ThatCode(() => Transport.Create(null, hooks))
					.Throws<ArgumentNullException>();
			}

			[Test]
			public void Exception_For_NullHooks()
			{
				var settings = new SerialTransportSettings();

				Check.ThatCode(() => Transport.Create(settings, (ITransportHooks)null))
					.Throws<ArgumentNullException>();
			}
		}

		[TestFixture]
		public class Create_Settings_Logger
		{
			[Test, Category("com0com")]
			public void Create_With_Logger()
			{
				// act
				var settings = new SerialTransportSettings();
				var logger = new ConsoleOutLogger("Foo", LogLevel.All, false, false, false, "G");
				var transport = Transport.Create(settings, logger);

				// assert
				Check.That(transport)
					.IsInstanceOf<DefaultSerialTransport>();

				var transportImpl = (DefaultSerialTransport)transport;

				Check.That(transportImpl.Settings)
					.Not.IsSameReferenceAs(settings);
				Check.That(transportImpl.Logger)
					.IsSameReferenceAs(logger);
				Check.That(transportImpl.Hooks)
					.IsNull();
			}

			[Test]
			public void Exception_For_NullSettings()
			{
				Check.ThatCode(() => Transport.Create(null, new NoOpLogger()))
					.Throws<ArgumentNullException>();
			}

			[Test]
			public void Exception_For_NullLogger()
			{
				var settings = new SerialTransportSettings();

				Check.ThatCode(() => Transport.Create(settings, (ILog)null))
					.Throws<ArgumentNullException>();
			}
		}

		[TestFixture]
		public class Create_Settings_Logger_Hooks
		{
			[Test, Category("com0com")]
			public void Create_With_Logger()
			{
				// act
				var hooks = new Mock<ITransportHooks>();
				var settings = new SerialTransportSettings();
				var logger = new ConsoleOutLogger("Foo", LogLevel.All, false, false, false, "G");
				var transport = Transport.Create(settings, logger, hooks.Object);

				// assert
				Check.That(transport)
					.IsInstanceOf<DefaultSerialTransport>();

				var transportImpl = (DefaultSerialTransport)transport;

				Check.That(transportImpl.Settings)
					.Not.IsSameReferenceAs(settings);
				Check.That(transportImpl.Logger)
					.IsSameReferenceAs(logger);
				Check.That(transportImpl.Hooks)
					.IsSameReferenceAs(hooks.Object);
			}

			[Test]
			public void Exception_For_NullSettings()
			{
				var hooks = new Mock<ITransportHooks>();

				Check.ThatCode(() => Transport.Create(null, new NoOpLogger(), hooks.Object))
					.Throws<ArgumentNullException>();
			}

			[Test]
			public void Exception_For_NullLogger()
			{
				var settings = new SerialTransportSettings();
				var hooks = new Mock<ITransportHooks>();

				Check.ThatCode(() => Transport.Create(settings, (ILog)null, hooks.Object))
					.Throws<ArgumentNullException>();
			}

			[Test]
			public void Exception_For_NullHooks()
			{
				var settings = new SerialTransportSettings();

				Check.ThatCode(() => Transport.Create(settings, new NoOpLogger(), null))
					.Throws<ArgumentNullException>();
			}
		}
	}
}
