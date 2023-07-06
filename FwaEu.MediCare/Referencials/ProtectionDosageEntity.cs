using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.MediCare.Initialization;
using System;

namespace FwaEu.MediCare.Referencials
{

    public class ProtectionDosageEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }

        public int ProtectionId { get; set; }

        public int Quantity { get; set; }

        public DateTime Hour { get; set; }


        public DateTime UpdatedOn { get { return _dateTime; } set { } }
        private static DateTime _dateTime = DateTime.Now;
        public bool IsNew() => Id == 0;
    }

    [ConnectionString("Generic")]
    public class ProtectionDosageEntityEntityClassMap : ClassMap<ProtectionDosageEntity>
    {
        public ProtectionDosageEntityEntityClassMap()
        {
            Table("MDC_ProtectionDosages");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.ProtectionId).Column("ProtectionId");
            Map(entity => entity.Quantity).Column("Quantity");
            Map(entity => entity.Hour).Column("Hour");
            Map(entity => entity.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }


    public class ProtectionDosageEntityRepository : DefaultRepository<ProtectionDosageEntity, int>, IQueryByIds<ProtectionDosageEntity, int>
    {
        public System.Linq.IQueryable<ProtectionDosageEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}