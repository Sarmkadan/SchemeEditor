using System.Threading.Tasks;

namespace SchemeEditor.Notifications.Abstractions
{
	public interface INotificationService
	{
		Task Send(INotification notification);
	}
}