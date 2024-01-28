using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Permissions;
using FwaEu.Modules.GenericAdmin;
using FwaEu.Modules.GenericAdminMasterData;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.ElderAid.Organizations.GenericAdmin
{
    public class OrganizationModel
    {
        [Property(IsKey = true, IsEditable = false)]
        public int? Id { get; set; }

        [Required]
        public string InvariantId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string DatabaseName { get; set; }
        public bool? IsActive { get; set; }

        public string PharmacyEmail { get; set; }

        public string PublicWebURL { get; set; }
        public string PublicMobileURL { get; set; }

        [Required]
        public int OrderPeriodicityDays { get; set; }
        [Required]
        public int OrderPeriodicityDayOfWeek { get; set; }
        [Required]
        public DateTime LastPeriodicityOrder { get; set; }
        [Required]
        public int PeriodicityOrderActivationDaysNumber { get; set; }
        public bool? IsStockPharmacyPerBox { get; set; }
    }

    public class OrganizationEntityToModelGenericAdminModelConfiguration
        : EntityToModelGenericAdminModelConfiguration<OrganizationEntity, int, OrganizationModel, MainSessionContext>
    {
        public OrganizationEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        private readonly CurrentUserPermissionService _currentUserPermissionService;

        public OrganizationEntityToModelGenericAdminModelConfiguration(IServiceProvider serviceProvider,
            CurrentUserPermissionService currentUserPermissionService)
            : base(serviceProvider)
        {
            _currentUserPermissionService = currentUserPermissionService
                ?? throw new ArgumentNullException(nameof(currentUserPermissionService));
        }

        public override string Key => nameof(OrganizationEntity);

        public override async Task<bool> IsAccessibleAsync()
        {
            var currentUser = this._currentUserPermissionService.CurrentUserService.User;
            return currentUser.Entity.IsAdmin;
        }

        protected override Task FillEntityAsync(OrganizationEntity entity, OrganizationModel model)
        {
            entity.InvariantId = model.InvariantId;
            entity.Name = model.Name;
            entity.DatabaseName= model.DatabaseName;
            entity.IsActive = model.IsActive;
            entity.PublicWebURL= model.PublicWebURL;
            entity.PublicMobileURL= model.PublicMobileURL;
            entity.PharmacyEmail= model.PharmacyEmail;
            entity.OrderPeriodicityDays = model.OrderPeriodicityDays;
            entity.OrderPeriodicityDayOfWeek= model.OrderPeriodicityDayOfWeek;
            entity.LastPeriodicityOrder = model.LastPeriodicityOrder;
            entity.PeriodicityOrderActivationDaysNumber= model.PeriodicityOrderActivationDaysNumber;
            entity.IsStockPharmacyPerBox = model.IsStockPharmacyPerBox;
            return Task.CompletedTask;
        }

        protected override async Task<OrganizationEntity> GetEntityAsync(OrganizationModel model)
        {
            return await this.GetRepository().GetAsync(model.Id.Value);
        }

        protected override Expression<Func<OrganizationEntity, OrganizationModel>> GetSelectExpression()
        {
            return entity => new OrganizationModel
            {
                Id = entity.Id,
                InvariantId = entity.InvariantId,
                Name = entity.Name,
                DatabaseName= entity.DatabaseName,
                PublicMobileURL= entity.PublicMobileURL,
                PublicWebURL= entity.PublicWebURL,
                IsStockPharmacyPerBox= entity.IsStockPharmacyPerBox,
                PharmacyEmail= entity.PharmacyEmail,
                IsActive=entity.IsActive,
                PeriodicityOrderActivationDaysNumber = entity.PeriodicityOrderActivationDaysNumber,
                LastPeriodicityOrder = entity.LastPeriodicityOrder,
                OrderPeriodicityDayOfWeek=entity.OrderPeriodicityDayOfWeek,
                OrderPeriodicityDays = entity.OrderPeriodicityDays,
            };
        }

        protected override bool IsNew(OrganizationModel model)
        {
            return model.Id == null;
        }

        protected override void SetIdToModel(OrganizationModel model, OrganizationEntity entity)
        {
            model.Id = entity.Id;
        }
    }
}
