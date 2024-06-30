// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using Amarok.Shared;


namespace InlayTester.Shared.Transports;


/// <summary>
///     This interface provides hook methods for <see cref="ITransport"/>.
/// </summary>
public interface ITransportHooks
{
    /// <summary>
    ///     This hook method is invoked before the data is sent. The implementer is able to monitor or even manipulate the
    ///     data.
    /// </summary>
    /// 
    /// <param name="data">
    ///     The data to be sent.
    /// </param>
    void BeforeSend(ref BufferSpan data);

    /// <summary>
    ///     This hook method is invoked after data has been received. The implementer is able to monitor or even manipulate the
    ///     data.
    /// </summary>
    /// 
    /// <param name="data">
    ///     The data being received.
    /// </param>
    void AfterReceived(ref BufferSpan data);
}
