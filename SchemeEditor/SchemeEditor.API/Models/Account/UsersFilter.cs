using SchemeEditor.Domain.Models;

namespace SchemeEditor.API.Models.Account
{
	public class UsersFilter
	{
		public string Company { get; set; }
		public bool? IsBlocked { get; set; }
		public long? RoleId { get; set; }
		public string Name { get; set; }
		public bool SkipCurrent { get; set; }
		public int Page { get; set; }
		public int Take { get; set; } = 10;
	}
}