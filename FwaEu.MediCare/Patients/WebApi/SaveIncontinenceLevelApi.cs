using System;

namespace FwaEu.MediCare.Patients.WebApi
{
    public class SaveIncontinenceLevelApi
    {
        public int Id { get; set; }
        public IncontinenceLevel Level { get; set; }
        public DateTime? DateStart { get; set; }
        public DateTime? DateEnd { get; set; }
    }
}