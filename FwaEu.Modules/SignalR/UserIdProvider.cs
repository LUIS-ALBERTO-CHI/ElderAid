using FwaEu.Fwamework.Users;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.SignalR
{
	public class UserIdProvider : Microsoft.AspNetCore.SignalR.IUserIdProvider
	{
		private readonly IIdentityResolver _identityResolver;

		public UserIdProvider(IIdentityResolver identityResolver)
		{
			this._identityResolver = identityResolver
				?? throw new ArgumentNullException(nameof(identityResolver));
		}

		public string GetUserId(HubConnectionContext connection)
		{
			return this._identityResolver.ResolveIdentity(connection.User);
		}
	}
}
