using System.Collections.Generic;

namespace Membus
{
	static class Extensions
	{
		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key)
		{
			return GetOrDefault(self, key, default(TValue));
		}

		public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> self, TKey key, TValue defaultValue)
		{
			TValue result;
			return self.TryGetValue(key, out result) ? result : defaultValue;
		}
	}
}