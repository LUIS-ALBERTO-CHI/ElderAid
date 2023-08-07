using FluentNHibernate.Data;
using FwaEu.Fwamework.Data.Database.Sessions;
using FwaEu.Fwamework.Globalization;
using FwaEu.Fwamework.Temporal;
using FwaEu.MediCare.Orders;
using FwaEu.MediCare.Orders.BackgroundTasks;
using FwaEu.Modules.MasterData;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Organizations.MasterData
{
	public class OrganizationEntityMasterDataProvider : EntityMasterDataProvider<OrganizationEntity, int, OrganizationMasterDataModel, OrganizationEntityRepository>
	{
        private readonly PeriodicOrderOptions _periodicOrderOptions;
        private readonly ICurrentDateTime _currentDateTime;


        public OrganizationEntityMasterDataProvider(
            MainSessionContext sessionContext,
            ICulturesService culturesService, IOptions<PeriodicOrderOptions> periodicOrderOptions, ICurrentDateTime currentDateTime)
            : base(sessionContext, culturesService)
        {
            _periodicOrderOptions = periodicOrderOptions.Value;
            _currentDateTime = currentDateTime;
        }
        protected override Expression<Func<OrganizationEntity, OrganizationMasterDataModel>> CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new OrganizationMasterDataModel(entity.Id, entity.InvariantId, entity.Name, entity.DatabaseName, entity.PublicWebURL, entity.PublicMobileURL, entity.PharmacyEmail, entity.OrderPeriodicityDays, entity.OrderPeriodicityDayOfWeek, entity.LastPeriodicityOrder, GetDelayToExecuteOrder(entity.OrderPeriodicityDays, entity.OrderPeriodicityDayOfWeek, entity.LastPeriodicityOrder), entity.PeriodicityOrderActivationDaysNumber, entity.IsStockPharmacyPerBox, entity.CreatedBy.Id, entity.CreatedOn, entity.UpdatedBy.Id, entity.UpdatedOn);
        }
        protected override Expression<Func<OrganizationEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Name.Contains(search);
        }

        private DateTime GetDelayToExecuteOrder(int orderPeriodicityDays, int orderPeriodicityDayOfWeek, DateTime lastPeriodicityOrder)
        {
            var options = _periodicOrderOptions;
            var today = _currentDateTime.Now;

            // Calculate the difference between the current date and dateStart
            TimeSpan timeSpanDifference = today - lastPeriodicityOrder;
            // Calculate the number of periods completed
            int periodsCompleted = (int)Math.Ceiling(timeSpanDifference.TotalDays / orderPeriodicityDays);

            var nextPeriodicityOrder = lastPeriodicityOrder.AddDays(periodsCompleted * orderPeriodicityDays);

            nextPeriodicityOrder = nextPeriodicityOrder.Date + new TimeSpan(options.ValidationHour, options.ValidationMinutes, 0);

            int numberOfElements = Enum.GetValues(typeof(DayOfWeek)).Length;

            if (!((int)nextPeriodicityOrder.DayOfWeek == orderPeriodicityDayOfWeek || (int)nextPeriodicityOrder.DayOfWeek + numberOfElements == orderPeriodicityDayOfWeek))
            {
                // Calculate the difference in days to get to the desired dayOfWeek
                int daysToAdd = orderPeriodicityDayOfWeek - (((nextPeriodicityOrder.DayOfWeek == 0) ? numberOfElements : 0)
                                                                   + (int)nextPeriodicityOrder.DayOfWeek);

                nextPeriodicityOrder = nextPeriodicityOrder.AddDays(daysToAdd);
            }

            return nextPeriodicityOrder;
        }

    }

	public class OrganizationMasterDataModel
    {
        public OrganizationMasterDataModel(int id, string invariantId, string name, string databaseName, string publicWebURL, string publicMobileURL, string pharmacyEmail, 
                                        int orderPeriodicityDays, int orderPeriodicityDayOfWeek, DateTime lastPeriodicityOrder, DateTime nextPeriodicityOrder, int? periodicityOrderActivationDaysNumber, 
                                            bool? isStockPharmacyPerBox, int? createdById, DateTime? createdOn, int? updatedById, DateTime? updatedOn)
        {
            Id = id;
            InvariantId = invariantId;
            Name = name;
            DatabaseName = databaseName;
            PublicWebURL = publicWebURL;
            PublicMobileURL = publicMobileURL;
            PharmacyEmail = pharmacyEmail;
            OrderPeriodicityDays = orderPeriodicityDays;
            OrderPeriodicityDayOfWeek = orderPeriodicityDayOfWeek;
            LastPeriodicityOrder = lastPeriodicityOrder;
            PeriodicityOrderActivationDaysNumber = periodicityOrderActivationDaysNumber;
            IsStockPharmacyPerBox = isStockPharmacyPerBox;
            NextPeriodicityOrder = nextPeriodicityOrder;
            CreatedById = createdById;
            CreatedOn = createdOn;
            UpdatedById = updatedById;
            UpdatedOn = updatedOn;
        }

        

        public int Id { get; }
		public string InvariantId { get; }
        public string Name { get; }
        public string DatabaseName { get; set; }
        public string PublicWebURL { get; set; }
        public string PublicMobileURL { get; set; }
        public string PharmacyEmail { get; set; }
        public int OrderPeriodicityDays { get; set; }
        public int OrderPeriodicityDayOfWeek { get; set; }
        public DateTime LastPeriodicityOrder { get; set; }
        public DateTime NextPeriodicityOrder { get; set; }

        public int? PeriodicityOrderActivationDaysNumber { get; set; }
        public bool? IsStockPharmacyPerBox { get; set; }

        public int? CreatedById { get; set; }
        public DateTime? CreatedOn { get; set; }

        public int? UpdatedById { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

   

}
