// Copyright (c) 2022, Olaf Kober <olaf.kober@outlook.com>

using System;
using Amarok.Contracts;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;


namespace InlayTester.Shared.Transports;


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
