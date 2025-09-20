using Application.Common;
using Application.DTOs.Filters;
using Application.Interfaces.Common;
using Application.Interfaces.DTOs.Filters;
using Domain.Entities;

namespace Application.Interfaces.Repositories
{
    public interface IWorkerServiceExecutionRepository : IRepository<WorkerServiceExecution, Guid>
    {
        
    }
}