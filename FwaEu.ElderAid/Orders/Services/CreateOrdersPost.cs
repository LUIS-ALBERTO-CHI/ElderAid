using System;

namespace FwaEu.ElderAid.Orders.Services
{
    public class CreateOrdersPost
    {
        public int? PatientId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public bool IsBox { get; set; }
    }
}
