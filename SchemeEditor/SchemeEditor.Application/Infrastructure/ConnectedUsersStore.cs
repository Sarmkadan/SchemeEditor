using System.Collections.Concurrent;

namespace SchemeEditor.Application.Infrastructure
{
	public class ConnectedUsersStore
	{
		public static ConcurrentDictionary<long, string> Store = new ConcurrentDictionary<long, string>();
	}
}