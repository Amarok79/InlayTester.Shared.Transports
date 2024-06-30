// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

namespace InlayTester.Shared.Transports;


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
    ///     Request-to-Send (RTS) hardware flow control is used. RTS signals that data is available for transmission. If the
    ///     input buffer becomes full, the RTS line will be set to false. The RTS line will be set to true when more room
    ///     becomes available in the input buffer.
    /// </summary>
    RequestToSend,

    /// <summary>
    ///     Both the Request-to-Send (RTS) hardware control and the XON/XOFF software controls are used.
    /// </summary>
    RequestToSendXOnXOff,

    /// <summary>
    ///     The XON/XOFF software control protocol is used. The XOFF control is sent to stop the transmission of data. The XON
    ///     control is sent to resume the transmission. These software controls are used instead of Request to Send (RTS) and
    ///     Clear to Send (CTS) hardware controls.
    /// </summary>
    XOnXOff,
}
