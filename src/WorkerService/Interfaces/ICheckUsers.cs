namespace WorkerService.Interfaces
{
    public interface ICheckUsers 
    { 
        Task RunCheckUsersAsync(CancellationToken stoppingToken); 
    }
}