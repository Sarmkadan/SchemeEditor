using System.Collections.Generic;

namespace SchemeEditor.Identity.Settings
{
	public class BasicRoles
	{
		public static IDictionary<string, string> Roles = new Dictionary<string, string>
		{
			{"ADMINISTRATOR", "Администратор"},
			{"MODERATOR", "Прескриптор"},
			{"USER", "Пользователь"},
			{"NOT ACTIVE", "Не активный"},
		};
	}
}
