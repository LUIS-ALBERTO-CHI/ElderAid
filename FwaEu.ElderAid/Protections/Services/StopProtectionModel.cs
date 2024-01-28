using System;

namespace FwaEu.ElderAid.Protections.Services
{
    public class StopProtectionModel
    {
        public int ProtectionId { get; set; }
        public DateTime StopDate { get; set; }
        public int PatientId { get; set; }
    }
}