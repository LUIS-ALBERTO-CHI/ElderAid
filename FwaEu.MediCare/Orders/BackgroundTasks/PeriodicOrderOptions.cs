namespace FwaEu.MediCare.Orders.BackgroundTasks
{
    public class PeriodicOrderOptions
    {
        public int BackgroundTaskRegularityInMinutes { get; set; }
        public int ValidationHour { get; set; }
        public int ValidationMinutes { get; set; }
    }
}