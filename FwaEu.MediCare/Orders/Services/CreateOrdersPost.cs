using System;

namespace FwaEu.MediCare.Orders.Services
{
    public class CreateOrdersPost
    {
        public int PatientId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
