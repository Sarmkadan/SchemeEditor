using Microsoft.AspNetCore.Identity;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Identity.Infrastructure
{
	public class IdentityPasswordHasher<TUser>: IPasswordHasher<TUser> where TUser: class
	{
		public string HashPassword(TUser user, string password)
		{
			return PasswordHasher.HashPassword(password);
		}

		public PasswordVerificationResult VerifyHashedPassword(TUser user, string hashedPassword, string providedPassword)
		{
			return PasswordHasher.Verify(hashedPassword, providedPassword)
				? PasswordVerificationResult.Success
				: PasswordVerificationResult.Failed;
		}
	}
}