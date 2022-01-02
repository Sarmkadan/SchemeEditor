using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Identity.Infrastructure
{
	public class RoleStore<TUser, TRole>: IRoleStore<TRole>
		where TUser: IApplicationUser
		where TRole: class, IApplicationRole
	{
		private readonly IAccountService<TUser, TRole> _service;
		public RoleStore(IAccountService<TUser, TRole> service)
		{
			_service = service;
		}

		public void Dispose()
		{
			
		}

		public async Task<IdentityResult> CreateAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await _service.CreateRoleAsync(role);
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> UpdateAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await _service.UpdateRoleAsync(role);
			return IdentityResult.Success;
		}

		public async Task<IdentityResult> DeleteAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await _service.DeleteRoleAsync(role.Id);
			return IdentityResult.Success;
		}

		public async Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return role.Id.ToString();
		}

		public async Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return role.Name;
		}

		public async Task SetRoleNameAsync(TRole role, string roleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			role.Name = roleName;
			await _service.UpdateRoleAsync(role);
		}

		public async Task<string> GetNormalizedRoleNameAsync(TRole role, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			await Task.Delay(1, cancellationToken);
			return role.NormalizedName;
		}

		public async Task SetNormalizedRoleNameAsync(TRole role, string normalizedName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			role.NormalizedName = normalizedName;
			await _service.UpdateRoleAsync(role);
		}

		public async Task<TRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await _service.GetRole(long.Parse(roleId));
		}

		public async Task<TRole> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			return await _service.FindRole(normalizedRoleName);
		}
	}
}