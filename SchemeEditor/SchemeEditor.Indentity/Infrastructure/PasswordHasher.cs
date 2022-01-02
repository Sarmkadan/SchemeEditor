using System;
using System.Text;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SchemeEditor.Identity.Infrastructure
{
	public class PasswordHasher
	{
		private const string Salt = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9";
		public static string HashPassword(string password)
		{
			var saltBytes = Encoding.UTF8.GetBytes(Salt);
			return Convert.ToBase64String(KeyDerivation.Pbkdf2(
				password: password,
				salt: saltBytes,
				prf: KeyDerivationPrf.HMACSHA1,
				iterationCount: 10000,
				numBytesRequested: 256 / 8));
		}

		public static bool Verify(string hash, string password)
		{
			var newHash = HashPassword(password);
			return newHash.Equals(hash);
		}
	}
}