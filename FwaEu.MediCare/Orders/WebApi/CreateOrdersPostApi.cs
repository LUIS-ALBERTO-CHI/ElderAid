using System;

namespace FwaEu.MediCare.Orders.WebApi
{
    public class CreateOrdersPostApi
    {
        public int PatientId { get; set; }
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
