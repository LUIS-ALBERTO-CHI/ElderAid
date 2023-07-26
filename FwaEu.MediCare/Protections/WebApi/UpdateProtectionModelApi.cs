using System;
using System.Collections.Generic;

namespace FwaEu.MediCare.Protections.WebApi
{
    public class UpdateProtectionModelApi
    {
        public int ProtectionId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime StopDate { get; set; }
        public Dictionary<TimeSpan, int> ProtectionDosages { get; set; }
    }
}
