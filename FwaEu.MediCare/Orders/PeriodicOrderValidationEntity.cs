using AutoMapper;
using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;
using FwaEu.MediCare.Organizations;
using System;
using System.Linq;

namespace FwaEu.MediCare.Orders
{

    public class PeriodicOrderValidationEntity : ICreationAndUpdateTracked
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public int PatientId { get; set; }
        public OrganizationEntity Organization { get; set; }
        public int Quantity { get; set; }
        public int DefaultQuantity { get; set; }

        public DateTime? OrderedOn { get; set; }
        public UserEntity ValidatedBy { get; set; }
        public DateTime ValidatedOn { get; set; }

        public UserEntity CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserEntity UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsNew() => Id == 0;
    }

    public class PeriodicOrderValidationEntityClassMap : ClassMap<PeriodicOrderValidationEntity>
    {
        public PeriodicOrderValidationEntityClassMap()
        {
            Not.LazyLoad();

            Id(entity => entity.Id).GeneratedBy.Identity();
            Map(entity => entity.ArticleId).Not.Nullable();
            Map(entity => entity.PatientId).Not.Nullable();
            References(entity => entity.Organization).Not.Nullable();

            Map(entity => entity.Quantity).Not.Nullable();
            Map(entity => entity.DefaultQuantity).Not.Nullable();

            Map(entity => entity.OrderedOn);
            References(entity => entity.ValidatedBy).Nullable();
            Map(entity => entity.ValidatedOn).Not.Nullable();

            this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
        }
    }

    public class PeriodicOrderValidationEntityRepository : DefaultRepository<PeriodicOrderValidationEntity, int>
    {
    }
}