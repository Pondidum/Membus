using System;
using System.Security.Cryptography;
using Should;
using Xunit;

namespace Membus.Tests
{
	public class Scratchpad
	{
		
		private WeakReference CreateAndStore()
		{
			return new WeakReference(new Temp());
		}

		private WeakReference CreateActionAndStore(Action action)
		{
			return new WeakReference(new TempAction(action));
		}

		[Fact]
		public void Run()
		{
			var wr = CreateAndStore();

			GC.WaitForPendingFinalizers();
			GC.Collect();

			wr.IsAlive.ShouldBeFalse();
		}

		[Fact]
		public void RunAgain()
		{
			var count = 0;
			var wr = CreateActionAndStore(() => count++);

			GC.WaitForPendingFinalizers();
			GC.Collect();

			wr.IsAlive.ShouldBeFalse();
		}



		private class Temp
		{
			
		}

		private class TempAction
		{
			private readonly Action _action;

			public TempAction(Action action)
			{
				_action = action;
			}
		}
	}
}
