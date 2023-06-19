using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using System;

namespace FwaEu.MediCare.Referencials
{
    public enum ArticleType
    {
        Medicine = 0,
        CareEquipment = 1,
        Protection = 2
    }

    public class ArticleEntity
    {
        public int Id { get; set; }

        public ArticleType Type { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double AmountRemains { get; set; }
        public double Unit { get; set; }
        public bool IsFavorite { get; set; }
        public string Packaging { get; set; }
        public string ThumbnailURL { get; set; }

        public string ImageURLs { get; set; }

        public int? AlternativePackagingCount { get; set; }
        public int? SubstitutionsCount { get; set; }

    }

    public class ArticleEntityClassMap : ClassMap<ArticleEntity>
    {
        public ArticleEntityClassMap()
        {
            Table("MEDICARE_EMS.dbo.MDC_Articles");

            ReadOnly();
            Not.LazyLoad();
            SchemaAction.None();

            Id(entity => entity.Id).Column("Id");
            Map(entity => entity.Title).Column("Name");
            Map(entity => entity.Price).Column("Price");
            Map(entity => entity.Type).Column("family");
            Map(entity => entity.AmountRemains).Column("LeftAtCharge");
            Map(entity => entity.Packaging).Column("Contenant");
            Map(entity => entity.ThumbnailURL).Column("ThumbNailURL");
            Map(entity => entity.ImageURLs).Column("fullURLList");

        }
    }


    public class ArticleEntityRepository : DefaultRepository<ArticleEntity, int>
    {
    }

}
