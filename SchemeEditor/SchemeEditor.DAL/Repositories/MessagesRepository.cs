using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LinqToDB;
using Mapster;
using SchemeEditor.Abstraction.Application.Models;
using SchemeEditor.Abstraction.DAL.Repositories;
using SchemeEditor.Abstraction.DAL.Services;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Abstractions;

namespace SchemeEditor.DAL.Repositories
{
	public class MessagesRepository: IMessagesRepository
	{
		private readonly IConnectionService<SchemeEditorContext> _connectionService;
		private readonly IExecutionContext<User> _executionContext;

		public MessagesRepository(IConnectionService<SchemeEditorContext> connectionService, IExecutionContext<User> executionContext)
		{
			_connectionService = connectionService;
			_executionContext = executionContext;
		}

		public async Task<IEnumerable<MessageUser>> List(MessagesFilter filter)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var query = context.UserMessages.LoadWith(x => x.Message)
					.AsQueryable();

				if (filter.UserId.HasValue)
				{
					query = query.Where(x => x.UserId == filter.UserId);
				}
				if (filter.CreatedAt.HasValue)
				{
					query = query.Where(x => filter.CreatedAt.Value.Date <= x.CreatedAt).Where(x => filter.CreatedAt.Value.Date.AddDays(1) > x.CreatedAt);
				}
				if (filter.IsRead.HasValue)
				{
					query = query.Where(x => x.IsRead == filter.IsRead.Value);
				}

				query = query.OrderBy(x => x.IsRead).ThenByDescending(x => x.Id);

				if (filter.Page.HasValue && filter.Take.HasValue)
				{
					query = query.Skip((filter.Page.Value - 1) * filter.Take.Value);
				}

				if (filter.Take.HasValue)
				{
					query = query.Take(filter.Take.Value);;
				}

				return await query.ToListAsync();
			}
		}

		public async Task<IEnumerable<Message>> History(MessagesFilter filter)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var query = context.Messages.LoadWith(x => x.MessageUsers).LoadWith(x => x.Author).AsQueryable();
				if (filter.UserId.HasValue)
				{
					query = query.Where(x => x.MessageUsers.Any(y => y.UserId == filter.UserId));
				}
				if (filter.CreatedAt.HasValue)
				{
					query = query.Where(x => filter.CreatedAt.Value.Date <= x.CreatedAt).Where(x => filter.CreatedAt.Value.Date.AddDays(1) > x.CreatedAt);
				}

				query = query.OrderBy(x => x.Id);

				if (filter.Page.HasValue && filter.Take.HasValue)
				{
					query = query.Skip((filter.Page.Value - 1) * filter.Take.Value);
				}

				if (filter.Take.HasValue)
				{
					query = query.Take(filter.Take.Value);;
				}
				return await query.ToListAsync();
			}
		}

		public async Task<Message> Get(long id)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				return await context.Messages.LoadWith(x => x.MessageUsers).FirstOrDefaultAsync(x => x.Id == id);
			}
		}

		public async Task<Message> Create(Message message)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var dbModel = message.Adapt<Message>();
				dbModel.CreatedAt = this._executionContext.Now;
				dbModel.CreatedBy = this._executionContext.User.Id;
				dbModel.ModifiedAt = this._executionContext.Now;
				dbModel.ModifiedBy = this._executionContext.User.Id;
				dbModel.Id = await context.InsertWithInt64IdentityAsync(dbModel);
				return dbModel;
			}
		}

		public async Task<Message> CreateMessageUser(Message message, long userId)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var dbModel = new MessageUser
				{
					CreatedAt = this._executionContext.Now,
					CreatedBy = this._executionContext.User.Id,
					ModifiedAt = this._executionContext.Now,
					ModifiedBy = this._executionContext.User.Id,
					UserId = userId,
					MessageId = message.Id
				};
				dbModel.Id = await context.InsertWithInt64IdentityAsync(dbModel);
				var newMessage = message.Adapt<Message>();
				newMessage.MessageUsers.Append(dbModel);
				return newMessage;
			}
		}

		public async Task<MessageUser> UpdateUserMessage(MessageUser messageUser)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var dbMessageUser = await context.UserMessages.FirstAsync(x => x.Id == messageUser.Id);
				var updateModel = messageUser.Adapt<MessageUser>();
				updateModel.CreatedAt = dbMessageUser.CreatedAt;
				updateModel.CreatedBy = dbMessageUser.CreatedBy;
				updateModel.ModifiedAt = this._executionContext.Now;
				updateModel.ModifiedBy = this._executionContext.User.Id;
				await context.UpdateAsync(updateModel);
				return updateModel;
			}
		}

		public async Task Delete(long id)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				await context.UserMessages.DeleteAsync(x => x.MessageId == id);
				await context.Messages.DeleteAsync(x => x.Id == id);
			}
		}

		public async Task<int> HistoryCount(MessagesFilter filter)
		{
			using (var context = _connectionService.GetNewDefaultConnection())
			{
				var query = context.Messages.LoadWith(x => x.MessageUsers).LoadWith(x => x.Author).AsQueryable();
				if (filter.UserId.HasValue)
				{
					query = query.Where(x => x.MessageUsers.Any(y => y.UserId == filter.UserId));
				}
				if (filter.CreatedAt.HasValue)
				{
					query = query.Where(x => filter.CreatedAt.Value.Date <= x.CreatedAt).Where(x => filter.CreatedAt.Value.Date.AddDays(1) > x.CreatedAt);
				}

				query = query.OrderBy(x => x.Id);

				if (filter.Page.HasValue && filter.Take.HasValue)
				{
					query = query.Skip((filter.Page.Value - 1) * filter.Take.Value);
				}

				if (filter.Take.HasValue)
				{
					query = query.Take(filter.Take.Value); ;
				}

				return await query.CountAsync();
			}
		}
	}
}