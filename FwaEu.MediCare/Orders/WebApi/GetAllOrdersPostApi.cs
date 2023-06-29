using System;

namespace FwaEu.MediCare.Orders.WebApi
{
    public class GetAllOrdersPostApi
    {
        public int? PatientId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
