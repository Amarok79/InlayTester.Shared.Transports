// Copyright (c) 2018, Olaf Kober <olaf.kober@outlook.com>

using NFluent;
using NUnit.Framework;


namespace InlayTester.Shared.Transports
{
	[TestFixture]
	public class Test_TransportDataReceivedEventArgs
	{
		[Test]
		public void Construction()
		{
			var data = BufferSpan.From(0x11, 0x22, 0x33);
			var args = new TransportDataReceivedEventArgs(data);

			Check.That(args.Data.ToArray())
				.ContainsExactly(0x11, 0x22, 0x33);
		}
	}
}
