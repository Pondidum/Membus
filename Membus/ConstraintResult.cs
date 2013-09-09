using System;

namespace Membus
{
	public class ConstraintResult
	{
		public string Message { get; private set; }
		public Levels Level { get; private set; }

		public ConstraintResult(Levels level, String message)
		{
			Level = level;
			Message = message;
		}

		public enum Levels
		{
			Success,
			Info,
			Warning,
			Fail,
		}
	}
}