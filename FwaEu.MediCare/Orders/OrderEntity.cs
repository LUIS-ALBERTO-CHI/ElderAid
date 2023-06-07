using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.Fwamework.Users;

using System;

namespace FwaEu.MediCare.Orders
{
    public enum OrederState
    {
        Pending = 0,
        Canceled = 1,
        Delivred = 2
    }


    public class OrderEntity : ICreationAndUpdateTracked
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public int? PatientId { get; set; }
        public int? BuildingId { get; set; }
        public OrederState State { get; set; }

        public UserEntity CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public UserEntity UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }
    }


    public class OrderEntityClassMap : ClassMap<OrderEntity>
    {
        public OrderEntityClassMap()
        {
            Not.LazyLoad();

            Id(entity => entity.Id).GeneratedBy.Identity();
            Map(entity => entity.ArticleId);
            Map(entity => entity.Quantity);
            Map(entity => entity.PatientId);
            Map(entity => entity.BuildingId);
            Map(entity => entity.State);
            this.AddCreationAndUpdateTrackedPropertiesIntoMapping();
        }
    }


    public class OrderEntityRepository : DefaultRepository<OrderEntity, int>
    {
    }

}
