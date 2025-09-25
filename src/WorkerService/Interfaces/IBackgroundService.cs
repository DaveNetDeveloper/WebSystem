namespace WorkerService.Interfaces
{
    public interface IBackgroundService
    { 
        Task RunAsync(CancellationToken stoppingToken); 
    }
}