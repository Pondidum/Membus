using System;

namespace Membus.Constraints
{
	public class BeforePublishConstraint : Constraint
	{
		private readonly Action<object> _action;

		public BeforePublishConstraint(Action<Object> action)
		{
			_action = action;
		}

		public override void BeforePublish(object message)
		{
			_action.Invoke(message);
		}
	}
}
