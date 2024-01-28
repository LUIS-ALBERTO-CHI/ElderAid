using System;

namespace FwaEu.ElderAid.Stock.WebApi
{
    public class GetAllStockConsumptionPatientPostApi
    {
        public int? PatientId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
