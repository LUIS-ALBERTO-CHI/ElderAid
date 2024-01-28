using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Referencials.GenericAdmin
{
    public class DosageFormModel
    {
        [Property(IsKey = true, IsEditable = false)]
        public int? Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Property(IsEditable = false)]
        [MasterData("Users")]
        public int? CreatedById { get; set; }

        [Property(IsEditable = false)]
        public DateTime? CreatedOn { get; set; }

        [Property(IsEditable = false)]
        [MasterData("Users")]
        public int? UpdatedById { get; set; }

        [Property(IsEditable = false)]
        public DateTime? UpdatedOn { get; set; }

    }

    public class DosageFormEntityToModelGenericAdminModelConfiguration
    : EntityToModelGenericAdminModelConfiguration<DosageFormEntity, int, DosageFormModel, MainSessionContext>
    {
        public DosageFormEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider)
        : base(serviceProvider)
        {
        }

        private readonly CurrentUserPermissionService _currentUserPermissionService;

        public DosageFormEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider,
            CurrentUserPermissionService currentUserPermissionService)
            : base(serviceProvider)
        {
            _currentUserPermissionService = currentUserPermissionService
                ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public override string Key => nameof(DosageFormEntity);

        public override async Task<bool> IsAccessibleAsync()
        {
            var currentUser = this._currentUserPermissionService.CurrentUserService.User;
            return currentUser.Entity.IsAdmin;
        }

        protected override Task FillEntityAsync(DosageFormEntity entity, DosageFormModel model)
        {
            entity.Name = model.Name;

            return Task.CompletedTask;
        }

        protected override async Task<DosageFormEntity> GetEntityAsync(DosageFormModel model)
        {
            return await this.GetRepository().GetAsync(model.Id.Value);
        }

        protected override Expression<Func<DosageFormEntity, DosageFormModel>> GetSelectExpression()
        {
            return entity => new DosageFormModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                CreatedOn = entity.CreatedOn,
                CreatedById = entity.CreatedBy.Id,
                UpdatedById = entity.UpdatedBy.Id,
                UpdatedOn = entity.UpdatedOn,
            };
        }

        protected override bool IsNew(DosageFormModel model)
        {
            return model.Id == null;

        }

        protected override void SetIdToModel(DosageFormModel model, DosageFormEntity entity)
        {
            model.Id = entity.Id;
        }
    }
}
