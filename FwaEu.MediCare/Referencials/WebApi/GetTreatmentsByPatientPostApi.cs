using System;

namespace FwaEu.MediCare.Referencials.WebApi
{
    public class GetTreatmentsByPatientPostApi
    {
        public int PatientId { get; set; }
        public int? TreatmentType { get; set; }
        public int? ArticleFamily { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
