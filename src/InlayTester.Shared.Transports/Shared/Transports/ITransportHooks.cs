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

#pragma warning disable S3874 // "out" and "ref" parameters should not be used

using Amarok.Shared;


namespace InlayTester.Shared.Transports
{
	/// <summary>
	/// This interface provides hook methods for <see cref="ITransport"/>.
	/// </summary>
	public interface ITransportHooks
	{
		/// <summary>
		/// This hook method is invoked before the data is sent.
		/// 
		/// The implementer is able to monitor or even manipulate the data.
		/// </summary>
		/// 
		/// <param name="data">
		/// The data to be sent.</param>
		void BeforeSend(ref BufferSpan data);

		/// <summary>
		/// This hook method is invoked after data has been received.
		/// 
		/// The implementer is able to monitor or even manipulate the data.
		/// </summary>
		/// 
		/// <param name="data">
		/// The data being received.</param>
		void AfterReceived(ref BufferSpan data);
	}
}
