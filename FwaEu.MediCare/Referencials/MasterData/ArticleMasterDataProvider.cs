using FwaEu.Modules.MasterData;
using System.Threading.Tasks;
using System;
using FwaEu.Fwamework.Globalization;
using FwaEu.MediCare.GenericRepositorySession;
using System.Globalization;
using System.Linq.Expressions;
using Aspose.Cells.Charts;

namespace FwaEu.MediCare.Referencials.MasterData
{
    public class ArticleMasterDataProvider : EntityMasterDataProvider<ArticleEntity, int, ArticleEntityMasterDataModel, ArticleEntityRepository>
    {
        public ArticleMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<ArticleEntity, ArticleEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new ArticleEntityMasterDataModel(entity.Id, entity.Type, entity.Title, entity.Price, entity.AmountRemains, entity.Unit, entity.IsFavorite, entity.Packaging, entity.ThumbnailURL, entity.ImageURLs, entity.AlternativePackagingCount, entity.SubstitutionsCount);
        }

        protected override Expression<Func<ArticleEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Title.Contains(search);
        }
    }

    public class ArticleEntityMasterDataModel
    {
        public ArticleEntityMasterDataModel(int id, ArticleType type, string title, double price, double amountRemains, double unit, bool isFavorite, string packaging, string thumbnailURL, string imageURLs, int? alternativePackagingCount, int? substitutionsCount)
        {
            Id = id;
            Type = type;
            Title = title;
            Price = price;
            AmountRemains = amountRemains;
            Unit = unit;
            IsFavorite = isFavorite;
            Packaging = packaging;
            ThumbnailURL = thumbnailURL;
            ImageURLs = imageURLs;
            AlternativePackagingCount = alternativePackagingCount;
            SubstitutionsCount = substitutionsCount;
        }

        public int Id { get; }
        public ArticleType Type { get; }
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
}