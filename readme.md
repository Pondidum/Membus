Usage
=====

	public class Form1 : Form
	{
		private readonly IBus _bus;

		public Form1(IBus bus)
		{
			_bus = bus;
			_bus.AutoWire(this);
		}

		private void Handle(SomeMessage message)
		{
			// methods called Handle with one argument will be auto wired up by bus.AutoWire
			// unwiring will automagically happen on dispose
		}

		private void CreateButton_OnClick(Object sender, EventArgs e)
		{
			_bus.Publish(new CandidateCreatedEvent{
				Name = txtName.Text,
				Sex = cmbSex as Gender,
			});
		}

	}
	