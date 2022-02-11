// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.IO;
using Amarok.Events;
using Amarok.Shared;


namespace InlayTester.Shared.Transports;


/// <summary>
///     This interface represents a transport for sending and receiving data.
/// </summary>
public interface ITransport : IDisposable
{
    /// <summary>
    ///     An event that is raised for data that has been received.
    /// </summary>
    Event<BufferSpan> Received { get; }


    /// <summary>
    ///     Opens the transport. A transport can be opened and closed multiple times.
    /// </summary>
    /// 
    /// <exception cref="ObjectDisposedException">
    ///     A method or property was called on an already disposed object.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The transport has already been opened before.
    /// </exception>
    /// <exception cref="IOException">
    ///     The transport settings seem to be invalid.
    /// </exception>
    void Open();

    /// <summary>
    ///     Closes the transport. A transport can be opened and closed multiple times.
    /// </summary>
    /// 
    /// <exception cref="ObjectDisposedException">
    ///     A method or property was called on an already disposed object.
    /// </exception>
    void Close();


    /// <summary>
    ///     Sends the given data over the transport.
    /// </summary>
    /// 
    /// <param name="data">
    ///     The data to send.
    /// </param>
    /// 
    /// <exception cref="ObjectDisposedException">
    ///     A method or property was called on an already disposed object.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    ///     The transport has not been opened yet.
    /// </exception>
    void Send(BufferSpan data);
}
