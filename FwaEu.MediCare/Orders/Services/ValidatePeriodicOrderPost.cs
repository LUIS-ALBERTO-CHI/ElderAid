using System;

namespace FwaEu.MediCare.Orders
{
    public class ValidatePeriodicOrderPost
    {
        public int PatientId { get; set; }
        public ArticleValidatePeriodicOrderPost[] Articles { get; set; }
    }

    public class ArticleValidatePeriodicOrderPost
    {
        public int ArticleId { get; set; }
        public int Quantity { get; set; }
        public int DefaultQuantity { get; set; }
    }

}
