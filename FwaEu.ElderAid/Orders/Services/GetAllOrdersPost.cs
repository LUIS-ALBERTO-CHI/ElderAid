using System;

namespace FwaEu.ElderAid.Orders.Services
{
    public class GetAllOrdersPost
    {
        public int? PatientId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}