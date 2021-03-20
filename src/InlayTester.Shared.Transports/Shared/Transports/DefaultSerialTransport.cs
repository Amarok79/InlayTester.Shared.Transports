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
using Amarok.Events;
using Amarok.Shared;
using Microsoft.Extensions.Logging;
using Lib = RJCP.IO.Ports;


namespace InlayTester.Shared.Transports
{
    internal sealed class DefaultSerialTransport : ITransport
    {
        // data
        private readonly Lib.SerialPortStream mStream = new();
        private readonly SerialTransportSettings mSettings;
        private readonly EventSource<BufferSpan> mReceivedEvent = new();
        private readonly ILogger mLogger;
        private readonly ITransportHooks? mHooks;


        public SerialTransportSettings Settings => mSettings;

        public ILogger Logger => mLogger;

        public ITransportHooks? Hooks => mHooks;


        public DefaultSerialTransport(SerialTransportSettings settings, ILogger logger, ITransportHooks? hooks)
        {
            mSettings = settings;
            mLogger   = logger;
            mHooks    = hooks;

            mStream.DataReceived  += _HandleDataReceived;
            mStream.ErrorReceived += _HandleErrorReceived;
        }


        /// <summary>
        /// An event that is raised for data that has been received.
        /// </summary>
        public Event<BufferSpan> Received => mReceivedEvent.Event;


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
        public void Open()
        {
            _ThrowIfDisposed();
            _ThrowIfAlreadyOpen();

            try
            {
                mStream.PortName  = mSettings.PortName;
                mStream.BaudRate  = mSettings.Baud;
                mStream.DataBits  = mSettings.DataBits;
                mStream.Parity    = Convert(mSettings.Parity);
                mStream.StopBits  = Convert(mSettings.StopBits);
                mStream.Handshake = Convert(mSettings.Handshake);

                mStream.DiscardNull   = false;
                mStream.ParityReplace = 0xff;

                mStream.Open();

                mLogger.LogOpen(mSettings.PortName, mSettings);
            }
            catch (Exception exception)
            {
                mLogger.LogFailedToOpen(mSettings.PortName, mSettings, exception);

                throw;
            }
        }

        internal static Lib.Parity Convert(Parity value)
        {
            return value switch {
                Parity.Even  => Lib.Parity.Even,
                Parity.Mark  => Lib.Parity.Mark,
                Parity.None  => Lib.Parity.None,
                Parity.Odd   => Lib.Parity.Odd,
                Parity.Space => Lib.Parity.Space,
                _            => throw new NotSupportedException($"The given Parity '{value}' is not supported."),
            };
        }

        internal static Lib.StopBits Convert(StopBits value)
        {
            return value switch {
                StopBits.One => Lib.StopBits.One,
                StopBits.OnePointFive => Lib.StopBits.One5,
                StopBits.Two => Lib.StopBits.Two,
                _ => throw new NotSupportedException($"The given StopBits '{value}' is not supported."),
            };
        }

        internal static Lib.Handshake Convert(Handshake value)
        {
            return value switch {
                Handshake.None => Lib.Handshake.None,
                Handshake.RequestToSend => Lib.Handshake.Rts,
                Handshake.RequestToSendXOnXOff => Lib.Handshake.RtsXOn,
                Handshake.XOnXOff => Lib.Handshake.XOn,
                _ => throw new NotSupportedException($"The given Handshake '{value}' is not supported."),
            };
        }


        /// <summary>
        /// Closes the transport. A transport can be opened and closed multiple times.
        /// </summary>
        /// 
        /// <exception cref="ObjectDisposedException">
        /// A method or property was called on an already disposed object.</exception>
        public void Close()
        {
            _ThrowIfDisposed();

            try
            {
                mStream.Close();

                mLogger.LogClose(mSettings.PortName);
            }
            catch (Exception exception)
            {
                mLogger.LogFailedToClose(mSettings.PortName, exception);

                throw;
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            mStream.Dispose();
            mReceivedEvent.Dispose();

            mLogger.LogDispose(mSettings.PortName);
        }


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
        public void Send(BufferSpan data)
        {
            _ThrowIfDisposed();
            _ThrowIfNotOpen();

            try
            {
                // invoke hook
                mHooks?.BeforeSend(ref data);

                // send data
                mStream.Write(data.Buffer, data.Offset, data.Count);

                mLogger.LogSend(mSettings.PortName, data);
            }
            catch (Exception exception)
            {
                mLogger.LogFailedToSend(mSettings.PortName, data, exception);

                throw;
            }
        }


        private void _HandleDataReceived(Object? sender, Lib.SerialDataReceivedEventArgs e)
        {
            try
            {
                if (e.EventType == Lib.SerialData.NoData)
                    return;

                var data = _ReadDataReceived();
                mReceivedEvent.Invoke(data);
            }
            catch (Exception)
            {
                // swallow
            }
        }

        private BufferSpan _ReadDataReceived()
        {
            try
            {
                // receive data
                var bytesToRead = mStream.BytesToRead;
                var buffer      = new Byte[bytesToRead];
                var bytesRead   = mStream.Read(buffer, 0, bytesToRead);
                var data        = BufferSpan.From(buffer, bytesRead);

                // invoke hook
                mHooks?.AfterReceived(ref data);

                mLogger.LogReceive(mSettings.PortName, data);

                return data;
            }
            catch (Exception exception)
            {
                mLogger.LogFailedToReceive(mSettings.PortName, exception);

                throw;
            }
        }


        private void _HandleErrorReceived(Object? sender, Lib.SerialErrorReceivedEventArgs e)
        {
            if (e.EventType == Lib.SerialError.NoError)
                return;

            mLogger.LogSerialError(mSettings.PortName, e.EventType);
        }


        private void _ThrowIfDisposed()
        {
            if (mStream.IsDisposed)
                throw new ObjectDisposedException(nameof(DefaultSerialTransport));
        }

        private void _ThrowIfAlreadyOpen()
        {
            if (mStream.IsOpen)
                throw new InvalidOperationException("The transport has already been opened before.");
        }

        private void _ThrowIfNotOpen()
        {
            if (!mStream.IsOpen)
                throw new InvalidOperationException("The transport has not been opened yet.");
        }
    }
}
