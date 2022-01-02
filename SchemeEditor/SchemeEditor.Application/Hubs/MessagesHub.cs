using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using SchemeEditor.Application.Infrastructure;
using SchemeEditor.Domain.Models;
using SchemeEditor.Identity.Infrastructure;

namespace SchemeEditor.Application.Hubs
{
	public class MessagesHub: Hub
	{
		private readonly UserManager<User> _userManager;

		public MessagesHub(UserManager<User> userManager)
		{
			_userManager = userManager;
		}

		public async Task Send(string message)
		{
			await Clients.All.SendAsync("Send", message);
		}

		public override async Task OnConnectedAsync()
		{
			var userId = await this.GetUserId();
			ConnectedUsersStore.Store.AddOrUpdate(userId, this.Context.ConnectionId, (x, y) => this.Context.ConnectionId);
			await base.OnConnectedAsync();
		}

		public override async Task OnDisconnectedAsync(Exception exception)
		{
			var userId = await this.GetUserId();
			ConnectedUsersStore.Store.TryRemove(userId, out _);
			await base.OnDisconnectedAsync(exception);
		}

		private async Task<long> GetUserId()
		{
			var userName = this.Context.User.Identity.UserName();
			return (await this._userManager.FindByEmailAsync(userName)).Id;
		}
	}
}