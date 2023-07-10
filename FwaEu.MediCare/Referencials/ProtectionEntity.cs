using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.MediCare.Initialization;
using System;

namespace FwaEu.MediCare.Referencials
{

    public class ProtectionEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int ArticleId { get; set; }

        public string DosageDescription { get; set; }
        public int QuantityPerDay { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

        public DateTime UpdatedOn { get { return _dateTime; } set { } }
        private static DateTime _dateTime = DateTime.Now;
        public bool IsNew() => Id == 0;
    }

    [ConnectionString("Generic")]
    public class ProtectionEntityClassMap : ClassMap<ProtectionEntity>
    {
        public ProtectionEntityClassMap()
        {
            Table("MDC_Protections");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.DateStart).Column("DateStart");
            Map(entity => entity.DateEnd).Column("DateEnd");
            Map(entity => entity.ArticleId).Column("ArticleId");
            Map(entity => entity.PatientId).Column("PatientId");
            Map(entity => entity.DosageDescription).Column("DosageDescription");
            Map(entity => entity.QuantityPerDay).Column("QuantityPerDay");

            Map(entity => entity.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }


    public class ProtectionEntityRepository : DefaultRepository<ProtectionEntity, int>, IQueryByIds<ProtectionEntity, int>
    {
        public System.Linq.IQueryable<ProtectionEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}