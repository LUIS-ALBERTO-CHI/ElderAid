using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.ElderAid.Initialization;
using System;

namespace FwaEu.ElderAid.Stock
{
    public class StockConsumptionEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ArticleId { get; set; }
        public double Quantity { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsNew() => Id == 0;
    }

    [ConnectionString("Generic")]
    public class StockConsumptionEntityClassMap : ClassMap<StockConsumptionEntity>
    {
        public StockConsumptionEntityClassMap()
        {
            Table("MDC_StockConsumption");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.PatientId).Column("PatientId");
            Map(entity => entity.ArticleId).Column("ArticleId");
            Map(entity => entity.Quantity).Column("Quantity");
            Map(entity => entity.UpdatedBy).Column("UpdatedBy");
            Map(entity => entity.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }


    public class StockConsumptionEntityRepository : DefaultRepository<StockConsumptionEntity, int>, IQueryByIds<StockConsumptionEntity, int>
    {
        public System.Linq.IQueryable<StockConsumptionEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}