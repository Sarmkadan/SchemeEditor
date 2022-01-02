using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SchemeEditor.Abstraction.Application.Models;
using SchemeEditor.Abstraction.Application.Services;
using SchemeEditor.API.Models.Messages;
using SchemeEditor.Application.Infrastructure;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.API.Controllers
{
	public class MessageController : BaseController
	{
		private readonly IMessageService _messageService;
		private readonly IAccountService<User, Role> _accountService;
		public MessageController(IExecutionContext<User> executionContext, UserManager<User> userManager, IMessageService messageService, IAccountService<User, Role> accountService) : base(executionContext, userManager)
		{
			_messageService = messageService;
			_accountService = accountService;
		}

		[HttpGet("{messageId}")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<Message>> Get(long messageId)
		{
			var message = await this._messageService.Get(messageId);
			return Ok(message);
		}

		[HttpGet("list")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<IEnumerable<MessageUser>>> List([FromQuery]MessagesFilter filter)
		{
			var messages = await this._messageService.List(filter);
			return Ok(messages);
		}

		[HttpGet("history")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<IEnumerable<Message>>> History([FromQuery]MessagesFilter filter)
		{
			var messages = await this._messageService.History(filter);
			return Ok(messages);
		}

		[HttpGet("history-count")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<int>> HistoryCount([FromQuery]MessagesFilter filter)
		{
			var count = await this._messageService.HistoryCount(filter);
			return Ok(count);
		}


		[HttpGet("count")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<int>> Count(bool? isRead)
		{
			var messages = await this._messageService.List(new MessagesFilter{IsRead = isRead, UserId = this.ExecutionContext.User.Id});
			return Ok(messages.Count());
		}

		[HttpPost]
		[ProducesResponseType(200)]
		[Authorize("ADMINISTRATOR")]
		public async Task<ActionResult<Message>> Create(Message message)
		{
			var newMessage = await this._messageService.Create(message);
			return Ok(newMessage);
		}

		[HttpPost("send")]
		[ProducesResponseType(200)]
		[Authorize("ADMINISTRATOR")]
		public async Task<ActionResult<Message>> Send(SendMessageModel model)
		{
			var message = new Message
			{
				Body = model.Text,
				Title = model.Title,
				Addresses = string.Join(", ", model.Addresses.Select(x => x.Title))
			};
			var inRoles = (await Task.WhenAll(model.Addresses.Where(x => x.Type == AddresseType.Role)
				.Select(async x => await this._accountService.GetUsersInRoleAsync(x.Name)))).SelectMany(x => x);

			var users = model.Addresses.Where(x => x.Type == AddresseType.User)
				.Where(x => inRoles.All(y => y.Id != x.Id));

			message.MessageUsers = inRoles.Select(x => new MessageUser { UserId = x.Id }).Concat(users.Select(x => new MessageUser { UserId = x.Id }))
				.GroupBy(x => x.UserId)
				.Select(x => x.First());
			var newMessage = await this._messageService.Create(message);
			return Ok(newMessage);
		}

		[HttpPut]
		[ProducesResponseType(200)]
		public async Task<ActionResult<MessageUser>> SetIsRead(MessageUser messageUser)
		{
			var result = await _messageService.SetIsRead(messageUser);
			return Ok(result);
		}

		[HttpDelete("{messageId}")]
		[ProducesResponseType(200)]
		public async Task<ActionResult<MessageUser>> Delete(long messageId)
		{
			await _messageService.Delete(messageId);
			return Ok();
		}
	}
}