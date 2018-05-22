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
using Common.Logging;
using RJCP.IO.Ports;


namespace InlayTester.Shared.Transports
{
	internal sealed class DefaultSerialTransport :
		ITransport
	{
		// data
		private readonly SerialPortStream mStream = new SerialPortStream();
		private readonly SerialTransportSettings mSettings;
		private readonly ILog mLog;


		public DefaultSerialTransport(SerialTransportSettings settings, ILog log)
		{
			mSettings = settings;
			mLog = log;

			mStream.DataReceived += _HandleDataReceived;
			mStream.ErrorReceived += _HandleErrorReceived;
		}


		/// <summary>
		/// An event that is raised for data that has been received.
		/// </summary>
		public event EventHandler<TransportDataReceivedEventArgs> Received;


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
				mStream.Parity = _Convert(mSettings.Parity);
				mStream.StopBits = _Convert(mSettings.StopBits);
				mStream.Handshake = _Convert(mSettings.Handshake);

				mStream.DiscardNull = false;
				mStream.ParityReplace = 0xff;

				mStream.Open();

				#region (logging)
				{
					mLog.InfoFormat(CultureInfo.InvariantCulture,
						"Opened serial transport on '{0}' with {1},{2},{3},{4},{5}.",
						mSettings.PortName,
						mSettings.Baud,
						mSettings.DataBits,
						mSettings.Parity,
						mSettings.StopBits,
						mSettings.Handshake
					);
				}
				#endregion
			}
			catch (Exception exception)
			{
				#region (logging)
				{
					mLog.ErrorFormat(CultureInfo.InvariantCulture,
						"FAILED to open serial transport on '{0}' with {1},{2},{3},{4},{5}.",
						exception,
						mSettings.PortName,
						mSettings.Baud,
						mSettings.DataBits,
						mSettings.Parity,
						mSettings.StopBits,
						mSettings.Handshake
					);
				}
				#endregion

				throw;
			}
		}

		private static Parity _Convert(System.IO.Ports.Parity value)
		{
			switch (value)
			{
				case System.IO.Ports.Parity.Even:
					return Parity.Even;
				case System.IO.Ports.Parity.Mark:
					return Parity.Mark;
				case System.IO.Ports.Parity.None:
					return Parity.None;
				case System.IO.Ports.Parity.Odd:
					return Parity.Odd;
				case System.IO.Ports.Parity.Space:
					return Parity.Space;
				default:
					throw ExceptionFactory.NotSupportedException("The given Parity '{0}' is not supported.", null, value);
			}
		}

		private static StopBits _Convert(System.IO.Ports.StopBits value)
		{
			switch (value)
			{
				case System.IO.Ports.StopBits.One:
					return StopBits.One;
				case System.IO.Ports.StopBits.OnePointFive:
					return StopBits.One5;
				case System.IO.Ports.StopBits.Two:
					return StopBits.Two;
				default:
					throw ExceptionFactory.NotSupportedException("The given StopBits '{0}' is not supported.", null, value);
			}
		}

		private static Handshake _Convert(System.IO.Ports.Handshake value)
		{
			switch (value)
			{
				case System.IO.Ports.Handshake.None:
					return Handshake.None;
				case System.IO.Ports.Handshake.RequestToSend:
					return Handshake.Rts;
				case System.IO.Ports.Handshake.RequestToSendXOnXOff:
					return Handshake.RtsXOn;
				case System.IO.Ports.Handshake.XOnXOff:
					return Handshake.XOn;
				default:
					throw ExceptionFactory.NotSupportedException("The given Handshake '{0}' is not supported.", null, value);
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
						"Closed serial transport on '{0}'.",
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
						"FAILED to close serial transport on '{0}'.",
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
					"Disposed serial transport on '{0}'.",
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
				mStream.Write(data.Buffer, data.Offset, data.Count);

				#region (logging)
				{
					if (mLog.IsTraceEnabled)
					{
						mLog.TraceFormat(CultureInfo.InvariantCulture,
							"Sent data via serial transport on '{0}'; Data={1}.",
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
						"FAILED to send data via serial transport on '{0}'; Data={1}.",
						exception,
						mSettings.PortName,
						data
					);
				}
				#endregion

				throw;
			}
		}


		private void _HandleDataReceived(Object sender, SerialDataReceivedEventArgs e)
		{
			try
			{
				if (e.EventType == SerialData.NoData)
					return;

				var data = _ReadDataReceived();
				_RaiseReceivedEvent(data);
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
				var bytesToRead = mStream.BytesToRead;
				var buffer = new Byte[bytesToRead];
				var bytesRead = mStream.Read(buffer, 0, bytesToRead);
				var data = BufferSpan.From(buffer, bytesRead);

				#region (logging)
				{
					if (mLog.IsTraceEnabled)
					{
						mLog.TraceFormat(CultureInfo.InvariantCulture,
							"Received data via serial transport on '{0}'; Data={1}.",
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
						"FAILED to receive data via serial transport on '{0}'.",
						exception,
						mSettings.PortName
					);
				}
				#endregion

				throw;
			}
		}

		private void _RaiseReceivedEvent(BufferSpan data)
		{
			try
			{
				if (!data.IsEmpty)
					this.Received?.Invoke(this, new TransportDataReceivedEventArgs(data));
			}
			catch (Exception exception)
			{
				#region (logging)
				{
					mLog.ErrorFormat(CultureInfo.InvariantCulture,
						"User code invoked for event 'Received' on serial transport on '{0}' threw an exception.",
						exception,
						mSettings.PortName
					);
				}
				#endregion

				throw;
			}
		}


		private void _HandleErrorReceived(Object sender, SerialErrorReceivedEventArgs e)
		{
			if (e.EventType == SerialError.NoError)
				return;

			#region (logging)
			{
				mLog.WarnFormat(CultureInfo.InvariantCulture,
					"A serial error of type '{0}' occurred on serial transport on '{1}'.",
					e.EventType,
					mSettings.PortName
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
