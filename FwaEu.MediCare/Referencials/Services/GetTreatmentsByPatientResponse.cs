using System;

namespace FwaEu.MediCare.Referencials
{
    public class GetTreatmentsByPatientResponse
    {
        public int Id { get; set; }
        public int? PatientId { get; set; }
        public int? State { get; set; }

        public int? ArticleType { get; set; }

        public int PrescribedArticleId { get; set; }
        public int AppliedArticleId { get; set; }

        public string DosageDescription { get; set; }

        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}
