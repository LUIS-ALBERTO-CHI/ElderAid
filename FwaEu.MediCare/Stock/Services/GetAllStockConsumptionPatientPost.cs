using System;

namespace FwaEu.MediCare.Stock.Services
{
    public class GetAllStockConsumptionPatientPost
    {
        public int? PatientId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}