// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using System.Globalization;
using Amarok.Contracts;


namespace InlayTester.Shared.Transports;


/// <summary>
///     This type wraps settings for serial transports.
/// </summary>
public sealed class SerialTransportSettings
{
    /// <summary>
    ///     Gets or sets the name of the serial port, i.e. "COM1". Defaults to "COM1".
    /// </summary>
    public String PortName { get; set; }

    /// <summary>
    ///     Gets or sets the baud rate of the serial port, i.e. 9600 baud. Defaults to 9600.
    /// </summary>
    public Int32 Baud { get; set; }

    /// <summary>
    ///     Gets or sets the data bits to use for the serial port. Defaults to 8.
    /// </summary>
    public Int32 DataBits { get; set; }

    /// <summary>
    ///     Gets or sets the parity to use for the serial port. Defaults to None.
    /// </summary>
    public Parity Parity { get; set; }

    /// <summary>
    ///     Gets or sets the stop bits to use for the serial port. Defaults to 1.
    /// </summary>
    public StopBits StopBits { get; set; }

    /// <summary>
    ///     Gets or sets the handshake protocol to use for the serial port. Defaults to None.
    /// </summary>
    public Handshake Handshake { get; set; }


    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    public SerialTransportSettings()
    {
        PortName  = "COM1";
        Baud      = 9600;
        DataBits  = 8;
        Parity    = Parity.None;
        StopBits  = StopBits.One;
        Handshake = Handshake.None;
    }

    /// <summary>
    ///     Initializes a new instance.
    /// </summary>
    /// 
    /// <param name="settings">
    ///     The settings to copy from.
    /// </param>
    /// 
    /// <exception cref="ArgumentNullException">
    ///     A null reference was passed to a method that did not accept it as a valid argument.
    /// </exception>
    public SerialTransportSettings(SerialTransportSettings settings)
    {
        Verify.NotNull(settings, nameof(settings));

        PortName  = settings.PortName;
        Baud      = settings.Baud;
        DataBits  = settings.DataBits;
        Parity    = settings.Parity;
        StopBits  = settings.StopBits;
        Handshake = settings.Handshake;
    }


    /// <summary>
    ///     Returns a string that represents the current instance.
    /// </summary>
    public override String ToString()
    {
        return String.Format(
            CultureInfo.InvariantCulture,
            "{0},{1},{2},{3},{4},{5}",
            PortName,
            Baud,
            DataBits,
            Parity,
            StopBits,
            Handshake
        );
    }
}
