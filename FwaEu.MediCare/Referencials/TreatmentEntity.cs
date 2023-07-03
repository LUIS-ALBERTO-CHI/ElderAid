﻿using FluentNHibernate.Mapping;
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

    [ConnectionString("Generic")]
    public class TreatmentEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }
        public TreatmentType Type { get; set; }

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