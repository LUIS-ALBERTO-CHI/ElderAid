﻿using System;

namespace FwaEu.ElderAid.Articles.WebApi
{
    public class GetArticlesBySearchResponseApi
    {
        public int Id { get; set; }
        public ArticleType? ArticleType { get; set; }
        public string Title { get; set; }
        public string GroupName { get; set; }
        public double? Price { get; set; }
        public int? InsuranceCode { get; set; }
        public double? LeftAtCharge { get; set; }
        public double? CountInBox { get; set; }
        public string Unit { get; set; }
        public string InvoicingUnit { get; set; }
        public bool? IsFavorite { get; set; }
        public string LeftAtChargeExplanation { get; set; }
        public int? AlternativePackagingCount { get; set; }
        public int? SubstitutionsCount { get; set; }
        public int? LikesCount { get; set; }
        public bool IsGalenicDosageForm { get; set; }
        public bool IsDeleted { get; set; }
        public int PharmaCode { get; set; }
    }
}
