﻿using System;

namespace FwaEu.MediCare.Orders.WebApi
{
    public class GetAllOrdersResponseApi
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int ArticleId { get; set; }
        public double Quantity { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public int? UpdatedBy { get; set; }
        public int? State { get; set; }
    }
}
