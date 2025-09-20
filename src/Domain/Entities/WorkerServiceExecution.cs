namespace Domain.Entities
{
    public class WorkerServiceExecution
    {
        public Guid id { get; set; }
        public string workerService { get; set; }
        public string result { get; set; } 
        public string? resultDetailed { get; set; }
        public DateTime executionTime { get; set; } 
    }
}