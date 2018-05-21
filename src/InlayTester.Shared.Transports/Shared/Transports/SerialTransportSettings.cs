/* MIT License
 * 
 * Copyright (c) 2018, Olaf Kober
 * https://github.com/Amarok79/InlayTester.Shared
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

#pragma warning disable CS3003 // Type is not CLS-compliant

using System;
using RJCP.IO.Ports;


namespace InlayTester.Shared.Transports
{
	/// <summary>
	/// This type wraps settings for serial transports.
	/// </summary>
	public sealed class SerialTransportSettings
	{
		/// <summary>
		/// Gets or sets the name of the serial port, i.e. "COM1".
		/// </summary>
		public String PortName { get; set; }

		/// <summary>
		/// Gets or sets the baud rate of the serial port, i.e. 9600 baud.
		/// Defaults to 9600.
		/// </summary>
		public Int32 Baud { get; set; }

		/// <summary>
		/// Gets or sets the data bits to use for the serial port.
		/// Defaults to 8.
		/// </summary>
		public Int32 DataBits { get; set; }

		/// <summary>
		/// Gets or sets the parity to use for the serial port.
		/// Defaults to None.
		/// </summary>
		public Parity Parity { get; set; }

		/// <summary>
		/// Gets or sets the stop bits to use for the serial port.
		/// Defaults to 1.
		/// </summary>
		public StopBits StopBits { get; set; }

		/// <summary>
		/// Gets or sets the handshake protocol to use for the serial port.
		/// Defaults to None.
		/// </summary>
		public Handshake Handshake { get; set; }


		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public SerialTransportSettings()
		{
			this.Baud = 9600;
			this.DataBits = 8;
			this.Parity = Parity.None;
			this.StopBits = StopBits.One;
			this.Handshake = Handshake.None;
		}

		/// <summary>
		/// Initializes a new instance.
		/// </summary>
		public SerialTransportSettings(SerialTransportSettings settings)
		{
			this.PortName = settings.PortName;
			this.Baud = settings.Baud;
			this.DataBits = settings.DataBits;
			this.Parity = settings.Parity;
			this.StopBits = settings.StopBits;
			this.Handshake = settings.Handshake;
		}
	}
}
