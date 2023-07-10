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
            return entity => new ProtectionEntityMasterDataModel(entity.Id, entity.ArticleId, entity.PatientId, entity.DosageDescription, entity.QuantityPerDay,entity.DateStart, entity.DateEnd);
        }

        protected override Expression<Func<ProtectionEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.PatientId.ToString().Contains(search);
        }
    }

    public class ProtectionEntityMasterDataModel
    {
        public ProtectionEntityMasterDataModel(int id, int articleId, int patientId, string dosageDescription, int quantityPerDay, DateTime? dateStart, DateTime? dateEnd)
        {
            Id = id;
            ArticleId = articleId;
            PatientId = patientId;
            DosageDescription = dosageDescription;
            DateStart = dateStart;
            DateEnd = dateEnd;
            QuantityPerDay = quantityPerDay;
        }

        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int PatientId { get; set; }
        public string DosageDescription { get; set; }
        public int QuantityPerDay { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}