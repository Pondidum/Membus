using System;
using System.Collections.Generic;

namespace Membus
{
	public class ConstraintCollection : Constraint
	{
		private readonly List<Constraint> _constraints;

		public ConstraintCollection()
		{
			_constraints = new List<Constraint>();
		}

		public void Add(Constraint constraint)
		{
			_constraints.Add(constraint);
		}

		public override void BeforeHandlerRegistered(Type type, Action<Object> handler)
		{
			_constraints.ForEach(c => c.BeforeHandlerRegistered(type, handler));
		}

		public override void BeforePublish(object message)
		{
			_constraints.ForEach(c => c.BeforePublish(message));
		}
	}
}
