using FwaEu.Modules.MasterData;
using System.Threading.Tasks;
using System;
using System.Collections;
using FwaEu.Fwamework.Globalization;
using FwaEu.MediCare.GenericRepositorySession;
using System.Globalization;
using System.Linq.Expressions;

namespace FwaEu.MediCare.Referencials.MasterData
{

    public class TreatmentMasterDataProvider : EntityMasterDataProvider<TreatmentEntity, int, TreatmentEntityMasterDataModel, TreatmentEntityRepository>
    {
        public TreatmentMasterDataProvider(GenericSessionContext sessionContext, ICulturesService culturesService) : base(sessionContext, culturesService)
        {
        }

        protected override Expression<Func<TreatmentEntity, TreatmentEntityMasterDataModel>>
            CreateSelectExpression(CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => new TreatmentEntityMasterDataModel(entity.Id, entity.Type, entity.ArticleType, entity.PrescribedArticleId, entity.AppliedArticleId, entity.DosageDescription, entity.DateStart, entity.DateEnd);
        }

        protected override Expression<Func<TreatmentEntity, bool>> CreateSearchExpression(string search,
            CultureInfo userCulture, CultureInfo defaultCulture)
        {
            return entity => entity.DosageDescription.Contains(search);
        }
    }

    public class TreatmentEntityMasterDataModel
    {
        public TreatmentEntityMasterDataModel(int id, TreatmentType type, ArticleType articleType, int prescribedArticleId, int appliedArticleId, string dosageDescription, DateTime? dateStart, DateTime? dateEnd)
        {
            Id = id;
            Type = type;
            ArticleType = articleType;
            PrescribedArticleId = prescribedArticleId;
            AppliedArticleId = appliedArticleId;
            DosageDescription = dosageDescription;
            DateStart = dateStart;
            DateEnd = dateEnd;
        }

        public int Id { get; set; }
        public TreatmentType Type { get; set; }

        public ArticleType ArticleType { get; set; }

        public int PrescribedArticleId { get; set; }
        public int AppliedArticleId { get; set; }

        public string DosageDescription { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

    }
}