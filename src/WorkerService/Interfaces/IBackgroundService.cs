namespace WorkerService.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBackgroundService
    { 
        Task RunAsync(CancellationToken stoppingToken); 
    }
}