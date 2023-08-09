namespace FwaEu.MediCare.Orders
{
    public class PeriodicOrderOptions
    {
        public int BackgroundTaskRegularityInMinutes { get; set; }
        public int ValidationHour { get; set; }
        public int ValidationMinutes { get; set; }
        public string RobotEmail { get; set; }
    }
}