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

using System;
using Amarok.Shared;
using Microsoft.Extensions.Logging;
using RJCP.IO.Ports;


namespace InlayTester.Shared.Transports
{
    internal static class LoggerExtensions
    {
        #region Open (1)

        private static readonly Action<ILogger, String, SerialTransportSettings, Exception?> sOpen =
            LoggerMessage.Define<String, SerialTransportSettings>(
                LogLevel.Information,
                1,
                "[{PortName}]  OPENED    {Settings}"
            );

        private static readonly Action<ILogger, String, SerialTransportSettings, Exception?> sFailedToOpen =
            LoggerMessage.Define<String, SerialTransportSettings>(
                LogLevel.Error,
                1001,
                "[{PortName}]  FAILED to open. Settings: '{Settings}'."
            );


        public static void LogOpen(this ILogger logger, String portName, SerialTransportSettings settings)
        {
            sOpen(logger, portName, settings, null);
        }

        public static void LogFailedToOpen(
            this ILogger logger,
            String portName,
            SerialTransportSettings settings,
            Exception exception
        )
        {
            sFailedToOpen(logger, portName, settings, exception);
        }

        #endregion

        #region Close (2)

        private static readonly Action<ILogger, String, Exception?> sClose = LoggerMessage.Define<String>(
            LogLevel.Information,
            2,
            "[{PortName}]  CLOSED"
        );

        private static readonly Action<ILogger, String, Exception?> sFailedToClose =
            LoggerMessage.Define<String>(LogLevel.Error, 1002, "[{PortName}]  FAILED to close.");


        public static void LogClose(this ILogger logger, String portName)
        {
            sClose(logger, portName, null);
        }

        public static void LogFailedToClose(this ILogger logger, String portName, Exception exception)
        {
            sFailedToClose(logger, portName, exception);
        }

        #endregion

        #region Dispose (3)

        private static readonly Action<ILogger, String, Exception?> sDispose = LoggerMessage.Define<String>(
            LogLevel.Information,
            3,
            "[{PortName}]  DISPOSED"
        );

        public static void LogDispose(this ILogger logger, String portName)
        {
            sDispose(logger, portName, null);
        }

        #endregion

        #region Send (4)

        private static readonly Action<ILogger, String, BufferSpan, Exception?> sSend =
            LoggerMessage.Define<String, BufferSpan>(LogLevel.Trace, 4, "[{PortName}]  SENT      {Data}");

        private static readonly Action<ILogger, String, BufferSpan, Exception?> sFailedToSend =
            LoggerMessage.Define<String, BufferSpan>(
                LogLevel.Error,
                1004,
                "[{PortName}]  FAILED to send. Data: '{Data}'."
            );


        public static void LogSend(this ILogger logger, String portName, BufferSpan data)
        {
            sSend(logger, portName, data, null);
        }

        public static void LogFailedToSend(this ILogger logger, String portName, BufferSpan data, Exception exception)
        {
            sFailedToSend(logger, portName, data, exception);
        }

        #endregion

        #region Receive (5)

        private static readonly Action<ILogger, String, BufferSpan, Exception?> sReceive =
            LoggerMessage.Define<String, BufferSpan>(LogLevel.Trace, 5, "[{PortName}]  RECEIVED  {Data}");

        private static readonly Action<ILogger, String, Exception?> sFailedToReceive =
            LoggerMessage.Define<String>(LogLevel.Error, 1005, "[{PortName}]  FAILED to receive.");


        public static void LogReceive(this ILogger logger, String portName, BufferSpan data)
        {
            sReceive(logger, portName, data, null);
        }

        public static void LogFailedToReceive(this ILogger logger, String portName, Exception exception)
        {
            sFailedToReceive(logger, portName, exception);
        }

        #endregion

        #region SerialError (6)

        private static readonly Action<ILogger, String, SerialError, Exception?> sSerialError =
            LoggerMessage.Define<String, SerialError>(LogLevel.Warning, 6, "[{PortName}]  SERIAL ERROR  '{Error}'");

        public static void LogSerialError(this ILogger logger, String portName, SerialError error)
        {
            sSerialError(logger, portName, error, null);
        }

        #endregion
    }
}
