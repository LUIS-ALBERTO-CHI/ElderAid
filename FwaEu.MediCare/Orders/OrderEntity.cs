using System;

namespace FwaEu.MediCare.Orders
{
    public enum OrderState
    {
        Pending = 0,
        Cancelled = 1,
        Delivred = 2
    }


    public class OrderEntity
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public int? PatientId { get; set; }
        public OrderState State { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }

        public bool IsNew()
        {
            return Id == 0;
        }
    }

}
