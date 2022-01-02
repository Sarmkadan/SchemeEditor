using Microsoft.Extensions.DependencyInjection;
using SchemeEditor.Notifications.Abstractions;

namespace SchemeEditor.Notifications.Infrastructure
{
	public  static class StartupHelper
	{
		public static void RegisterNotifications(this IServiceCollection services)
		{
			services.AddTransient<INotification, FakeNotification>();
			services.AddTransient<INotificationService, FakeNotificationService>();
		}
	}
}