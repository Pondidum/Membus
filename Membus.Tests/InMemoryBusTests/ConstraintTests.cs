using System;
using System.Data;
using Membus.Constraints;
using Xunit;

namespace Membus.Tests.InMemoryBusTests
{
	public class ConstraintTests
	{

		private class ThrowingBeforeRegisterConstraint : Constraint
		{
			public override void BeforeHandlerRegistered(Type type, Action<object> handler)
			{
				throw new ConstraintException();
			}
		}

		private class ThrowingBeforePublishConstraint : Constraint
		{
			public override void BeforePublish(object message)
			{
				throw new ConstraintException();
			}
		}

		[Fact]
		public void When_a_constraint_doesnt_allow_any_handlers_to_be_wired()
		{
			var bus = new InMemoryBus();
			Action<String> action = s => { };

			bus.Constraints.Add(new ThrowingBeforeRegisterConstraint());

			Assert.Throws<ConstraintException>(() => bus.Wire(action));
		}

		[Fact]
		public void When_a_constraint_doesnt_allow_publishing()
		{
			var bus = new InMemoryBus();
			Action<String> action = s => { };

			bus.Constraints.Add(new ThrowingBeforePublishConstraint());

			Assert.Throws<ConstraintException>(() => bus.Publish("string"));
		}
	}
}
