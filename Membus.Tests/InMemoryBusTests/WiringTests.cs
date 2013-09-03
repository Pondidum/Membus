using System;
using Membus.Tests.TestData;
using Should;
using Xunit;

namespace Membus.Tests.InMemoryBusTests
{
	public class WiringTests
	{

		[Fact]
		public void When_wire_is_passed_a_null_handler()
		{
			var bus = new InMemoryBus();

			Assert.Throws<ArgumentNullException>(() => bus.Wire<BlankMessage>(null));
		}

		[Fact]
		public void When_the_bus_recives_a_message_it_should_invoke_the_handler()
		{
			var bus = new InMemoryBus();
			var count = 0;
			Action<BlankMessage> handler = message => count++;

			bus.Wire(handler);
			bus.Publish(new BlankMessage());

			count.ShouldEqual(1);
		}

		[Fact]
		public void When_the_bus_recieves_a_message_and_there_is_no_handler()
		{
			var bus = new InMemoryBus();

			Assert.DoesNotThrow(() => bus.Publish(new BlankMessage()));
		}

		[Fact]
		public void When_a_handler_is_unwired()
		{
			var bus = new InMemoryBus();
			var count = 0;
			Action<BlankMessage> handler = message => count++;

			bus.Wire(handler);
			bus.UnWire(handler);

			bus.Publish(new BlankMessage());

			count.ShouldEqual(0);
		}

	}
}
