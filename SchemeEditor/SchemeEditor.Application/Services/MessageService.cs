using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using SchemeEditor.Abstraction.Application.Models;
using SchemeEditor.Abstraction.Application.Services;
using SchemeEditor.Abstraction.DAL.Repositories;
using SchemeEditor.Application.Hubs;
using SchemeEditor.Application.Infrastructure;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Application.Services
{
	public class MessageService: IMessageService
	{
		private readonly IMessagesRepository _messagesRepository;
		private readonly IHubContext<MessagesHub> _messagesHub;

		public MessageService(IMessagesRepository messagesRepository, IHubContext<MessagesHub> messagesHub)
		{
			_messagesRepository = messagesRepository;
			_messagesHub = messagesHub;
		}

		public async Task<IEnumerable<MessageUser>> List(MessagesFilter filter)
		{
			return await this._messagesRepository.List(filter);
		}

		public async Task<IEnumerable<Message>> History(MessagesFilter filter)
		{
			return await this._messagesRepository.History(filter);
		}

		public async Task<Message> Get(long id)
		{
			return await this._messagesRepository.Get(id);
		}

		public async Task<Message> Create(Message message)
		{
			var newMessage = await this._messagesRepository.Create(message);
			foreach (var messageMessageUser in message.MessageUsers)
			{
				newMessage = await this._messagesRepository.CreateMessageUser(newMessage, messageMessageUser.UserId);
			}

			var connections = ConnectedUsersStore.Store.Where(x => message.MessageUsers.Any(y => y.UserId == x.Key)).Select(x => x.Value).ToList();
			await this._messagesHub.Clients.Clients(connections).SendAsync("Send", message.Title);

			return newMessage;
		}

		public async Task<MessageUser> SetIsRead(MessageUser messageUser)
		{
			messageUser.IsRead = true;
			return await this._messagesRepository.UpdateUserMessage(messageUser);
		}

		public async Task Delete(long id)
		{
			await this._messagesRepository.Delete(id);
		}

		public async Task<int> HistoryCount(MessagesFilter filter)
		{
			return await this._messagesRepository.HistoryCount(filter);
		}
	}
}