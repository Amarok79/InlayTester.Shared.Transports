// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

namespace InlayTester.Shared.Transports;


/// <summary>
///     Defines the supported parity modes for serial communication.
/// </summary>
public enum Parity
{
    /// <summary>
    ///     No parity check occurs.
    /// </summary>
    None,

    /// <summary>
    ///     Sets the parity bit so that the count of bits set is an even number.
    /// </summary>
    Even,

    /// <summary>
    ///     Sets the parity bit so that the count of bits set is an odd number.
    /// </summary>
    Odd,

    /// <summary>
    ///     Leaves the parity bit set to 1.
    /// </summary>
    Mark,

    /// <summary>
    ///     Leaves the parity bit set to 0.
    /// </summary>
    Space,
}
