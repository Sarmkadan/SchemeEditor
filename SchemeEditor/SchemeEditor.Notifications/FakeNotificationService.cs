using System.Threading.Tasks;
using SchemeEditor.Notifications.Abstractions;

namespace SchemeEditor.Notifications
{
	public class FakeNotificationService: INotificationService
	{
		public async Task Send(INotification notification)
		{
			await Task.Delay(1);
			// Do nothing
		}
	}
}