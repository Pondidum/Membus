using System;
using System.Linq;

namespace Membus.Constraints
{
	public class UniqueHandlerConstraint : Constraint
	{
		public override void BeforeHandlerRegistered(Type type, Action<Object> handler)
		{
			if (HandlersFor(type).Any())
			{
				throw new Exception();
			}
		}
	}
}
