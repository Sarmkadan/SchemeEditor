using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Microsoft.AspNetCore.Http;
using SchemeEditor.Abstraction.DAL.Services;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;
using SchemeEditor.Identity.Infrastructure;

namespace SchemeEditor.DAL.Repositories
{
	public class AccountRepository: IAccountService<User, Role>
	{
		private readonly IConnectionService<SchemeEditorContext> _connectionService;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IExecutionContext<User> _executionContext;

		public AccountRepository(IConnectionService<SchemeEditorContext> connectionService, IHttpContextAccessor httpContextAccessor, IExecutionContext<User> executionContext)
		{
			_connectionService = connectionService;
			_httpContextAccessor = httpContextAccessor;
			_executionContext = executionContext;
		}

		public void Dispose()
		{
		}

		public async Task<User> Get(long userId)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var user = await context.Users.LoadWith(x => x.UserRoles).FirstOrDefaultAsync(x => x.Id == userId);
				if (user == null)
				{
					return null;
				}
				var roles = await context.UserRoles.LoadWith(x => x.Role).Where(x => x.UserId == userId).ToListAsync();
				user.UserRoles = roles;
				return user;
			}
		}

		public async Task<User> GetByPhone(string phone)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var user = await context.Users.LoadWith(x => x.UserRoles).FirstOrDefaultAsync(x => x.Phone == phone);
				if (user == null)
				{
					return null;
				}
				user.UserRoles = await context.UserRoles.LoadWith(x => x.Role).Where(x => x.UserId == user.Id).ToListAsync();
				return user;
			}
		}

		public async Task<User> Find(string userName)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var user = await context.Users.LoadWith(x => x.UserRoles).FirstOrDefaultAsync(x => x.NormalizedEmail == userName);
				if (user == null)
				{
					return null;
				}
				user.UserRoles = await context.UserRoles.LoadWith(x => x.Role).Where(x => x.UserId == user.Id).ToListAsync();
				return user;
			}
		}

		public async Task<User> FindByPhone(string phone)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var user = await context.Users.LoadWith(x => x.UserRoles).FirstOrDefaultAsync(x => x.Phone == phone);
				if (user == null)
				{
					return null;
				}
				user.UserRoles = await context.UserRoles.LoadWith(x => x.Role).Where(x => x.UserId == user.Id).ToListAsync();
				return user;
			}
		}

		public async Task<User> CreateAsync(User user)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var currentUser = await GetCurrentUser();
				user.CreatedAt = this._executionContext.Now;
				user.CreatedBy = currentUser?.Id ?? 1;
				user.ModifiedAt = this._executionContext.Now;
				user.CreatedBy = currentUser?.Id ?? 1;
				user.NormalizedEmail = user.Email.ToUpper();
				user.PasswordHash = PasswordHasher.HashPassword(user.PasswordHash);
                
				user.Id = await context.InsertWithInt64IdentityAsync(user);
				return user;
			}
		}

		public async Task<User> UpdateAsync(long userId, User user, string newPassword = null)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var currentUser = await this.Get(userId);
				if (currentUser == null)
				{
					throw new KeyNotFoundException("Обновляемый пользователь не зарегистрирован");
				}
				user.CreatedAt = currentUser.CreatedAt;
				user.CreatedBy = currentUser.CreatedBy;
				user.ModifiedAt = this._executionContext.Now;
				user.ModifiedBy = currentUser.Id;
				user.NormalizedEmail = user.Email.ToUpper();

				user.PasswordHash = string.IsNullOrWhiteSpace(newPassword)
					? currentUser.PasswordHash
					: PasswordHasher.HashPassword(newPassword);
				await context.UpdateAsync(user);
				return await this.Get(userId);
			}
		}

		public async Task DeleteAsync(long userId)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				await context.UserRoles.DeleteAsync(x => x.UserId == userId);
				await context.Schemes.DeleteAsync(x => x.CreatedBy == userId);
                await context.UserMessages.DeleteAsync(x => x.UserId == userId || x.CreatedBy == userId);
				await context.Users.DeleteAsync(s => s.Id == userId);
			}
		}

		public async Task<Role> GetRole(long roleId)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				return await context.Roles.LoadWith(x => x.UserRoles).FirstOrDefaultAsync(x => x.Id == roleId);
			}
		}

		public async Task<Role> FindRole(string roleName)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				return await context.Roles.LoadWith(x => x.UserRoles).FirstOrDefaultAsync(x => x.NormalizedName == roleName);
			}
		}

		public async Task<Role> CreateRoleAsync(Role role)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var currentUser = await GetCurrentUser();
				role.CreatedAt = this._executionContext.Now;
				role.CreatedBy = currentUser?.Id ?? 1;
				role.ModifiedAt = this._executionContext.Now;
				role.ModifiedBy = currentUser?.Id ?? 1;
                
				role.Id = await context.InsertWithInt64IdentityAsync(role);
				return role;
			}
		}

		public async Task<Role> UpdateRoleAsync(Role role)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var currentUser = await GetCurrentUser();
				role.ModifiedAt = this._executionContext.Now;
				role.ModifiedBy = currentUser?.Id ?? 1;
				await context.UpdateAsync(role);
				return role;
			}
		}

		public async Task DeleteRoleAsync(long roleId)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				await context.Roles.DeleteAsync(s => s.Id == roleId);
			}
		}

		public async Task AddToRoleAsync(User user, string roleName)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var currentUser = await GetCurrentUser();
				var role = await context.Roles.LoadWith(x => x.UserRoles).FirstOrDefaultAsync(x => x.NormalizedName == roleName);
				var userRole = new UserRole
				{
					CreatedAt = this._executionContext.Now,
					CreatedBy = currentUser?.Id ?? 1,
					ModifiedAt = this._executionContext.Now,
					ModifiedBy = currentUser?.Id ?? 1,
					User = user,
					Role = role,
					UserId = user.Id,
					RoleId = role.Id
				};
				role.Id = await context.InsertWithInt64IdentityAsync(userRole);
				var roles = user.UserRoles.ToList();
				roles.Add(userRole);
				user.UserRoles = roles;
			}
		}

		public async Task RemoveFromRoleAsync(User user, string roleName)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				await context.UserRoles.LoadWith(x => x.Role).DeleteAsync(x => x.UserId == user.Id && x.Role.NormalizedName == roleName);
				user.UserRoles = user.UserRoles.Where(x => x.Role.NormalizedName != roleName);
			}
		}

		public async Task<IList<string>> GetRolesAsync(User user)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				return await context.UserRoles.LoadWith(x => x.Role).Where(x => x.UserId == user.Id).Select(x => x.Role.NormalizedName).ToListAsync();
			}
		}

		public async Task<IList<Role>> GetAllRolesAsync()
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				return await context.Roles.ToListAsync();
			}
		}

		public async Task<bool> IsInRoleAsync(User user, string roleName)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				return await context.UserRoles.LoadWith(x => x.Role).Where(x => x.UserId == user.Id).AnyAsync(x => x.Role.NormalizedName == roleName);
			}
		}

		public async Task<IList<User>> GetUsersInRoleAsync(string roleName)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				return await context.UserRoles.LoadWith(x => x.Role).Where(x => x.Role.NormalizedName == roleName).Select(x => x.User).ToListAsync();
			}
		}

		public async Task<IList<User>> GetFilteredUsers(Func<IQueryable<User>, IQueryable<User>> func)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var users = await func(context.Users.LoadWith(x => x.UserRoles).Where(x => x.Id != 1).AsQueryable()).OrderBy(x => x.Id).ToListAsync();
				return await Task.WhenAll(users.Select(async x => await Get(x.Id)));
			}
		}

		public async Task<int> GetFilteredUsersCount(Func<IQueryable<User>, IQueryable<User>> func)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				return await func(context.Users.LoadWith(x => x.UserRoles).Where(x => x.Id != 1).AsQueryable()).CountAsync();
			}
		}

		public async Task ClearUserRoles(User user)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				await context.UserRoles.DeleteAsync(x => x.UserId == user.Id);
			}
		}

		private async Task<User> GetCurrentUser()
		{
			return await this.Find(this._httpContextAccessor.HttpContext.User.Identity.Name);
		}
	}
}
