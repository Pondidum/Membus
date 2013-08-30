using System;
using System.Collections.Generic;

namespace Membus
{
	public class InMemoryBus : IBus
	{
		private readonly IDictionary<Type, Dictionary<int, Action<Object>>> _handlers;

		public InMemoryBus()
		{
			_handlers = new Dictionary<Type, Dictionary<int, Action<Object>>>();
		}

		public void Publish<T>(T message)
		{
			var type = typeof(T);
			var handlers = _handlers[type];

			foreach (var pair in handlers)
			{
				pair.Value(message);
			}
		}

		public void Wire<T>(Action<T> handler)
		{
			var type = typeof(T);
			var handlers = _handlers[type];

			if (handlers == null)
			{
				handlers = new Dictionary<int, Action<Object>>();
				_handlers[type] = handlers;
			}

			var hash = handler.GetHashCode();

			handlers[hash] = message => handler.Invoke((T)message);
		}

		public void UnWire<T>(Action<T> handler)
		{
			var type = typeof(T);
			var handlers = _handlers[type];

			if (handlers == null)
			{
				return;
			}

			var hash = handler.GetHashCode();

			if (handlers.ContainsKey(hash))
			{
				handlers.Remove(hash);
			}
		}

		public void AutoWire(object host)
		{

		}
	}
}
