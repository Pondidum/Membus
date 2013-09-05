using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Membus
{
	public class InMemoryBus : IBus
	{
		private readonly IDictionary<Type, List<Handler>> _handlers;
		private readonly AutoWire _wiring;

		public InMemoryBus()
		{
			_handlers = new Dictionary<Type, List<Handler>>();
			_wiring = new AutoWire(Wire, UnWire);
		}


		public void Publish<T>(T message)
		{
			_wiring.Tidy();

			var type = typeof(T);
			var handlers = _handlers.GetOrDefault(type);

			if (handlers == null)
			{
				return;
			}

			foreach (var entry in handlers)
			{
				entry.Action.Invoke(message);
			}
		}

		public void Wire<T>(Action<T> handler)
		{
			if (handler == null) throw new ArgumentNullException("handler");

			Wire(typeof(T), new Handler(handler.GetHashCode(), message => handler.Invoke((T)message)));
		}

		private void Wire(Type type, Handler handler)
		{
			var handlers = _handlers.GetOrDefault(type);

			if (handlers == null)
			{
				handlers = new List<Handler>();
				_handlers[type] = handlers;
			}

			handlers.Add(handler);
		}

		public void UnWire<T>(Action<T> handler)
		{
			if (handler == null) throw new ArgumentNullException("handler");

			UnWire(typeof(T), handler.GetHashCode());
		}

		private void UnWire(Type type, int hash)
		{
			var handlers = _handlers.GetOrDefault(type);

			if (handlers == null)
			{
				return;
			}

			handlers.RemoveAll(h => h.Hash == hash);
		}

		public void AutoWire(object host)
		{
			_wiring.Hook(host);
		}
	}
}
