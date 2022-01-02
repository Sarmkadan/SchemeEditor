using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Identity.Infrastructure
{
	public class UserStore<TUser, TRole>: IUserStore<TUser>,
		IUserEmailStore<TUser>,
		IUserPhoneNumberStore<TUser>,
		IUserTwoFactorStore<TUser>,
		IUserPasswordStore<TUser>,
		IUserRoleStore<TUser>,
		IPasswordValidator<TUser>
		where TUser: class, IApplicationUser
		where TRole: IApplicationRole
	{
		private readonly IAccountService<TUser, TRole> _service;
		public UserStore(IAccountService<TUser, TRole> service)
		{
			_service = service;
		}

		public void Dispose()
		{
			
		}


		public async Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.Id.ToString();
		}

		public async Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.Email;
		}

		public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			user.Email = userName;
			if (user.Id != 0)
			{
				await _service.UpdateAsync(user.Id, user);
			}
		}

		public async Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.NormalizedEmail;
		}

		public async Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			user.NormalizedEmail = normalizedName;
			if (user.Id != 0)
			{
				await _service.UpdateAsync(user.Id, user);
			}
		}

		public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await _service.CreateAsync(user);
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await _service.UpdateAsync(user.Id, user);
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await _service.DeleteAsync(user.Id);
			return IdentityResult.Success;
		}

		public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await _service.Get(long.Parse(userId));
		}

		public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await _service.Find(normalizedUserName);
		}

		public async Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			user.Email = email;
			if (user.Id != 0)
			{
				await _service.UpdateAsync(user.Id, user);
			}
		}

		public async Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.Email;
		}

		public async Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.EmailConfirmed;
		}

		public async Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			user.EmailConfirmed = confirmed;
			if (user.Id != 0)
			{
				await _service.UpdateAsync(user.Id, user);
			}
		}

		public async Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return await _service.Find(normalizedEmail);
		}

		public async Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.NormalizedEmail;
		}

		public async Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			user.NormalizedEmail = normalizedEmail;
			if (user.Id != 0)
			{
				await _service.UpdateAsync(user.Id, user);
			}
		}

		public async Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			user.Phone = phoneNumber;
			if (user.Id != 0)
			{
				await _service.UpdateAsync(user.Id, user);
			}
		}

		public async Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.Phone;
		}

		public async Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.PhoneConfirmed;
		}

		public async Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			user.PhoneConfirmed = confirmed;
			if (user.Id != 0)
			{
				await _service.UpdateAsync(user.Id, user);
			}
		}

		public async Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
		}

		public async Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return false;
		}

		public async Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			user.PasswordHash = passwordHash;
			if (user.Id != 0)
			{
				await _service.UpdateAsync(user.Id, user);
			}
		}

		public async Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return user.PasswordHash;
		}

		public async Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return !string.IsNullOrEmpty(user.PasswordHash);
		}

		public async Task AddToRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await _service.AddToRoleAsync(user, roleName);
		}

		public async Task RemoveFromRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await _service.RemoveFromRoleAsync(user, roleName);
		}

		public async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await _service.GetRolesAsync(user);
		}

		public async Task<bool> IsInRoleAsync(TUser user, string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await _service.IsInRoleAsync(user, roleName);
		}

		public async Task<IList<TUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await _service.GetUsersInRoleAsync(roleName);
		}

		public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
		{
			var hash = await this.GetPasswordHashAsync(user, CancellationToken.None);
			return PasswordHasher.Verify(hash, password)
				? IdentityResult.Success
				: IdentityResult.Failed();
		}
	}
}