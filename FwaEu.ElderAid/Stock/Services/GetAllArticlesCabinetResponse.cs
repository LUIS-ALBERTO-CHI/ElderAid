using System;

namespace FwaEu.ElderAid.Stock.Services
{
    public class GetAllArticlesCabinetResponse
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public double MinQuantity { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int CabinetId { get; set; }
    }
}