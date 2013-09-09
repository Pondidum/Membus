using System;
using System.Collections.Generic;
using System.Linq;

namespace Membus
{
	public class Constraint
	{
		private IDictionary<Type, List<Handler>> _handlers;

		internal void Initialise(IDictionary<Type, List<Handler>> handlers)
		{
			_handlers = handlers;
		}

		protected IEnumerable<Action<Object>> HandlersFor(Type type)
		{
			var handlers = _handlers.GetOrDefault(type);

			if (handlers != null)
			{
				return handlers.Select(h => h.Action);
			}

			return Enumerable.Empty<Action<Object>>();
		}

		public virtual void BeforeHandlerRegistered(Type type, Action<Object> handler)
		{
			
		}

		public virtual void BeforePublish(object message)
		{
			
		}

	}
}
