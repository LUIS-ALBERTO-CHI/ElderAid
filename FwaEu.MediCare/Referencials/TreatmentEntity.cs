using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.MediCare.Patients;
using System;

namespace FwaEu.MediCare.Referencials
{
    public enum TreatmentType
    {
        Fixe = 0,
        Reserve = 1,
        Erased = 2
    }

    public class TreatmentEntity
    {
        public int Id { get; set; }
        public TreatmentType Type { get; set; }

        public ArticleType ArticleType { get; set; }

        public int PrescribedArticleId { get; set; }
        public int AppliedArticleId { get; set; }

        public string DosageDescription { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

    }

    public class TreatmentEntityClassMap : ClassMap<TreatmentEntity>
    {
        public TreatmentEntityClassMap()
        {
            Table("MEDICARE_EMS.dbo.MDC_Articles");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Id(entity => entity.DateStart).Column("startDate");
            Id(entity => entity.DateEnd).Column("endDate");
            Id(entity => entity.Type).Column("state");

        }
    }


    public class TreatmentEntityRepository : DefaultRepository<TreatmentEntity, int>
    {
    }

}
