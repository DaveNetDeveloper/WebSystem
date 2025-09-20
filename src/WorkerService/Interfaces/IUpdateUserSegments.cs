namespace WorkerService.Interfaces
{
    public interface IUpdateUserSegments
    { 
        Task RunUpdateUserSegmentsAsync(CancellationToken stoppingToken); 
    }
}