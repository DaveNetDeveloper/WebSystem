namespace WorkerService.Configuration
{
    public class JobSettings
    { 
        public string JobName { get; set; } = null!;
        public int? IntervalMinutes { get; set; }
        public int? IntervalDays { get; set; }
        public DateTime? ScheduledTime { get; set; }
        public bool Enabled { get; set; } = true;
    }
}