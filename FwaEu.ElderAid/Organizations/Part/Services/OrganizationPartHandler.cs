using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Organizations.Part.Services
{
	public class OrganizationPartHandler : EditablePartHandler<OrganizationPartModel, OrganizationPartModel>
	{
		public const string PartName = "Organizations";
		public override string Name => PartName;

		public OrganizationPartHandler(
			UserSessionContext userSessionContext,
			CurrentUserPermissionService currentUserPermissionService)
		{
			this._userSessionContext = userSessionContext
				?? throw new ArgumentNullException(nameof(userSessionContext));

			this._currentUserPermissionService = currentUserPermissionService
				?? throw new ArgumentNullException(nameof(currentUserPermissionService));
		}

		private readonly UserSessionContext _userSessionContext;
		private readonly CurrentUserPermissionService _currentUserPermissionService;

		public override async Task<OrganizationPartModel> LoadAsync()
		{
			var repositorySession = this._userSessionContext.SessionContext.RepositorySession;
			var user = this._userSessionContext.LoadingModel;

			var ids = await repositorySession
				.Create<OrganizationUserLinkEntityRepository>()
				.QueryByUserId(user.Id)
				.Select(ur => ur.Organization.Id)
				.ToListAsync();

			return new OrganizationPartModel()
			{
				SelectedIds = ids.ToArray(),
			};
		}

		public override Task<IPartSaveResult> SaveAsync(OrganizationPartModel model)
		{
			return Task.FromResult<IPartSaveResult>(new PartSaveResult(afterSaveTask: async () =>
			{

				var repositorySession = this._userSessionContext.SessionContext.RepositorySession;
				var user = this._userSessionContext.SaveUserEntity;

				var userOrganizationRepository = repositorySession.Create<OrganizationUserLinkEntityRepository>();
				var organizationRepository = repositorySession.Create<AdminOrganizationEntityRepository>();

				var currentSelectedIds = await userOrganizationRepository.QueryByUserId(user.Id)
					.Select(up => up.Organization.Id).ToListAsync();

				var idsToDelete = currentSelectedIds.Except(model.SelectedIds).ToArray();
				var idsToCreate = model.SelectedIds.Except(currentSelectedIds).ToArray();

				if (idsToDelete.Any())
				{
					await userOrganizationRepository.Query()
						.Where(ur => idsToDelete.Contains(ur.Organization.Id))
						.DeleteAsync(CancellationToken.None);
				}

				foreach (var idToCreate in idsToCreate)
				{
					var organization = await organizationRepository.GetAsync(idToCreate);

					await userOrganizationRepository.SaveOrUpdateAsync(new OrganizationUserLinkEntity()
					{
						User = user,
						Organization = organization,
					});
				}
			}));
		}
		public override Task<bool> CurrentUserCanEditAsync()
		{
			var currentUser = this._currentUserPermissionService.CurrentUserService.User;

			return Task.FromResult(currentUser.Entity.IsAdmin);
		}
	}
}
