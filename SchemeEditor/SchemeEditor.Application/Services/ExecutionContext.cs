using System;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Application.Services
{
	public class ExecutionContext: IExecutionContext<User>
	{
		private readonly IHttpContextAccessor _httpContextAccessor;
		private readonly IConfiguration _configuration;

		public ExecutionContext(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
		{
			_httpContextAccessor = httpContextAccessor;
			_configuration = configuration;
			this.Now = DateTime.UtcNow;
		}

		public User User { get; set; }
		public HttpContext HttpContext => this._httpContextAccessor.HttpContext;
		public IConfiguration Configuration => this._configuration;
		public DateTime Now { get; private set; }
		public void SetUser(User user)
		{
			this.User = user;
		}
	}
}
