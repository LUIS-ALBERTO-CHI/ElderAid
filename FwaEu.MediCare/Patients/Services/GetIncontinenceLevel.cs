using System;

namespace FwaEu.MediCare.Patients
{
    public class GetIncontinenceLevel
    {
        public int Id { get; set; }
        public int IncontinenceLevel { get; set; }
        public double AnnualFixedPrice { get; set; }
        public double DailyFixedPrice { get; set; }
        public double DailyProtocolEntered { get; set; }

        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public double FixedPrice { get; set; }
        public double Consumed { get; set; }
        public double OverPassed { get; set; }

        public DateTime? VirtualDateWithoutOverPassed { get; set; }
    }
}