using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Globalization;
using FwaEu.MediCare.GenericRepositorySession;
using System.Globalization;
using System.Linq.Expressions;
using System.Linq;
using System.Threading.Tasks;
using FwaEu.Fwamework.Data.Database.Sessions;
using NHibernate.Linq;

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

		protected override IQueryable<StockConsumptionEntity> GetBaseQuery()
		{
            return base.GetBaseQuery().OrderByDescending(x => x.UpdatedOn);
		}

		protected override IQueryable<StockConsumptionEntity> GetBaseQueryByIds(int[] ids)
		{
			return base.GetBaseQueryByIds(ids).OrderByDescending(x => x.UpdatedOn);
		}

		public override async Task<MasterDataChangesInfo> GetChangesInfoAsync(MasterDataProviderGetChangesParameters parameters)
		{
            SessionContext.RepositorySession.Create<StockConsumptionEntityRepository>().Query();

            var query = SessionContext.RepositorySession.Create<StockConsumptionEntityRepository>().Query();

			var count = await query.CountAsync();
			var maxUpdatedOn = count > 0 ? await query.MaxAsync(x => x.UpdatedOn) : default(DateTime?);
			var info = new MasterDataChangesInfo(maxUpdatedOn, count);

            return info;

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