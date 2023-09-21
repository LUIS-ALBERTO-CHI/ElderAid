using System;

namespace FwaEu.MediCare.Articles
{
    public class GetArticlesByIdsReponse
    {
        public int Id { get; set; }
        public int ArticleType { get; set; }
        public string Title { get; set; }
        public string GroupName { get; set; }
        public decimal? Price { get; set; }
        public int? InsuranceCode { get; set; }
        public decimal? LeftAtCharge { get; set; }
        public double? CountInBox { get; set; }
        public string Unit { get; set; }
        public string InvoicingUnit { get; set; }
        public bool? IsFavorite { get; set; }
        public string LeftAtChargeExplanation { get; set; }
        public string Packaging { get; set; }
        public int? AlternativePackagingCount { get; set; }
        public int? SubstitutionsCount { get; set; }
        public int? LikesCount { get; set; }
        public bool IsGalenicDosageForm { get; set; }
        public int IsDeleted { get; set; }
        public int PharmaCode { get; set; }
    }
}
