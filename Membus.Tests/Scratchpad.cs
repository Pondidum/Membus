using System;
using System.Security.Cryptography;
using Should;
using Xunit;

namespace Membus.Tests
{
	public class Scratchpad
	{
		[Fact]
		public void Run()
		{
			Action first = Method;
			Action second = Method;

			//first.ShouldEqual(second);

			Action third = () => first();
			Action fourth = () => first();

			third.ShouldEqual(fourth);
		}

		

		private void Method()
		{
			
		}
	}
}
