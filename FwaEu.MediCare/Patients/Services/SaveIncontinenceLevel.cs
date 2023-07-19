using System;

namespace FwaEu.MediCare.Patients
{
    public class SaveIncontinenceLevel
    {
        public int Id { get; set; }
        public IncontinenceLevel Level { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}