using System;

namespace FwaEu.MediCare.Orders.Services
{
    public class GetAllOrdersResponse
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public int? State { get; set; }
    }
}