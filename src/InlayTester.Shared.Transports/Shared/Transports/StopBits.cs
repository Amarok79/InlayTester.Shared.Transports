// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

namespace InlayTester.Shared.Transports;


/// <summary>
///     Defines the number of stop bits for serial communication.
/// </summary>
public enum StopBits
{
    /// <summary>
    ///     One stop bit is used.
    /// </summary>
    One,

    /// <summary>
    ///     1.5 stop bits are used.
    /// </summary>
    OnePointFive,

    /// <summary>
    ///     Two stop bits are used.
    /// </summary>
    Two,
}
