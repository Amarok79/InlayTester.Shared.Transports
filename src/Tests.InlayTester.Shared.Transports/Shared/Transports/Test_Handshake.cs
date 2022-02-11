// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports;


[TestFixture]
public class Test_Handshake
{
    [Test]
    public void TestValues()
    {
        Check.That(Enum.GetValues(typeof(Handshake)))
           .IsOnlyMadeOf(
                Handshake.None,
                Handshake.RequestToSend,
                Handshake.RequestToSendXOnXOff,
                Handshake.XOnXOff
            );
    }
}
