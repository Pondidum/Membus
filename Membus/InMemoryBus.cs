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
		private readonly List<WeakWiring> _wiring;

		public InMemoryBus()
		{
			_handlers = new Dictionary<Type, List<Handler>>();
			_wiring = new List<WeakWiring>();
		}

		private void TidyWiring()
		{
			var dead = _wiring.Where(w => w.Host.IsAlive == false).ToList();

			dead.ForEach(w => w.Unwire());
			dead.ForEach(w => _wiring.Remove(w));
		}

		public void Publish<T>(T message)
		{
			TidyWiring();

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
			var weakHost = new WeakReference(host);
			var methods = host.GetType()
							  .GetMethods(BindingFlags.Instance | BindingFlags.NonPublic)
							  .Where(m => m.Name == "Handle")
							  .Where(m => m.GetParameters().Count() == 1)
							  .ToList();

			var handlers = new Dictionary<Type, Handler>();

			foreach (var method in methods)
			{
				var type = method.GetParameters().First().ParameterType;
				Action<Object> action = parameter => method.Invoke(weakHost.Target, new[] { parameter });

				var handler = new Handler(action.GetHashCode(), action);

				handlers.Add(type, handler);
			}

			Action wire = () =>
			{
				foreach (var handler in handlers)
				{
					Wire(handler.Key, handler.Value);
				}

			};

			Action unwire = () =>
			{
				foreach (var handler in handlers)
				{
					UnWire(handler.Key, handler.Value.Hash);
				}
			};

			wire();

			_wiring.Add(new WeakWiring(weakHost, unwire));
		}

		private struct WeakWiring
		{
			public readonly WeakReference Host;
			public readonly Action Unwire;

			public WeakWiring(WeakReference host, Action unwire)
			{
				Host = host;
				Unwire = unwire;
			}
		}

		private struct Handler
		{
			public readonly int Hash;
			public readonly Action<Object> Action;

			public Handler(int hash, Action<Object> action)
			{
				Hash = hash;
				Action = action;
			}
		}
	}
}
