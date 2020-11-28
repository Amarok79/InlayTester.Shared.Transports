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

namespace InlayTester.Shared.Transports
{
	/// <summary>
	/// Defines the supported parity modes for serial communication.
	/// </summary>
	public enum Parity
	{
		/// <summary>
		/// No parity check occurs.
		/// </summary>
		None,

		/// <summary>
		/// Sets the parity bit so that the count of bits set is an even number.
		/// </summary>
		Even,

		/// <summary>
		/// Sets the parity bit so that the count of bits set is an odd number.
		/// </summary>
		Odd,

		/// <summary>
		/// Leaves the parity bit set to 1.
		/// </summary>
		Mark,

		/// <summary>
		/// Leaves the parity bit set to 0.
		/// </summary>
		Space,
	}
}
