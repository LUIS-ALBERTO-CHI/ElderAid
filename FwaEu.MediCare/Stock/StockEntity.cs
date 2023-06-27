using System;

namespace FwaEu.MediCare.Stock
{
    public class StockConsumptionEntity
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }

        public double Quantity { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsNew() => Id == 0;
    }
}
