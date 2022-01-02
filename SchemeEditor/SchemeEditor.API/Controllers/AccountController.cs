using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SchemeEditor.API.Models.Account;
using SchemeEditor.Application.Infrastructure;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;
using SchemeEditor.Identity.Infrastructure;
using SchemeEditor.Notifications;
using SchemeEditor.Notifications.Abstractions;

namespace SchemeEditor.API.Controllers
{
	public class AccountController : BaseController
	{
		private readonly IAccountService<User, Role> _accountRepository;
		private readonly INotificationService _notificationService;
		private readonly SignInManager<User> _signInManager;
		public AccountController(IExecutionContext<User> context, IAccountService<User, Role> accountRepository, INotificationService notificationService, UserManager<User> userManager, SignInManager<User> signInManager)
			: base(context, userManager)
		{
			_accountRepository = accountRepository;
			_notificationService = notificationService;
			_signInManager = signInManager;
		}

		[HttpPost("login")]
        [ProducesResponseType(200)]
		public async Task<ActionResult<UserModel>> Login(LoginModel model)
		{
			var user = await this.UserManager.FindByNameAsync(model.Login) ?? await this._accountRepository.GetByPhone(model.Login);
			if (user == null)
				return Unauthorized();

			if (user.IsBlocked || !await this.LoginUser(user.Email, model.Password, model.Remember))
				return Unauthorized();

			user.EmailConfirmed = true;
			user = await this._accountRepository.UpdateAsync(user.Id, user.Adapt<User>());
			this.HttpContext.Response.Headers.Add("JwtToken", user.GetJwtToken(this.ExecutionContext.Configuration));
			return Ok(user.Adapt<UserModel>());
		}

		[HttpGet("logout")]
		[ProducesResponseType(200)]
		[Authorize]
		public async Task<ActionResult<UserModel>> Logout()
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				await this._signInManager.SignOutAsync();
			}
			return Ok();
		}

		[HttpGet("validate-password")]
		[ProducesResponseType(200)]
		[Authorize]
		public async Task<ActionResult<bool>> ValidatePassword(string oldPassword)
		{
			var passwordIsValid = await this.UserManager.CheckPasswordAsync(this.ExecutionContext.User, oldPassword);
			return Ok(passwordIsValid);
		}

		[HttpPut("reset-password")]
		[ProducesResponseType(200)]
		[Authorize("ADMINISTRATOR")]
		public async Task<ActionResult<bool>> ResetPassword(long userId)
		{
			var user = await this.UserManager.FindByIdAsync(userId.ToString());
			await this._accountRepository.UpdateAsync(userId, user, user.Email.GeneratePassword());
			await this._accountRepository.UpdateAsync(userId, user, "111"); //TODO: удалить
			await this._notificationService.Send(new FakeNotification { Subject = "Пароль был сброшен", Payload = user.PasswordHash, To = user.Phone });
			return Ok(user.Adapt<UserModel>());
		}

		[HttpPut("forgot-password")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<object>> ForgotPassword(string login)
		{
			if (string.IsNullOrEmpty(login))
			{
				return this.ValidationProblem(new ValidationProblemDetails(new Dictionary<string, string[]>
				{
					{"email", new [] {"Нужно запомнить Email"}}
				}));
			}
			var user = await this.UserManager.FindByEmailAsync(login) ?? await this._accountRepository.GetByPhone(login);
			if (user == null)
				return Ok();

			await this._accountRepository.UpdateAsync(user.Id, user, user.Email.GeneratePassword());
			await this._accountRepository.UpdateAsync(user.Id, user, "111"); //TODO: удалить
			await this._notificationService.Send(new FakeNotification {Subject = "Пароль был сброшен", Payload = user.PasswordHash, To = user.Phone});
			return Ok();
		}

		[HttpPut("update")]
		[ProducesResponseType(200)]
		[Authorize]
		public async Task<ActionResult<UserModel>> Update(UserModel model)
		{
			var user = await this._accountRepository.UpdateAsync(model.Id, model.Adapt<User>(), model.Password);
			return Ok(user.Adapt<UserModel>());
		}

		[HttpGet("current")]
		[ProducesResponseType(200)]
		[Authorize]
		public async Task<ActionResult<UserModel>> Current()
		{
			await Task.Delay(1);
			return Ok(this.ExecutionContext.User.Adapt<UserModel>());
		}

		[HttpGet("confirm-email")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<bool>> ConfirmEmail(string email, string token)
		{
			var user = await this.UserManager.FindByNameAsync(email);
			if (user == null)
				return NotFound();

			var result = user.ConfirmTokenIsValid(ExecutionContext.Configuration, token);
			user.EmailConfirmed = result;
			if (user.EmailConfirmed)
			{
				await this._accountRepository.UpdateAsync(user.Id, user.Adapt<User>());
			}
			return Ok(result);
		}

		[HttpPost("register")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<UserModel>> Register(RegisterModel registerModel)
		{
			if (HttpContext.User.Identity.IsAuthenticated)
			{
				var modelStateDictionary = new ModelStateDictionary();
				modelStateDictionary.AddModelError("", "Вы сначала должны выйти");
				return ValidationProblem(modelStateDictionary);
			}
			if (!ModelState.IsValid)
			{
				return ValidationProblem(ModelState);
			}

			if ((await this.UserManager.FindByNameAsync(registerModel.Email)) != null) {
				var modelStateDictionary = new ModelStateDictionary();
				modelStateDictionary.AddModelError("Email", "Пользователь с таким адресом уже зарегистрирован");
				return ValidationProblem(modelStateDictionary);
			}
			if ((await this._accountRepository.FindByPhone(registerModel.Phone)) != null)
			{
				var modelStateDictionary = new ModelStateDictionary();
				modelStateDictionary.AddModelError("Phone", "Пользователь с таким телефоном уже зарегистрирован");
				return ValidationProblem(modelStateDictionary);
			}
			var userModel = registerModel.Adapt<User>();
			userModel.PasswordHash = registerModel.Email.GeneratePassword();
			userModel.PasswordHash = "111"; //TODO: удалить
			if (!(await this.UserManager.CreateAsync(userModel)).Succeeded)
				return StatusCode(520);

			var user = await this.UserManager.FindByNameAsync(userModel.Email);
			await this.UserManager.AddToRoleAsync(user, "Not Active");
			var url = Url.Action("ConfirmEmail", "Account", new
			{
				email = user.Email,
				token = user.GetJwtToken(ExecutionContext.Configuration)
			});
			await this._notificationService.Send(new FakeNotification{Subject = "Подтвердите Email", Payload = url, To = user.Email});
			await this._notificationService.Send(new FakeNotification{Subject = "Подтвердите Телефон", Payload = userModel.PasswordHash, To = user.Phone});
			return Ok(user.Adapt<UserModel>());

		}

		[HttpGet("roles")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
		{
			var roles = await _accountRepository.GetAllRolesAsync();
			return Ok(roles);
		}

		[HttpPut("{userId}/change-role")]
		[ProducesResponseType(200)]
		[Authorize("ADMINISTRATOR")]
		public async Task<ActionResult<UserModel>> ChangeRole(long userId, Role role)
		{
			var user = await this.UserManager.FindByIdAsync(userId.ToString());
			await this._accountRepository.ClearUserRoles(user);
			await this.UserManager.AddToRoleAsync(user, role.NormalizedName);
			return Ok(await this._accountRepository.Get(userId));
		}

		[HttpDelete("{userId}")]
		[ProducesResponseType(200)]
		[Authorize("ADMINISTRATOR")]
		public async Task<ActionResult> Delete(long userId)
		{
			await this._accountRepository.DeleteAsync(userId);
			return Ok();
		}

		[HttpGet("list")]
		[ProducesResponseType(200)]
		[Authorize("ADMINISTRATOR")]
		public async Task<ActionResult<IEnumerable<UserModel>>> GetUsers([FromQuery]UsersFilter filter)
		{
			var users = await _accountRepository.GetFilteredUsers(query =>
			{
				if (!string.IsNullOrWhiteSpace(filter.Name))
				{
					query = query.Where(x =>
						((x.Name ?? "") + (x.LastName ?? "") + (x.MiddleName ?? "")).Contains(filter.Name));
				}

				if (!string.IsNullOrWhiteSpace(filter.Company))
				{
					query = query.Where(x => x.Company.Contains(filter.Company));
				}

				if (filter.IsBlocked.HasValue)
				{
					query = query.Where(x => x.IsBlocked == filter.IsBlocked.Value);
				}

				if (filter.RoleId.HasValue)
				{
					query = query.Where(x => x.UserRoles.Any(y => y.RoleId == filter.RoleId.Value));
				}

				if (filter.SkipCurrent)
				{
					query = query.Where(x => x.Id != this.ExecutionContext.User.Id);
				}

				return query.Skip((filter.Page - 1)*filter.Take).Take(filter.Take);
			});
			return Ok(users.Adapt<IEnumerable<UserModel>>());
		}

		[HttpGet("count")]
		[ProducesResponseType(200)]
		[Authorize("ADMINISTRATOR")]
		public async Task<ActionResult<int>> GetUsersCount([FromQuery]UsersFilter filter)
		{
			var count = await _accountRepository.GetFilteredUsersCount(query =>
			{
				if (!string.IsNullOrWhiteSpace(filter.Name))
				{
					query = query.Where(x =>
						((x.Name ?? "") + (x.LastName ?? "") + (x.MiddleName ?? "")).Contains(filter.Name));
				}

				if (!string.IsNullOrWhiteSpace(filter.Company))
				{
					query = query.Where(x => x.Company.Contains(filter.Company));
				}

				if (filter.IsBlocked.HasValue)
				{
					query = query.Where(x => x.IsBlocked == filter.IsBlocked.Value);
				}

				if (filter.RoleId.HasValue)
				{
					query = query.Where(x => x.UserRoles.Any(y => y.RoleId == filter.RoleId.Value));
				}

				if (filter.SkipCurrent)
				{
					query = query.Where(x => x.Id != this.ExecutionContext.User.Id);
				}

				return query;
			});
			return Ok(count);
		}

		private async Task<bool> LoginUser(string userName, string passwordHash, bool remember = true)
		{
			return (await this._signInManager.PasswordSignInAsync(userName, passwordHash, remember, false)).Succeeded;
		}
	}
}
