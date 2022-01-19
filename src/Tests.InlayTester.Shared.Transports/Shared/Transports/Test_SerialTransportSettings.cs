// MIT License
// 
// Copyright (c) 2021, Olaf Kober
// https://github.com/Amarok79/InlayTester.Shared.Transports
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

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
                PortName  = "COM12",
                Baud      = 38400,
                DataBits  = 7,
                Parity    = Parity.Even,
                StopBits  = StopBits.Two,
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
                PortName  = "COM12",
                Baud      = 38400,
                DataBits  = 7,
                Parity    = Parity.Even,
                StopBits  = StopBits.Two,
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
}
