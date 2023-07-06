using System;

namespace FwaEu.MediCare.Orders.WebApi
{
    public class ValidatePeriodicOrderPostApi
    {
        public int PatientId { get; set; }
        public ArticleValidatePeriodicOrderPostApi[] Articles { get; set; }
    }

    public class ArticleValidatePeriodicOrderPostApi
    {
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public int DefaultQuantity { get; set; }
    }

}
