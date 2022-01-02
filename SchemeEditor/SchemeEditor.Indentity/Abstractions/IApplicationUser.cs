using System.Collections.Generic;

namespace SchemeEditor.Identity.Abstractions
{
	public interface IApplicationUser
	{
		long Id { get; set; }
		string PasswordHash { get; set; }
		string Email { get; set; }
		bool EmailConfirmed { get; set; }
		string NormalizedEmail { get; set; }
		string Phone { get; set; }
		bool PhoneConfirmed { get; set; }
		IApplicationRole Role { get; }
	}

}