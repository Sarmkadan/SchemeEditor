using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchemeEditor.Identity.Abstractions
{
	public interface IAccountService<TUser, TRole>: IDisposable
		where TUser: IApplicationUser
		where TRole: IApplicationRole
	{
		Task<TUser> Get(long userId);
		Task<TUser> GetByPhone(string phone);
		Task<TUser> Find(string userName);
		Task<TUser> CreateAsync(TUser user);
		Task<TUser> UpdateAsync(long userId, TUser user, string newPassword = null);
		Task DeleteAsync(long userId);
		Task<TUser> FindByPhone(string phone);

		Task<TRole> GetRole(long roleId);
		Task<TRole> FindRole(string roleName);
		Task<TRole> CreateRoleAsync(TRole role);
		Task<TRole> UpdateRoleAsync(TRole role);
		Task DeleteRoleAsync(long roleId);

		Task AddToRoleAsync(TUser user, string roleName);
		Task RemoveFromRoleAsync(TUser user, string roleName);
		Task<IList<string>> GetRolesAsync(TUser user);
		Task<IList<TRole>> GetAllRolesAsync();
		Task<bool> IsInRoleAsync(TUser user, string roleName);
		Task<IList<TUser>> GetUsersInRoleAsync(string roleName);
		Task<IList<TUser>> GetFilteredUsers(Func<IQueryable<TUser>, IQueryable<TUser>> func);
		Task<int> GetFilteredUsersCount(Func<IQueryable<TUser>, IQueryable<TUser>> func);
		Task ClearUserRoles(TUser user);
	}
}