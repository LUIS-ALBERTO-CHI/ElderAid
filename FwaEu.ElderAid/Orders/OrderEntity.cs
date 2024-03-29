﻿using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.ElderAid.Initialization;
using System;
using System.Linq;

namespace FwaEu.ElderAid.Orders
{
    public enum OrderState
    {
        Pending = 0,
        Cancelled = 1,
        Delivred = 2
    }


    public class OrderEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public int? PatientId { get; set; }
        public OrderState State { get; set; }
        public bool IsBox {  get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsNew() => Id == 0;
    }

    [ConnectionString("Generic")]
    public class OrderEntityClassMap : ClassMap<OrderEntity>
    {
        public OrderEntityClassMap()
        {
            Table("MDC_Orders");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.ArticleId).Column("ArticleId");
            Map(entity => entity.Quantity).Column("Quantity");
            Map(entity => entity.PatientId).Column("PatientId");
            Map(entity => entity.State).Column("State");
            Map(entity => entity.IsBox).Column("IsBox");
            Map(entity => entity.UpdatedBy).Column("UpdatedBy");
            Map(entity => entity.UpdatedOn).Column("UpdatedOn");
        }
    }

    public class OrderEntityRepository : DefaultRepository<OrderEntity, int>, IQueryByIds<OrderEntity, int>
    {
        public IQueryable<OrderEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }


    }
}