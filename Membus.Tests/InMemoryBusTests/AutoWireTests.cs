using System;
using System.ComponentModel;
using System.Data.SqlClient;
using Membus.Tests.TestData;
using Should;
using Xunit;

namespace Membus.Tests.InMemoryBusTests
{
	public class AutoWireTests
	{

		[Fact]
		public void When_there_is_nothing_to_wire_up()
		{
			var bus = new InMemoryBus();
			var target = new NoMethodsClass();

			Assert.DoesNotThrow(() =>bus.AutoWire(target));
		}

		[Fact]
		public void When_there_is_one_method_matching()
		{
			var bus = new InMemoryBus();
			var count = 0;
			var target = new OneBlankMessageClass(() => count++);

			bus.AutoWire(target);
			bus.Publish(new BlankMessage());

			count.ShouldEqual(1);
		}

		private void NonDisposableTarget(InMemoryBus bus, Action action)
		{
			var target = new OneBlankMessageClass(action);

			bus.AutoWire(target);
			bus.Publish(new BlankMessage());
		}

		private void DisposableTarget(InMemoryBus bus, Action action)
		{
			using (var target = new OneBlankMessageDisposableClass(action))
			{
				bus.AutoWire(target);
				bus.Publish(new BlankMessage());
			}
		}

		[Fact]
		public void When_a_non_component_class_is_disposed_it_should_unwire()
		{
			var bus = new InMemoryBus();
			var count = 0;

			NonDisposableTarget(bus, () => count++);

			GC.WaitForPendingFinalizers();
			GC.Collect();

			bus.Publish(new BlankMessage());

			count.ShouldEqual(1);
		}

		[Fact]
		public void When_the_target_is_disposed_it_should_un_wire()
		{
			var bus = new InMemoryBus();
			var count = 0;

			DisposableTarget(bus, () => count++);

			GC.WaitForPendingFinalizers();
			GC.Collect();

			bus.Publish(new BlankMessage());

			count.ShouldEqual(1);
		}

		class NoMethodsClass
		{
		}

		class OneBlankMessageClass
		{
			private readonly Action _onCall;

			public OneBlankMessageClass(Action onCall)
			{
				_onCall = onCall;
			}

			private void Handle(BlankMessage message)
			{
				_onCall();
			}
		}

		class OneBlankMessageDisposableClass : IDisposable
		{
			private readonly Action _onCall;

			public OneBlankMessageDisposableClass(Action onCall)
			{
				_onCall = onCall;
			}

			private void Handle(BlankMessage message)
			{
				_onCall();
			}

			public void Dispose()
			{
				
			}
		}
	}
}
