using System;

namespace FwaEu.ElderAid.Orders.WebApi
{
    public class CreateOrdersPostApi
    {
        public int? PatientId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public bool IsBox { get; set; }
    }
}
