using Microsoft.AspNetCore.SignalR;

namespace SchemeEditor.Application.Infrastructure
{
	public class NameUserIdProvider : IUserIdProvider
	{
		public string GetUserId(HubConnectionContext connection)
		{
			return connection.User?.Identity?.Name;
		}
	}
}