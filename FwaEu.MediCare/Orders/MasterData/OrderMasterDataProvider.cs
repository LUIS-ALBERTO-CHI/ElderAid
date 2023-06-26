using FwaEu.Fwamework.Globalization;
using FwaEu.MediCare.GenericRepositorySession;
using FwaEu.Modules.MasterData;
using System;
using System.Collections;
using System.Globalization;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FwaEu.MediCare.Orders.MasterData
{

    public class OrderMasterDataProvider : EntityMasterDataProvider<OrderEntity, int, OrderEntityMasterDataModel, OrderEntityRepository>
    {
        public OrderMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<OrderEntity, OrderEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new OrderEntityMasterDataModel(entity.Id, entity.Type, entity.Title, entity.Price, entity.AmountRemains, entity.Unit, entity.IsFavorite, entity.Packaging, entity.ThumbnailURL, entity.ImageURLs, entity.AlternativePackagingCount, entity.SubstitutionsCount);
        }

        protected override Expression<Func<OrderEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Title.Contains(search);
        }
    }

    public class OrderEntityMasterDataModel
    {
        public OrderEntityMasterDataModel(int id, int articleId, double quantity, int? patientId, OrderState state, string updatedBy, DateTime updatedOn)
        {
            Id = id;
            ArticleId = articleId;
            Quantity = quantity;
            PatientId = patientId;
            State = state;
            UpdatedBy = updatedBy;
            UpdatedOn = updatedOn;
        }
        public int Id { get; }
        public OrderType Type { get; }
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