using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.API.Middlewares
{
	public class BlockedUserFilterMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly SignInManager<User> _signInManager;

		public BlockedUserFilterMiddleware(RequestDelegate next, SignInManager<User> signInManager)
		{
			this._next = next;
			_signInManager = signInManager;
		}

		public async Task InvokeAsync(HttpContext context, IExecutionContext<User> executionContext)
		{
			if (executionContext.User != null && executionContext.User.IsBlocked)
			{
				await this._signInManager.SignOutAsync();
			}
			await _next.Invoke(context);
		}
	}
}