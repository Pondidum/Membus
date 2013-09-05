using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Membus
{
	class AutoWire
	{
		readonly Action<Type, Handler> _wire;
		readonly Action<Type, int> _unwire;
		private readonly List<WeakWiring> _wiring;

		public AutoWire(Action<Type, Handler> wire, Action<Type, int> unwire)
		{
			_wire = wire;
			_unwire = unwire;
			_wiring = new List<WeakWiring>();
		}

		public void Hook(object host)
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
					_wire(handler.Key, handler.Value);
				}

			};

			Action unwire = () =>
			{
				foreach (var handler in handlers)
				{
					_unwire(handler.Key, handler.Value.Hash);
				}
			};

			wire();

			_wiring.Add(new WeakWiring(weakHost, unwire));
		}

		public void Tidy()
		{
			var dead = _wiring.Where(w => w.Host.IsAlive == false).ToList();

			dead.ForEach(w => w.Unwire());
			dead.ForEach(w => _wiring.Remove(w));
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
	}
}
