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
using System.IO;
using System.Threading;
using Common.Logging.Simple;
using NCrunch.Framework;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports
{
	[TestFixture]
	public class Test_DefaultSerialTransport
	{
		[Test, Serial]
		public void Open_Close_Dispose()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				transportA.Open();
				transportA.Close();
			}
		}

		[Test, Serial]
		public void Open_Close_Open_Dispose()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				transportA.Open();
				transportA.Close();
				transportA.Open();
			}
		}

		[Test, Serial]
		public void OpenThrowsException_When_AlreadyOpen()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				transportA.Open();

				Check.ThatCode(() => transportA.Open())
					.Throws<InvalidOperationException>();
			}
		}

		[Test, Serial]
		public void OpenThrowsException_When_AlreadyDisposed()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				transportA.Dispose();

				Check.ThatCode(() => transportA.Open())
					.Throws<ObjectDisposedException>();
			}
		}

		[Test, Serial]
		public void OpenThrowsException_When_SettingsAreInvalid()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "ABC",
			};

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				Check.ThatCode(() => transportA.Open())
					.Throws<IOException>();
			}
		}

		[Test, Serial]
		public void CloseThrowsException_When_AlreadyDisposed()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				transportA.Dispose();

				Check.ThatCode(() => transportA.Close())
					.Throws<ObjectDisposedException>();
			}
		}

		[Test, Serial]
		public void Close_When_NotOpened()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				Check.ThatCode(() => transportA.Close())
					.DoesNotThrow();

				transportA.Open();
			}
		}

		[Test, Serial]
		public void Dispose_When_NotOpened()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				Check.ThatCode(() => transportA.Dispose())
					.DoesNotThrow();
			}
		}

		[Test, Serial]
		public void SendThrowsException_When_NotOpened()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			var data = BufferSpan.From(0x12);

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				Check.ThatCode(() => transportA.Send(data))
					.Throws<InvalidOperationException>();
			}
		}

		[Test, Serial]
		public void SendThrowsException_When_AlreadyDisposed()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};

			var data = BufferSpan.From(0x12);

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				transportA.Dispose();

				Check.ThatCode(() => transportA.Send(data))
					.Throws<ObjectDisposedException>();
			}
		}

		[Test, Serial]
		public void SendReceive_SingleByte()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};
			var settingsB = new SerialTransportSettings {
				PortName = "COMB",
			};

			var data = BufferSpan.From(0x12);
			var log = new ConsoleOutLogger("Test", Common.Logging.LogLevel.Info, false, false, false, "U");

			using (var transportA = new DefaultSerialTransport(settingsA, log))
			{
				using (var transportB = new DefaultSerialTransport(settingsB, log))
				{
					var received = BufferSpan.Empty;
					transportB.Received += (sender, e) => received = received.Append(e.Data);

					transportB.Open();
					transportA.Open();

					transportA.Send(data);

					SpinWait.SpinUntil(() => received.Count == 1, 5000);

					Check.That(received.ToArray())
						.ContainsExactly(0x12);
				}
			}
		}

		[Test, Serial]
		public void SendReceive_MultipleBytes()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};
			var settingsB = new SerialTransportSettings {
				PortName = "COMB",
			};

			var data = BufferSpan.From(0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88);

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				using (var transportB = new DefaultSerialTransport(settingsB, new NoOpLogger()))
				{
					var received = BufferSpan.Empty;
					transportB.Received += (sender, e) => received = received.Append(e.Data);

					transportB.Open();
					transportA.Open();

					transportA.Send(data);

					SpinWait.SpinUntil(() => received.Count == 8, 5000);

					Check.That(received.ToArray())
						.ContainsExactly(0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88);
				}
			}
		}

		[Test, Serial]
		public void SendReceive_ManyBytes()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};
			var settingsB = new SerialTransportSettings {
				PortName = "COMB",
			};

			var random = new Random();
			var buffer = new Byte[1024];
			random.NextBytes(buffer);
			var data = BufferSpan.From(buffer);

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				using (var transportB = new DefaultSerialTransport(settingsB, new NoOpLogger()))
				{
					var received = BufferSpan.Empty;
					transportB.Received += (sender, e) => received = received.Append(e.Data);

					transportB.Open();
					transportA.Open();

					transportA.Send(data);

					SpinWait.SpinUntil(() => received.Count == buffer.Length, 5000);

					Check.That(received.ToArray())
						.ContainsExactly(buffer);
				}
			}
		}

		[Test, Serial]
		public void SendReceive_MultipleTransfers()
		{
			var settingsA = new SerialTransportSettings {
				PortName = "COMA",
			};
			var settingsB = new SerialTransportSettings {
				PortName = "COMB",
			};

			var data = BufferSpan.From(0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88);

			using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger()))
			{
				using (var transportB = new DefaultSerialTransport(settingsB, new NoOpLogger()))
				{
					var received = BufferSpan.Empty;
					transportB.Received += (sender, e) => received = received.Append(e.Data);

					transportB.Open();
					transportA.Open();

					transportA.Send(data);
					transportA.Send(data);
					transportA.Send(data);

					SpinWait.SpinUntil(() => received.Count == 3 * 8, 5000);

					Check.That(received.ToArray())
						.ContainsExactly(
							0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88,
							0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88,
							0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88
						);
				}
			}
		}
	}
}
