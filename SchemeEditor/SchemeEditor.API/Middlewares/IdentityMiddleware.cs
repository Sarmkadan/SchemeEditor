using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;
using SchemeEditor.Identity.Infrastructure;

namespace SchemeEditor.API.Middlewares
{
	public class IdentityMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly IConfiguration _configuration;

		public IdentityMiddleware(RequestDelegate next, IConfiguration configuration)
		{
			this._next = next;
			_configuration = configuration;
		}

		public async Task InvokeAsync(HttpContext context, IExecutionContext<User> executionContext)
		{
			var tokenElement = context.Request.Headers["Authorization"];
			if (tokenElement != StringValues.Empty)
			{
				var token = string.Join("", tokenElement.ToArray()).Replace("Bearer ", "");
				context.Response.Headers.RefreshToken(executionContext.User, _configuration, token);
			}
			await _next.Invoke(context);
		}
	}
}