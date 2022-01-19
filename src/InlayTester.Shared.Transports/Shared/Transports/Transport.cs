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
using Amarok.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


namespace InlayTester.Shared.Transports
{
    /// <summary>
    ///     This type represents a factory for <see cref="ITransport"/>.
    /// </summary>
    public static class Transport
    {
        /// <summary>
        ///     Creates a new serial <see cref="ITransport"/> for the given <paramref name="settings"/>.
        /// </summary>
        /// 
        /// <param name="settings">
        ///     The settings for the serial transport to create.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        ///     A null reference was passed to a method that did not accept it as a valid argument.
        /// </exception>
        public static ITransport Create(SerialTransportSettings settings)
        {
            Verify.NotNull(settings, nameof(settings));

            settings = new SerialTransportSettings(settings);

            return new DefaultSerialTransport(settings, NullLogger.Instance, null);
        }

        /// <summary>
        ///     Creates a new serial <see cref="ITransport"/> for the given <paramref name="settings"/>.
        /// </summary>
        /// 
        /// <param name="settings">
        ///     The settings for the serial transport to create.
        /// </param>
        /// <param name="hooks">
        ///     A hooks implementation being called for sent or received data.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        ///     A null reference was passed to a method that did not accept it as a valid argument.
        /// </exception>
        public static ITransport Create(SerialTransportSettings settings, ITransportHooks hooks)
        {
            Verify.NotNull(settings, nameof(settings));
            Verify.NotNull(hooks, nameof(hooks));

            settings = new SerialTransportSettings(settings);

            return new DefaultSerialTransport(settings, NullLogger.Instance, hooks);
        }

        /// <summary>
        ///     Creates a new serial <see cref="ITransport"/> for the given <paramref name="settings"/>.
        /// </summary>
        /// 
        /// <param name="settings">
        ///     The settings for the serial transport to create.
        /// </param>
        /// <param name="logger">
        ///     The logger that should be used for logging transport operations.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        ///     A null reference was passed to a method that did not accept it as a valid argument.
        /// </exception>
        public static ITransport Create(SerialTransportSettings settings, ILogger logger)
        {
            Verify.NotNull(settings, nameof(settings));
            Verify.NotNull(logger, nameof(logger));

            settings = new SerialTransportSettings(settings);

            return new DefaultSerialTransport(settings, logger, null);
        }

        /// <summary>
        ///     Creates a new serial <see cref="ITransport"/> for the given <paramref name="settings"/>.
        /// </summary>
        /// 
        /// <param name="settings">
        ///     The settings for the serial transport to create.
        /// </param>
        /// <param name="logger">
        ///     The logger that should be used for logging transport operations.
        /// </param>
        /// <param name="hooks">
        ///     A hooks implementation being called for sent or received data.
        /// </param>
        /// 
        /// <exception cref="ArgumentNullException">
        ///     A null reference was passed to a method that did not accept it as a valid argument.
        /// </exception>
        public static ITransport Create(SerialTransportSettings settings, ILogger logger, ITransportHooks hooks)
        {
            Verify.NotNull(settings, nameof(settings));
            Verify.NotNull(logger, nameof(logger));
            Verify.NotNull(hooks, nameof(hooks));

            settings = new SerialTransportSettings(settings);

            return new DefaultSerialTransport(settings, logger, hooks);
        }
    }
}
