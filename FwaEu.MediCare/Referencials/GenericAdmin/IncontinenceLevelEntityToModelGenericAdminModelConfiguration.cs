using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.MediCare.Patients;
using FwaEu.Modules.GenericAdmin;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Referencials.GenericAdmin
{
    public class IncontinenceLevelModel
    {
        [Property(IsKey = true, IsEditable = false)]
        public int? Id { get; set; }

        [Required]
        public IncontinenceLevel Level { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public double Amount { get; set; }
    }

    public class IncontinenceLevelEntityToModelGenericAdminModelConfiguration
        : EntityToModelGenericAdminModelConfiguration<IncontinenceLevelEntity, int, IncontinenceLevelModel, MainSessionContext>
    {
        public IncontinenceLevelEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        private readonly CurrentUserPermissionService _currentUserPermissionService;

        public IncontinenceLevelEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider,
            CurrentUserPermissionService currentUserPermissionService)
            : base(serviceProvider)
        {
            _currentUserPermissionService = currentUserPermissionService
                ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public override string Key => nameof(IncontinenceLevelEntity);

        public override async Task<bool> IsAccessibleAsync()
        {
            var currentUser = _currentUserPermissionService.CurrentUserService.User;
            return currentUser.Entity.IsAdmin;
        }

        protected override Task FillEntityAsync(IncontinenceLevelEntity entity, IncontinenceLevelModel model)
        {
            entity.Level = model.Level;
            entity.Year = model.Year;
            entity.Amount = model.Amount;
            return Task.CompletedTask;
        }

        protected override async Task<IncontinenceLevelEntity> GetEntityAsync(IncontinenceLevelModel model)
        {
            return await GetRepository().GetAsync(model.Id.Value);
        }

        protected override Expression<Func<IncontinenceLevelEntity, IncontinenceLevelModel>> GetSelectExpression()
        {
            return entity => new IncontinenceLevelModel
            {
                Id = entity.Id,
                Level = entity.Level,
                Year = entity.Year,
                Amount = entity.Amount
            };
        }

        protected override bool IsNew(IncontinenceLevelModel model)
        {
            return model.Id == null;
        }

        protected override void SetIdToModel(IncontinenceLevelModel model, IncontinenceLevelEntity entity)
        {
            model.Id = entity.Id;
        }
    }
}