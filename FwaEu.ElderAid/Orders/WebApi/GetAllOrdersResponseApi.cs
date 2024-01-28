using System;

namespace FwaEu.ElderAid.Orders.WebApi
{
    public class GetAllOrdersResponseApi
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public int? State { get; set; }
        public bool IsBox { get; set; }

    }
}
