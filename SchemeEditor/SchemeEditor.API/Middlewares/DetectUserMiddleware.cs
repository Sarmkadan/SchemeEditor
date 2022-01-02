using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;
using SchemeEditor.Identity.Infrastructure;

namespace SchemeEditor.API.Middlewares
{
	public class DetectUserMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _contextAccessor;

		public DetectUserMiddleware(RequestDelegate next, UserManager<User> userManager, IHttpContextAccessor contextAccessor)
		{
			this._next = next;
			_userManager = userManager;
			_contextAccessor = contextAccessor;
		}

		public async Task InvokeAsync(HttpContext context, IExecutionContext<User> executionContext)
		{
			var userName = this._contextAccessor.HttpContext.User.Identity.UserName();
			if (!string.IsNullOrEmpty(userName))
			{
				var user = await this._userManager.FindByEmailAsync(userName);
				executionContext.SetUser(user);
			}
			await _next.Invoke(context);
		}
	}
}