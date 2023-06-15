using System;

namespace FwaEu.MediCare.Referencials
{
    public enum ArticleType
    {
        Medicine = 0,
        CareEquipment = 1,
        Protection = 2
    }

    public class ArticleEntity
    {
        public int Id { get; set; }

        public ArticleType Type { get; set; }
        public string Title { get; set; }
        public double Price { get; set; }
        public double AmountRemains { get; set; }
        public double Unit { get; set; }
        public bool IsFavorite { get; set; }
        public string Packaging { get; set; }
        public string ThumbnailURL { get; set; }

        public string[] ImageURLs { get; set; }

        public int? AlternativePackagingCount { get; set; }
        public int? SubstitutionsCount { get; set; }

    }


}
