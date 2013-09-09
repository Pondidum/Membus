using System;
using System.Runtime.Serialization;

namespace Membus
{

	[Serializable]
	public class ConstraintException : Exception
	{
		public ConstraintException(String constraint, String action)
			: base(String.Format("Constraint {0}.{1} failed.", constraint, action))
		{
		}

		protected ConstraintException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}
	}
}
