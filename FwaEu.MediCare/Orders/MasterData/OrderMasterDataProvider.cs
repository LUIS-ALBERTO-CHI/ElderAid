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
            return entity => new OrderEntityMasterDataModel(entity.Id, entity.ArticleId, entity.Quantity,
                                                                  entity.PatientId, entity.State, entity.IsBox, entity.UpdatedBy, entity.UpdatedOn);
        }
    }

    public class OrderEntityMasterDataModel
    {
        public OrderEntityMasterDataModel(int id, int articleId, double quantity, int? patientId, OrderState state, bool? isBox, string updatedBy, DateTime updatedOn)
        {
            Id = id;
            ArticleId = articleId;
            Quantity = quantity;
            PatientId = patientId;
            State = state;
            IsBox = isBox;
            UpdatedBy = updatedBy;
            UpdatedOn = updatedOn;
        }
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public int? PatientId { get; set; }
        public OrderState State { get; set; }
        public bool? IsBox { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }

}