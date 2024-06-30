// Copyright (c) 2024, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports;


[TestFixture]
public class Test_StopBits
{
    [Test]
    public void TestValues()
    {
        Check.That(Enum.GetValues(typeof(StopBits))).IsOnlyMadeOf(StopBits.One, StopBits.OnePointFive, StopBits.Two);
    }
}
