using System;

namespace Membus.Constraints
{
	public class BeforeHandlerRegisteredConstraint : Constraint
	{
		private  readonly Action<Type, Action<object>> _action;

		public BeforeHandlerRegisteredConstraint(Action<Type, Action<Object>> action)
		{
			_action = action;
		}

		public override void BeforeHandlerRegistered(Type type, Action<object> handler)
		{
			_action.Invoke(type, handler);
		}
	}
}
