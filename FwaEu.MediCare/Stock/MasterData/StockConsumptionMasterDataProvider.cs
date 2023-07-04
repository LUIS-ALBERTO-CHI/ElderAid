using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Globalization;
using FwaEu.MediCare.GenericRepositorySession;
using System.Globalization;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Stock.MasterData
{
    public class StockConsumptionMasterDataProvider : EntityMasterDataProvider<StockConsumptionEntity, int, StockConsumptionEntityMasterDataModel, StockConsumptionEntityRepository>
    {
        public StockConsumptionMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<StockConsumptionEntity, StockConsumptionEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new StockConsumptionEntityMasterDataModel(entity.Id, entity.PatientId, entity.ArticleId, entity.Quantity, entity.UpdatedBy, entity.UpdatedOn);
        }

        protected override Expression<Func<StockConsumptionEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Quantity.ToString().Contains(search);
        }
    }

    public class StockConsumptionEntityMasterDataModel
    {
        public StockConsumptionEntityMasterDataModel(int id, int patientId, int articleId, double quantity, string updatedBy, DateTime updatedOn)
        {
            Id = id;
            PatientId = patientId;
            ArticleId = articleId;
            Quantity = quantity;
            UpdatedBy = updatedBy;
            UpdatedOn = updatedOn;
        }

        public int Id { get; set; }
        public int PatientId { get; set; }
        public int ArticleId { get; set; }
        public double Quantity { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}