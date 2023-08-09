using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Modules.MasterData;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Organizations.MasterData
{
	public class AdminOrganizationEntityMasterDataProvider : EntityMasterDataProvider<OrganizationEntity, int, AdminOrganizationMasterDataModel, AdminOrganizationEntityRepository>
	{

        public AdminOrganizationEntityMasterDataProvider(
            MainSessionContext sessionContext,
            ICulturesService culturesService)
            : base(sessionContext, culturesService)
        {
        }
        protected override Expression<Func<OrganizationEntity, AdminOrganizationMasterDataModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new AdminOrganizationMasterDataModel(entity.Id, entity.InvariantId, entity.Name, entity.DatabaseName, entity.PublicWebURL, entity.PublicMobileURL, entity.PharmacyEmail, entity.IsActive, entity.OrderPeriodicityDays, entity.OrderPeriodicityDayOfWeek, entity.LastPeriodicityOrder, entity.PeriodicityOrderActivationDaysNumber, entity.IsStockPharmacyPerBox, entity.CreatedBy.Id, entity.CreatedOn, entity.UpdatedBy.Id, entity.UpdatedOn);
        }
        protected override Expression<Func<OrganizationEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Name.Contains(search);
        }
    }

	public class AdminOrganizationMasterDataModel
    {
        public AdminOrganizationMasterDataModel(int id, string invariantId, string name, string databaseName, string publicWebURL, string publicMobileURL, string pharmacyEmail, bool? isActive, int? orderPeriodicityDays, int? orderPeriodicityDayOfWeek, DateTime? lastPeriodicityOrder, int? periodicityOrderActivationDaysNumber, bool? isStockPharmacyPerBox, int? createdById, DateTime? createdOn, int? updatedById, DateTime? updatedOn)
        {
            Id = id;
            InvariantId = invariantId;
            Name = name;
            DatabaseName = databaseName;
            IsActive = isActive;
            PublicWebURL = publicWebURL;
            PublicMobileURL = publicMobileURL;
            PharmacyEmail = pharmacyEmail;
            OrderPeriodicityDays = orderPeriodicityDays;
            OrderPeriodicityDayOfWeek = orderPeriodicityDayOfWeek;
            LastPeriodicityOrder = lastPeriodicityOrder;
            PeriodicityOrderActivationDaysNumber = periodicityOrderActivationDaysNumber;
            IsStockPharmacyPerBox = isStockPharmacyPerBox;
            CreatedById = createdById;
            CreatedOn = createdOn;
            UpdatedById = updatedById;
            UpdatedOn = updatedOn;
        }

        public int Id { get; }
		public string InvariantId { get; }
        public string Name { get; }
        public string DatabaseName { get; set; }
        public bool? IsActive { get; set; }
        public string PublicWebURL { get; set; }
        public string PublicMobileURL { get; set; }
        public string PharmacyEmail { get; set; }
        public int? OrderPeriodicityDays { get; set; }
        public int? OrderPeriodicityDayOfWeek { get; set; }
        public DateTime? LastPeriodicityOrder { get; set; }
        public int? PeriodicityOrderActivationDaysNumber { get; set; }
        public bool? IsStockPharmacyPerBox { get; set; }

        public int? CreatedById { get; set; }


        public DateTime? CreatedOn { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
