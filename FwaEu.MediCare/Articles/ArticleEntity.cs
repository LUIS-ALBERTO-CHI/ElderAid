using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.Fwamework.Data.Database.Tracking;
using FwaEu.MediCare.Initialization;
using System;

namespace FwaEu.MediCare.Articles
{
    public enum ArticleType
    {
        Medicine = 0,
        CareEquipment = 1,
        Protection = 2
    }

    public class ArticleEntity : IUpdatedOnTracked
    {
        public int Id { get; set; }

        public ArticleType ArticleType { get; set; }
        public string Title { get; set; }
        public string GroupName { get; set; }
        public double Price { get; set; }
        public double? AmountRemains { get; set; }
        public double? CountInBox { get; set; }
        public string Unit { get; set; }
        public string InvoicingUnit { get; set; }
        public bool? IsFavorite { get; set; }
        public string LeftAtChargeExplanation { get; set; }
        public int? AlternativePackagingCount { get; set; }
        public int? SubstitutionsCount { get; set; }
        public bool? IsDeleted  { get; set; }
        public bool IsGalenicDosageForm { get; set; }
        public int PharmaCode { get; set; }
        public DateTime UpdatedOn { get { return _dateTime; } set { } }
        private static DateTime _dateTime = DateTime.Now;
        public bool IsNew() => Id == 0;
    }

    [ConnectionString("Generic")]
    public class ArticleEntityClassMap : ClassMap<ArticleEntity>
    {
        public ArticleEntityClassMap()
        {
            Table("MDC_Articles");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.Title).Column("Title");
            Map(entity => entity.LeftAtChargeExplanation).Column("LeftAtChargeExplanation");
            Map(entity => entity.Price).Column("Price");
            Map(entity => entity.ArticleType).Column("ArticleType");
            Map(entity => entity.AmountRemains).Column("LeftAtCharge");
            Map(entity => entity.CountInBox).Column("CountInBox");          

            Map(entity => entity.AlternativePackagingCount).Column("AlternativePackagingCount");
            Map(entity => entity.SubstitutionsCount).Column("SubstitutionsCount");

            Map(entity => entity.GroupName).Column("GroupName");
            Map(entity => entity.Unit).Column("Unit");
            Map(entity => entity.InvoicingUnit).Column("InvoicingUnit");
            Map(entity => entity.IsGalenicDosageForm).Column("IsGalenicDosageForm");
            Map(entity => entity.IsDeleted).Column("IsDeleted");
            Map(entity => entity.PharmaCode).Column("PharmaCode");
        }
    }

    public class ArticleEntityRepository : DefaultRepository<ArticleEntity, int>, IQueryByIds<ArticleEntity, int>
    {
        public System.Linq.IQueryable<ArticleEntity> QueryByIds(int[] ids)
        {
            throw new NotImplementedException();
        }
    }
}