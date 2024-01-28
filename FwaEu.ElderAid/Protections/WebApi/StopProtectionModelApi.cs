using System;

namespace FwaEu.ElderAid.Protections.WebApi
{
    public class StopProtectionModelApi
    {
        public int ProtectionId { get; set; }
        public DateTime StopDate { get; set; }
        public int PatientId { get; set; }
    }
}