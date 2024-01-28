using System;

namespace FwaEu.ElderAid.Treatments.Services
{
    public class GetTreatmentsByPatientPost
    {
        public int PatientId { get; set; }
        public int? TreatmentType { get; set; }
        public int? ArticleFamily { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}
