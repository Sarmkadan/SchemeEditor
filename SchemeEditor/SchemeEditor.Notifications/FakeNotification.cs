using SchemeEditor.Notifications.Abstractions;

namespace SchemeEditor.Notifications
{
	public class FakeNotification: INotification
	{
		public string Subject { get; set; }
		public string Payload { get; set; }
		public string To { get; set; }
	}
}