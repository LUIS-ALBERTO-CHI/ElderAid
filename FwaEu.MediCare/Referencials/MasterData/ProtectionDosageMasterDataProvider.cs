using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Globalization;
using FwaEu.MediCare.GenericRepositorySession;
using System.Globalization;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Referencials.MasterData
{
    public class ProtectionDosageMasterDataProvider : EntityMasterDataProvider<ProtectionDosageEntity, int, ProtectionDosageEntityMasterDataModel, ProtectionDosageEntityRepository>
    {
        public ProtectionDosageMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<ProtectionDosageEntity, ProtectionDosageEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new ProtectionDosageEntityMasterDataModel(entity.Id, entity.ProtectionId, entity.Quantity, entity.Hour);
        }

        protected override Expression<Func<ProtectionDosageEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.Quantity.ToString().Contains(search);
        }
    }

    public class ProtectionDosageEntityMasterDataModel
    {
        public ProtectionDosageEntityMasterDataModel(int id, int protectionId, int quantity, DateTime hour)
        {
            Id = id;
            ProtectionId = protectionId;
            Quantity = quantity;
            Hour = hour;
        }

        public int Id { get; set; }
        public int ProtectionId { get; set; }
        public int Quantity { get; set; }
        public DateTime Hour { get; set; }
    }
}