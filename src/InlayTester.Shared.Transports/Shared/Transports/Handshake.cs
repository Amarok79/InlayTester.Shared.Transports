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

namespace InlayTester.Shared.Transports
{
    /// <summary>
    ///     Defines the handshake mode for serial communication.
    /// </summary>
    public enum Handshake
    {
        /// <summary>
        ///     No control is used for the handshake.
        /// </summary>
        None,

        /// <summary>
        ///     Request-to-Send (RTS) hardware flow control is used. RTS signals that data is available for
        ///     transmission. If the input buffer becomes full, the RTS line will be set to false. The RTS line
        ///     will be set to true when more room becomes available in the input buffer.
        /// </summary>
        RequestToSend,

        /// <summary>
        ///     Both the Request-to-Send (RTS) hardware control and the XON/XOFF software controls are used.
        /// </summary>
        RequestToSendXOnXOff,

        /// <summary>
        ///     The XON/XOFF software control protocol is used. The XOFF control is sent to stop the
        ///     transmission of data. The XON control is sent to resume the transmission. These software
        ///     controls are used instead of Request to Send (RTS) and Clear to Send (CTS) hardware controls.
        /// </summary>
        XOnXOff,
    }
}
