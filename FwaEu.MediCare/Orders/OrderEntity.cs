using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;

using System;

namespace FwaEu.MediCare.Orders
{
    public enum OrderState
    {
        Pending = 0,
        Cancelled = 1,
        Delivred = 2
    }


    public class OrderEntity
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public int? PatientId { get; set; }
        public OrderState State { get; set; }

        public string UpdatedBy { get; set; }
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
            Table("MEDICARE_EMS.dbo.MDC_Orders");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.ArticleId).Column("ArticleId");
            Map(entity => entity.Quantity).Column("Quantity");
            Map(entity => entity.PatientId).Column("PatientId");
            Map(entity => entity.State).Column("State");
            Map(entity => entity.UpdatedBy).Column("updatedBy");
            Map(entity => entity.UpdatedOn).Column("updatedOn");
        }
    }

    public class OrderEntityRepository : DefaultRepository<OrderEntity, int>
    {
    }

}
