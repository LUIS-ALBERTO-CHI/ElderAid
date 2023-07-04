using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.MediCare.Initialization;
using System;

namespace FwaEu.MediCare.Referencials
{
    public enum TreatmentType
    {
        Fixe = 0,
        Reserve = 1,
        Erased = 2
    }

    public class TreatmentEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public TreatmentType TreatmentType { get; set; }

        public ArticleType ArticleType { get; set; }

        public int PrescribedArticleId { get; set; }
        public int AppliedArticleId { get; set; }

        public string DosageDescription { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
        public DateTime UpdatedOn { get { return _dateTime; } set { } }
        private static DateTime _dateTime = DateTime.Now;
        public bool IsNew() => Id == 0;
    }

    [ConnectionString("Generic")]
    public class TreatmentEntityClassMap : ClassMap<TreatmentEntity>
    {
        public TreatmentEntityClassMap()
        {
            Table("MDC_Treatments");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.PatientId).Column("PatientId");
            Map(entity => entity.PrescribedArticleId).Column("PrescribedArticleId");
            Map(entity => entity.AppliedArticleId).Column("AppliedArticleId");
            Map(entity => entity.DosageDescription).Column("DosageDescription");
            Map(entity => entity.DateStart).Column("DateStart");
            Map(entity => entity.DateEnd).Column("DateEnd");
            Map(entity => entity.TreatmentType).Column("TreatmentType");
            Map(entity => entity.ArticleType).Column("ArticleType");
            Map(entity => entity.UpdatedOn).Column("UpdatedOn").Not.Nullable();
        }
    }


    public class TreatmentEntityRepository : DefaultRepository<TreatmentEntity, int>, IQueryByIds<TreatmentEntity, int>
    {
        public System.Linq.IQueryable<TreatmentEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}