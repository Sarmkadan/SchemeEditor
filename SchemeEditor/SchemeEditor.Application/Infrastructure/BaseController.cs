using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.Application.Infrastructure
{
	[Produces("application/json")]
	[Route("api/[controller]")]
	[ApiController]
	public class BaseController : ControllerBase
	{
		protected readonly IExecutionContext<User> ExecutionContext;
		protected readonly UserManager<User> UserManager;
		public BaseController(IExecutionContext<User> executionContext, UserManager<User> userManager)
		{
			ExecutionContext = executionContext;
			UserManager = userManager;
		}
	}
}