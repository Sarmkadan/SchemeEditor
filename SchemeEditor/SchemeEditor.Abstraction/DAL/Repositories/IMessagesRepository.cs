using System.Collections.Generic;
using System.Threading.Tasks;
using SchemeEditor.Abstraction.Application.Models;
using SchemeEditor.Domain.Models;

namespace SchemeEditor.Abstraction.DAL.Repositories
{
	public interface IMessagesRepository
	{
		Task<IEnumerable<MessageUser>> List(MessagesFilter filter);
		Task<IEnumerable<Message>> History(MessagesFilter filter);
		Task<Message> Get(long id);
		Task<Message> Create(Message message);
		Task<Message> CreateMessageUser(Message message, long userId);
		Task<MessageUser> UpdateUserMessage(MessageUser messageUser);
		Task Delete(long id);
		Task<int> HistoryCount(MessagesFilter filter);
	}
}