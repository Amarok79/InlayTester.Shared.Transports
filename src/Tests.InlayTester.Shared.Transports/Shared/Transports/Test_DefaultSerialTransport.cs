/* MIT License
 * 
 * Copyright (c) 2020, Olaf Kober
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
using Amarok.Events;
using Amarok.Shared;
using Common.Logging;
using Common.Logging.Simple;
using NCrunch.Framework;
using NFluent;
using NUnit.Framework;
using Lib = RJCP.IO.Ports;


namespace InlayTester.Shared.Transports
{
    public class Test_DefaultSerialTransport
    {
        [TestFixture]
        public class Convert_Parity
        {
            [Test]
            public void Test()
            {
                Check.That(DefaultSerialTransport.Convert(Parity.None)).IsEqualTo(Lib.Parity.None);
                Check.That(DefaultSerialTransport.Convert(Parity.Even)).IsEqualTo(Lib.Parity.Even);
                Check.That(DefaultSerialTransport.Convert(Parity.Odd)).IsEqualTo(Lib.Parity.Odd);
                Check.That(DefaultSerialTransport.Convert(Parity.Mark)).IsEqualTo(Lib.Parity.Mark);
                Check.That(DefaultSerialTransport.Convert(Parity.Space)).IsEqualTo(Lib.Parity.Space);
                Check.ThatCode(() => DefaultSerialTransport.Convert((Parity) 123)).Throws<NotSupportedException>();
            }
        }

        [TestFixture]
        public class Convert_StopBits
        {
            [Test]
            public void Test()
            {
                Check.That(DefaultSerialTransport.Convert(StopBits.One)).IsEqualTo(Lib.StopBits.One);
                Check.That(DefaultSerialTransport.Convert(StopBits.OnePointFive)).IsEqualTo(Lib.StopBits.One5);
                Check.That(DefaultSerialTransport.Convert(StopBits.Two)).IsEqualTo(Lib.StopBits.Two);
                Check.ThatCode(() => DefaultSerialTransport.Convert((StopBits) 123)).Throws<NotSupportedException>();
            }
        }

        [TestFixture]
        public class Convert_Handshake
        {
            [Test]
            public void Test()
            {
                Check.That(DefaultSerialTransport.Convert(Handshake.None)).IsEqualTo(Lib.Handshake.None);
                Check.That(DefaultSerialTransport.Convert(Handshake.RequestToSend)).IsEqualTo(Lib.Handshake.Rts);

                Check.That(DefaultSerialTransport.Convert(Handshake.RequestToSendXOnXOff))
                   .IsEqualTo(Lib.Handshake.RtsXOn);

                Check.That(DefaultSerialTransport.Convert(Handshake.XOnXOff)).IsEqualTo(Lib.Handshake.XOn);
                Check.ThatCode(() => DefaultSerialTransport.Convert((Handshake) 123)).Throws<NotSupportedException>();
            }
        }

        [TestFixture]
        public class OpenClose
        {
            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void Open_Close_Dispose()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    transportA.Open();
                    transportA.Close();
                }

                Assert.Pass();
            }

            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void Open_Close_Open_Dispose()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    transportA.Open();
                    transportA.Close();
                    transportA.Open();
                }

                Assert.Pass();
            }

            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void OpenThrowsException_When_AlreadyOpen()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    transportA.Open();

                    Check.ThatCode(() => transportA.Open()).Throws<InvalidOperationException>();
                }
            }

            [Test]
            public void OpenThrowsException_When_AlreadyDisposed()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    transportA.Dispose();

                    Check.ThatCode(() => transportA.Open()).Throws<ObjectDisposedException>();
                }
            }

            [Test]
            public void OpenThrowsException_When_SettingsAreInvalid()
            {
                var settingsA = new SerialTransportSettings { PortName = "ABC" };

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    Check.ThatCode(() => transportA.Open()).Throws<IOException>();
                }
            }

            [Test]
            public void CloseThrowsException_When_AlreadyDisposed()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    transportA.Dispose();

                    Check.ThatCode(() => transportA.Close()).Throws<ObjectDisposedException>();
                }
            }

            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void Close_When_NotOpened()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    Check.ThatCode(() => transportA.Close()).DoesNotThrow();

                    transportA.Open();
                }
            }

            [Test]
            public void Dispose_When_NotOpened()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    Check.ThatCode(() => transportA.Dispose()).DoesNotThrow();
                }
            }
        }

        [TestFixture]
        public class SendReceive
        {
            [Test]
            public void SendThrowsException_When_NotOpened()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                var data = BufferSpan.From(0x12);

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    Check.ThatCode(() => transportA.Send(data)).Throws<InvalidOperationException>();
                }
            }

            [Test]
            public void SendThrowsException_When_AlreadyDisposed()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };

                var data = BufferSpan.From(0x12);

                using (var transportA = new DefaultSerialTransport(settingsA, new NoOpLogger(), null))
                {
                    transportA.Dispose();

                    Check.ThatCode(() => transportA.Send(data)).Throws<ObjectDisposedException>();
                }
            }

            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void SendReceive_SingleByte()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };
                var settingsB = new SerialTransportSettings { PortName = "COMB" };

                var data = BufferSpan.From(0x12);

                var log = new DebugOutLogger(
                    "Test",
                    LogLevel.All,
                    false,
                    false,
                    false,
                    "U"
                );

                using (var transportA = new DefaultSerialTransport(settingsA, log, null))
                {
                    using (var transportB = new DefaultSerialTransport(settingsB, log, null))
                    {
                        EventRecorder<BufferSpan> recorder = EventRecorder.From(transportB.Received);

                        transportB.Open();
                        transportA.Open();

                        transportA.Send(data);

                        SpinWait.SpinUntil(() => recorder.Count == 1, 5000);

                        Check.That(recorder.Events[0].ToArray()).ContainsExactly(0x12);
                    }
                }
            }

            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void SendReceive_MultipleBytes()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };
                var settingsB = new SerialTransportSettings { PortName = "COMB" };

                var data = BufferSpan.From(
                    0x11,
                    0x22,
                    0x33,
                    0x44,
                    0x55,
                    0x66,
                    0x77,
                    0x88
                );

                var log = new DebugOutLogger(
                    "Test",
                    LogLevel.All,
                    false,
                    false,
                    false,
                    "U"
                );

                using (var transportA = new DefaultSerialTransport(settingsA, log, null))
                {
                    using (var transportB = new DefaultSerialTransport(settingsB, log, null))
                    {
                        EventRecorder<BufferSpan> recorder = EventRecorder.From(transportB.Received);

                        transportB.Open();
                        transportA.Open();

                        transportA.Send(data);

                        SpinWait.SpinUntil(() => recorder.Count == 8, 5000);

                        Check.That(recorder.Events[0].ToArray())
                       .ContainsExactly(
                            0x11,
                            0x22,
                            0x33,
                            0x44,
                            0x55,
                            0x66,
                            0x77,
                            0x88
                        );

                        transportA.Close();
                        transportB.Close();
                    }
                }
            }

            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void SendReceive_ManyBytes()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };
                var settingsB = new SerialTransportSettings { PortName = "COMB" };

                var random = new Random();
                var buffer = new Byte[1024];
                random.NextBytes(buffer);
                var data = BufferSpan.From(buffer);

                var log = new DebugOutLogger(
                    "Test",
                    LogLevel.All,
                    false,
                    false,
                    false,
                    "U"
                );

                using (var transportA = new DefaultSerialTransport(settingsA, log, null))
                {
                    using (var transportB = new DefaultSerialTransport(settingsB, log, null))
                    {
                        var received = BufferSpan.Empty;
                        transportB.Received.Subscribe(x => received = received.Append(x));

                        transportB.Open();
                        transportA.Open();

                        transportA.Send(data);

                        SpinWait.SpinUntil(() => received.Count == buffer.Length, 5000);

                        Check.That(received.ToArray()).ContainsExactly(buffer);
                    }
                }
            }

            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void SendReceive_MultipleTransfers()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };
                var settingsB = new SerialTransportSettings { PortName = "COMB" };

                var data = BufferSpan.From(
                    0x11,
                    0x22,
                    0x33,
                    0x44,
                    0x55,
                    0x66,
                    0x77,
                    0x88
                );

                var log = new DebugOutLogger(
                    "Test",
                    LogLevel.All,
                    false,
                    false,
                    false,
                    "U"
                );

                using (var transportA = new DefaultSerialTransport(settingsA, log, null))
                {
                    using (var transportB = new DefaultSerialTransport(settingsB, log, null))
                    {
                        var received = BufferSpan.Empty;
                        transportB.Received.Subscribe(x => received = received.Append(x));

                        transportB.Open();
                        transportA.Open();

                        transportA.Send(data);
                        transportA.Send(data);
                        transportA.Send(data);

                        SpinWait.SpinUntil(() => received.Count == 3 * 8, 5000);

                        Check.That(received.ToArray())
                       .ContainsExactly(
                            0x11,
                            0x22,
                            0x33,
                            0x44,
                            0x55,
                            0x66,
                            0x77,
                            0x88,
                            0x11,
                            0x22,
                            0x33,
                            0x44,
                            0x55,
                            0x66,
                            0x77,
                            0x88,
                            0x11,
                            0x22,
                            0x33,
                            0x44,
                            0x55,
                            0x66,
                            0x77,
                            0x88
                        );
                    }
                }
            }


            private class FakeSendHook : ITransportHooks
            {
                public BufferSpan SentData;
                public BufferSpan DataToSend;

                public void AfterReceived(ref BufferSpan data)
                {
                    // Method intentionally left empty.
                }

                public void BeforeSend(ref BufferSpan data)
                {
                    SentData = data;
                    data     = DataToSend;
                }
            }

            private class FakeReceiveHook : ITransportHooks
            {
                public BufferSpan DataReceived;
                public BufferSpan DataToReceive;

                public void AfterReceived(ref BufferSpan data)
                {
                    DataReceived = data;
                    data         = DataToReceive;
                }

                public void BeforeSend(ref BufferSpan data)
                {
                    // Method intentionally left empty.
                }
            }


            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void SendReceive_BeforeSendHook()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };
                var settingsB = new SerialTransportSettings { PortName = "COMB" };

                var hook = new FakeSendHook { DataToSend = BufferSpan.From(0x55, 0x55, 0x55) };

                var data = BufferSpan.From(
                    0x11,
                    0x22,
                    0x33,
                    0x44,
                    0x55,
                    0x66,
                    0x77,
                    0x88
                );

                var log = new DebugOutLogger(
                    "Test",
                    LogLevel.All,
                    false,
                    false,
                    false,
                    "U"
                );

                using (var transportA = new DefaultSerialTransport(settingsA, log, hook))
                {
                    using (var transportB = new DefaultSerialTransport(settingsB, log, null))
                    {
                        var received = BufferSpan.Empty;
                        transportB.Received.Subscribe(x => received = received.Append(x));

                        transportB.Open();
                        transportA.Open();

                        transportA.Send(data);

                        SpinWait.SpinUntil(() => received.Count == 8, 5000);

                        Check.That(received.ToArray()).ContainsExactly(0x55, 0x55, 0x55);

                        transportA.Close();
                        transportB.Close();

                        Check.That(hook.SentData.ToArray())
                       .ContainsExactly(
                            0x11,
                            0x22,
                            0x33,
                            0x44,
                            0x55,
                            0x66,
                            0x77,
                            0x88
                        );
                    }
                }
            }

            [Test, NUnit.Framework.Category("com0com"), Serial]
            public void SendReceive_AfterReceivedHook()
            {
                var settingsA = new SerialTransportSettings { PortName = "COMA" };
                var settingsB = new SerialTransportSettings { PortName = "COMB" };

                var hook = new FakeReceiveHook { DataToReceive = BufferSpan.From(0x55, 0x55, 0x55) };

                var data = BufferSpan.From(
                    0x11,
                    0x22,
                    0x33,
                    0x44,
                    0x55,
                    0x66,
                    0x77,
                    0x88
                );

                var log = new DebugOutLogger(
                    "Test",
                    LogLevel.All,
                    false,
                    false,
                    false,
                    "U"
                );

                using (var transportA = new DefaultSerialTransport(settingsA, log, null))
                {
                    using (var transportB = new DefaultSerialTransport(settingsB, log, hook))
                    {
                        var received = BufferSpan.Empty;
                        transportB.Received.Subscribe(x => received = received.Append(x));

                        transportB.Open();
                        transportA.Open();

                        transportA.Send(data);

                        SpinWait.SpinUntil(() => received.Count == 8, 5000);

                        Check.That(received.ToArray()).ContainsExactly(0x55, 0x55, 0x55);

                        transportA.Close();
                        transportB.Close();

                        Check.That(hook.DataReceived.ToArray())
                       .ContainsExactly(
                            0x11,
                            0x22,
                            0x33,
                            0x44,
                            0x55,
                            0x66,
                            0x77,
                            0x88
                        );
                    }
                }
            }
        }
    }
}
