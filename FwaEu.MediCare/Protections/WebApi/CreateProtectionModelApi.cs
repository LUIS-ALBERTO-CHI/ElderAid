using System;
using System.Collections.Generic;

namespace FwaEu.MediCare.Protections.WebApi
{
    public class CreateProtectionModelApi
    {
        public int PatientId { get; set; }
        public int ArticleId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        public Dictionary<TimeSpan, int> ProtectionDosages { get; set; }
        public string ArticleUnit { get; set; }
    }
}