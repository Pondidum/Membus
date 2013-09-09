Usage
=====

	public class Form1 : Form
	{
		public Form1(IBus bus)
		{
			bus.AutoWire(this);
		}

		private void Handle(SomeMessage message)
		{
			// methods called Handle with one argument will be auto wired up by bus.AutoWire
			// unwiring will automagically happen on dispose
		}

	}