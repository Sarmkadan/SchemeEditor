using SchemeEditor.Domain.Models;

namespace SchemeEditor.API.Models.Account
{
	public class UserModel
	{
		public long Id { get; set; }
		public string Email { get; set; }
		public bool EmailConfirmed { get; set; }
		public string Phone { get; set; }
		public bool PhoneConfirmed { get; set; }
		public string Name { get; set; }
		public string LastName { get; set; }
		public string MiddleName { get; set; }
		public string City { get; set; }
		public string Company  { get; set; }
		public string Password { get; set; }
		public string Position { get; set; }
		public bool IsBlocked { get; set; }
		public Role Role { get; set; }
	}
}