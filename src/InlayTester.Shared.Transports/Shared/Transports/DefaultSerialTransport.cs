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

#pragma warning disable S2221 // "Exception" should not be caught when not required by called methods

using System;
using System.Globalization;
using System.IO;
using Amarok.Events;
using Common.Logging;
using Lib = RJCP.IO.Ports;


namespace InlayTester.Shared.Transports
{
	internal sealed class DefaultSerialTransport :
		ITransport
	{
		// data
		private readonly Lib.SerialPortStream mStream = new Lib.SerialPortStream();
		private readonly SerialTransportSettings mSettings;
		private readonly ILog mLog;
		private readonly ITransportHooks mHooks;
		private readonly EventSource<BufferSpan> mReceivedEvent = new EventSource<BufferSpan>();


		public SerialTransportSettings Settings => mSettings;

		public ILog Logger => mLog;

		public ITransportHooks Hooks => mHooks;


		public DefaultSerialTransport(SerialTransportSettings settings, ILog logger, ITransportHooks hooks)
		{
			mSettings = settings;
			mLog = logger;
			mHooks = hooks;

			mStream.DataReceived += _HandleDataReceived;
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
				mStream.PortName = mSettings.PortName;
				mStream.BaudRate = mSettings.Baud;
				mStream.DataBits = mSettings.DataBits;
				mStream.Parity = Convert(mSettings.Parity);
				mStream.StopBits = Convert(mSettings.StopBits);
				mStream.Handshake = Convert(mSettings.Handshake);

				mStream.DiscardNull = false;
				mStream.ParityReplace = 0xff;

				mStream.Open();

				#region (logging)
				{
					mLog.InfoFormat(CultureInfo.InvariantCulture,
						"[{0}]  OPENED    {1}",
						mSettings.PortName,
						mSettings
					);
				}
				#endregion
			}
			catch (Exception exception)
			{
				#region (logging)
				{
					mLog.ErrorFormat(CultureInfo.InvariantCulture,
						"[{0}]  FAILED to open. Settings: '{1}'.",
						exception,
						mSettings.PortName,
						mSettings
					);
				}
				#endregion

				throw;
			}
		}

		internal static Lib.Parity Convert(Parity value)
		{
			switch (value)
			{
				case Parity.Even:
					return Lib.Parity.Even;
				case Parity.Mark:
					return Lib.Parity.Mark;
				case Parity.None:
					return Lib.Parity.None;
				case Parity.Odd:
					return Lib.Parity.Odd;
				case Parity.Space:
					return Lib.Parity.Space;
				default:
					throw ExceptionFactory.NotSupportedException("The given Parity '{0}' is not supported.", value);
			}
		}

		internal static Lib.StopBits Convert(StopBits value)
		{
			switch (value)
			{
				case StopBits.One:
					return Lib.StopBits.One;
				case StopBits.OnePointFive:
					return Lib.StopBits.One5;
				case StopBits.Two:
					return Lib.StopBits.Two;
				default:
					throw ExceptionFactory.NotSupportedException("The given StopBits '{0}' is not supported.", value);
			}
		}

		internal static Lib.Handshake Convert(Handshake value)
		{
			switch (value)
			{
				case Handshake.None:
					return Lib.Handshake.None;
				case Handshake.RequestToSend:
					return Lib.Handshake.Rts;
				case Handshake.RequestToSendXOnXOff:
					return Lib.Handshake.RtsXOn;
				case Handshake.XOnXOff:
					return Lib.Handshake.XOn;
				default:
					throw ExceptionFactory.NotSupportedException("The given Handshake '{0}' is not supported.", value);
			}
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

				#region (logging)
				{
					mLog.InfoFormat(CultureInfo.InvariantCulture,
						"[{0}]  CLOSED",
						mSettings.PortName
					);
				}
				#endregion
			}
			catch (Exception exception)
			{
				#region (logging)
				{
					mLog.ErrorFormat(CultureInfo.InvariantCulture,
						"[{0}]  FAILED to close.",
						exception,
						mSettings.PortName
					);
				}
				#endregion

				throw;
			}
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public void Dispose()
		{
			mStream.Dispose();

			#region (logging)
			{
				mLog.InfoFormat(CultureInfo.InvariantCulture,
					"[{0}]  DISPOSED",
					mSettings.PortName
				);
			}
			#endregion
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

				#region (logging)
				{
					if (mLog.IsTraceEnabled)
					{
						mLog.TraceFormat(CultureInfo.InvariantCulture,
							"[{0}]  SENT      {1}",
							mSettings.PortName,
							data
						);
					}
				}
				#endregion
			}
			catch (Exception exception)
			{
				#region (logging)
				{
					mLog.ErrorFormat(CultureInfo.InvariantCulture,
						"[{0}]  FAILED to send. Data: '{1}'.",
						exception,
						mSettings.PortName,
						data
					);
				}
				#endregion

				throw;
			}
		}


		private void _HandleDataReceived(Object sender, Lib.SerialDataReceivedEventArgs e)
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
				var buffer = new Byte[bytesToRead];
				var bytesRead = mStream.Read(buffer, 0, bytesToRead);
				var data = BufferSpan.From(buffer, bytesRead);

				// invoke hook
				mHooks?.AfterReceived(ref data);

				#region (logging)
				{
					if (mLog.IsTraceEnabled)
					{
						mLog.TraceFormat(CultureInfo.InvariantCulture,
							"[{0}]  RECEIVED  {1}",
							mSettings.PortName,
							data
						);
					}
				}
				#endregion

				return data;
			}
			catch (Exception exception)
			{
				#region (logging)
				{
					mLog.ErrorFormat(CultureInfo.InvariantCulture,
						"[{0}]  FAILED to receive.",
						exception,
						mSettings.PortName
					);
				}
				#endregion

				throw;
			}
		}


		private void _HandleErrorReceived(Object sender, Lib.SerialErrorReceivedEventArgs e)
		{
			if (e.EventType == Lib.SerialError.NoError)
				return;

			#region (logging)
			{
				mLog.WarnFormat(CultureInfo.InvariantCulture,
					"[{0}]  SERIAL ERROR  '{1}'",
					mSettings.PortName,
					e.EventType
				);
			}
			#endregion
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
