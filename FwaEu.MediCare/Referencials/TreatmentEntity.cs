using FluentNHibernate.Mapping;
using FwaEu.Fwamework.Data.Database;
using FwaEu.MediCare.Patients;
using System;

namespace FwaEu.MediCare.Referencials
{
    public enum TreatmentType
    {
        Fixe = 0,
        Reserve = 1,
        Erased = 2
    }

    public class TreatmentEntity
    {
        public int Id { get; set; }
        public TreatmentType Type { get; set; }

        public ArticleType ArticleType { get; set; }

        public int PrescribedArticleId { get; set; }
        public int AppliedArticleId { get; set; }

        public string DosageDescription { get; set; }



        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }

    }

}
