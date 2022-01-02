using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace SchemeEditor.Identity.Abstractions
{
	public interface IExecutionContext<TUser> where TUser: class, IApplicationUser
	{
		TUser User { get; }
		HttpContext HttpContext { get; }
		IConfiguration Configuration { get; }
		DateTime Now { get; }
		void SetUser(TUser user);
	}
}
