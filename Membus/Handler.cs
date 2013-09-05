using System;

namespace Membus
{
	internal struct Handler
	{
		public readonly int Hash;
		public readonly Action<object> Action;

		public Handler(int hash, Action<Object> action)
		{
			Hash = hash;
			Action = action;
		}
	}
}
