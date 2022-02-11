// Copyright (c) 2021, Olaf Kober <olaf.kober@outlook.com>

using System;
using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports;


[TestFixture]
public class Test_Parity
{
    [Test]
    public void TestValues()
    {
        Check.That(Enum.GetValues(typeof(Parity)))
           .IsOnlyMadeOf(Parity.None, Parity.Even, Parity.Odd, Parity.Mark, Parity.Space);
    }
}
