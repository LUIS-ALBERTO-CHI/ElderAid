using FwaEu.Fwamework.Authentication;
using FwaEu.Fwamework.Permissions;
using FwaEu.Fwamework.Users;
using FwaEu.Fwamework.Users.Parts;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Users
{
    public class ApplicationUserModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Login { get; set; }

    }

    public class ApplicationUserModelPartHandler : EditablePartHandler<ApplicationUserModel, ApplicationUserModel>
    {
        public override string Name => "Application";

        private readonly UserSessionContext _userSessionContext;
        private readonly CurrentUserPermissionService _currentUserPermissionService;
        private readonly IAuthenticationChangeInfoService _authenticationChangeInfoService;
        private readonly IUserSynchronizationService _userSynchronizationService;
        private readonly ILogger _logger;

        public ApplicationUserModelPartHandler(UserSessionContext userSessionContext,
            CurrentUserPermissionService currentUserPermissionService,
            IAuthenticationChangeInfoService authenticationChangeInfoService,
            IUserSynchronizationService userSynchronizationService,
            ILoggerFactory loggerFactory)
        {
            this._userSessionContext = userSessionContext
                ?? throw new ArgumentNullException(nameof(userSessionContext));

            this._currentUserPermissionService = currentUserPermissionService
                ?? throw new ArgumentNullException(nameof(currentUserPermissionService));

            this._authenticationChangeInfoService = authenticationChangeInfoService
                ?? throw new ArgumentNullException(nameof(authenticationChangeInfoService));

            this._userSynchronizationService = userSynchronizationService
                ?? throw new ArgumentNullException(nameof(userSynchronizationService));

            this._logger = loggerFactory.CreateLogger<HttpContextIdentityCurrentUserService>();
        }

        public override bool IsRequiredOnCreation => true;
        public override bool IsRequiredOnUpdate => true;
        public override bool IsRequiredOnUpdateForCurrentUser => false;

        public override async Task<bool> CurrentUserCanEditAsync()
        {
            var currentUser = this._currentUserPermissionService.CurrentUserService.User;
            var user = this._userSessionContext.SaveUserEntity;

            return currentUser.Entity.Id != user.Id
                && await this._currentUserPermissionService.HasPermissionAsync<UsersPermissionProvider>(
                    provider => provider.CanAdministrateUsers);
        }

        public override Task<ApplicationUserModel> LoadAsync()
        {
            var loadingModel = (IApplicationPartLoadingModelPropertiesAccessor)this._userSessionContext.LoadingModel;

            return Task.FromResult(new ApplicationUserModel()
            {
                FirstName = loadingModel.FirstName,
                LastName = loadingModel.LastName,
                Email = loadingModel.Email,
                Login = loadingModel.Login,
            });
        }
        //Cannot read properties of undefined (reading 'parentName')


        private static void ValidateSaveModel(ApplicationUserModel model)
        {
            if (model.Email?.Length < 3
                || !model.Email.Contains("@")
                || model.Email.StartsWith("@")
                || model.Email.EndsWith("@"))
            {
                throw new UserSaveValidationException("", "InvalidEmail",
                    "Invalid email.");
            }
        }

        public override Task<IPartSaveResult> SaveAsync(ApplicationUserModel model)
        {
            var entity = (IApplicationPartEntityPropertiesAccessor)this._userSessionContext.SaveUserEntity;

            ValidateSaveModel(model);

            var emailChanged = entity.Email != model.Email;

            entity.FirstName = model.FirstName;
            entity.LastName = model.LastName;
            entity.Email = model.Email;
            entity.Login = model.Login;

            return Task.FromResult<IPartSaveResult>(
                new PartSaveResult(afterSaveTask: async () =>
                {
                    if (emailChanged)
                    {
                        await this._authenticationChangeInfoService.SetLastChangeDateAsync(entity.Id);
                    }
					try
					{
                        await this._userSynchronizationService.SyncUserAsync(entity.Id);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Error trying this operation");
                    }
                }));
        }
    }
}
