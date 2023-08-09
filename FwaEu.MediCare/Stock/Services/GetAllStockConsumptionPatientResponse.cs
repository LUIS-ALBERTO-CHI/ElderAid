using System;

namespace FwaEu.MediCare.Stock.Services
{
    public class GetAllStockConsumptionPatientResponse
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}