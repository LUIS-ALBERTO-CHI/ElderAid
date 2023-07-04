using FwaEu.Modules.MasterData;
using System;
using FwaEu.Fwamework.Globalization;
using FwaEu.MediCare.GenericRepositorySession;
using System.Globalization;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Referencials.MasterData
{
    public class ProtectionMasterDataProvider : EntityMasterDataProvider<ProtectionEntity, int, ProtectionEntityMasterDataModel, ProtectionEntityRepository>
    {
        public ProtectionMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<ProtectionEntity, ProtectionEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new ProtectionEntityMasterDataModel(entity.Id, entity.ArticleId, entity.DosageDescription, entity.DateStart, entity.DateEnd);
        }

        protected override Expression<Func<ProtectionEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.DosageDescription.Contains(search);
        }
    }

    public class ProtectionEntityMasterDataModel
    {
        public ProtectionEntityMasterDataModel(int id, int articleId, string dosageDescription, DateTime? dateStart, DateTime? dateEnd)
        {
            Id = id;
            ArticleId = articleId;
            DosageDescription = dosageDescription;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public int Id { get; set; }
        public int ArticleId { get; set; }
        public string DosageDescription { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}