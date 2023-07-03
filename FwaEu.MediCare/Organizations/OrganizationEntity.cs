
using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using System;
using System.Collections.Generic;

namespace FwaEu.MediCare.Organizations
{
    public class OrganizationEntity : ICreationAndUpdateTracked
    {
       
        public int Id { get; set; }
        public string InvariantId { get; set; }
        public string Name { get; set; }

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
        public UserEntity CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserEntity UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }

        public override string ToString()
        {
            return Name;
        }

    }


    public class OrganizationEntityClassMap : ClassMap<OrganizationEntity>
    {
        public OrganizationEntityClassMap()
        {
            Not.LazyLoad();

            Id(entity => entity.Id).GeneratedBy.Identity();
            Map(entity => entity.InvariantId).Not.Nullable(); ;
            Map(entity => entity.Name).Not.Nullable(); ;
            Map(entity => entity.DatabaseName).Not.Nullable(); ;
            Map(entity => entity.IsActive).Default("1");
            Map(entity => entity.PublicWebURL);
            Map(entity => entity.PublicMobileURL);
            Map(entity => entity.PharmacyEmail);
            Map(entity => entity.OrderPeriodicityDays);
            Map(entity => entity.OrderPeriodicityDayOfWeek);
            Map(entity => entity.LastPeriodicityOrder);
            Map(entity => entity.PeriodicityOrderActivationDaysNumber);
            Map(entity => entity.IsStockPharmacyPerBox);
            this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
        }
    }

    public class OrganizationEntityRepository : DefaultRepository<OrganizationEntity, int>, IQueryByIds<OrganizationEntity, int>
    {
        public System.Linq.IQueryable<OrganizationEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<IRepositoryDataFilter<OrganizationEntity, int>> CreateDataFilters(
            RepositoryDataFilterContext<OrganizationEntity, int> context)
        {
            yield return new OrganizationEntityDataFilter();
        }
    }
}