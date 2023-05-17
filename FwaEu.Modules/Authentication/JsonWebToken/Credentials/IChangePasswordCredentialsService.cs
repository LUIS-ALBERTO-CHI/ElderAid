using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FwaEu.Modules.Authentication.JsonWebToken.Credentials
{
	public interface IChangePasswordCredentialsService
	{
		Task ChangePasswordAsync(string identity, string newPassword);
	}

	public class DefaultChangePasswordCredentialsService : IChangePasswordCredentialsService
	{
		public DefaultChangePasswordCredentialsService(
			UserSessionContext userSessionContext,
			IPasswordHasher passwordHasher,
			IAuthenticationChangeInfoService authenticationChangeInfoService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._passwordHasher = passwordHasher
				?? throw new ArgumentNullException(nameof(passwordHasher));

			this._authenticationChangeInfoService = authenticationChangeInfoService
				?? throw new ArgumentNullException(nameof(authenticationChangeInfoService));
		}

		private readonly UserSessionContext _userSessionContext;
		private readonly IPasswordHasher _passwordHasher;
		private readonly IAuthenticationChangeInfoService _authenticationChangeInfoService;

		public async Task ChangePasswordAsync(string identity, string newPassword)
		{
			var repositorySession = this._userSessionContext.SessionContext.RepositorySession;
			var user = this._userSessionContext.SaveUserEntity;

			if (user.Identity != identity)
			{
				throw new NotSupportedException(); // NOTE: Identity is not required in this implementation because we use SessionContext
			}

			var repository = repositorySession.Create<UserCredentialsEntityRepository>();
			var entity = (await repository.GetByUserIdAsync(user.Id))
				?? new UserCredentialsEntity() { User = user };

			if (newPassword != null)
			{
				if (newPassword.Length < 4)
				{
					throw new UserSaveValidationException("PasswordMustBeAtLeast4Chars",
						"The password must contain at least 4 chars.");
				}

				var newPasswordHash = this._passwordHasher.Hash(newPassword);
				if (entity.PasswordHash != newPasswordHash)
				{
					entity.PasswordHash = newPasswordHash;
					await repository.SaveOrUpdateAsync(entity);
					await this._authenticationChangeInfoService.SetLastChangeDateAsync(user.Id);
				}
			}
		}
	}
}
