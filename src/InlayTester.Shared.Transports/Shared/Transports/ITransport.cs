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

using System;
using System.IO;


namespace InlayTester.Shared.Transports
{
	/// <summary>
	/// This interface represents a transport for sending and receiving data.
	/// </summary>
	public interface ITransport :
		IDisposable
	{
		/// <summary>
		/// An event that is raised for data that has been received.
		/// </summary>
		event EventHandler<TransportDataReceivedEventArgs> Received;


		/// <summary>
		/// Opens the transport. A transport can be opened and closed multiple times.
		/// </summary>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has already been opened before.</exception>
		/// <exception cref="IOException">
		/// The transport settings seem to be invalid.</exception>
		void Open();

		/// <summary>
		/// Closes the transport. A transport can be opened and closed multiple times.
		/// </summary>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		void Close();


		/// <summary>
		/// Sends the given data over the transport.
		/// </summary>
		/// 
		/// <param name="data">
		/// The data to send.</param>
		/// 
		/// <exception cref="ObjectDisposedException">
		/// A method or property was called on an already disposed object.</exception>
		/// <exception cref="InvalidOperationException">
		/// The transport has not been opened yet.</exception>
		void Send(BufferSpan data);
	}
}
